using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VikingEnterprise.WpfClient.ViewModels;

namespace VikingEnterprise.WpfClient.Views;

public partial class WelcomeView : UserControl
{
#pragma warning disable CS8618
    public WelcomeView()
    {
    }
#pragma warning restore CS8618
    public WelcomeView(WelcomeViewModel p_viewModel)
    {
        DataContext = p_viewModel;
        InitializeComponent();
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}