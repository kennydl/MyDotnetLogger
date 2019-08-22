using Microsoft.Extensions.Configuration;
using My.Dotnet.Logger.Extensions;
using My.Dotnet.Logger.SeriLogConfig;
using My.Dotnet.Logger.TableStorage.Utilities;
using NUnit.Framework;
using Serilog;
using System.IO;

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
            var logger = _loggerConfiguration
                .Enrich.FromLogContext()
                .AddConsoleLogger()
                .CreateLogger();

            //log
            logger.Information("Successfully created the console logger");

            logger.AddProperty("PropertyTwo", new {Test = "dummyTwo"})
                .Information("Write to console with property {PropertyOne}", new {Test = "dummyOne"});
            Assert.Pass();
        }      
    }
}