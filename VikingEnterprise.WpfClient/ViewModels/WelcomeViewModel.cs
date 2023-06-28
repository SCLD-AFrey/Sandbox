using Microsoft.Extensions.Logging;
using VikingEnterprise.WpfClient.ViewModels.Workspace;

namespace VikingEnterprise.WpfClient.ViewModels;

public class WelcomeViewModel : ViewModelBase
{
    private readonly ILogger<WelcomeViewModel> m_logger;

    public WelcomeViewModel(ILogger<WelcomeViewModel> p_logger) : base("Welcome")
    {
        m_logger = p_logger;
    }
}