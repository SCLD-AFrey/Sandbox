using System;
using System.Threading.Tasks;
using Avalonia.Collections;
using Microsoft.Extensions.Logging;
using ReactiveUI.Fody.Helpers;
using VikingEnterprise.Client.Global.FileStructure;
using VikingEnterprise.Client.Models;
using VikingEnterprise.Client.Models.Enumerations;
using VikingEnterprise.Client.Models.Logging;
using VikingEnterprise.Client.Services;
using VikingEnterprise.Client.Views.MainApplication;

namespace VikingEnterprise.Client.ViewModels;

public class MainWindowViewModel : ViewModelBase
{

    private readonly UserCredential m_userCredential;
    private readonly ILogger<MainWindowViewModel> m_logger;
    private readonly CommonFiles m_commonFiles;
    private readonly CommonFiles m_files;
    private readonly ServerConnectionService m_ServerConnectionService;
    private readonly IUserService m_userService;

    public MainWindowViewModel(ILogger<MainWindowViewModel> p_logger, 
        CommonFiles p_commonFiles, 
        CommonFiles p_files, 
        ServerConnectionService p_ServerConnectionService, 
        UserCredential p_userCredential, 
        IUserService p_userService, 
        NavigationService p_navigationService,
        MainWindowModel p_mainWindowModel)
    {
        m_logger = p_logger;
        m_commonFiles = p_commonFiles;
        m_files = p_files;
        m_ServerConnectionService = p_ServerConnectionService;
        m_userCredential = p_userCredential;
        m_userService = p_userService;
        NavigationService = p_navigationService;
        Model = p_mainWindowModel;

        m_logger.LogDebug("Initializing MainWindowViewModel");

        CollectionSink.SetCollection(Messages);
        IsConnected = false;
        TitleMessage = "Viking Enterprise";
        OnlineMessage = m_ServerConnectionService.CheckAsync().Result;
        Model.SetNavigation(NavigationTarget.Welcome);
    }
    [Reactive] public AvaloniaList<ConsoleLogMessage>   Messages          { get; set; } = new ();
    [Reactive] public string TitleMessage { get; set; }
    [Reactive] public string OnlineMessage { get; set; }
    [Reactive] public bool IsConnected { get; set; }
    [Reactive] public string UserName { get; set; } = string.Empty;
    [Reactive] public string UserPassword { get; set; } = string.Empty;
    [Reactive] public bool ShowLogin { get; set; } = true;
    [Reactive] public string StatusDisplay { get; set; } = string.Empty;
    [Reactive] public NavigationService NavigationService { get; set; }
    [Reactive] public int SelectedPageIndex { get; set; } = 0;
    public MainWindowModel Model { get; set; }

    public void OnNavigationClick(object p_parameter)
    {
        if ( p_parameter is not string stringParameter ) return;
        if ( !Enum.TryParse(stringParameter, true, out NavigationTarget navigationTarget) ) return;
        Model.SetNavigation(navigationTarget);
    }

    public async Task OnConnectClick()
    {
        var tempMessage = StatusDisplay;
        StatusDisplay = "Connecting...";
        OnlineMessage = await m_ServerConnectionService.CheckAsync();
        StatusDisplay = tempMessage;
    }

    public void OnLoginClick()
    {
        var tempMessage = StatusDisplay;
        StatusDisplay = $"Logging in {UserName!}...";
        m_userCredential.Username = UserName;
        m_userCredential.Password = UserPassword;
        m_userService.LoginUser(out var status);
        StatusDisplay = status;
        ShowLogin = m_userCredential.Oid <= 0;
        if (m_userCredential.Oid > 0) Model.SetNavigation(NavigationTarget.Home);
    }

    public void OnLogoutClick()
    {
        m_userCredential.Reset();
        ShowLogin = m_userCredential.Oid <= 0;
        Model.SetNavigation(NavigationTarget.Welcome);
    }

}