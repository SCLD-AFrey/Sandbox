using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using ReactiveUI.Fody.Helpers;
using VikingEnterprise.Client.Models;
using VikingEnterprise.Client.Services;

namespace VikingEnterprise.Client.ViewModels.MainApplication;

public class UserManagementViewModel : ViewModelBase
{
    private readonly ServerConnection m_serverConnection;
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly IUserService m_userService;

    public UserManagementViewModel(ILogger<UserManagementViewModel> p_logger, IUserService p_userService, ServerConnection p_serverConnection)
    {
        m_userService = p_userService;
        m_serverConnection = p_serverConnection;
        p_logger.LogDebug("Initializing UserManagementViewModel");

        Users = m_userService.GetUsers(out var message).Result;

        SelectedUser = Users[0];
    }
    
    [Reactive] public List<UserCredential> Users { get; set; }
    [Reactive] public UserCredential SelectedUser { get; set; } = new UserCredential();
}