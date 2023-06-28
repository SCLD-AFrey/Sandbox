using ReactiveUI;

namespace VikingEnterprise.WpfClient.ViewModels;

public class ViewModelBase : ReactiveObject
{
    public ViewModelBase(string p_pageTitle)
    {
        PageTitle = p_pageTitle;
    }

    public string PageTitle { get; set; }
}