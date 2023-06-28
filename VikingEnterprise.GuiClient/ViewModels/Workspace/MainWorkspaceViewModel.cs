using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReactiveUI.Fody.Helpers;
using Serilog;
using VikingEnterprise.GuiClient.Models.Enumerations;
using VikingEnterprise.GuiClient.Models.Global;
using VikingEnterprise.GuiClient.Services;
using VikingEnterprise.GuiClient.Views.MainApplication;

namespace VikingEnterprise.GuiClient.ViewModels.Workspace;

public class MainWorkspaceViewModel : ViewModelBase
{
    private readonly ILogger<MainWorkspaceViewModel> m_logger;
    private readonly SettingsService m_settingsService;
    private readonly UserService m_userService;
    private readonly NavigationService m_navigationService;
    public MainWorkspaceViewModel(ILogger<MainWorkspaceViewModel> p_logger, 
        LoginView p_loginView, 
        HomeView p_homeView, 
        UserView p_userView, 
        SettingsService p_settingsService, 
        UserService p_userService, NavigationService p_navigationService)
    {
        m_logger = p_logger;
        LoginView = p_loginView;
        HomeView = p_homeView;
        UserView = p_userView;
        m_settingsService = p_settingsService;
        m_userService = p_userService;
        m_navigationService = p_navigationService;
        m_logger.LogInformation("MainWorkspaceViewModel created");
        ClientConfiguration = m_settingsService.ClientConfiguration;
    }
    public string ApplicationTitle => "Viking Enterprise";
    
    [Reactive] public int SelectedPageIndex { get; set; }
    public ClientConfiguration ClientConfiguration { get; set; }
    
    public LoginView LoginView { get; }
    public HomeView HomeView { get; }
    public UserView UserView { get; }
    
    public void SetSelectedPageIndex(string p_pageName)
    {
        SelectedPageIndex =
            m_navigationService.SetNavigationIndex(Enum.TryParse(p_pageName, out NavigationTarget navTarget)
                ? navTarget
                : NavigationTarget.Login);
    }

    public async Task OnLogoutClick()
    {
        m_userService.SetCurrentUser(new UserCredential());
        await m_settingsService.SetCurrentUser(m_userService.GetCurrentUser()!);
        await m_settingsService.SaveSettings();
        SetSelectedPageIndex(NavigationTarget.Login.ToString());
    }
}