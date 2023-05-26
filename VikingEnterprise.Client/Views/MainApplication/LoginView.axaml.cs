using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VikingEnterprise.Client.Models.MainApplication;
using VikingEnterprise.Client.ViewModels.MainApplication;

namespace VikingEnterprise.Client.Views.MainApplication;

public partial class LoginView : UserControl
{
#pragma warning disable CS8618
    public LoginView()
    {
    }
#pragma warning restore CS8618
    public LoginView(LoginViewModel p_viewModel, LoginModel p_model)
    {
        DataContext = p_viewModel;
        Model       = p_model;
        InitializeComponent();
    }
    public LoginModel Model { get; set; }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}