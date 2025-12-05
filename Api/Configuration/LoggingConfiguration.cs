using Serilog;

namespace Api.Configuration;

/// <summary>
/// Configuration for Serilog logging
/// </summary>
public static class LoggingConfiguration
{
    /// <summary>
    /// Configures Serilog logger
    /// </summary>
    public static void ConfigureSerilog()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(
                path: "logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30)
            .CreateLogger();
    }
}