namespace VikingEnterprise.Server.Services.Worker;

public class BackgroundWorkerHeartbeatMonitor
{
    public BackgroundWorkerHeartbeatMonitor(ILogger<BackgroundWorkerHeartbeatMonitor> p_logger)
    {
        m_logger = p_logger;
    }

    private readonly ILogger<BackgroundWorkerHeartbeatMonitor> m_logger;
    
    public async Task Start(CancellationToken p_stoppingToken)
    {
        while ( !p_stoppingToken.IsCancellationRequested )
        {
            m_logger.LogInformation("Viking Enterprise Service service is running");
            await Task.Delay(60000, p_stoppingToken);
        }
    }
}