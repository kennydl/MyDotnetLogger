using Microsoft.Azure.Cosmos.Table;
using My.Dotnet.Logger.TableStorage.Entities;
using System.Collections.Generic;

namespace My.Dotnet.Logger.TableStorage.Dto.Response
{
    public class LogResponse
    {
        public IEnumerable<LogEntity> Results { get; set; }
        public bool HasRows { get; set; }
        public TableContinuationToken ContinuationToken { get; internal set; }
    }
}
