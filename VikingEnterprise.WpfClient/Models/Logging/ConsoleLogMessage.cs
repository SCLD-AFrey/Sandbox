using Serilog.Events;

namespace VikingEnterprise.WpfClient.Models.Logging;

public class ConsoleLogMessage
{
    public LogEventLevel LogLevel { get; set; }
    public string? Text { get; set; }
}