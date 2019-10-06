using System;

namespace My.Dotnet.Logger.TableStorage.Entities
{
    public class LogEntity : LogPropertiesEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Level { get; set; }
        public string RenderedMessage { get; set; }
        public string Properties { get; set; }        
    }
}
