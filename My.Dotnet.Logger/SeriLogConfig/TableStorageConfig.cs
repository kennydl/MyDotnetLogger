using My.Dotnet.Logger.KeyGenerator;
using My.Dotnet.Logger.TableStorage.Utilities;
using Serilog;

namespace My.Dotnet.Logger.SeriLogConfig
{
    public static class TableStorageConfig
    {
        public static LoggerConfiguration AddAzureTableStorageLogger(this LoggerConfiguration log, string _connectionString, string tableName)
        {
            log.WriteTo.AzureTableStorage(
                AzureStorageUtil.GetLoggerStorageAccount(_connectionString),
                storageTableName: tableName,
                keyGenerator: new TableStorageKeyGenerator()                
            );
            return log;
        }     
    }
}