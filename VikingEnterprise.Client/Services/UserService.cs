using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarfBuzzSharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VikingEnterprise.Client.Models;
using VikingEnterprise.Client.ViewModels;
using VikingEnterprise.Client.ViewModels.MainApplication;
using VikingEnterprise.Client.Views.MainApplication;
using VikingEntity.Server.Protos.UserManager;

namespace VikingEnterprise.Client.Services;

public class UserService : IUserService
{
    private readonly ClientConfiguration m_clientConfiguration;
    private readonly ILogger<ServerConnectionService> m_logger;
    private readonly RpcClientFactory m_rpcClientFactory;
    private readonly UserCredential m_userCredential;
    private readonly IServiceProvider m_serviceProvider;

    public UserService(ClientConfiguration p_clientConfiguration, ILogger<ServerConnectionService> p_logger, RpcClientFactory p_rpcClientFactory, UserCredential p_userCredential, IServiceProvider p_serviceProvider)
    {
        m_clientConfiguration = p_clientConfiguration;
        m_logger = p_logger;
        m_rpcClientFactory = p_rpcClientFactory;
        m_userCredential = p_userCredential;
        m_serviceProvider = p_serviceProvider;
        UserCredentialRepo = GetUsers(out string message).Result;
    }
    
    public List<UserCredential> UserCredentialRepo { get; set; } = new();

    public Task LoginUser(out string p_message)
    {
        p_message = string.Empty;
        var client = m_rpcClientFactory.UserManagerRpcClient();
        var response = client.Login(new G_LoginRequest
        {
            Username = m_userCredential.Username,
            Password = m_userCredential.Password
        });

        if (response.ResultCase == G_LoginResponse.ResultOneofCase.Success)
        {
            m_userCredential.IsLoggedIn = true;
            m_userCredential.Oid = response.Success.User.Oid; 
            m_userCredential.Username = response.Success.User.Username;
        }
        else
        {
            p_message = response.Failure.Message;
            m_userCredential.Reset();
        }

        return Task.CompletedTask;
    }

    public Task<UserCredential> GetUser(int p_oid, out string p_message)
    {
        p_message = string.Empty;
        var user = new UserCredential();
        var client = m_rpcClientFactory.UserManagerRpcClient();
        var response = client.GetUser(new G_GetUserRequest());
        if (response.ResultCase == G_GetUserResponse.ResultOneofCase.Success)
        {
            user.Oid = response.Success.User.Oid;
            user.Username = response.Success.User.Username;
            user.IsActive = response.Success.User.IsActive;
        }
        else
        {
            p_message = response.Failure.Message;
        }

        return Task.FromResult(user);
    }

    public Task<List<UserCredential>> GetUsers(out string p_message)
    {
        p_message = string.Empty;
        List<UserCredential> users = new();
        var client = m_rpcClientFactory.UserManagerRpcClient();
        var response = client.GetUsers(new G_GetUsersRequest());
        if (response.ResultCase == G_GetUsersResponse.ResultOneofCase.Success)
        {
            users.AddRange(
                response.Success.Users.Select(
                        p_u => new UserCredential()
                        {
                            Oid = p_u.Oid, 
                            Username = p_u.Username, 
                            IsActive = p_u.IsActive
                        }
                    ).OrderBy(p_x => p_x.Username)
                );
        }
        else
        {
            p_message = response.Failure.Message;
        }
        
        users.Where(p_x => p_x.Oid == m_userCredential.Oid).ToList().ForEach(p_x => p_x.IsLoggedIn = true);

        return Task.FromResult(users);
    }

    public Task<UserCredential> CreateUser(UserCredential p_userCredential, out string p_message)
    {
        p_message = string.Empty;
        var client = m_rpcClientFactory.UserManagerRpcClient();
        var response = client.CreateUser(new G_CreateUserRequest());
        if (response.ResultCase == G_CreateUserResponse.ResultOneofCase.Success)
        {
            p_userCredential.Oid = response.Success.User.Oid;
            p_userCredential.Username = response.Success.User.Username;
            p_userCredential.IsActive = response.Success.User.IsActive;
        }

        return Task.FromResult(p_userCredential);
    }

    public Task<UserCredential> ModifyUser(UserCredential p_userCredential, out string p_message)
    {
        p_message = string.Empty;
        var client = m_rpcClientFactory.UserManagerRpcClient();
        var response = client.ModifyUser(new G_ModifyUserRequest());
        if (response.ResultCase == G_ModifyUserResponse.ResultOneofCase.Success)
        {
            p_userCredential.Oid = response.Success.User.Oid;
            p_userCredential.Username = response.Success.User.Username;
            p_userCredential.IsActive = response.Success.User.IsActive;
        }
        if(m_userCredential.Oid == p_userCredential.Oid)
        {
            m_userCredential.IsLoggedIn = p_userCredential.IsLoggedIn;
        }
        
        return Task.FromResult(p_userCredential);
    }
}