using My.Dotnet.Logger.Enricher;
using Serilog.Context;
using System;

namespace My.Dotnet.Logger.Extensions
{
    public static class SeriLogExtension
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