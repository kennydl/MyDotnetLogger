using Serilog.Core;
using Serilog.Events;
using System;

namespace My.Dotnet.Logger.Enricher
{
    public class TableStorageEnricher : ILogEventEnricher
    {
        private readonly string _name;
        private readonly object _value;
        private readonly bool _destructureObjects;
        private IDisposable _bookmark;

        public TableStorageEnricher(string name, object value, bool destructureObjects = true)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Property name must not be null or empty.", nameof(name));
            _name = name;
            _value = value;
            _destructureObjects = destructureObjects;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));
            if (propertyFactory == null) throw new ArgumentNullException(nameof(propertyFactory));
            var property = propertyFactory.CreateProperty(_name, _value, _destructureObjects);
            logEvent.AddPropertyIfAbsent(property);

            // Dispose this property later. That is remove the property from structure
            _bookmark?.Dispose(); 
        }

        public void AddBookmark(IDisposable bookmark)
        {
            _bookmark = bookmark;
        }
    }
}