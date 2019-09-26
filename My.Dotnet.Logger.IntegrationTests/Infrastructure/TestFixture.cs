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
            var logger = ServiceProvider.GetService<ILoggerFactory>().CreateLogger<TestFixture>();
            logger.LogInformation("data", new { PropertyOne = "dummyOne" }, "Write to storage table dummy 1");
            logger.LogWarning("data", new { PropertyOne = "dummyTwo" }, "Write to storage table dummy 2");
            logger.LogError("data", new { PropertyOne = "dummyThree" }, "Write to storage table dummy 3");
            logger.LogWarning("data", new { PropertyOne = "dummyTwo" }, "Write to storage table dummy 2");
            logger.LogError("data", new { PropertyOne = "dummyThree" }, "Write to storage table dummy 3");
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
