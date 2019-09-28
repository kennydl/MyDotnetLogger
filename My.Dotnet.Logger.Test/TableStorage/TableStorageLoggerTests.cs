using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using My.Dotnet.Logger.Extensions;
using My.Dotnet.Logger.SeriLogConfig;
using My.Dotnet.Logger.TableStorage.Utilities;
using NUnit.Framework;
using Serilog;
using System.IO;
using System.Threading.Tasks;

namespace My.Dotnet.Logger.Tests.TableStorage
{
    public class TableStorageLoggerTests
    {
        private LoggerConfiguration _loggerConfiguration;
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;
        private const string _testTableName = "TestLog";
        private string _connectionString;

        [OneTimeSetUp]
        public void Init()
        {
            IConfiguration configuration = null;
            if (File.Exists("testsettings.json"))
            {
                configuration = new ConfigurationBuilder().AddJsonFile("testsettings.json").Build();
                _connectionString = configuration.GetConnectionString("AzureTableStorage");
                _storageAccount = AzureStorageUtil.GetLoggerStorageAccount(_connectionString);
                var tableClient = _storageAccount.CreateCloudTableClient();
                _table = tableClient.GetTableReference(_testTableName);
            }                              
        }

        [OneTimeTearDown]
        public async Task CleanUp()
        {   
            await _table.DeleteAsync();
        }

        [SetUp]
        public void Setup()
        {
            _loggerConfiguration = new LoggerConfiguration();
        }

        [Test]
        public void Write_logg_to_azureTableStorage()
        {                      
            var logger = _loggerConfiguration
                .Enrich.FromLogContext()
                .AddAzureTableStorageLogger(_connectionString, _testTableName).CreateLogger();

            logger.AddProperty("PropertyTwo", "dummyTwo")
                .Information("Write to storage table with template. PropertyOne = {propertyOne}", "dummyOne");
            logger.AddProperty("PropertyThree", "dummyThree")
                .Information("Write to table storage without template");

            Assert.Pass();
        }
    }
}
