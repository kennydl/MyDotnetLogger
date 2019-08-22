using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using My.Dotnet.Logger.TableStorage.Extensions.ServiceCollection;
using Serilog;
using My.Dotnet.Logger.Extensions;
using Serilog.Events;
using My.Dotnet.Logger.SeriLogConfig;
using My.Dotnet.Logger.TableStorage.Utilities;
using System.Diagnostics;

namespace My.Dotnet.Logger.IntegrationTests.Infrastructure
{
    public class TestFixture
    {
        public readonly string TableName = "IntegrationTestLog";
        private readonly string _connectionString;
        private CloudStorageAccount _storageAccount;
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

            _storageAccount  = _serviceCollection.AddStorageAccount(_connectionString);
            _serviceCollection.AddServiceTableStorageLogger();
            ServiceProvider = _serviceCollection.BuildServiceProvider();

            ConfigureLogger();
            SeedLogData();
        }

        public void DropCloudTable()
        {
            var client = _storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(TableName);
            table.Delete();
        }

        private void SeedLogData()
        {
            //How To Create a logger with serviceProvider
            var logger = ServiceProvider.GetService<ILoggerFactory>().CreateLogger<TestFixture>();
            logger.LogInformation("data", new { propertyOne = "dummyOne" }, "Write to storage table dummy 1");
            logger.LogWarning("data", new { propertyOne = "dummyTwo" }, "Write to storage table dummy 2");
            logger.LogError("data", new { propertyOne = "dummyThree" }, "Write to storage table dummy 3");
            logger.LogWarning("data", new { propertyOne = "dummyTwo" }, "Write to storage table dummy 2");
            logger.LogError("data", new { propertyOne = "dummyThree" }, "Write to storage table dummy 3");
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
