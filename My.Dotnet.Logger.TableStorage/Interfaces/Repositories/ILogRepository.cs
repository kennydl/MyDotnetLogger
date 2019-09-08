using Microsoft.Azure.Cosmos.Table;
using My.Dotnet.Logger.TableStorage.Dto.Response;
using My.Dotnet.Logger.TableStorage.Models;
using System.Threading.Tasks;

namespace My.Dotnet.Logger.TableStorage.Interfaces.Repositories
{
    public interface ILogRepository
    {
        LogResponse GetAll(LogFilterRequest filterRequest);
        Task<LogResponse> GetSegmentedFilterAsync(LogFilterRequest filterRequest, TableContinuationToken continuationToken = null);        
    }
}
