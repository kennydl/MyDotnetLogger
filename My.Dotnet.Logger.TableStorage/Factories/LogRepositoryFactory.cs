using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using My.Dotnet.Logger.Data.Context;
using My.Dotnet.Logger.TableStorage.Interfaces.Repositories;
using My.Dotnet.Logger.TableStorage.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace My.Dotnet.Logger.TableStorage.Factories
{
    public interface ILogRepositoryFactory
    {
        ILogRepository CreateRepository();
    }

    public class LogRepositoryFactory : ILogRepositoryFactory
    {
        private readonly ILogServiceContext _context;
        private readonly ILogger<ILogRepositoryFactory> _logger;
        private readonly CloudStorageAccount _storageAccount;        

        public LogRepositoryFactory(ILogServiceContext context, ILogger<ILogRepositoryFactory> logger, CloudStorageAccount storageAccount)
        {
            _context = context;
            _logger = logger;
            _storageAccount = storageAccount;
        }

        public ILogRepository CreateRepository()
        {
            return new LogRepository(_logger, _storageAccount, _context.TableName);
        }
    }
}
