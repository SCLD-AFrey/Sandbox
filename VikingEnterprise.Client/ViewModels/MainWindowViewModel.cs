using System.Threading.Tasks;
using Avalonia.Collections;
using Microsoft.Extensions.Logging;
using ReactiveUI.Fody.Helpers;
using VikingEnterprise.Client.Global.FileStructure;
using VikingEnterprise.Client.Models;
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
    private readonly ServerConnection m_serverConnection;
    private readonly IUserService m_userService;

    public MainWindowViewModel(ILogger<MainWindowViewModel> p_logger, 
        CommonFiles p_commonFiles, 
        CommonFiles p_files, 
        ServerConnection p_serverConnection, 
        UserCredential p_userCredential, 
        IUserService p_userService, 
        MainApplicationView p_mainApplicationView,
        UserManagementView p_userManagementView)
    {
        m_logger = p_logger;
        m_commonFiles = p_commonFiles;
        m_files = p_files;
        m_serverConnection = p_serverConnection;
        m_userCredential = p_userCredential;
        m_userService = p_userService;
        MainApplicationView = p_mainApplicationView;
        UserManagementView = p_userManagementView;

        m_logger.LogDebug("Initializing MainWindowViewModel");

        CollectionSink.SetCollection(Messages);
        IsConnected = false;
        TitleMessage = "Viking Enterprise";
        OnlineMessage = m_serverConnection.CheckAsync().Result;
    }
    [Reactive] public AvaloniaList<ConsoleLogMessage>   Messages          { get; set; } = new ();
    [Reactive] public string TitleMessage { get; set; }
    [Reactive] public string OnlineMessage { get; set; }
    [Reactive] public bool IsConnected { get; set; }
    [Reactive] public string UserName { get; set; } = string.Empty;
    [Reactive] public string UserPassword { get; set; } = string.Empty;
    [Reactive] public bool ShowLogin { get; set; } = true;
    [Reactive] public string StatusDisplay { get; set; } = string.Empty;
    [Reactive] public int SelectedPageIndex { get; set; } = 1;
    public MainApplicationView MainApplicationView { get; set; }
    public UserManagementView UserManagementView { get; set; }
    
    

    public async Task OnConnectClick()
    {
        StatusDisplay = "Connecting...";
        OnlineMessage = await m_serverConnection.CheckAsync();
        StatusDisplay = "";
    }

    public void OnLoginClick()
    {
        StatusDisplay = "Logging in...";
        m_userCredential.Username = UserName;
        m_userCredential.Password = UserPassword;
        m_userService.LoginUser(out var status);
        StatusDisplay = status;
        ShowLogin = m_userCredential.Oid <= 0;
        StatusDisplay = "";
    }

    public void OnLogoutClick()
    {
        m_userCredential.Reset();
        ShowLogin = m_userCredential.Oid <= 0;
    }

}