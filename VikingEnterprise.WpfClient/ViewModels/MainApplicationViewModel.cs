using Microsoft.Extensions.Logging;
using VikingEnterprise.WpfClient.ViewModels.Workspace;

namespace VikingEnterprise.WpfClient.ViewModels;

public class MainApplicationViewModel : ViewModelBase
{
    private readonly ILogger<MainWorkspaceViewModel> m_logger;

    public MainApplicationViewModel(ILogger<MainWorkspaceViewModel> p_logger)
    {
        m_logger = p_logger;
    }
}