module Tests

open System
open Xunit
open Ahghee
open System


let Id id = { NodeIRI.Domain = "biggraph://example.com"; NodeIRI.Database="test"; NodeIRI.Graph="People"; NodeIRI.NodeId=id; NodeIRI.RouteKey= None} 
let TestCreateExternalIRI = ExternalIRI (System.Uri( "https://ahghee.com" )) 
let TestCreateBinary = Binary { MimeBytes.Mime= Some("application/json"); MimeBytes.Bytes = Array.Empty<byte>() } 

[<Fact>]
let ``Can create an InternalIRI type`` () =
    let d : Data = InternalIRI (Id "1") 
    let success = match d with 
        | InternalIRI (nodeIRI) -> true
        | ExternalIRI (external) -> false
        | Binary (data) -> false   
    Assert.True(success)  
    
[<Fact>]
let ``Can create an ExternalIRI type`` () =
    let d : Data = TestCreateExternalIRI
    
    let success = match d with 
        | InternalIRI (nodeIRI) -> false
        | ExternalIRI (external) -> true
        | Binary (data) -> false
    Assert.True success          

[<Fact>]
let ``Can create a Binary type`` () =
    let d : Data = TestCreateBinary
    let success = match d with 
        | InternalIRI (nodeIRI) -> false
        | ExternalIRI (external) -> false
        | Binary (data) -> true
    Assert.True success   

let mimePlainTextUtf8 = Some("text/plain;charset=utf-8")
let BStr (text:string) = Binary { Mime = mimePlainTextUtf8 ; Bytes = Text.UTF8Encoding.UTF8.GetBytes(text) }
    
let Prop (key:Data) (values:seq<Data>) =
    let pair = { Pair.Key = key; Value = (values |> Seq.toArray)}   
    pair  
    
let PropStr (key:string) (values:seq<string>) = Prop (BStr key) (values |> Seq.map(fun x -> BStr x))  
let PropStrData (key:string) (values:seq<Data>) = Prop (BStr key) values      
    
[<Fact>]
let ``Can create a Pair`` () =
    let pair = PropStr "firstName" [|"Richard"; "Dick"|]

    let success = match pair.Key with 
                | Binary (b) when b.Mime.IsSome && b.Mime.Value = mimePlainTextUtf8.Value -> true 
                | _ -> false
    Assert.True success     
    
[<Fact>]
let ``Can create a Node`` () =
    let node = { 
                Node.NodeIds = [| Id "1" |] 
                Node.Attributes = [|
                                    PropStr "firstName" [|"Richard"; "Dick"|] 
                                  |]
               }

    let empty = node.Attributes |> Seq.isEmpty
    Assert.True (not empty)
    
[<Fact>]
let ``Can traverse local graph index`` () =
    let node1 = { 
                Node.NodeIds = [| Id "1" |] 
                Node.Attributes = [|
                                    PropStr "firstName" [|"Richard"; "Dick"|] 
                                    PropStrData "follows" [| InternalIRI (Id "2") |] 
                                  |]
               }
               
    let node2 = { 
                Node.NodeIds = [| Id "2" |] 
                Node.Attributes = [|
                                    PropStr "firstName" [|"Sam"; "Sammy"|] 
                                    PropStrData "follows" [| InternalIRI (Id "1") |]
                                  |]
               }
               
    let node3 = { 
                Node.NodeIds = [| Id "3" |] 
                Node.Attributes = [|
                                    PropStr "firstName" [|"Jim"|]
                                    PropStrData "follows" [| InternalIRI (Id "1"); InternalIRI (Id "2") |] 
                                  |]
               }      
               
    let graph:Graph = new Graph()
    graph.Add [| node1; node2; node3 |]
    
    let nodesWithIncomingEdges = node3.Attributes 
                                         |> Seq.collect (fun y -> y.Value 
                                                               |> Seq.map (fun x -> match x with  
                                                               | InternalIRI(id) -> Some(id) 
                                                               | _ -> None))   
                                         |> Seq.filter (fun x -> match x with 
                                                                 | Some id -> true 
                                                                 | _ -> false)
                                         |> Seq.map    (fun x -> x.Value )
                                         |> Seq.distinct
                                         |> Seq.map (fun x -> graph.TryFind x)                                                                            

    Assert.NotEmpty nodesWithIncomingEdges

    
