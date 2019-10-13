using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using My.Dotnet.Logger.Extensions;
using My.Dotnet.Logger.SeriLogConfig;
using My.Dotnet.Logger.TableStorage.Extensions.ServiceCollection;
using Serilog;
using Serilog.Events;

namespace My.Dotnet.Logger.IntegrationTests.Infrastructure
{
    public class TestFixture
    {
        public readonly string TableName = "IntegrationTestLog";
        private readonly string _connectionString;
        private readonly IServiceCollection _serviceCollection;
        public readonly ServiceProvider ServiceProvider;
        private ILogger<TestFixture> _logger;

        public int NrOfInfoLogs;
        public int NrOfWarningLogs;
        public int NrOfErrorLogs;
        public int NrOfLogs { get
            {
                return NrOfInfoLogs + NrOfWarningLogs + NrOfErrorLogs;
            }
        }

        public TestFixture(string connectionString)
        {
            _connectionString = connectionString;
            _serviceCollection = new ServiceCollection()
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddSerilog();
                });

            _serviceCollection.AddTableStorageLogRepository(_connectionString);
            ServiceProvider = _serviceCollection.BuildServiceProvider();

            ConfigureLogger();
            SeedLogData();
        }

        public void DropCloudTable()
        {
            var client = ServiceProvider.GetService<CloudStorageAccount>().CreateCloudTableClient();
            var table = client.GetTableReference(TableName);
            table.Delete();
        }

        private void SeedLogData()
        {
            //How To Create a logger with serviceProvider
            _logger = ServiceProvider.GetService<ILoggerFactory>().CreateLogger<TestFixture>();
            LogInfo();
            LogWarning();
            LogWarning();
            LogError();
            LogError();
        }

        private void LogInfo()
        {
            _logger.LogInformation("data", new { PropertyOne = "propertyOne" }, "Write to storage table dummy 1");
            NrOfInfoLogs += 1;
        }

        private void LogWarning()
        {
            _logger.LogWarning("data", new { PropertyTwo = "propertyTwo" }, "Write to storage table dummy 2");
            NrOfWarningLogs += 1;
        }

        private void LogError()
        {
            _logger.LogError("data", new { PropertyThree = "propertyThree" }, "Write to storage table dummy 3");
            NrOfErrorLogs += 1;
        }

        private void ConfigureLogger()
        {
            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .AddConsoleLogger();

            loggerConfig.AddAzureTableStorageLogger(_connectionString, TableName);
            Log.Logger = loggerConfig.CreateLogger();
        }
    }
}
