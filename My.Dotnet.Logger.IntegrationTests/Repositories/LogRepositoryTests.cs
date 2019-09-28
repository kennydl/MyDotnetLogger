using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using My.Dotnet.Logger.IntegrationTests.Models.Folders;
using My.Dotnet.Logger.TableStorage.Extensions.Struct;
using My.Dotnet.Logger.TableStorage.Interfaces.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My.Dotnet.Logger.IntegrationTests.Repositories
{
    public class LogRepositoryTests
    {        
        private ILogRepository _repository;
        private const int _nrOfLogs = 5;

        [SetUp]
        public void Setup()
        {
            _repository = SetupFixture.Fixture.ServiceProvider.GetService<ILogRepository>();
        }

        [TestCaseSource("GetLogsFilterLevelTestCases")]
        public void Get_all_logs_filter_level_should_equal_nr_of_all_respective_level_logs(LogLevel level, int assertNrOfLogs)
        {
            // Arrange
            var request = new MockLogFilterRequest() { 
                Level = level 
            };         

            // Act
            var response = _repository.GetAll(request);

            // Assert            
            Assert.AreEqual(assertNrOfLogs, response.Results.Count());
        }

        [TestCaseSource("GetLogsFilterLevelTestCases")]
        public async Task Get_segmented_logs_filter_level_should_equal_nr_of_all_respective_level_logs(LogLevel level, int assertNrOfLogs)
        {
            // Arrange
            var request = new MockLogFilterRequest() { Level = level };

            // Act
            var response = await _repository.GetSegmentedFilterAsync(request);

            // Assert
            Assert.AreEqual(assertNrOfLogs, response.Results.Count());
        }

        private static IEnumerable<TestCaseData> GetLogsFilterLevelTestCases()
        {
            yield return new TestCaseData(LogLevel.Information, 1);
            yield return new TestCaseData(LogLevel.Warning, 2);
            yield return new TestCaseData(LogLevel.Error, 2);
            yield return new TestCaseData(LogLevel.None, _nrOfLogs);
        }

        [Test]
        public void Get_all_logs_no_filter_should_equal_nr_of_all_logs()
        {
            // Act
            var response = _repository.GetAll(new MockLogFilterRequest());

            // Assert
            Assert.AreEqual(SetupFixture.Fixture.NrOfLogs, response.Results.Count());
        }

        [Test]
        public async Task Get_segmented_logs_no_filter_should_equal_nr_of_all_logs()
        {
            // Act
            var request = new MockLogFilterRequest();
            var response = await _repository.GetSegmentedFilterAsync(request);

            // Assert
            Assert.AreEqual(SetupFixture.Fixture.NrOfLogs, response.Results.Count());
            Assert.False(response.HasRowsLeft);
        }

        [Test]
        public async Task Get_segmented_logs_filter_partition_key_should_equal_nr_of_all_logs()
        {
            // Arrange
            var request = new MockLogFilterRequest() {
                PartitionKeyFrom = DateTime.Now.ToDateString(),
                PartitionKeyTo = DateTime.Now.ToDateString()
            };

            // Act
            var response = await _repository.GetSegmentedFilterAsync(request);

            // Assert
            Assert.AreEqual(SetupFixture.Fixture.NrOfLogs, response.Results.Count());
        }

        [Test]
        public async Task Get_segmented_logs_filter_row_key_should_equal_nr_of_all_logs()
        {
            // Arrange
            var request = new MockLogFilterRequest()
            {
                RowKeyFrom = "00:00:00.0000",
                RowKeyTo = "23:59:59.9999"
            };

            // Act
            var response = await _repository.GetSegmentedFilterAsync(request);

            // Assert
            Assert.AreEqual(SetupFixture.Fixture.NrOfLogs, response.Results.Count());
        }

        [Test]
        public async Task Get_segmented_logs_filter_rendered_message_should_equal_nr_of_warning_logs()
        {
            // Arrange
            var request = new MockLogFilterRequest()
            {
                RenderedMessage = "dummy 2"           
            };

            // Act
            var response = await _repository.GetSegmentedFilterAsync(request);

            // Assert
            Assert.AreEqual(SetupFixture.Fixture.NrOfWarningLogs, response.Results.Count());            
        }

        [Test]
        public async Task Get_segmented_logs_take_2_should_return_2_logs()
        {
            // Arrange
            var request = new MockLogFilterRequest() { 
                Take = 2 
            };

            // Act
            var response = await _repository.GetSegmentedFilterAsync(request);

            // Assert
            Assert.AreEqual(2, response.Results.Count());
            Assert.IsNotNull(response.ContinuationToken);
            Assert.True(response.HasRowsLeft);
        }     
    }
}
