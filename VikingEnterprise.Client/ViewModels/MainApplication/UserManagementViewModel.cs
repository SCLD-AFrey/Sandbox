using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using ReactiveUI.Fody.Helpers;
using VikingEnterprise.Client.Models;
using VikingEnterprise.Client.Services;

namespace VikingEnterprise.Client.ViewModels.MainApplication;

public class UserManagementViewModel : ViewModelBase
{
    private readonly ServerConnectionService m_ServerConnectionService;

    public UserManagementViewModel(ILogger<UserManagementViewModel> p_logger, IUserService p_userService, ServerConnectionService p_ServerConnectionService)
    {
        UserService = p_userService;
        m_ServerConnectionService = p_ServerConnectionService;
        p_logger.LogDebug("Initializing UserManagementViewModel");
    }
    [Reactive] public string PageHeaderText { get; set; } = "User Management";
    public IUserService UserService { get; set; }
}