using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VikingEnterprise.Client.Models.MainApplication;
using VikingEnterprise.Client.ViewModels.MainApplication;

namespace VikingEnterprise.Client.Views.MainApplication;

public partial class MainApplicationView : UserControl
{
#pragma warning disable CS8618
    public MainApplicationView()
    {
    }
#pragma warning restore CS8618
    public MainApplicationView(MainApplicationViewModel p_viewModel, MainApplicationModel p_model)
    {
        DataContext = p_viewModel;
        Model       = p_model;
        InitializeComponent();
    }
    public MainApplicationModel Model { get; set; }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}