using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VikingEnterprise.Client.Models.MainApplication;
using VikingEnterprise.Client.ViewModels.MainApplication;

namespace VikingEnterprise.Client.Views.MainApplication;

public partial class UserManagementView : UserControl
{
#pragma warning disable CS8618
    public UserManagementView()
    {
    }
#pragma warning restore CS8618
    public UserManagementView(UserManagementViewModel p_viewModel, UserManagementModel p_model)
    {
        DataContext = p_viewModel;
        Model       = p_model;
        InitializeComponent();
    }
    public UserManagementModel Model { get; set; }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}