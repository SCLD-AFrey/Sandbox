using System;
using System.Globalization;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using VikingEnterprise.GuiClient.Models.Global;
using VikingEntity.Server.Protos.Connection;
using VikingEntity.Server.Protos.UserManager;

namespace VikingEnterprise.GuiClient.Services;

public class RpcClientService
{
    private readonly ILogger<RpcClientService> m_logger;
    private readonly ClientConfiguration m_clientConfiguration;
    private readonly string m_serverAddress;
    
    public RpcClientService(ClientConfiguration p_clientConfiguration, ILogger<RpcClientService> p_logger)
    {
        m_logger = p_logger;
        m_clientConfiguration = p_clientConfiguration;
        m_serverAddress = $"https://{m_clientConfiguration.ServerAddress}:{m_clientConfiguration.Port}";
        m_logger.LogDebug("Initializing RpcClient");
    }

    private ConnectionRpc.ConnectionRpcClient ConnectionRpcClient()
    {
        return new ConnectionRpc.ConnectionRpcClient(GrpcChannel.ForAddress(m_serverAddress));
    }
    
    public UserManagerRpc.UserManagerRpcClient UserManagerRpcClient()
    {
        return new UserManagerRpc.UserManagerRpcClient(GrpcChannel.ForAddress(m_serverAddress));
    }

    public async Task<bool> CheckConnectionAsync()
    {
        var isConnected = false;
        try
        {
            var client = ConnectionRpcClient();
            var response = client.CheckServerConnection(new G_ConnectCheckRequest());
            isConnected = DateTime.TryParse(response.ServerTime.ToDateTime().ToString(CultureInfo.InvariantCulture), out var serverTime);
            m_logger.LogInformation("Client connection check complete - Server Time: {ServerTime}", serverTime);
        } 
        catch (Exception e)
        {
            m_logger.LogError("Client connection check FAILED - Server Time: {ServerTime}", DateTime.UtcNow);
        }
        return await Task.FromResult(isConnected);
    }
}