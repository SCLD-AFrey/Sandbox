using VikingEnterprise.Client.Models.Enumerations;
using VikingEnterprise.Client.Services;
using VikingEnterprise.Client.ViewModels.MainApplication;

namespace VikingEnterprise.Client.Models;

public class MainWindowModel
{
    private readonly NavigationService m_navigationService;
    private readonly MainApplicationViewModel m_mainApplicationViewModel;

    public MainWindowModel(NavigationService p_navigationService, MainApplicationViewModel p_mainApplicationViewModel)
    {
        m_navigationService = p_navigationService;
        m_mainApplicationViewModel = p_mainApplicationViewModel;
    }

    public void SetNavigation(NavigationTarget p_parameter)
    {
        m_navigationService.SetNavigation(p_parameter);
    }
}