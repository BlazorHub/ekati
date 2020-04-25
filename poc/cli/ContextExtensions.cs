using System;
using System.Security.Principal;
using Ahghee;
using Ahghee.Grpc;
using cli_grammer;
using Google.Protobuf;
using Range = Ahghee.Grpc.Range;

namespace cli
{
    public static class ContextExtensions
    {
        public static NodeID ToNodeID(this AHGHEEParser.NodeidContext id)
        {
            var json = id.obj();
                                
            if (json != null)
            {
                var text = json.GetText();
                var ab = Google.Protobuf.JsonParser.Default.Parse<NodeID>(text);
                ab.Pointer = Utils.NullMemoryPointer();
                return ab;
            }
        
            var dburi = id.id();

            var ac = new NodeID
            {
                Iri = dburi.GetText().Trim('"'),
                Pointer = Utils.NullMemoryPointer()
            };
            if (id.remote() != null)
            {
                ac.Remote = id.remote().GetText().Trim('"');
            }

            return ac;
        }

        public static DataBlock ToDataBlockNodeID(this string str)
        {
            var nid = new NodeID {Iri = str};
            nid.Pointer = new MemoryPointer();
            return new DataBlock
            {
                Nodeid = nid
            };
        }

        public static DataBlock ToDataBlock(this AHGHEEParser.ValueContext v)
        {
            var db = new DataBlock();

            if (v.STRING() != null)
            {
                db.Str = v.STRING().GetText().Trim('"');
            } else if (v.NUMBER() != null)
            {
                var numberstr = v.NUMBER().GetText();
                if (Int32.TryParse(numberstr, out var i32))
                {
                    db.I32 = i32;
                } else if (Int64.TryParse(numberstr, out var i64))
                {
                    db.I64 = i64;
                } else if (UInt64.TryParse(numberstr, out var ui64))
                {
                    db.Ui64 = ui64;
                } else if (Single.TryParse(numberstr, out var sing))
                {
                    db.F = sing;
                }
                else if(Double.TryParse(numberstr, out var doub))
                {
                    db.D = doub;
                }
            } else if (v.obj() != null)
            {
                var obj = v.obj().GetText();
                try
                {
                    var nid = JsonParser.Default.Parse<NodeID>(obj);
                    if (nid != null)
                    {
                        db.Nodeid = nid;
                        db.Nodeid.Pointer = new MemoryPointer();
                    }
                }
                catch (Exception e)
                {
                }
                try
                {
                    var mp = JsonParser.Default.Parse<MemoryPointer>(obj);
                    if (mp != null)
                    {
                        db.Memorypointer = mp;
                    }
                }
                catch (Exception e)
                {
                }
                try
                {
                    var tb = JsonParser.Default.Parse<TypeBytes>(obj);
                    if (tb != null)
                    {
                        db.Metabytes = tb;
                    }
                }
                catch (Exception e)
                {
                }
                // json object is not a native value type
                
            } else if (v.arr() != null)
            {
                // array is not a native value type.
                
            } else if (!string.IsNullOrWhiteSpace(v.GetText()))
            {
                var t = v.GetText().Trim('"');
                if (Boolean.TryParse(t, out var boo))
                {
                    db.B = boo;
                } else if (t == "null")
                {
                    // do nothing.    
                }
            }
            // json fallback
            if (db.DataCase == DataBlock.DataOneofCase.None)
            {
                var jsonstuff = v.GetText(); // must be some kind of json at this point.. (I hope :P)
                                
                var tb = new TypeBytes();
                tb.Typeiri = "application/json";
                tb.Bytes = ByteString.CopyFromUtf8(jsonstuff);
                db.Metabytes = tb;
            }

            return db;
        }

        public static FollowOperator.Types.FollowAny ToFollowAny(this AHGHEEParser.AnynumContext ctx)
        {
            return new FollowOperator.Types.FollowAny()
            {
                Range = ctx.range()?.ToRange()
            };
        }

        public static Range ToRange(this AHGHEEParser.RangeContext ctx)
        {
            return new Range()
            {
                From = Int32.Parse(ctx.@from()?.GetText() ?? "0"),
                To = Int32.Parse(ctx.to().GetText() ?? "0")
            };
        }

        public static FollowOperator.Types.EdgeNum ToEdgeNum(this AHGHEEParser.EdgenumContext ctx)
        {
            var e = new FollowOperator.Types.EdgeNum();
            var db = ctx.value()?.ToDataBlock();
            if (db != null)
            {
                e.EdgeRange = new FollowOperator.Types.EdgeRange
                {
                    Edge = db,
                    Range = ctx.range().ToRange()
                };
            }
            else
            {
                e.EdgeCmp = new FollowOperator.Types.EdgeCMP()
                {
                    Left = ctx.edgenum(0).ToEdgeNum(),
                    BOOLOP =ctx.BOOLOP().GetText(),
                    Right = ctx.edgenum(1).ToEdgeNum()
                };
            }

            return e;
        }
        public static FollowOperator ToFollowOperator(this AHGHEEParser.FollowContext ctx)
        {
            var fo = new FollowOperator();

            if (ctx.anynum() != null)
            {
                fo.FollowAny = ctx.anynum().ToFollowAny();
            }
            else
            {
                fo.FollowEdge = ctx.edgenum().ToEdgeNum();
            }
            return fo;
        }

        public static FilterOperator.Types.Compare ToCompare(this AHGHEEParser.CompareContext ctx)
        {
            var key = ctx.wfkey()?.value().ToDataBlock();
            if (key!=null)
            {
                var value = ctx.wfvalue()?.value().ToDataBlock();
                return new FilterOperator.Types.Compare()
                {
                    KevValueCmp = new FilterOperator.Types.CompareKeyValue()
                    {
                        Property = key,
                        MATHOP = ctx.MATHOP().GetText(),
                        Value = value
                    }
                };
            }

            var left = ctx.compare(0);
            var right = ctx.compare(1);
            
            return new FilterOperator.Types.Compare()
            {
                CompoundCmp = new FilterOperator.Types.CompareCompound()
                {
                    Left = left.ToCompare(),
                    BOOLOP = ctx.BOOLOP().GetText(),
                    Right = right.ToCompare()
                }
            };
        }
        
        public static FilterOperator ToFilterOperator(this AHGHEEParser.WherefilterContext ctx)
        {
            var filter = new FilterOperator();
            filter.Compare = ctx.compare()?.ToCompare();
            return filter;
        }
        public static Step ToPipeFlow(this AHGHEEParser.PipeContext ctx)
        {
            var cmd = ctx.pipecmd();
            if (cmd.follow() != null)
            {
                var follow = cmd.follow().ToFollowOperator();
                return new Step()
                {
                    Follow = follow,
                    Next = ctx.pipe()?.ToPipeFlow()
                };
            } 
            
            if (cmd.wherefilter() != null)
            {
                var filter = cmd.wherefilter().ToFilterOperator();
                return new Step()
                {
                    Where = filter,
                    Next = ctx.pipe()?.ToPipeFlow()
                };
            }
            throw new NotImplementedException("Only Follow and Filter are available in PipeContext so far.");
        }
    }
}