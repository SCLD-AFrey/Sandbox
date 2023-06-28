using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using VikingEnterprise.GuiClient.Models.Global;
using VikingEnterprise.GuiClient.Models.Logging;
using VikingEnterprise.GuiClient.Services;
using VikingEnterprise.GuiClient.ViewModels.MainApplication;
using VikingEnterprise.GuiClient.ViewModels.Workspace;
using VikingEnterprise.GuiClient.Views.MainApplication;
using VikingEnterprise.GuiClient.Views.Workspace;

namespace VikingEnterprise.GuiClient;

public partial class GuiClientApp : Application
{
    private readonly IHost m_appHost;
    
    public GuiClientApp()
    {
        try
        {
            m_appHost = Host.CreateDefaultBuilder()
                .ConfigureLogging(p_options =>
                {
                    p_options.AddDebug();
                    p_options.AddSerilog();
                })
                .ConfigureServices(ConfigureServices).Build();
        } catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void ConfigureServices(IServiceCollection p_services)
    {
        try
        {
            p_services.AddSingleton<CommonDirectories>();
            p_services.AddSingleton<CommonFiles>();
        
            p_services.AddSingleton<SettingsService>();
            p_services.AddSingleton<RpcClientService>();
            p_services.AddSingleton<UserService>();
            p_services.AddSingleton<NavigationService>();
        
            p_services.AddSingleton<MainWorkspaceViewModel>();
            p_services.AddSingleton<MainWorkspaceWindow>();

            p_services.AddSingleton<LoginViewModel>();
            p_services.AddSingleton<LoginView>();
            p_services.AddSingleton<HomeViewModel>();
            p_services.AddSingleton<HomeView>();
            p_services.AddSingleton<UserViewModel>();
            p_services.AddSingleton<UserView>();
        } catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }



    public override async void OnFrameworkInitializationCompleted()
    {
        InitializeApplication();
        try
        {
            await m_appHost.StartAsync();
     
            if ( ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop )
            {
                desktop.ShutdownRequested += DesktopOnShutdownRequested;
                desktop.MainWindow = m_appHost.Services.GetService<MainWorkspaceWindow>();
            }

            base.OnFrameworkInitializationCompleted(); 
        } catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        
    }

    private void InitializeApplication()
    {
        try
        {
            var filesService = m_appHost.Services.GetRequiredService<CommonFiles>();
        
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(LogEventLevel.Debug)
                .WriteTo.Sink(new CollectionSink())
                .WriteTo.RollingFile(new JsonFormatter(), filesService.LogsPath, retainedFileCountLimit: 31)
                .CreateLogger();
        
        } catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    private async void DesktopOnShutdownRequested(object? p_sender, ShutdownRequestedEventArgs p_e)
    {
        await m_appHost.StopAsync();
    }
}