

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

        public LogResponse GetAll(LogFilterRequest request)
        {
            var table = GetTableReference(request.TableName);
            var results = table.ExecuteQuery(CreateTableQuery(request));
            return new SegmentedLogResultEntity()
            {
                Results = results.Select(r => r.MapToLogEntity())
            }.MapToLogResponse();
        }

        public async Task<LogResponse> GetSegmentedFilterAsync(LogFilterRequest request, TableContinuationToken continuationToken = default)
        {
            var table = GetTableReference(request.TableName);
            var queryResponse = await table.ExecuteQuerySegmentedAsync(
                    CreateTableQuery(request).Take(request.Take),
                    continuationToken
                )
                .ConfigureAwait(false);

            var segmentResult = await FilterRenderedMessage(new SegmentedLogResultEntity()
            {
                Results = queryResponse.Results.Select(r => r.MapToLogEntity()),
                ContinuationToken = queryResponse.ContinuationToken
            }, request);
            return segmentResult.MapToLogResponse();
        }

        private CloudTable GetTableReference(string tableName)
        {
            if (_tables.ContainsKey(tableName))
                return _tables[tableName];
            var table = _tableClient.GetTableReference(tableName);            
            _tables.Add(tableName, table);
            return table;
        }

        private async Task<SegmentedLogResultEntity> FilterRenderedMessage(SegmentedLogResultEntity segmentResult, LogFilterRequest request)
        {
            if (!string.IsNullOrEmpty(request.RenderedMessage))
            {
                segmentResult.Results = segmentResult.Results.Where(
                    entity => entity.RenderedMessage.Contains(request.RenderedMessage)
                );

                while (segmentResult.Results.Count() < request.Take && segmentResult.ContinuationToken != default)
                {
                    request.Take = request.Take - segmentResult.Results.Count();
                    var response = await GetSegmentedFilterAsync(request, segmentResult.ContinuationToken);
                    segmentResult.ContinuationToken = response.ContinuationToken;
                    segmentResult.Results = segmentResult.Results.Concat(response.Results);
                }
            }
            return segmentResult;
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
