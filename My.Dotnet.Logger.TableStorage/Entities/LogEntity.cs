using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json.Linq;

namespace My.Dotnet.Logger.TableStorage.Entities
{
    public class LogEntity : TableEntity
    {
        public string MessageTemplate { get; set; }
        public string Level { get; set; }
        public string RenderedMessage { get; set; }
        public JObject Data { get; set; }
    }
}
