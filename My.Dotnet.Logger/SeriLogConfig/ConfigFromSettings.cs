using Microsoft.Extensions.Configuration;
using Serilog;

namespace My.Dotnet.Logger.SeriLogConfig
{
    public static class ConfigFromSettings
    {
        public static LoggerConfiguration AddLoggerFromConfig(this LoggerConfiguration log, IConfiguration configuration)
        {
            log.ReadFrom.Configuration(configuration);
            return log;
        }
    }
}