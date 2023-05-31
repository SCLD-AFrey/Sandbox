using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using VikingEntity.Server.Protos.Connection;

namespace VikingEnterprise.Client.Models;

public class ServerConnection
{
    private readonly ClientConfiguration m_clientConfiguration;
    private readonly ILogger<ServerConnection> m_logger;
    private readonly RpcClientFactory m_rpcClientFactory;
    public ServerConnection(ClientConfiguration p_clientConfiguration, ILogger<ServerConnection> p_logger, RpcClientFactory p_rpcClientFactory)
    {
        m_clientConfiguration = p_clientConfiguration;
        m_logger = p_logger;
        m_rpcClientFactory = p_rpcClientFactory;
        m_logger.LogDebug("Initializing ServerConnection");
    }

    public Task<string> CheckAsync()
    {
        string message;
        try
        {
            var client = m_rpcClientFactory.ConnectionRpcClient();
            var response = client.CheckServerConnection(new G_ConnectCheckRequest());
            m_clientConfiguration.IsConnected = true;
            message = $"Online - {response.ServerTime.ToDateTime().ToUniversalTime():G}";
        } catch (RpcException e)
        {
            m_clientConfiguration.IsConnected = false;
            message = $"Offline - {DateTime.UtcNow.ToUniversalTime():G}";
        }

        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        m_logger.LogDebug(message: message);

        return Task.FromResult(message);
    }
    
    
}