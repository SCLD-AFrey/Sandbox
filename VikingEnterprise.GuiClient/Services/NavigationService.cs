using System;
using Microsoft.Extensions.Logging;
using VikingEnterprise.GuiClient.Models.Enumerations;
using VikingEnterprise.GuiClient.Models.Global;

namespace VikingEnterprise.GuiClient.Services;

public class NavigationService
{
    
    private readonly ILogger<SettingsService> m_logger;
    private readonly CommonFiles m_commonFiles;
    private readonly UserService m_userService;

    public NavigationService(ILogger<SettingsService> p_logger, CommonFiles p_commonFiles, UserService p_userService)
    {
        m_logger = p_logger;
        m_commonFiles = p_commonFiles;
        m_userService = p_userService;
        m_logger.LogInformation("NavigationService created"); 
    }
    
    public int SetNavigationIndex(NavigationTarget p_navigationTarget)
    {

        if(m_userService.GetCurrentUser()!.Oid == 0 && p_navigationTarget != NavigationTarget.Login)
            return NavigateToLoginScreen();
        return p_navigationTarget switch
        {
            NavigationTarget.Login => NavigateToLoginScreen(),
            NavigationTarget.Home => NavigateToHomeScreen(),
            NavigationTarget.UserManagement => NavigateToUserManagementScreen(),
            _ => m_userService.GetCurrentUser()!.Oid > 0 
                ? NavigateToHomeScreen() 
                : NavigateToLoginScreen()
        };
    }
    private int NavigateToLoginScreen()
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