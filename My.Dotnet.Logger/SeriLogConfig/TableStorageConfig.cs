using System;
using System.Globalization;
using Microsoft.WindowsAzure.Storage;
using My.Dotnet.Logger.Formatter;
using My.Dotnet.Logger.KeyGenerator;
using Serilog;

namespace My.Dotnet.Logger.SeriLogConfig
{
    public static class TableStorageConfig
    {
        public static LoggerConfiguration AddAzureTableStorageLogger(this LoggerConfiguration log, CloudStorageAccount storageAccount, string tableName)
        {
            log.WriteTo.AzureTableStorage(
                    storageAccount,
                    storageTableName: tableName,
                    keyGenerator: new TableStorageKeyGenerator()
                )
                .Enrich.WithProperty("Test", new { PropOne = "sad", PropTwo = "woop" }, true)
                .Enrich.FromLogContext();

            return log;
        }
    }
}