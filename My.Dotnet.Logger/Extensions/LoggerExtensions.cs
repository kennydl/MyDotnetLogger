using System;
using Microsoft.Extensions.Logging;
using My.Dotnet.Logger.Enricher;
using Serilog.Context;

namespace My.Dotnet.Logger.Extensions
{
    public static class LoggerExtensions
    {
        private static string dataPropertyName = "data";
        private static string errorPropertyName = "error";

        public static ILogger<T> AddProperty<T>(this ILogger<T> logger, string name, object property, bool destructureObject = true)
        {
            var enricher = new TableStorageEnricher(name, property, destructureObject);
            var bookmark = LogContext.Push(enricher);
            enricher.AddBookmark(bookmark);
            return logger;
        }

        public static ILogger<T> AddProperty<T>(this ILogger<T> logger, string name, object property, out IDisposable bookmark, bool destructureObject = true)
        {
            var enricher = new TableStorageEnricher(name, property, destructureObject);
            bookmark = LogContext.Push(enricher);
            return logger;
        }

        public static ILogger<T> LogInformation<T>(this ILogger<T> logger, object property, string message, params object[] args)
        {
            logger.AddProperty(dataPropertyName, property);
            logger.LogInformation(message, args);
            return logger;
        }

        public static ILogger<T> LogInformation<T>(this ILogger<T> logger, object property, Exception exception, string message, params object[] args)
        {
            logger.AddProperty(dataPropertyName, property);
            logger.LogInformation(exception, message, args);
            return logger;
        }

        public static ILogger<T> LogInformation<T>(this ILogger<T> logger, string name, object property, string message, params object[] args)
        {
            logger.AddProperty(name, property);
            logger.LogInformation(message, args);
            return logger;
        }

        public static ILogger<T> LogInformation<T>(this ILogger<T> logger, string name, object property, Exception exception, string message, params object[] args)
        {
            logger.AddProperty(name, property);
            logger.LogInformation(exception, message, args);
            return logger;
        }

        public static ILogger<T> LogDebug<T>(this ILogger<T> logger, object property, string message, params object[] args)
        {
            logger.AddProperty(dataPropertyName, property);
            logger.LogDebug(message, args);
            return logger;
        }

        public static ILogger<T> LogDebug<T>(this ILogger<T> logger, object property, Exception exception, string message, params object[] args)
        {
            logger.AddProperty(dataPropertyName, property);
            logger.LogDebug(exception, message, args);
            return logger;
        }

        public static ILogger<T> LogDebug<T>(this ILogger<T> logger, string name, object property, string message, params object[] args)
        {
            logger.AddProperty(name, property);
            logger.LogDebug(message, args);
            return logger;
        }

        public static ILogger<T> LogDebug<T>(this ILogger<T> logger, string name, object property, Exception exception, string message, params object[] args)
        {
            logger.AddProperty(name, property);
            logger.LogDebug(exception, message, args);
            return logger;
        }

        public static ILogger<T> LogWarning<T>(this ILogger<T> logger, object property, string message, params object[] args)
        {
            logger.AddProperty(dataPropertyName, property);
            logger.LogWarning(message, args);
            return logger;
        }

        public static ILogger<T> LogWarning<T>(this ILogger<T> logger, object property, Exception exception, string message, params object[] args)
        {
            logger.AddProperty(dataPropertyName, property);
            logger.LogWarning(exception: exception, message, args);
            return logger;
        }


        public static ILogger<T> LogWarning<T>(this ILogger<T> logger, string name, object property, string message, params object[] args)
        {
            logger.AddProperty(name, property);
            logger.LogWarning(message, args);
            return logger;
        }

        public static ILogger<T> LogWarning<T>(this ILogger<T> logger, string name, object property, Exception exception, string message, params object[] args)
        {
            logger.AddProperty(name, property);
            logger.LogWarning(exception: exception, message, args);
            return logger;
        }

        public static ILogger<T> LogError<T>(this ILogger<T> logger, object property, string message, params object[] args)
        {
            logger.AddProperty(errorPropertyName, property);
            logger.LogError(message, args);
            return logger;
        }

        public static ILogger<T> LogError<T>(this ILogger<T> logger, object property, Exception exception, string message, params object[] args)
        {
            logger.AddProperty(errorPropertyName, property);
            logger.LogError(exception: exception, message, args);
            return logger;
        }

        public static ILogger<T> LogError<T>(this ILogger<T> logger, string name, object property, string message, params object[] args)
        {
            logger.AddProperty(name, property);
            logger.LogError(message, args);
            return logger;
        }

        public static ILogger<T> LogError<T>(this ILogger<T> logger, string name, object property, Exception exception, string message, params object[] args)
        {
            logger.AddProperty(name, property);
            logger.LogError(exception: exception, message, args);
            return logger;
        }

        public static ILogger<T> LogCritical<T>(this ILogger<T> logger, object property, string message, params object[] args)
        {
            logger.AddProperty(errorPropertyName, property);
            logger.LogCritical(message, args);
            return logger;
        }

        public static ILogger<T> LogCritical<T>(this ILogger<T> logger, object property, Exception exception, string message, params object[] args)
        {
            logger.AddProperty(errorPropertyName, property);
            logger.LogCritical(exception: exception, message, args);
            return logger;
        }

        public static ILogger<T> LogCritical<T>(this ILogger<T> logger, string name, object property, string message, params object[] args)
        {
            logger.AddProperty(name, property);
            logger.LogCritical(message, args);
            return logger;
        }

        public static ILogger<T> LogCritical<T>(this ILogger<T> logger, string name, object property, Exception exception, string message, params object[] args)
        {
            logger.AddProperty(name, property);
            logger.LogCritical(exception: exception, message, args);
            return logger;
        }
    }
}