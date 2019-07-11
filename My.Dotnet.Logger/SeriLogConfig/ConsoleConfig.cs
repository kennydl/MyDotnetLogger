using Serilog;
using Serilog.Events;

namespace My.Dotnet.Logger.SeriLogConfig
{
    public static class ConsoleConfig
    {
        public static LoggerConfiguration AddConsoleLogger(this LoggerConfiguration log, LogEventLevel logLevel = LogEventLevel.Debug)
        {
            log.WriteTo.Console(
                restrictedToMinimumLevel: logLevel,
                outputTemplate:
                "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level:u3}] {Message:lj}{NewLine}{Exception}");

            //Log.CloseAndFlush();
            return log;
        }
    }
}