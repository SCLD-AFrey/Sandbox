using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VikingEnterprise.Client.Models;
using VikingEnterprise.Client.ViewModels;

namespace VikingEnterprise.Client.Views;

public partial class MainWindowView : Window
{
#pragma warning disable CS8618
    public MainWindowView()
    {
    }
#pragma warning restore CS8618
    
    private readonly ServerConnection m_serverConnection;
    public MainWindowView(MainWindowViewModel p_viewModel, MainWindowModel p_model, ServerConnection p_serverConnection)
    {
        DataContext = p_viewModel;
        Model       = p_model;
        m_serverConnection = p_serverConnection;
        InitializeComponent();

    }
    
    public MainWindowModel Model { get; set; }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
}