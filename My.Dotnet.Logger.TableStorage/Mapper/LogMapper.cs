using My.Dotnet.Logger.TableStorage.Dto.Response;
using My.Dotnet.Logger.TableStorage.Entities;
using Newtonsoft.Json;
using System.Linq;

namespace My.Dotnet.Logger.TableStorage.Mapper
{
    public static class LogMapper
    {
        public static LogResponse MapToLogResponse(this SegmentedLogResultEntity segment)
        {            
            return new LogResponse()
            {
                Results = segment.Results,
                HasRowsLeft = segment.ContinuationToken != default,
                ContinuationToken = segment.ContinuationToken
            };
        }

        public static LogEntity MapToLogEntity(this LogTableEntity logTableEntity)
        {            
            var data = JsonConvert.DeserializeObject<LogDataTableEntity>(logTableEntity.Data);
            var properties = JsonConvert.DeserializeObject<LogPropertiesEntity>(data.Properties);
            return new LogEntity()
            {
                PartitionKey = logTableEntity.PartitionKey,
                RowKey = logTableEntity.RowKey,
                Timestamp = logTableEntity.Timestamp,
                Level = logTableEntity.Level,
                RenderedMessage = logTableEntity.RenderedMessage,
                SourceContext = properties.SourceContext,
                ActionId = properties.ActionId,
                ActionName = properties.ActionName,
                RequestId = properties.RequestId,
                RequestPath = properties.RequestPath,
                Properties = data.Properties,
            };
        }
    }
}
