using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VikingEnterprise.GuiClient.ViewModels.MainApplication;

namespace VikingEnterprise.GuiClient.Views.MainApplication;

public partial class UserView : UserControl
{
#pragma warning disable CS8618
    public UserView()
    {
    }
#pragma warning restore CS8618
    public UserView(UserViewModel p_viewModel)
    {
        DataContext = p_viewModel;
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}