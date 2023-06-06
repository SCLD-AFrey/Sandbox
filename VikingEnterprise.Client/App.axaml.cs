using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using VikingEnterprise.Client.Global.FileStructure;
using VikingEnterprise.Client.Models;
using VikingEnterprise.Client.Models.Logging;
using VikingEnterprise.Client.Models.MainApplication;
using VikingEnterprise.Client.Services;
using VikingEnterprise.Client.ViewModels;
using VikingEnterprise.Client.ViewModels.MainApplication;
using VikingEnterprise.Client.Views;
using VikingEnterprise.Client.Views.MainApplication;
using VikingEnterprise.Server.Global.FileStructure;

namespace VikingEnterprise.Client;

public partial class App : Application
{
    private readonly IHost m_appHost;
    private static   bool  SkipLogin              { get; } = false;
    private          bool  ExitWarningWindowIsOpen { get; set; }
    
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

    private void ConfigureServices(IServiceCollection p_services)
    {
        p_services.AddSingleton<CommonDirectories>();
        p_services.AddSingleton<CommonFiles>();
        p_services.AddSingleton<ClientConfiguration>();
        p_services.AddSingleton<RpcClientFactory>();
        p_services.AddSingleton<ServerConnection>();
        p_services.AddSingleton<IUserService, UserService>();
        p_services.AddSingleton<UserCredential>();

        
        p_services.AddSingleton<MainWindowModel>();
        p_services.AddSingleton<MainWindowViewModel>();
        p_services.AddSingleton<MainWindowView>();
        
        p_services.AddSingleton<MainApplicationModel>();
        p_services.AddSingleton<MainApplicationViewModel>();
        p_services.AddSingleton<MainApplicationView>();
        
        p_services.AddSingleton<UserManagementModel>();
        p_services.AddSingleton<UserManagementViewModel>();
        p_services.AddSingleton<UserManagementView>();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
#if DEBUG
        var logLevel = LogEventLevel.Debug;
#else
        var logLevel = LogEventLevel.Information;
#endif

        var filesService = m_appHost.Services.GetRequiredService<CommonFiles>();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(logLevel)
            .WriteTo.Sink(new CollectionSink())
            .WriteTo.RollingFile(new JsonFormatter(), filesService.LogsPath, retainedFileCountLimit: 31)
            .CreateLogger();

        filesService.SaveClientConfig("localhost", "50002");

        await m_appHost.StartAsync();
        if ( ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop )
        {
            desktop.ShutdownRequested += DesktopOnShutdownRequested;

            desktop.MainWindow = m_appHost.Services.GetService<MainWindowView>();
        }

        base.OnFrameworkInitializationCompleted();
        
    }

    private async void DesktopOnShutdownRequested(object? p_sender, ShutdownRequestedEventArgs p_e)
    {
        await m_appHost.StopAsync();
    }
}