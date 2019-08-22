using My.Dotnet.Logger.IntegrationTests.Infrastructure;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace My.Dotnet.Logger.IntegrationTests
{
    [SetUpFixture]
    class SetupFixture
    {
        public static TestFixture Fixture { get; private set; }
        //public static IConfiguration Configuration { get; private set; } 
        public static string StorageConnectionString { get; set; }

        [OneTimeSetUp]
        public void SetUp()
        {
            
            IConfiguration configuration;
            if (File.Exists("integrationtestsettings.json"))
            {
                configuration = new ConfigurationBuilder().AddJsonFile("integrationtestsettings.json").Build();
                StorageConnectionString = configuration.GetConnectionString("AzureTableStorage");
            }

            Fixture = new TestFixture(StorageConnectionString);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Fixture.DropCloudTable();
        }
    }

}
