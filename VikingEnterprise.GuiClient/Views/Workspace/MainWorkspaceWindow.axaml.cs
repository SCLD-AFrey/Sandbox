using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VikingEnterprise.GuiClient.ViewModels.Workspace;

namespace VikingEnterprise.GuiClient.Views.Workspace;

public partial class MainWorkspaceWindow : Window
{
#pragma warning disable CS8618
    public MainWorkspaceWindow()
    {
    }
#pragma warning restore CS8618
    public MainWorkspaceWindow(MainWorkspaceViewModel p_viewModel)
    {
        DataContext = p_viewModel;
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}