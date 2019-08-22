using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace My.Dotnet.Logger.TableStorage.Dto
{
    public class SegmentedResult<T> where T : ITableEntity
    {
        public IEnumerable<T> Results { get; set; }
        public TableContinuationToken ContinuationToken { get; set; }
    }
}
