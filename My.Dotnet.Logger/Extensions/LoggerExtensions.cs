﻿using System;
using Microsoft.Extensions.Logging;
using My.Dotnet.Logger.Enricher;
using Serilog.Context;

namespace My.Dotnet.Logger.Extensions
{
    public static class LoggerExtensions
    {
        public static ILogger<T> AddProperty<T>(this ILogger<T> logger, string name, object value, bool destructureObject = true)
        {
            var enricher = new TableStorageEnricher(name, value, destructureObject);
            var bookmark = LogContext.Push(enricher);
            enricher.AddBookmark(bookmark);
            return logger;
        }

        public static ILogger<T> AddProperty<T>(this ILogger<T> logger, string name, object value, out IDisposable bookmark, bool destructureObject = true)
        {
            var enricher = new TableStorageEnricher(name, value, destructureObject);
            bookmark = LogContext.Push(enricher);
            return logger;
        }

        public static ILogger<T> LogInformation<T>(this ILogger<T> logger, object value, string message, params object[] args)
        {
            logger.AddProperty("property", value);
            logger.LogInformation(message, args);
            return logger;
        }

        public static ILogger<T> LogInformation<T>(this ILogger<T> logger, object value, Exception exception, string message, params object[] args)
        {
            logger.AddProperty("property", value);
            logger.LogInformation(exception: exception, message, args);
            return logger;
        }

        public static ILogger<T> LogDebug<T>(this ILogger<T> logger, object value, string message, params object[] args)
        {
            logger.AddProperty("property", value);
            logger.LogDebug(message, args);
            return logger;
        }

        public static ILogger<T> LogDebug<T>(this ILogger<T> logger, object value, Exception exception, string message, params object[] args)
        {
            logger.AddProperty("property", value);
            logger.LogDebug(exception: exception, message, args);
            return logger;
        }

        public static ILogger<T> LogWarning<T>(this ILogger<T> logger, object value, string message, params object[] args)
        {
            logger.AddProperty("property", value);
            logger.LogWarning(message, args);
            return logger;
        }

        public static ILogger<T> LogWarning<T>(this ILogger<T> logger, object value, Exception exception, string message, params object[] args)
        {
            logger.AddProperty("property", value);
            logger.LogWarning(exception: exception, message, args);
            return logger;
        }

        public static ILogger<T> LogError<T>(this ILogger<T> logger, object value, string message, params object[] args)
        {
            logger.AddProperty("property", value);
            logger.LogError(message, args);
            return logger;
        }

        public static ILogger<T> LogError<T>(this ILogger<T> logger, object value, Exception exception, string message, params object[] args)
        {
            logger.AddProperty("property", value);
            logger.LogError(exception: exception, message, args);
            return logger;
        }

        public static ILogger<T> LogCritical<T>(this ILogger<T> logger, object value, string message, params object[] args)
        {
            logger.AddProperty("property", value);
            logger.LogCritical(message, args);
            return logger;
        }

        public static ILogger<T> LogCritical<T>(this ILogger<T> logger, object value, Exception exception, string message, params object[] args)
        {
            logger.AddProperty("property", value);
            logger.LogCritical(exception: exception, message, args);
            return logger;
        }
    }
}