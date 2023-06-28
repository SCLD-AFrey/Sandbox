﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VikingEnterprise.GuiClient.ViewModels.MainApplication;

namespace VikingEnterprise.GuiClient.Views.MainApplication;

public partial class HomeView : UserControl
{
#pragma warning disable CS8618
    public HomeView()
    {
    }
#pragma warning restore CS8618
    public HomeView(HomeViewModel p_viewModel)
    {
        DataContext = p_viewModel;
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}