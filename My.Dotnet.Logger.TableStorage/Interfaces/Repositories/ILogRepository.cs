using Microsoft.Azure.Cosmos.Table;
using My.Dotnet.Logger.TableStorage.Dto.Response;
using My.Dotnet.Logger.TableStorage.Models;
using System.Threading.Tasks;

namespace My.Dotnet.Logger.TableStorage.Interfaces.Repositories
{
    public interface ILogRepository
    {
        Task<LogResponse> GetAllAsync(LogFilterRequest filterRequest);
        Task<LogResponse> GetSegmentedResultAsync(LogFilterRequest filterRequest, TableContinuationToken continuationToken = null);        
    }
}
