using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using VikingEntity.Server.Protos.Connection;


namespace VikingEnterprise.Client.Models;

public class RpcClientFactory
{
    
    private readonly ILogger<RpcClientFactory> m_logger;
    private readonly ClientConfiguration m_clientConfiguration;
    private readonly string m_serverAddress;
    public RpcClientFactory(ClientConfiguration p_clientConfiguration, ILogger<RpcClientFactory> p_logger)
    {
        m_logger = p_logger;
        m_clientConfiguration = p_clientConfiguration;
        m_serverAddress = $"https://{m_clientConfiguration.ServerAddress}:{m_clientConfiguration.Port}";
        m_logger.LogDebug("Initializing RpcClient");
    }

    public ConnectionRpc.ConnectionRpcClient ConnectionRpcClient()
    {
        return new ConnectionRpc.ConnectionRpcClient(GrpcChannel.ForAddress(m_serverAddress));
    }
    
    // public LoginRpc.LoginRpcClient LoginRpcClient()
    // {
    //     return new LoginRpc.LoginRpcClient(GrpcChannel.ForAddress(m_serverAddress));
    // }
}