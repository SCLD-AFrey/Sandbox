using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VikingEnterprise.WpfClient.Models;
using VikingEnterprise.WpfClient.Models.Enumerations;
using VikingEnterprise.WpfClient.Models.Global;
using VikingEnterprise.WpfClient.ViewModels;
using VikingEnterprise.WpfClient.ViewModels.Workspace;
using VikingEnterprise.WpfClient.Views;

namespace VikingEnterprise.WpfClient.Services;

public class NavigationService
{
    private readonly ILogger<NavigationService> m_logger;
    private readonly ClientConfiguration m_clientConfiguration;
    private readonly IServiceProvider m_serviceProvider;
    private readonly UserService m_userService;

    public NavigationService(ClientConfiguration p_clientConfiguration, IServiceProvider p_serviceProvider, UserService p_userService, ILogger<NavigationService> p_logger)
    {
        m_clientConfiguration = p_clientConfiguration;
        m_serviceProvider = p_serviceProvider;
        m_userService = p_userService;
        m_logger = p_logger;
        MainApplicationView = m_serviceProvider.GetService<MainApplicationView>();
        UserManagementView = m_serviceProvider.GetService<UserManagementView>();
        WelcomeView = m_serviceProvider.GetService<WelcomeView>();
    }
    public WelcomeView? WelcomeView { get; set; }
    public MainApplicationView? MainApplicationView { get; set; }
    public UserManagementView? UserManagementView { get; set; }
    
    public int SetNavigation(NavigationTarget p_navigationTarget)
    {
        try
        {
            if(m_userService.CurrentUserCred.Oid == 0 && p_navigationTarget != NavigationTarget.Welcome)
                return NavigateToWelcomeScreen();
            return p_navigationTarget switch
            {
                NavigationTarget.Welcome => NavigateToWelcomeScreen(),
                NavigationTarget.Home => NavigateToHomeScreen(),
                NavigationTarget.UserManagement => NavigateToUserManagementScreen(),
                _ => m_userService.CurrentUserCred.Oid > 0 
                    ? NavigateToHomeScreen() 
                    : NavigateToWelcomeScreen()
            };
        } catch (Exception e)
        {
            m_logger.LogError(e, "Error setting navigation NavigationTarget = {NavigationTarget}", p_navigationTarget);
            return NavigateToWelcomeScreen();
        }

    }

    private int NavigateToWelcomeScreen()
    {
        return 0;
    }

    private int NavigateToHomeScreen()
    {
        return 1;
    }

    private int NavigateToUserManagementScreen()
    {
        return 2;
    }
}