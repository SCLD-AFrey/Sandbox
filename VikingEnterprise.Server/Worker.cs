using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using VikingEnterprise.Server.Global.FileStructure;
using VikingEnterprise.Server.Services.Grpc;
using VikingEnterprise.Server.Services.Host;
using VikingEnterprise.Server.Services.Host.Database;
using VikingEnterprise.Server.Services.Worker;

namespace VikingEnterprise.Server;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker>                  m_logger;
    private readonly BackgroundWorkerHeartbeatMonitor m_monitor;

    public Worker(ILogger<Worker> p_logger, BackgroundWorkerHeartbeatMonitor p_monitor)
    {
        m_logger  = p_logger;
        m_monitor = p_monitor;
    }

    protected override async Task ExecuteAsync(CancellationToken p_stoppingToken)
    {
#pragma warning disable CS4014
        m_monitor.Start(p_stoppingToken);
#pragma warning restore CS4014

        m_logger.LogInformation("Starting Viking Enterprise Service");
        Console.WriteLine("Starting Viking Enterprise Service");

        await StartServer(p_stoppingToken);
    }

    public override Task StopAsync(CancellationToken p_stoppingToken)
    {
        m_logger.LogInformation("Stopping Viking Enterprise Service");
        Console.WriteLine("Stopping Viking Enterprise Service");

        return base.StopAsync(p_stoppingToken);
    }

    private async Task StartServer(CancellationToken p_cancellationToken)
    {
        var builder = WebApplication.CreateBuilder();

        builder.Services.AddGrpc();
        builder.Services.AddSingleton<CommonDirectories>();
        builder.Services.AddSingleton<CommonFiles>();
        builder.Services.AddSingleton<EncryptionService>();
        builder.Services.AddSingleton<DatabaseUtilities>();
        builder.Services.AddSingleton<DatabaseInterface>();
        builder.Services.AddSingleton<StartupService>();

        var commonFilesService = builder.Services.BuildServiceProvider().GetRequiredService<CommonFiles>();

        builder.Logging.AddFile(commonFilesService.LogsPath, 
            minimumLevel: LogLevel.Information, 
            levelOverrides: new Dictionary<string, LogLevel>
            {
                { "Grpc.AspNetCore.Server", LogLevel.Warning },
                { "Microsoft.AspNetCore", LogLevel.Warning }
            },
            isJson: true, 
            retainedFileCountLimit: 31);
        
        builder.WebHost.ConfigureKestrel(p_options =>
        {
            p_options.ListenAnyIP(50002,
                p_listenOptions =>
                {
                    p_listenOptions.Protocols = Microsoft
                        .AspNetCore.Server
                        .Kestrel.Core
                        .HttpProtocols.Http2;
                    p_listenOptions.UseHttps();
                }
            );
        });
        

        var app = builder.Build();

        app.UseRouting();

        // Configure the HTTP request pipeline.

        app.MapGrpcService<ConnectionService>();

        app.MapGrpcService<UserService>();

        app.MapGet("/", () => "Communication with Viking Enterprise Service must be made through a Viking Enterprise Client .");
        var startupService = app.Services.GetRequiredService<StartupService>();
        
        await startupService.InitDatabase();

        await app.RunAsync(p_cancellationToken);
    }
}