using System;
using Microsoft.Extensions.Logging;
using My.Dotnet.Logger.Enricher;
using Serilog.Context;

namespace My.Dotnet.Logger
{
    public static class LoggerExtensions
    {
        public static Logger<T> AddProperty<T>(this Logger<T> logger, string name, object value, bool destructureObject = true)
        {
            var enricher = new TableStorageEnricher(name, value, destructureObject);
            var bookmark = LogContext.Push(enricher);
            enricher.AddBookmark(bookmark);
            return logger;
        }

        public static Logger<T> AddProperty<T>(this Logger<T> logger, string name, object value, out IDisposable bookmark, bool destructureObject = true)
        {
            var enricher = new TableStorageEnricher(name, value, destructureObject);
            bookmark = LogContext.Push(enricher);
            return logger;
        }
    }
}