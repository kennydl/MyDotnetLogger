using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using My.Dotnet.Logger;
using My.Dotnet.Logger.SeriLogConfig;
using NUnit.Framework;
using Serilog;

namespace My.Dotnet.Logger.Test
{
    public class LoggerTest
    {
        private LoggerConfiguration _loggerConfiguration;
        [SetUp]
        public void Setup()
        {
            _loggerConfiguration = new LoggerConfiguration();
        }

        [Test]
        public void Write_To_Console()
        {
            var logger = _loggerConfiguration.AddConsoleLogger().CreateLogger();

            //log
            logger.Information("Successfully created the console logger");

            logger.AddProperty("PropertyTwo", new {Test = "dummyTwo"})
                .Information("Write to console with property {PropertyOne}", new {Test = "dummyOne"});
            Assert.Pass();
        }

        [Test, Explicit]
        public void Write_To_AzureTableStorage()
        {;
            IConfiguration configuration = null;
            if (File.Exists("localsettings.json"))
            {
               configuration = new ConfigurationBuilder().AddJsonFile("localsettings.json").Build();
            }

            var connectionString = configuration?.GetConnectionString("AzureTableStorage");
            var storageAccount = CreateStorageAccountFromConnectionString(connectionString);

            var logger = _loggerConfiguration.AddAzureTableStorageLogger(storageAccount, "test").CreateLogger();
            logger.AddProperty("PropertyTwo", "dummyTwo")
                .Information("Write to storage table with template. PropertyOne = {propertyOne}", "dummyOne");
            logger.AddProperty("PropertyThree", "dummyThree")
                .Information("Write to table storage without template");

            Assert.Pass();
        }

        private CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                Console.ReadLine();
                throw;
            }

            return storageAccount;
        }
    }
}