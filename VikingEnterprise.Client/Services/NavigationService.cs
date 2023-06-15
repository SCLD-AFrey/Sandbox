using System;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI.Fody.Helpers;
using VikingEnterprise.Client.Models;
using VikingEnterprise.Client.Models.Enumerations;
using VikingEnterprise.Client.ViewModels;
using VikingEnterprise.Client.Views.MainApplication;

namespace VikingEnterprise.Client.Services;

public class NavigationService
{
    private readonly ClientConfiguration m_clientConfiguration;
    private readonly UserCredential m_userCredential;
    private readonly IServiceProvider m_serviceProvider;

    public NavigationService(ClientConfiguration p_clientConfiguration, UserCredential p_userCredential, IServiceProvider p_serviceProvider)
    {
        m_clientConfiguration = p_clientConfiguration;
        m_userCredential = p_userCredential;
        m_serviceProvider = p_serviceProvider;
        MainApplicationView = m_serviceProvider.GetRequiredService<MainApplicationView>();
        UserManagementView = m_serviceProvider.GetRequiredService<UserManagementView>();
        WelcomeView = m_serviceProvider.GetRequiredService<WelcomeView>();
    }
    
    public WelcomeView WelcomeView { get; set; }
    public MainApplicationView MainApplicationView { get; set; }
    public UserManagementView UserManagementView { get; set; }

    public void SetNavigation(NavigationTarget p_navigationTarget)
    {

        switch ( p_navigationTarget )
        {
            case NavigationTarget.Welcome:
                NavigateToWelcomeScreen();
                break;
            case NavigationTarget.Home:
                NavigateToHomeScreen();
                break;
            case NavigationTarget.UserManagement:
                NavigateToUserManagementScreen();
                break;
            default:
                NavigateToHomeScreen();
                break;
        }
    }

    private void NavigateToWelcomeScreen()
    {
        var mainWindowViewModel = m_serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainWindowViewModel.SelectedPageIndex = 1;
    }

    private void NavigateToHomeScreen()
    {
        var mainWindowViewModel = m_serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainWindowViewModel.SelectedPageIndex = 2;
    }

    private void NavigateToUserManagementScreen()
    {
        var mainWindowViewModel = m_serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainWindowViewModel.SelectedPageIndex = 3;
    }
}