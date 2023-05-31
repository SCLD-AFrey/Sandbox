using Microsoft.Extensions.Logging;

namespace VikingEnterprise.Client.ViewModels.MainApplication;

public class MainApplicationViewModel : ViewModelBase
{
    private readonly ILogger<MainWindowViewModel> m_logger;

    public MainApplicationViewModel(ILogger<MainWindowViewModel> p_logger)
    {
        m_logger = p_logger;
        m_logger.LogDebug("Initializing MainApplicationViewModel");
    }
}