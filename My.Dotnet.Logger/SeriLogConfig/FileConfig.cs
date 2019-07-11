using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace My.Dotnet.Logger.SeriLogConfig
{
    public static class FileConfig
    {
        public static LoggerConfiguration AddFileLogger(this LoggerConfiguration log, LogEventLevel logLevel = LogEventLevel.Debug)
        {
            log.WriteTo.File(
                "log.log",
                restrictedToMinimumLevel: logLevel
            );
            return log;
        }

        public static LoggerConfiguration AddJsonFileLogger(this LoggerConfiguration log, LogEventLevel logLevel = LogEventLevel.Debug)
        {
            log.WriteTo.File(
                new CompactJsonFormatter(),
                "log.log",
                restrictedToMinimumLevel: logLevel
            );
            return log;
        }
    }
}