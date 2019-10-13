

using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using My.Dotnet.Logger.TableStorage.Dto.Response;
using My.Dotnet.Logger.TableStorage.Entities;
using My.Dotnet.Logger.TableStorage.Interfaces.Repositories;
using My.Dotnet.Logger.TableStorage.Mapper;
using My.Dotnet.Logger.TableStorage.Models;
using My.Dotnet.Logger.TableStorage.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My.Dotnet.Logger.TableStorage.Repositories
{
    public class LogRepository : TableStorageLogQuery, ILogRepository
    {
        private readonly CloudTableClient _tableClient;        
        private readonly Dictionary<string, CloudTable> _tables = new Dictionary<string, CloudTable>();
        private readonly ILogger<LogRepository> _logger;

        public LogRepository(ILogger<LogRepository> logger, CloudStorageAccount storageAccount)
        {
            _tableClient = storageAccount.CreateCloudTableClient();
            _logger = logger;
        }

        public async Task<LogResponse> GetAllAsync(LogFilterRequest request)
        {
            var table = GetTableReference(request.TableName);
            var tableQueries = CreateTableQuery(request);
            var tableResults = table.ExecuteQuery(tableQueries);
            var logResult = CreateLogResult(tableResults);

            var filteredResult = await GetFilteredData(logResult, request);
            return filteredResult.MapToLogResponse();
        }

        public async Task<LogResponse> GetSegmentedResultAsync(LogFilterRequest request, TableContinuationToken token = default)
        {
            var table = GetTableReference(request.TableName);
            var tableQueries = CreateTableQuery(request).Take(request.Take);
            var tableResponse = await table.ExecuteQuerySegmentedAsync(tableQueries, token)
                .ConfigureAwait(false);
            var logResult = CreateLogResult(tableResponse.Results, tableResponse.ContinuationToken);

            var filteredResult = await GetFilteredData(logResult, request);
            return filteredResult.MapToLogResponse();
        }

        private LogResultEntity CreateLogResult(IEnumerable<LogTableEntity> results, TableContinuationToken token = default)
        {
            return new LogResultEntity()
            {
                Results = results.Select(r => r.MapToLogEntity()),
                ContinuationToken = token
            };
        }

        private CloudTable GetTableReference(string tableName)
        {
            if (_tables.ContainsKey(tableName))
                return _tables[tableName];
            var table = _tableClient.GetTableReference(tableName);            
            _tables.Add(tableName, table);
            return table;
        }

        private async Task<LogResultEntity> GetFilteredData(LogResultEntity results, LogFilterRequest request)
        {
            if (!string.IsNullOrEmpty(request.FilterData))
            {
                results.Results = FilterResultProperties(results, request);
                if (HasContinuationTokenAndResultsNotFilled(results, request))
                {
                    results.Results = await ContinueGetSegmentedResult(results, request);
                }
            }
            return results;
        }

        private IEnumerable<LogEntity> FilterResultProperties(LogResultEntity results, LogFilterRequest request)
        {
            return results.Results.Where(entity =>
                entity.RenderedMessage.Contains(request.FilterData, System.StringComparison.OrdinalIgnoreCase) ||
                entity.Properties.Contains(request.FilterData, System.StringComparison.OrdinalIgnoreCase )
            );
        }

        private bool HasContinuationTokenAndResultsNotFilled(LogResultEntity results, LogFilterRequest request)
        {
            return results.ContinuationToken != default && results.Results.Count() < request.Take;
        }

        private async Task<IEnumerable<LogEntity>> ContinueGetSegmentedResult(LogResultEntity results, LogFilterRequest request)
        {
            request.Take = request.Take - results.Results.Count();
            var response = await GetSegmentedResultAsync(request, results.ContinuationToken);
            results.ContinuationToken = response.ContinuationToken;
            return results.Results.Concat(response.Results); 
        }

        private TableQuery<LogTableEntity> CreateTableQuery(LogFilterRequest request)
        {           
            var filters = new string[]
            {
                KeyQuery("PartitionKey", request.PartitionKeyFrom, request.PartitionKeyTo),
                KeyQuery("RowKey", request.RowKeyFrom, request.RowKeyTo),
                DateTimeOffsetQuery("Timestamp", request.TimestampFrom, request.TimestampTo),
                LogLevelQuery("Level", request.Level)
            };

            var filterString = CombineAllQueries(filters, TableOperators.And);
            if (!string.IsNullOrEmpty(filterString))
                return new TableQuery<LogTableEntity>().Where(filterString);
            return new TableQuery<LogTableEntity>();
        }
    }
}
