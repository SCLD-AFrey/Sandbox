using VikingEnterprise.Server;
using VikingEnterprise.Server.Global.FileStructure;
using VikingEnterprise.Server.Services.Worker;


var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(p_services =>
    {
        p_services.AddHostedService<Worker>();
        p_services.AddSingleton<BackgroundWorkerHeartbeatMonitor>();
        p_services.AddSingleton<CommonDirectories>();
        p_services.AddSingleton<CommonFiles>();
    })
    // ReSharper disable once InconsistentNaming
    .ConfigureLogging((_, p_builder) =>
    {
#pragma warning disable ASP0000
        var filesService = p_builder.Services.BuildServiceProvider().GetRequiredService<CommonFiles>();
#pragma warning restore ASP0000
        p_builder.AddFile(filesService.LogsPath, isJson: true, retainedFileCountLimit: 1);
    });
    
    
hostBuilder.UseWindowsService();
var host = hostBuilder.Build();
await host.RunAsync();