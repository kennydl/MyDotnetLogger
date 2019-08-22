using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using My.Dotnet.Logger.Data.Context;
using My.Dotnet.Logger.TableStorage.Extensions.Struct;
using My.Dotnet.Logger.TableStorage.Factories;
using My.Dotnet.Logger.TableStorage.Interfaces.Repositories;
using My.Dotnet.Logger.TableStorage.Models;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace My.Dotnet.Logger.IntegrationTests.Repositories
{
    public class LogRepositoryTests
    {
        private LogServiceContext _serviceContext;
        private ILogRepository _repository;
        private const int _maxNrOfLogs = 5;

        [SetUp]
        public void Setup()
        {
            _serviceContext = (LogServiceContext)SetupFixture.Fixture.ServiceProvider.GetService<ILogServiceContext>();
            _serviceContext.TableName = SetupFixture.Fixture.TableName;
            _repository = SetupFixture.Fixture.ServiceProvider.GetService<ILogRepositoryFactory>().CreateRepository();
        }

        [Test]
        public void Get_all_logs_no_filter()
        {       
            // Act
            var response = _repository.GetAll(new LogFilterRequest());

            // Assert
            Assert.AreEqual(_maxNrOfLogs, response.Results.Count());
        }

        [TestCase(LogLevel.Information, 1)]
        [TestCase(LogLevel.Warning, 2)]
        [TestCase(LogLevel.Error, 2)]
        [TestCase(LogLevel.None, _maxNrOfLogs)]
        public void Get_all_logs_filter_level(LogLevel level, int assertNrOfLogs)
        {
            // Arrange
            var request = new LogFilterRequest() { Level = level };
            

            // Act
            var response = _repository.GetAll(request);

            // Assert            
            Assert.AreEqual(assertNrOfLogs, response.Results.Count());
        }
      
        [Test]
        public async Task Get_segmented_logs_no_filter()
        {
            // Act
            var response = await _repository.GetSegmentedFilterAsync(new LogFilterRequest());

            // Assert
            Assert.AreEqual(_maxNrOfLogs, response.Results.Count());
            Assert.False(response.HasRows);
        }

        [Test]
        public async Task Get_segmented_logs_filter_partition_key()
        {
            // Arrange
            var request = new LogFilterRequest() {
                PartitionKeyFrom = DateTime.Now.ToDateString(),
                PartitionKeyTo = DateTime.Now.ToDateString()
            };

            // Act
            var response = await _repository.GetSegmentedFilterAsync(request);

            // Assert
            Assert.AreEqual(_maxNrOfLogs, response.Results.Count());
        }

        [Test]
        public async Task Get_segmented_logs_filter_row_key()
        {
            // Arrange
            var request = new LogFilterRequest()
            {
                RowKeyFrom = "00:00:00.0000",
                RowKeyTo = "23:59:59.9999"
            };

            // Act
            var response = await _repository.GetSegmentedFilterAsync(request);

            // Assert
            Assert.AreEqual(_maxNrOfLogs, response.Results.Count());
        }

        [Test]
        public async Task Get_segmented_logs_filter_rendered_message()
        {
            // Arrange
            var request = new LogFilterRequest()
            {
                RenderedMessage = "dummy 2",
                Take = 2
            };

            // Act
            var response = await _repository.GetSegmentedFilterAsync(request);

            // Assert
            Assert.AreEqual(2, response.Results.Count());
            Assert.IsNotNull(response.ContinuationToken);
        }

        [Test]
        public async Task Get_segmented_logs_take_2()
        {
            // Arrange
            var request = new LogFilterRequest() { Take = 2 };

            // Act
            var response = await _repository.GetSegmentedFilterAsync(request);

            // Assert
            Assert.AreEqual(2, response.Results.Count());
            Assert.True(response.HasRows);
        }

        [TestCase(LogLevel.Information, 1)]
        [TestCase(LogLevel.Warning, 2)]
        [TestCase(LogLevel.Error, 2)]
        [TestCase(LogLevel.None, _maxNrOfLogs)]
        public async Task Get_segmented_logs_filter_level(LogLevel level, int assertNrOfLogs)
        {
            // Arrange
            var request = new LogFilterRequest() { Level = level };

            // Act
            var response = await _repository.GetSegmentedFilterAsync(request);

            // Assert
            Assert.AreEqual(assertNrOfLogs, response.Results.Count());
        }
    }
}
