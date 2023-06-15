using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VikingEnterprise.WpfClient.ViewModels;
using VikingEnterprise.WpfClient.ViewModels.Workspace;

namespace VikingEnterprise.WpfClient.Views.Workspace;

public partial class MainWorkspaceView : Window
{
#pragma warning disable CS8618
    public MainWorkspaceView()
    {
    }
#pragma warning restore CS8618
    public MainWorkspaceView(MainWorkspaceViewModel p_viewModel)
    {
        DataContext = p_viewModel;
        InitializeComponent();
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}