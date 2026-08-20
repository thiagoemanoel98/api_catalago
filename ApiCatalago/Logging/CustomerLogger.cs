namespace ApiCatalago.Logging;

public class CustomerLogger : ILogger
{
    readonly string loggerName;
    readonly CustomLoggerProviderConfiguration loggerConfig;

    public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
    {
        loggerName = name;
        loggerConfig = config;
    }

    // Escopo para mensagens de log
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

    public bool IsEnabled(LogLevel logLevel) => logLevel == loggerConfig.LogLevel;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
        Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;
        if (loggerConfig.EventId != 0 && loggerConfig.EventId != eventId.Id) return;

        var message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {loggerName} - {formatter(state, exception)}{Environment.NewLine}";

        var directory = Path.GetDirectoryName(loggerConfig.FilePath);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        File.AppendAllText(loggerConfig.FilePath, message);
    }
}
