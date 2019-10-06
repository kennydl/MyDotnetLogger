using System;
using System.Collections.Generic;
using System.Text;

namespace My.Dotnet.Logger.TableStorage.Entities
{
    public class LogPropertiesEntity
    {
        public string SourceContext { get; set; }
        public string ActionId { get; set; }
        public string ActionName { get; set; }
        public string RequestId { get; set; }
        public string RequestPath { get; set; }
    }
}
