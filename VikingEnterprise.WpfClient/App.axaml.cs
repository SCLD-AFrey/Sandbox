using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using VikingEnterprise.WpfClient.Models.Global;
using VikingEnterprise.WpfClient.Models.Logging;
using VikingEnterprise.WpfClient.Services;
using VikingEnterprise.WpfClient.ViewModels;
using VikingEnterprise.WpfClient.ViewModels.Workspace;
using VikingEnterprise.WpfClient.Views;
using VikingEnterprise.WpfClient.Views.Workspace;

namespace VikingEnterprise.WpfClient;

public partial class App : Application
{
    private readonly IHost m_appHost;
    private static   bool  SkipLogin              { get; } = false;
    
    public App()
    {
        m_appHost = Host.CreateDefaultBuilder()
            .ConfigureLogging(p_options =>
            {
                p_options.AddDebug();
                p_options.AddSerilog();
            })
            .ConfigureServices(ConfigureServices).Build();
    }
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {

        var filesService = m_appHost.Services.GetRequiredService<CommonFiles>();
        var settingsService = m_appHost.Services.GetRequiredService<SettingsService>();
        
        settingsService.LoadSettings();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(LogEventLevel.Debug)
            .WriteTo.Sink(new CollectionSink())
            .WriteTo.RollingFile(new JsonFormatter(), filesService.LogsPath, retainedFileCountLimit: 31)
            .CreateLogger();

        await m_appHost.StartAsync();
        if ( ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop )
        {
            desktop.ShutdownRequested += DesktopOnShutdownRequested;
            desktop.MainWindow = m_appHost.Services.GetService<MainWorkspaceView>();
        }

        base.OnFrameworkInitializationCompleted();
        
    }

    private async void DesktopOnShutdownRequested(object? p_sender, ShutdownRequestedEventArgs p_e)
    {
        await m_appHost.StopAsync();
    }
    private void ConfigureServices(IServiceCollection p_services)
    {
        p_services.AddSingleton<CommonDirectories>();
        p_services.AddSingleton<CommonFiles>();
        p_services.AddSingleton<SettingsService>();
        p_services.AddSingleton<ClientConfiguration>();
        p_services.AddSingleton<RpcClientService>();
        p_services.AddSingleton<UserService>();
        p_services.AddSingleton<NavigationService>();

        p_services.AddSingleton<WelcomeViewModel>();
        p_services.AddSingleton<WelcomeView>();
        
        p_services.AddSingleton<MainWorkspaceViewModel>();
        p_services.AddSingleton<MainWorkspaceView>();
        
        p_services.AddSingleton<MainApplicationViewModel>();
        p_services.AddSingleton<MainApplicationView>();
        
        p_services.AddSingleton<UserManagementViewModel>();
        p_services.AddSingleton<UserManagementView>();
    }
    
}