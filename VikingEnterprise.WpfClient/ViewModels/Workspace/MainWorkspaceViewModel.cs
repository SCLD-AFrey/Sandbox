using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReactiveUI.Fody.Helpers;
using Serilog;
using VikingEnterprise.WpfClient.Models.Global;
using VikingEnterprise.WpfClient.Services;

namespace VikingEnterprise.WpfClient.ViewModels.Workspace;

public class MainWorkspaceViewModel : ViewModelBase
{
    private readonly ILogger<MainWorkspaceViewModel> m_logger;
    private readonly RpcClientService m_rpcClientService;
    private readonly UserService m_userService;

    public MainWorkspaceViewModel(ILogger<MainWorkspaceViewModel> p_logger, RpcClientService p_rpcClientService, UserService p_userService, NavigationService p_navigationService)
    {
        m_logger = p_logger;
        m_rpcClientService = p_rpcClientService;
        m_userService = p_userService;
        NavigationService = p_navigationService;
        ApplicationTitle = "Viking Enterprise";
        PageTitle = "Welcome";
        OnlineStatus = "Offline";
        IsConnected = false;
        IsLoggedIn = false;
        CheckConnection();
        SelectedPageIndex = m_userService.CurrentUserCred.Oid == 0 ? 0 : 1;
    }
    
    [Reactive] public string ApplicationTitle { get; set; }
    [Reactive] public string PageTitle { get; set; }
    [Reactive] public string OnlineStatus { get; set; }
    [Reactive] public string StatusMessage { get; set; }
    [Reactive] public bool IsConnected { get; set; }
    [Reactive] public string UserName { get; set; }
    [Reactive] public string UserPassword { get; set; }
    [Reactive] public bool IsLoggedIn { get; set; }
    public ClientConfiguration ClientConfiguration { get; set; }
    public NavigationService NavigationService { get; set; }
    [Reactive] public int SelectedPageIndex { get; set; }

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
    }

    public void OnLogoutClick()
    {
        IsLoggedIn = false;
        m_userService.CurrentUserCred.Reset();
    }

}