// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: types.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Ahghee.Grpc {
  public static partial class WatDbService
  {
    static readonly string __ServiceName = "ahghee.grpc.WatDbService";

    static readonly grpc::Marshaller<global::Ahghee.Grpc.LoadFile> __Marshaller_ahghee_grpc_LoadFile = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ahghee.Grpc.LoadFile.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ahghee.Grpc.LoadFileResponse> __Marshaller_ahghee_grpc_LoadFileResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ahghee.Grpc.LoadFileResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ahghee.Grpc.Node> __Marshaller_ahghee_grpc_Node = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ahghee.Grpc.Node.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ahghee.Grpc.PutResponse> __Marshaller_ahghee_grpc_PutResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ahghee.Grpc.PutResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ahghee.Grpc.Query> __Marshaller_ahghee_grpc_Query = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ahghee.Grpc.Query.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ahghee.Grpc.GetMetricsRequest> __Marshaller_ahghee_grpc_GetMetricsRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ahghee.Grpc.GetMetricsRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ahghee.Grpc.GetMetricsResponse> __Marshaller_ahghee_grpc_GetMetricsResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ahghee.Grpc.GetMetricsResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ahghee.Grpc.GetStatsRequest> __Marshaller_ahghee_grpc_GetStatsRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ahghee.Grpc.GetStatsRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ahghee.Grpc.GetStatsResponse> __Marshaller_ahghee_grpc_GetStatsResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ahghee.Grpc.GetStatsResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ahghee.Grpc.ListStatsRequest> __Marshaller_ahghee_grpc_ListStatsRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ahghee.Grpc.ListStatsRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ahghee.Grpc.ListStatsResponse> __Marshaller_ahghee_grpc_ListStatsResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ahghee.Grpc.ListStatsResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ahghee.Grpc.ListPoliciesRequest> __Marshaller_ahghee_grpc_ListPoliciesRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ahghee.Grpc.ListPoliciesRequest.Parser.ParseFrom);

    static readonly grpc::Method<global::Ahghee.Grpc.LoadFile, global::Ahghee.Grpc.LoadFileResponse> __Method_Load = new grpc::Method<global::Ahghee.Grpc.LoadFile, global::Ahghee.Grpc.LoadFileResponse>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "Load",
        __Marshaller_ahghee_grpc_LoadFile,
        __Marshaller_ahghee_grpc_LoadFileResponse);

    static readonly grpc::Method<global::Ahghee.Grpc.Node, global::Ahghee.Grpc.PutResponse> __Method_Put = new grpc::Method<global::Ahghee.Grpc.Node, global::Ahghee.Grpc.PutResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Put",
        __Marshaller_ahghee_grpc_Node,
        __Marshaller_ahghee_grpc_PutResponse);

    static readonly grpc::Method<global::Ahghee.Grpc.Query, global::Ahghee.Grpc.Node> __Method_Get = new grpc::Method<global::Ahghee.Grpc.Query, global::Ahghee.Grpc.Node>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "Get",
        __Marshaller_ahghee_grpc_Query,
        __Marshaller_ahghee_grpc_Node);

    static readonly grpc::Method<global::Ahghee.Grpc.GetMetricsRequest, global::Ahghee.Grpc.GetMetricsResponse> __Method_GetMetrics = new grpc::Method<global::Ahghee.Grpc.GetMetricsRequest, global::Ahghee.Grpc.GetMetricsResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetMetrics",
        __Marshaller_ahghee_grpc_GetMetricsRequest,
        __Marshaller_ahghee_grpc_GetMetricsResponse);

    static readonly grpc::Method<global::Ahghee.Grpc.GetStatsRequest, global::Ahghee.Grpc.GetStatsResponse> __Method_GetStats = new grpc::Method<global::Ahghee.Grpc.GetStatsRequest, global::Ahghee.Grpc.GetStatsResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetStats",
        __Marshaller_ahghee_grpc_GetStatsRequest,
        __Marshaller_ahghee_grpc_GetStatsResponse);

    static readonly grpc::Method<global::Ahghee.Grpc.ListStatsRequest, global::Ahghee.Grpc.ListStatsResponse> __Method_ListStats = new grpc::Method<global::Ahghee.Grpc.ListStatsRequest, global::Ahghee.Grpc.ListStatsResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ListStats",
        __Marshaller_ahghee_grpc_ListStatsRequest,
        __Marshaller_ahghee_grpc_ListStatsResponse);

    static readonly grpc::Method<global::Ahghee.Grpc.ListPoliciesRequest, global::Ahghee.Grpc.Node> __Method_ListPolicies = new grpc::Method<global::Ahghee.Grpc.ListPoliciesRequest, global::Ahghee.Grpc.Node>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "ListPolicies",
        __Marshaller_ahghee_grpc_ListPoliciesRequest,
        __Marshaller_ahghee_grpc_Node);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Ahghee.Grpc.TypesReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of WatDbService</summary>
    [grpc::BindServiceMethod(typeof(WatDbService), "BindService")]
    public abstract partial class WatDbServiceBase
    {
      public virtual global::System.Threading.Tasks.Task Load(global::Ahghee.Grpc.LoadFile request, grpc::IServerStreamWriter<global::Ahghee.Grpc.LoadFileResponse> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Ahghee.Grpc.PutResponse> Put(global::Ahghee.Grpc.Node request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task Get(global::Ahghee.Grpc.Query request, grpc::IServerStreamWriter<global::Ahghee.Grpc.Node> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Ahghee.Grpc.GetMetricsResponse> GetMetrics(global::Ahghee.Grpc.GetMetricsRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Ahghee.Grpc.GetStatsResponse> GetStats(global::Ahghee.Grpc.GetStatsRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Ahghee.Grpc.ListStatsResponse> ListStats(global::Ahghee.Grpc.ListStatsRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task ListPolicies(global::Ahghee.Grpc.ListPoliciesRequest request, grpc::IServerStreamWriter<global::Ahghee.Grpc.Node> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for WatDbService</summary>
    public partial class WatDbServiceClient : grpc::ClientBase<WatDbServiceClient>
    {
      /// <summary>Creates a new client for WatDbService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public WatDbServiceClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for WatDbService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public WatDbServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected WatDbServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected WatDbServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual grpc::AsyncServerStreamingCall<global::Ahghee.Grpc.LoadFileResponse> Load(global::Ahghee.Grpc.LoadFile request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Load(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncServerStreamingCall<global::Ahghee.Grpc.LoadFileResponse> Load(global::Ahghee.Grpc.LoadFile request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_Load, null, options, request);
      }
      public virtual global::Ahghee.Grpc.PutResponse Put(global::Ahghee.Grpc.Node request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Put(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Ahghee.Grpc.PutResponse Put(global::Ahghee.Grpc.Node request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Put, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Ahghee.Grpc.PutResponse> PutAsync(global::Ahghee.Grpc.Node request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return PutAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Ahghee.Grpc.PutResponse> PutAsync(global::Ahghee.Grpc.Node request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Put, null, options, request);
      }
      public virtual grpc::AsyncServerStreamingCall<global::Ahghee.Grpc.Node> Get(global::Ahghee.Grpc.Query request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Get(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncServerStreamingCall<global::Ahghee.Grpc.Node> Get(global::Ahghee.Grpc.Query request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_Get, null, options, request);
      }
      public virtual global::Ahghee.Grpc.GetMetricsResponse GetMetrics(global::Ahghee.Grpc.GetMetricsRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetMetrics(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Ahghee.Grpc.GetMetricsResponse GetMetrics(global::Ahghee.Grpc.GetMetricsRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetMetrics, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Ahghee.Grpc.GetMetricsResponse> GetMetricsAsync(global::Ahghee.Grpc.GetMetricsRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetMetricsAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Ahghee.Grpc.GetMetricsResponse> GetMetricsAsync(global::Ahghee.Grpc.GetMetricsRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetMetrics, null, options, request);
      }
      public virtual global::Ahghee.Grpc.GetStatsResponse GetStats(global::Ahghee.Grpc.GetStatsRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetStats(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Ahghee.Grpc.GetStatsResponse GetStats(global::Ahghee.Grpc.GetStatsRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetStats, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Ahghee.Grpc.GetStatsResponse> GetStatsAsync(global::Ahghee.Grpc.GetStatsRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetStatsAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Ahghee.Grpc.GetStatsResponse> GetStatsAsync(global::Ahghee.Grpc.GetStatsRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetStats, null, options, request);
      }
      public virtual global::Ahghee.Grpc.ListStatsResponse ListStats(global::Ahghee.Grpc.ListStatsRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ListStats(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Ahghee.Grpc.ListStatsResponse ListStats(global::Ahghee.Grpc.ListStatsRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_ListStats, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Ahghee.Grpc.ListStatsResponse> ListStatsAsync(global::Ahghee.Grpc.ListStatsRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ListStatsAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Ahghee.Grpc.ListStatsResponse> ListStatsAsync(global::Ahghee.Grpc.ListStatsRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_ListStats, null, options, request);
      }
      public virtual grpc::AsyncServerStreamingCall<global::Ahghee.Grpc.Node> ListPolicies(global::Ahghee.Grpc.ListPoliciesRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ListPolicies(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncServerStreamingCall<global::Ahghee.Grpc.Node> ListPolicies(global::Ahghee.Grpc.ListPoliciesRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_ListPolicies, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override WatDbServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new WatDbServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(WatDbServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_Load, serviceImpl.Load)
          .AddMethod(__Method_Put, serviceImpl.Put)
          .AddMethod(__Method_Get, serviceImpl.Get)
          .AddMethod(__Method_GetMetrics, serviceImpl.GetMetrics)
          .AddMethod(__Method_GetStats, serviceImpl.GetStats)
          .AddMethod(__Method_ListStats, serviceImpl.ListStats)
          .AddMethod(__Method_ListPolicies, serviceImpl.ListPolicies).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, WatDbServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_Load, serviceImpl == null ? null : new grpc::ServerStreamingServerMethod<global::Ahghee.Grpc.LoadFile, global::Ahghee.Grpc.LoadFileResponse>(serviceImpl.Load));
      serviceBinder.AddMethod(__Method_Put, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Ahghee.Grpc.Node, global::Ahghee.Grpc.PutResponse>(serviceImpl.Put));
      serviceBinder.AddMethod(__Method_Get, serviceImpl == null ? null : new grpc::ServerStreamingServerMethod<global::Ahghee.Grpc.Query, global::Ahghee.Grpc.Node>(serviceImpl.Get));
      serviceBinder.AddMethod(__Method_GetMetrics, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Ahghee.Grpc.GetMetricsRequest, global::Ahghee.Grpc.GetMetricsResponse>(serviceImpl.GetMetrics));
      serviceBinder.AddMethod(__Method_GetStats, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Ahghee.Grpc.GetStatsRequest, global::Ahghee.Grpc.GetStatsResponse>(serviceImpl.GetStats));
      serviceBinder.AddMethod(__Method_ListStats, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Ahghee.Grpc.ListStatsRequest, global::Ahghee.Grpc.ListStatsResponse>(serviceImpl.ListStats));
      serviceBinder.AddMethod(__Method_ListPolicies, serviceImpl == null ? null : new grpc::ServerStreamingServerMethod<global::Ahghee.Grpc.ListPoliciesRequest, global::Ahghee.Grpc.Node>(serviceImpl.ListPolicies));
    }

  }
}
#endregion
