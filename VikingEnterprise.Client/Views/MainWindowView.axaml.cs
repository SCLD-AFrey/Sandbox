using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VikingEnterprise.Client.Models;
using VikingEnterprise.Client.Services;
using VikingEnterprise.Client.ViewModels;

namespace VikingEnterprise.Client.Views;

public partial class MainWindowView : Window
{
#pragma warning disable CS8618
    public MainWindowView()
    {
    }
#pragma warning restore CS8618
    
    private readonly ServerConnectionService m_ServerConnectionService;
    public MainWindowView(MainWindowViewModel p_viewModel, MainWindowModel p_model, ServerConnectionService p_ServerConnectionService)
    {
        DataContext = p_viewModel;
        Model       = p_model;
        m_ServerConnectionService = p_ServerConnectionService;
        InitializeComponent();

    }
    
    public MainWindowModel Model { get; set; }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
}