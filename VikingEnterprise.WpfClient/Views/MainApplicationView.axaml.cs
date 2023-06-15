using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VikingEnterprise.WpfClient.ViewModels;

namespace VikingEnterprise.WpfClient.Views;

public partial class MainApplicationView : UserControl
{
#pragma warning disable CS8618
    public MainApplicationView()
    {
    }
#pragma warning restore CS8618
    public MainApplicationView(MainApplicationViewModel p_viewModel)
    {
        DataContext = p_viewModel;
        InitializeComponent();
    }


    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}