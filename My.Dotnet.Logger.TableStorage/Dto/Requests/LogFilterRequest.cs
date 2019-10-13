using Microsoft.Extensions.Logging;
using System;

namespace My.Dotnet.Logger.TableStorage.Models
{
    public class LogFilterRequest
    {
        public string TableName { get; set; }
        public string PartitionKeyFrom { get; set; }
        public string PartitionKeyTo { get; set; }
        public string RowKeyFrom { get; set; }
        public string RowKeyTo { get; set; }
        public DateTimeOffset TimestampFrom { get; set; }
        public DateTimeOffset TimestampTo { get; set; }
        public LogLevel Level { get; set; } = LogLevel.None;
        public string FilterData { get; set; }        
        public int Take { get; set; } = 50;
        public int NrOfPages { get; set; } = 1;
    }
}
