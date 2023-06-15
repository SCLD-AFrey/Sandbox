using Microsoft.Extensions.Logging;

namespace VikingEnterprise.WpfClient.ViewModels;

public class WelcomeViewModel : ViewModelBase
{
    private readonly ILogger<WelcomeViewModel> m_logger;

    public WelcomeViewModel(ILogger<WelcomeViewModel> p_logger)
    {
        m_logger = p_logger;
    }
}