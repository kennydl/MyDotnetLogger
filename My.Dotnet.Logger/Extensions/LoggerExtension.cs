using System;
using Microsoft.Extensions.Logging;
using My.Dotnet.Logger.Enricher;
using Serilog.Context;

namespace My.Dotnet.Logger.Extensions
{
    public static class LoggerExtension
    {
        private static string dataPropertyName = "Data";
        private static string errorPropertyName = "Error";

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

        public static ILogger<T> LogInformation<T>(this ILogger<T> logger, object data, string message, params object[] args)
        {
            logger.AddProperty(dataPropertyName, data);
            logger.LogInformation(message, args);
            return logger;
        }

        public static ILogger<T> LogInformation<T>(this ILogger<T> logger, object data, Exception exception, string message, params object[] args)
        {
            logger.AddProperty(dataPropertyName, data);
            logger.LogInformation(exception, message, args);
            return logger;
        }

        public static ILogger<T> LogInformation<T>(this ILogger<T> logger, string name, object data, string message, params object[] args)
        {
            logger.AddProperty(name, data);
            logger.LogInformation(message, args);
            return logger;
        }

        public static ILogger<T> LogInformation<T>(this ILogger<T> logger, string name, object data, Exception exception, string message, params object[] args)
        {
            logger.AddProperty(name, data);
            logger.LogInformation(exception, message, args);
            return logger;
        }

        public static ILogger<T> LogDebug<T>(this ILogger<T> logger, object data, string message, params object[] args)
        {
            logger.AddProperty(dataPropertyName, data);
            logger.LogDebug(message, args);
            return logger;
        }

        public static ILogger<T> LogDebug<T>(this ILogger<T> logger, object data, Exception exception, string message, params object[] args)
        {
            logger.AddProperty(dataPropertyName, data);
            logger.LogDebug(exception, message, args);
            return logger;
        }

        public static ILogger<T> LogDebug<T>(this ILogger<T> logger, string name, object data, string message, params object[] args)
        {
            logger.AddProperty(name, data);
            logger.LogDebug(message, args);
            return logger;
        }

        public static ILogger<T> LogDebug<T>(this ILogger<T> logger, string name, object data, Exception exception, string message, params object[] args)
        {
            logger.AddProperty(name, data);
            logger.LogDebug(exception, message, args);
            return logger;
        }

        public static ILogger<T> LogWarning<T>(this ILogger<T> logger, object data, string message, params object[] args)
        {
            logger.AddProperty(dataPropertyName, data);
            logger.LogWarning(message, args);
            return logger;
        }

        public static ILogger<T> LogWarning<T>(this ILogger<T> logger, object data, Exception exception, string message, params object[] args)
        {
            logger.AddProperty(dataPropertyName, data);
            logger.LogWarning(exception: exception, message, args);
            return logger;
        }


        public static ILogger<T> LogWarning<T>(this ILogger<T> logger, string name, object data, string message, params object[] args)
        {
            logger.AddProperty(name, data);
            logger.LogWarning(message, args);
            return logger;
        }

        public static ILogger<T> LogWarning<T>(this ILogger<T> logger, string name, object data, Exception exception, string message, params object[] args)
        {
            logger.AddProperty(name, data);
            logger.LogWarning(exception: exception, message, args);
            return logger;
        }

        public static ILogger<T> LogError<T>(this ILogger<T> logger, object error, string message, params object[] args)
        {
            logger.AddProperty(errorPropertyName, error);
            logger.LogError(message, args);
            return logger;
        }

        public static ILogger<T> LogError<T>(this ILogger<T> logger, object error, Exception exception, string message, params object[] args)
        {
            logger.AddProperty(errorPropertyName, error);
            logger.LogError(exception: exception, message, args);
            return logger;
        }

        public static ILogger<T> LogError<T>(this ILogger<T> logger, string name, object error, string message, params object[] args)
        {
            logger.AddProperty(name, error);
            logger.LogError(message, args);
            return logger;
        }

        public static ILogger<T> LogError<T>(this ILogger<T> logger, string name, object error, Exception exception, string message, params object[] args)
        {
            logger.AddProperty(name, error);
            logger.LogError(exception: exception, message, args);
            return logger;
        }

        public static ILogger<T> LogCritical<T>(this ILogger<T> logger, object error, string message, params object[] args)
        {
            logger.AddProperty(errorPropertyName, error);
            logger.LogCritical(message, args);
            return logger;
        }

        public static ILogger<T> LogCritical<T>(this ILogger<T> logger, object error, Exception exception, string message, params object[] args)
        {
            logger.AddProperty(errorPropertyName, error);
            logger.LogCritical(exception: exception, message, args);
            return logger;
        }

        public static ILogger<T> LogCritical<T>(this ILogger<T> logger, string name, object error, string message, params object[] args)
        {
            logger.AddProperty(name, error);
            logger.LogCritical(message, args);
            return logger;
        }

        public static ILogger<T> LogCritical<T>(this ILogger<T> logger, string name, object error, Exception exception, string message, params object[] args)
        {
            logger.AddProperty(name, error);
            logger.LogCritical(exception: exception, message, args);
            return logger;
        }
    }
}