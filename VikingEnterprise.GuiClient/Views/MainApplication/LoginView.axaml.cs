using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace VikingEnterprise.GuiClient.Views.MainApplication;

public partial class LoginView : UserControl
{
#pragma warning disable CS8618
    public LoginView()
    {
    }
#pragma warning restore CS8618
    public LoginView(LoginViewModel p_viewModel)
    {
        DataContext = p_viewModel;
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

public class LoginViewModel
{
}