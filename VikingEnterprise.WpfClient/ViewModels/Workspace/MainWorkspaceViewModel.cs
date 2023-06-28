using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReactiveUI.Fody.Helpers;
using Serilog;
using VikingEnterprise.WpfClient.Models.Enumerations;
using VikingEnterprise.WpfClient.Models.Global;
using VikingEnterprise.WpfClient.Services;
using VikingEnterprise.WpfClient.Views;

namespace VikingEnterprise.WpfClient.ViewModels.Workspace;

public class MainWorkspaceViewModel : ViewModelBase
{
    private readonly ILogger<MainWorkspaceViewModel> m_logger;
    private readonly RpcClientService m_rpcClientService;
    private readonly UserService m_userService;
    private readonly SettingsService m_settingsService;

    public MainWorkspaceViewModel(ILogger<MainWorkspaceViewModel> p_logger, 
        RpcClientService p_rpcClientService, UserService p_userService, NavigationService p_navigationService, SettingsService p_settingsService) : base("Main Workspace")
    {
        m_logger = p_logger;
        m_rpcClientService = p_rpcClientService;
        m_userService = p_userService;
        NavigationService = p_navigationService;
        m_settingsService = p_settingsService;
        ApplicationTitle = "Viking Enterprise";
        PageTitle = "Welcome";
        OnlineStatus = "Offline";
        IsConnected = false;
        IsLoggedIn = false;
        CheckConnection();
        SelectedPageIndex = NavigationService.SetNavigation(IsLoggedIn ? NavigationTarget.Home : NavigationTarget.Welcome);

        if (m_settingsService.ClientConfig.RequireLogin == false &&
            m_settingsService.ClientConfig.UserCredential.Oid > 0)
        {
            m_userService.SetCurrentUser(m_userService.GetUser(m_settingsService.ClientConfig.UserCredential.Oid));
        }
    }
    
    [Reactive] public string ApplicationTitle { get; set; }
    [Reactive] public string OnlineStatus { get; set; }
    [Reactive] public string StatusMessage { get; set; } 
    [Reactive] public bool IsConnected { get; set; }
    [Reactive] public string UserName { get; set; }
    [Reactive] public string UserPassword { get; set; }
    [Reactive] public bool IsLoggedIn { get; set; }
    public ClientConfiguration ClientConfiguration { get; set; }
    public NavigationService NavigationService { get; set; }
    [Reactive] public int SelectedPageIndex { get; set; }
    [Reactive] public bool RequireLogin { get; set; }

    private void CheckConnection()
    {
        IsConnected = m_rpcClientService.CheckConnectionAsync().Result;
        if (IsConnected)
        {
            OnlineStatus = "Online";
        }
    }

    public async void OnLoginClick()
    {
        var response = await m_userService.LoginAsync(UserName, UserPassword);
        IsLoggedIn = response;
        m_settingsService.ClientConfig.RequireLogin = RequireLogin;
        m_settingsService.ClientConfig.UserCredential = m_userService.CurrentUserCred;
        await m_settingsService.SaveSettings();
        SelectedPageIndex = NavigationService.SetNavigation(IsLoggedIn ? NavigationTarget.Home : NavigationTarget.Welcome);
    }

    public async void OnLogoutClick()
    {
        IsLoggedIn = false;
        m_userService.CurrentUserCred.Reset();
        m_settingsService.ClientConfig.RequireLogin = true;
        m_settingsService.ClientConfig.UserCredential.Reset();
        await m_settingsService.SaveSettings();
        SelectedPageIndex = NavigationService.SetNavigation(NavigationTarget.Welcome);
    }

    public void OnNavClick(string p_view)
    {
        SelectedPageIndex = p_view switch
        {
            "Home" => NavigationService.SetNavigation(NavigationTarget.Home),
            "UserManagement" => NavigationService.SetNavigation(NavigationTarget.UserManagement),
            _ => NavigationService.SetNavigation(NavigationTarget.Home)
        };
    }

}