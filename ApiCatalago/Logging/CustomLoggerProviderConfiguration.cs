namespace ApiCatalago.Logging;

public class CustomLoggerProviderConfiguration
{
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;
    public int EventId { get; set; } = 0;
    public string FilePath { get; set; } = "logs/api_log.txt";
}