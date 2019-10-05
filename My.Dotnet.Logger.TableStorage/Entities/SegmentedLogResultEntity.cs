using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;

namespace My.Dotnet.Logger.TableStorage.Entities
{
    public class SegmentedLogResultEntity
    {
        public IEnumerable<LogEntity> Results { get; set; }
        public TableContinuationToken ContinuationToken { get; set; }
    }
}
