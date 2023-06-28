using Microsoft.Extensions.Logging;

namespace VikingEnterprise.GuiClient.ViewModels.MainApplication;

public class HomeViewModel : ViewModelBase
{
    private readonly ILogger<HomeViewModel> m_logger;

    public HomeViewModel(ILogger<HomeViewModel> p_logger)
    {
        m_logger = p_logger;
        m_logger.LogDebug("HomeViewModel created");
    }
}