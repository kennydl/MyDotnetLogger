using System;
using My.Dotnet.Logger.TableStorage.Extensions.Struct;
using Serilog.Events;
using Serilog.Sinks.AzureTableStorage.KeyGenerator;

namespace My.Dotnet.Logger.KeyGenerator
{
    public class TableStorageKeyGenerator : IKeyGenerator
    {
        protected long RowId;

        public TableStorageKeyGenerator()
        {
            this.RowId = 0L;
        }

        public virtual string GeneratePartitionKey(LogEvent logEvent)
        {
            return DateTime.Now.ToDateString();
        }

        public virtual string GenerateRowKey(LogEvent logEvent, string suffix = null)
        {
            return DateTime.Now.ToTimeString();
        }
    }
}