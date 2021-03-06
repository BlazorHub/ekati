
use std::sync;
use std::sync::mpsc;
use std::thread;
use std::time::Duration;
use std::fs::OpenOptions;
use parity_rocksdb::{Options};
use parity_rocksdb::WriteBatch;
use parity_rocksdb::WriteOptions;
use ::shard::shardindex::ShardIndex;
use ::shard::io::IO;
use ::protobuf::*;
use mytypes::types::*;
use fileio::bufferedasync::*;
use ::threadpool::ThreadPool;
use tokio_linux_aio::AioContext;
use futures_cpupool::CpuPool;
use std::convert;
use std::mem;
use memmap;
use libc::{c_long, c_void, mlock};
use libc::{close, open, O_DIRECT, O_RDONLY, O_RDWR};
use std::os::unix::io::RawFd;
use std::os::unix::ffi::OsStrExt;
use futures::Future;
use futures;
use std::time::SystemTime;


pub struct ShardWorker {
    pub post: mpsc::Sender<self::IO>,
    thread: thread::JoinHandle<()>,
    shard_id: u32
//    scan_index : Vec<Pointer>
}

impl ShardWorker {
    /// Sets the file position and length
    /// Returns the position + length which is the next position
    fn make_pointer(shard_id: u32, last_file_position: u64, size: u64) -> Pointer {
        use ::protobuf::Message;
        let mut ptr = Pointer::new();
        ptr.partition_key = shard_id as u32;
        ptr.filename = 0 as u32; // todo: this is not correct. idea use 1 byte for level and remaining bytes for file number
        ptr.length = size;
        ptr.offset = last_file_position;
        ptr
    }

    /// This is used by sort_key_values internally. Maybe you should use that.
    fn _sorted_key_values(nf: &Node_Fragment) -> ( RepeatedField<Key>, RepeatedField<Value>) {
        let mut indexed_keys: Vec<(usize, &Key)> = Vec::new();
        let mut sorted_keys = ::protobuf::RepeatedField::<Key>::new();
        let mut sorted_values_by_key = ::protobuf::RepeatedField::<Value>::new();

        let k = nf.get_keys();
        let v = nf.get_values();
        indexed_keys = k.iter().enumerate().collect();

        indexed_keys.sort_unstable_by_key(|t| t.1.get_name());
        // now move the keys and the values by the indexed_keys
        // keys

        for indexed_key in indexed_keys {
            sorted_keys.push(k[indexed_key.0].clone());
            sorted_values_by_key.push(v[indexed_key.0].clone());
        }

        (sorted_keys, sorted_values_by_key)
    }

    /// this replaces the keys and values with sorted ones to enable searching keys, and having
    /// the values ordinal match the keys' ordinal
    fn sort_key_values(nf: &mut Node_Fragment){

        let (ks,vs) = ShardWorker::_sorted_key_values(&nf);
        nf.set_keys(ks);
        nf.set_values(vs);
    }


