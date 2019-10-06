using System;
using System.Collections.Generic;
using System.Text;

namespace My.Dotnet.Logger.TableStorage.Entities
{
    internal class LogDataTableEntity
    {
        DateTimeOffset Timestamp { get; set; }
        public string Level { get; set; }
        public string MessageTemplate { get; set; }
        public string Properties { get; set; }
    }
}
