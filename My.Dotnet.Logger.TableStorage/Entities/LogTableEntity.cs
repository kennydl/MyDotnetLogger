using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json.Linq;

namespace My.Dotnet.Logger.TableStorage.Entities
{
    public class LogTableEntity : TableEntity
    {
        public string MessageTemplate { get; set; }
        public string Level { get; set; }
        public string RenderedMessage { get; set; }
        public string Exception { get; set; }
        public string Data { get; set; }
    }
}
