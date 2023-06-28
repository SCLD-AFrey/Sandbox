using Microsoft.Extensions.Logging;

namespace VikingEnterprise.GuiClient.ViewModels.MainApplication;

public class UserViewModel : ViewModelBase
{
    private readonly ILogger<UserViewModel> m_logger;

    public UserViewModel(ILogger<UserViewModel> p_logger)
    {
        m_logger = p_logger;
        m_logger.LogDebug("UserViewModel created");
    }
}