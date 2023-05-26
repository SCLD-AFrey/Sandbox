using Microsoft.Extensions.Logging;

namespace VikingEnterprise.Client.ViewModels.MainApplication;

public class LoginViewModel : ViewModelBase
{
    private readonly ILogger<MainWindowViewModel> m_logger;

    public LoginViewModel(ILogger<MainWindowViewModel> p_logger)
    {
        m_logger = p_logger;
    }
}