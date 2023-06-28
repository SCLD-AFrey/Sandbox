using Serilog.Events;

namespace VikingEnterprise.GuiClient.Models.Logging;

public class ConsoleLogMessage
{
    public LogEventLevel LogLevel { get; set; }
    public string? Text { get; set; }
}