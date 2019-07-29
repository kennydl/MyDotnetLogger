
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System;
using My.Dotnet.Logger.Enricher;
using My.Dotnet.Logger.KeyGenerator;
using Serilog.Context;

namespace My.Dotnet.Logger.Extensions
{
    public static class SeriLogExtensions
    {
        public static Serilog.Core.Logger AddProperty(this Serilog.Core.Logger logger, string name, object value, bool destructureObject = true)
        {
            var enricher = new TableStorageEnricher(name, value, destructureObject);
            var bookmark = LogContext.Push(enricher);
            enricher.AddBookmark(bookmark);
            return logger;
        }

        public static Serilog.Core.Logger AddProperty(this Serilog.Core.Logger logger, string name, object value, out IDisposable bookmark, bool destructureObject = true)
        {
            var enricher = new TableStorageEnricher(name, value, destructureObject);
            bookmark = LogContext.Push(enricher);
            return logger;
        }
    }
}