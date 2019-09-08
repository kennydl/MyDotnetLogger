using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;

namespace My.Dotnet.Logger.TableStorage.Entities
{
    public class SegmentedResultEntity<T> where T : ITableEntity
    {
        public IEnumerable<T> Results { get; set; }
        public TableContinuationToken ContinuationToken { get; set; }
    }
}
