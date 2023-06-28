using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using VikingEnterprise.WpfClient.Models;
using VikingEntity.Server.Protos.UserManager;
using ILogger = Serilog.ILogger;

namespace VikingEnterprise.WpfClient.Services;

public class UserService
{
    private readonly ILogger<UserService> m_logger;
    private readonly RpcClientService m_rpcClientService;

    public UserService(ILogger<UserService> p_logger, RpcClientService p_rpcClientService)
    {
        m_logger = p_logger;
        m_rpcClientService = p_rpcClientService;
    }

    public UserCredential GetUser(string p_username)
    {
        var client = m_rpcClientService.UserManagerRpcClient();
        var reply = client.GetUser(new G_GetUserRequest()
        {
            ByUsername = new G_GetUserRequest_Username()
            {
                Username = p_username
            }
        });
        return ParseReply(reply);
    }
    public UserCredential GetUser(int p_oid)
    {
        var client = m_rpcClientService.UserManagerRpcClient();
        var reply = client.GetUser(new G_GetUserRequest()
        {
            ByOid = new G_GetUserRequest_Oid()
            {
                Oid = p_oid
            }
        });
        return ParseReply(reply);
    }

    private UserCredential ParseReply(G_GetUserResponse p_response)
    {
        switch (p_response.ResultCase)
        {
            case G_GetUserResponse.ResultOneofCase.Success:
                return new UserCredential()
                {
                    Oid = p_response.Success.User.Oid,
                    Username = p_response.Success.User.Username,
                    LastLogin = DateTime.UtcNow,
                };
                break;
            case G_GetUserResponse.ResultOneofCase.Failure:
                return new UserCredential();
                break;
            case G_GetUserResponse.ResultOneofCase.None:
            default:
                return new UserCredential();
        }
    }
    
    public UserCredential CurrentUserCred { get; set; } = new ();
    
    public void SetCurrentUser(UserCredential p_userCredential)
    {
        CurrentUserCred = p_userCredential;
    }
    
    public async Task<bool> LoginAsync(string p_userName, string p_userPassword)
    {
        try
        {
            CurrentUserCred.Reset();

            if (string.IsNullOrEmpty(p_userName) || string.IsNullOrEmpty(p_userPassword))
            {
                m_logger.LogInformation("Username and Password are required");
                return await Task.FromResult(false);
            }
            
            CurrentUserCred.Username = p_userName;
     
            if (m_rpcClientService.CheckConnectionAsync().Result == false)
            {
                m_logger.LogInformation("Client not connected for Login");
                return await Task.FromResult(false);
            }
        
        
            var client = m_rpcClientService.UserManagerRpcClient();
            var response = await client.LoginAsync(new G_LoginRequest
            {
                Username = p_userName,
                Password = p_userPassword
            });
        
            if(response.ResultCase == G_LoginResponse.ResultOneofCase.Failure)
            {
                m_logger.LogInformation("User {Username} failed - '{Message}'", p_userName, response.Failure.Message);
                return await Task.FromResult(false);
            }
        
            if(response.Success.User.IsActive == false)
            {
                m_logger.LogInformation("User {Username} is not active", p_userName);
                return await Task.FromResult(false);
            }

            CurrentUserCred = new UserCredential()
            {
                Oid = response.Success.User.Oid,
                Username = response.Success.User.Username,
                LastLogin = DateTime.UtcNow,
            };

            return await Task.FromResult(true);
        } catch (Exception e)
        {
            m_logger.LogError(e, "Error in LoginAsync");
            CurrentUserCred.Reset();
            return await Task.FromResult(false);
        }

    }
}