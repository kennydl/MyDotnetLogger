using System;

namespace My.Dotnet.Logger.TableStorage.Entities
{
    public class LogEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Level { get; set; }
        public string RenderedMessage { get; set; }
        public object Properties { get; set; }
    }
}
