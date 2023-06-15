using System;
using Microsoft.Extensions.DependencyInjection;
using VikingEnterprise.WpfClient.Models;
using VikingEnterprise.WpfClient.Models.Global;
using VikingEnterprise.WpfClient.Views;

namespace VikingEnterprise.WpfClient.Services;

public class NavigationService
{
    private readonly ClientConfiguration m_clientConfiguration;
    private readonly IServiceProvider m_serviceProvider;
    private readonly UserService m_userService;

    public NavigationService(ClientConfiguration p_clientConfiguration, IServiceProvider p_serviceProvider, UserService p_userService)
    {
        m_clientConfiguration = p_clientConfiguration;
        m_serviceProvider = p_serviceProvider;
        m_userService = p_userService;
        MainApplicationView = m_serviceProvider.GetRequiredService<MainApplicationView>();
        UserManagementView = m_serviceProvider.GetRequiredService<UserManagementView>();
        WelcomeView = m_serviceProvider.GetRequiredService<WelcomeView>();
    }
    public WelcomeView WelcomeView { get; set; }
    public MainApplicationView MainApplicationView { get; set; }
    public UserManagementView UserManagementView { get; set; }
}