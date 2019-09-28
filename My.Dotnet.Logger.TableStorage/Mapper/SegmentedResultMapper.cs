using My.Dotnet.Logger.TableStorage.Dto.Response;
using My.Dotnet.Logger.TableStorage.Entities;

namespace My.Dotnet.Logger.TableStorage.Mapper
{
    public static class SegmentedResultMapper
    {
        public static LogResponse MapToLogResponse(this SegmentedResultEntity<LogEntity> segment)
        {
            return new LogResponse()
            {
                Results = segment.Results,
                HasRowsLeft = segment.ContinuationToken != default,
                ContinuationToken = segment.ContinuationToken
            };
        }
    }
}
