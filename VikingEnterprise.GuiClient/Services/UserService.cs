using Microsoft.Extensions.Logging;
using VikingEnterprise.GuiClient.Models.Global;

namespace VikingEnterprise.GuiClient.Services;

public class UserService
{
    
    private readonly ILogger<SettingsService> m_logger;
    private readonly CommonFiles m_commonFiles;

    public UserService(ILogger<SettingsService> p_logger, CommonFiles p_commonFiles)
    {
        m_logger = p_logger;
        m_commonFiles = p_commonFiles;
        m_logger.LogInformation("UserService created");
        CurrentUser = new UserCredential();
    }

    private UserCredential? CurrentUser { get; set; }

    public UserCredential? GetCurrentUser()
    {
        return CurrentUser;
    }
    
    public void SetCurrentUser(UserCredential p_userCredential)
    {
        CurrentUser = p_userCredential;
    }
}