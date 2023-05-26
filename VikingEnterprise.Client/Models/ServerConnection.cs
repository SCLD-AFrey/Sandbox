using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using VikingEntity.Server.Protos.Connection;

namespace VikingEnterprise.Client.Models;

public class ServerConnection
{
    private readonly ClientConfiguration m_clientConfiguration;
    private ILogger<ServerConnection> m_logger;
    public ServerConnection(ClientConfiguration p_clientConfiguration, ILogger<ServerConnection> p_logger)
    {
        m_clientConfiguration = p_clientConfiguration;
        m_logger = p_logger;
        m_logger.LogDebug("Initializing ServerConnection");
    }

    public Connection.ConnectionClient ConnectionClient(ProvisionClientType p_provisionClientType)
    {
        var channel = GrpcChannel.ForAddress($"https://{m_clientConfiguration.ServerAddress}:{m_clientConfiguration.Port}");
        
        var client = p_provisionClientType switch
        {
            ProvisionClientType.Connection => new Connection.ConnectionClient(channel),
            _ => throw new ArgumentOutOfRangeException(nameof(p_provisionClientType), p_provisionClientType, null)
        };
        
        return client;
    }
    public string CheckServerConnection()
    {
        var message = string.Empty;
        try
        {
            var client = ConnectionClient(ServerConnection.ProvisionClientType.Connection);
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

        return message;
    }
    
    public enum ProvisionClientType
    {
        Connection
    }
    
    
}