    /// Creates and starts up a shard with it's own IO thread.
    pub fn new(shard_id:u32, create_testing_directory:bool) -> ShardWorker {
        use std::fs;
        use std::env;
        use std::path;
        use std::io::*;
        use std::mem;
        use std::slice;
        use ::protobuf::Message;

        let (a,receiver) = mpsc::channel::<IO>();
        let test_dir = create_testing_directory.clone();
        let _shard_id = shard_id.clone();

        let sw: ShardWorker = ShardWorker{
            post: a,
            thread: thread::spawn(move ||{
                // todo: allow specifying the directory when creating the shard
                // to allow spreading shards across multiple physical disks
                let dir = env::current_dir().unwrap().join(path::Path::new(&format!("shard-{}",shard_id)));

                if create_testing_directory{
                    fs::remove_dir_all(&dir).unwrap();
                }

                match fs::create_dir(&dir){
                    Ok(_) => Ok(()),
                    Err(ref _e) if _e.kind() == ErrorKind::AlreadyExists => Ok(()),
                    Err(ref _e) => Err(_e)
                }.expect("Create Directory or already exists");


                let node_index_db_path =  dir.join(path::Path::new("node_index"));
                // todo: tune it. https://github.com/facebook/rocksdb/wiki/RocksDB-Tuning-Guide

                // NodeID -> Pointers
                let mut index = ShardIndex::new(node_index_db_path.to_str().unwrap());

                // todo: level files
                // todo: index files
                // filename should be = "{filenameInt}.{fileversion}"
                let file_name = format!("{}.0",0);
                let file_path_buf = dir.join(path::Path::new(&file_name));
                let file_path = file_path_buf.as_path();
                let file_error = format!("Could not open file: {}",&file_path.to_str().expect("valid path"));
                let mut file_out = OpenOptions::new().create(true).write(true).open(&file_path).expect(&file_error);
                let pre_alloc_size = 1024 * 50000;
                file_out.set_len(pre_alloc_size).expect("File size reserved");
                file_out.flush().unwrap();
                let mut file_out = OpenOptions::new().write(true).open(&file_path).expect(&file_error);

                let mut last_file_position = file_out.seek(SeekFrom::Start(0)).expect("Couldn't seek to current position");



                let mut bufaio = BufferedAsync::new(5, 32, _shard_id);

                loop {
                    let data = receiver.recv_timeout(Duration::from_millis(1));
                    match data {
                        Ok(io) => match io {
                            // todo: throw this Add operation into a future on a current thread pool, so when we block on nodes.recv_timeout we can process from some other IO::Add operation that comes in
                            // because as it is now, all items from the current add operation have to come in and finish being written, before we start to put in any nodes from then next Add operation in line.
                            IO::Add {nodes, callback}  => {

                                // I think we want two memtables. A, and B so that when A is full we can have another thread start writing it out
                                // and in the mean time, start adding data into memtable B.
                                // This means that if a read request comes in that needs to read from memtables
                                // if the data is in the active memtable, then we are ok to read it.
                                // if it is in the memtable that was sent to flush to disk, then we need to wait for that operation to finish,
                                // and then just use a standard file read to access that data. Assuming we aren't yet using DirectIO, that data
                                // would be in the OS page cache.

                                // We likely need a struct to represent this memtable state machine.

                                let memtable_capacity:usize = 1024*64;
                                let buffer_flush_len:usize = 1024*64;
                                let mut memtable : Vec<u8> = Vec::with_capacity(memtable_capacity);

                                let mut index_batch_opts = WriteOptions::new();

                                // todo: figure out what needs to be done so we can disable_wal for more perf.
                                // disable_wal can make writes faster, but we will need to have a
                                // index recovery mechanisim in case we crash, or the process
                                // closes before the index is fully synced.
                                //index_batch_opts.disable_wal(true);

                                let mut index_batch = WriteBatch::new();
                                loop {
                                    // todo: use nodes.try_recv() and if it isn't ready, then take this IO::Add and put it in another channel that we will interleave with the main channel
                                    // we will have to flush the buffer to disk if we put anything in it... unless move the buffer out of this operation and share it across multiple ops.
                                    // Or we could learn how to use the futures crate.
                                    //let xxxx = nodes.try_recv()
                                    let mut node = nodes.recv_timeout(Duration::from_millis(1));
                                    match node {
                                        Ok(ref mut _n) =>{
                                            //println!("Got Nodes");
                                            let buffer_pos = memtable.len() as u64;
                                            let total_pos = buffer_pos + last_file_position;

                                            {
                                                // make sure our fragment has reserved space for other fragment pointers.
                                                // we are using 3 instead of 2 to enable fanning out the fragment loading, vs 2 would be following a linked list.
                                                while _n.fragments.len() > 3 {
                                                    _n.fragments.pop();
                                                }
                                                while _n.fragments.len() < 3 {
                                                    _n.fragments.push(Pointer::new());
                                                }
                                                // todo: check in the nodeid->pointer index to see if we should be setting a pointer.
                                                // and tracking that other fragments need to know about this new one.

                                                // make sure our nodeId does not have a pointer in it.
                                                let size_incoming = _n.compute_size();
                                                // scope to hold mut_id
                                                {
                                                    let mut id = _n.mut_id();
                                                    if (&id).has_node_id() {
                                                        id.mut_node_id().set_node_pointer({
                                                            ShardWorker::make_pointer(_shard_id, total_pos, size_incoming as u64)
                                                        });
                                                    } else if (&id).has_global_node_id() {
                                                        // and if we have a global_node_id convert it to local_node_id
                                                        let mut local_id = id.mut_global_node_id().take_nodeid();
                                                        local_id.set_node_pointer({
                                                            ShardWorker::make_pointer(_shard_id, total_pos, size_incoming as u64)
                                                        });
                                                        id.set_node_id(local_id);
                                                    }
                                                }
                                                let size_before = _n.compute_size();
                                                // scope for mut_id
                                                {
                                                    let mut id = _n.mut_id();
                                                    id.mut_node_id().set_node_pointer({
                                                        ShardWorker::make_pointer(_shard_id, total_pos, size_before as u64)
                                                    });
                                                }
                                                let size_after = _n.compute_size();
                                                assert_eq!(size_before, size_after, "We are testing to makes sure the size of our fragment didn't change when we set it's pointer");
                                            }

                                            // Must make sure that our keys are sorted for quick lookup later; unless we decide we can do this later.
                                            ShardWorker::sort_key_values(_n);

                                            // NOTE: By writing Length Delimited, the offset in our Pointer, points to Length, not the beginning of the data
                                            // So the offset is "offset" by an i32.
                                            _n.write_length_delimited_to_vec(&mut memtable).expect("write_to_bytes");

                                            // todo: Would adding to the index in a separate channel speed this up?
                                            // todo: Would a "multi_put" be faster?
                                            // Add this nodeid to the nodeid index
                                            let _key = _n.mut_id().mut_node_id();
                                            let _values = &mut Pointers::new();
                                            {
                                                // todo: Anyway to avoid doing this clone?
                                                let _value = _key.get_node_pointer().clone();
                                                _values.pointers.insert(0, _value);
                                            }
                                            // must clear the node pointer before using it as the Key in the lookup
                                            _key.clear_node_pointer();

                                            index.node_index_merge(&mut index_batch, _key, _values);

                                            if &memtable.len() >= &buffer_flush_len {
                                                // flush the buffer
                                                let _written_size = file_out.write(&memtable).expect("file write");
                                                last_file_position = last_file_position + _written_size as u64;
                                                assert_ne!(0,last_file_position, "We are testing that after wrote data, we are incrementing our last_file_position");
                                                &memtable.clear();
                                            }
                                            continue;
                                        },
                                        Err(_e) => {
                                            // the happy error is - Got nodes err: channel is empty and sending half is closed
                                            // which means we got through all the sent items :)
                                            // anything else is sadness.
                                            warn!("Got nodes err: {}", _e);
                                            break;
                                        }
                                    }

                                }
                                // flush the index_batch
                                // todo: can we do this async some how? Look at AIO for linux https://github.com/hmwill/tokio-linux-aio
                                index.db.write(index_batch).unwrap();
                                // flush the buffer
                                let _written_size = file_out.write(&memtable).expect("file write");
                                &memtable.clear();
                                file_out.flush().expect("flush file");
                                callback.send(Ok(())).expect("callback still open");
                                // todo: background work to link fragments
                            },
                            IO::ReadNodeFragments {nodeids, callback} => {
                                let mut all_rocks_time:Duration = Duration::new(0,0);
                                let mut futures = Vec::new();
                                // /////////////////////////////
                                // WARNING:: If the client never closes their side of the nodeids channel, then we block here forever.
                                // /////////////////////////////
                                // flush the writer side... if we are going to read from it.
                                // file_out should really be a block of memory that we just
                                // write to disk when it fills up. And we should be reading
                                // from all the files we have created that are "closed" and
                                // not "superseded".
                                file_out.flush().unwrap();
                                for nodeid in nodeids.iter() {
                                    let mut frag_pointers: Vec<Pointer> = Vec::new();
                                    let mut from_index = false;
                                    if nodeid.has_node_pointer() {
                                        frag_pointers.push(nodeid.get_node_pointer().to_owned());
                                    } else {
                                        from_index = true;
                                        {
                                            let rocks_time = SystemTime::now();
                                            let got = &index.node_index_get(&nodeid);
                                            all_rocks_time = all_rocks_time + rocks_time.elapsed().unwrap();
                                            got.iter().for_each(|opts| {
                                                opts.iter().for_each(|pts| {
                                                    pts.get_pointers().iter().for_each(|p| {
                                                        frag_pointers.push(p.clone());
                                                    })
                                                })
                                            });
                                        }
                                    }


                                    if from_index {
                                        // we should have all the node_fragment pointers we know about
                                        let n_frag = frag_pointers.len();
                                        let fpi = frag_pointers.iter();
                                        fpi.for_each(|p| {

                                            let my_cb = callback.clone();
                                            let abc = bufaio.read_async(p.filename, p.offset, p.length, my_cb);
                                            futures.push(abc);

                                            ()
                                        });
                                    } else {
                                        // as we load a fragment we either need to follow internal pointers
                                        // or we need to go back to the index.
                                    }
                                }
                                // Wait for completion
                                let result = futures::future::join_all(futures).wait();
                                info!("RocksTime in {}s {}ms", all_rocks_time.as_secs(), all_rocks_time.subsec_millis());
                                assert!(result.is_ok());
                            }
                            IO::NoOP => debug!("Got NoOp"),
                            IO::Shutdown => {
                                // index.db shutdown?
                                // todo: if we have a thread pool running maintenance tasks or IO:Add tasks, we need to call shutdown on those.
                                break;
                            }
                        },
                        Err(_e) => thread::sleep(Duration::from_millis(1))
                    }
                }



            }),
            shard_id
        };
        sw
    }
}