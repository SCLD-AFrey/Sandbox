using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using VikingEntity.Server.Protos.Connection;

namespace VikingEnterprise.Server.Services.Grpc;

public class ConnectionService : ConnectionRpc.ConnectionRpcBase
{
    public override Task<G_ConnectCheckResponse> CheckServerConnection(G_ConnectCheckRequest p_request, ServerCallContext p_context)
    {
        G_ConnectCheckResponse reply = new()
        {
            ServerTime = DateTime.UtcNow.ToTimestamp()
        };
        return Task.FromResult(reply);
    }
}