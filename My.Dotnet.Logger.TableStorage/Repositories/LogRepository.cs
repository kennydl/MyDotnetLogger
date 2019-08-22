

using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using My.Dotnet.Logger.TableStorage.Dto;
using My.Dotnet.Logger.TableStorage.Dto.Response;
using My.Dotnet.Logger.TableStorage.Entities;
using My.Dotnet.Logger.TableStorage.Factories;
using My.Dotnet.Logger.TableStorage.Interfaces.Repositories;
using My.Dotnet.Logger.TableStorage.Mapper;
using My.Dotnet.Logger.TableStorage.Models;
using My.Dotnet.Logger.TableStorage.Queries;
using System.Threading.Tasks;
using System.Linq;

namespace My.Dotnet.Logger.TableStorage.Repositories
{
    public class LogRepository : TableStorageQuery, ILogRepository
    {
        private readonly CloudTableClient _tableClient;
        private readonly CloudTable _table;

        public LogRepository(ILogger<ILogRepositoryFactory> logger, CloudStorageAccount storageAccount, string tableName)
        {
            _tableClient = storageAccount.CreateCloudTableClient();
            _table = _tableClient.GetTableReference(tableName);
        }

        public LogResponse GetAll(LogFilterRequest request)
        {
            return new SegmentedResult<LogEntity>()
            {
                Results = _table.ExecuteQuery(CreateTableQuery(request))
            }.MapToLogResponse();
        }

        public async Task<LogResponse> GetSegmentedFilterAsync(LogFilterRequest request, TableContinuationToken continuationToken = default)
        {
            var queryResponse = await _table.ExecuteQuerySegmentedAsync(
                CreateTableQuery(request).Take(request.Take),
                continuationToken
            ).ConfigureAwait(false);

            var segmentResult = new SegmentedResult<LogEntity>()
            {
                Results = queryResponse.Results,
                ContinuationToken = queryResponse.ContinuationToken
            };

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

            return segmentResult.MapToLogResponse();          
        }

        private TableQuery<LogEntity> CreateTableQuery(LogFilterRequest request)
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
            {
                return new TableQuery<LogEntity>().Where(filterString);
            }

            return new TableQuery<LogEntity>();
        }         
    }
}
