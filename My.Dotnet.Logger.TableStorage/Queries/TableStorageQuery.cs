using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace My.Dotnet.Logger.TableStorage.Queries
{
    public abstract class TableStorageQuery
    {
        public string CombineAllQueries(string[] filters, string operatorString, bool addParantheses = true)
        {
            filters = filters.Where(f => !string.IsNullOrEmpty(f)).ToArray();
            if (filters.Length < 2) return filters.FirstOrDefault();
            if (addParantheses) return string.Join($" {operatorString} ", filters.Select(f => "(" + f + ")").ToArray());
            return string.Join($" {operatorString} ", filters);
        }

        public string KeyQuery(string columnName, string keyFrom, string keyTo)
        {
            var filterFrom = TableQuery.GenerateFilterCondition(columnName, QueryComparisons.GreaterThanOrEqual, keyFrom);
            var filterTo = TableQuery.GenerateFilterCondition(columnName, QueryComparisons.LessThanOrEqual, keyTo);

            if (!string.IsNullOrEmpty(keyFrom) && !string.IsNullOrEmpty(keyTo))
            {
                return TableQuery.CombineFilters(filterFrom, TableOperators.And, filterTo);
            }
            if (!string.IsNullOrEmpty(keyFrom)) return filterFrom;
            if (!string.IsNullOrEmpty(keyTo)) return filterTo;

            return string.Empty;
        }

        public string DateTimeOffsetQuery(string columnName, DateTimeOffset timeFrom, DateTimeOffset timeTo)
        {
            var filterFrom = TableQuery.GenerateFilterConditionForDate(columnName, QueryComparisons.GreaterThanOrEqual, timeFrom);
            var filterTo = TableQuery.GenerateFilterConditionForDate(columnName, QueryComparisons.LessThanOrEqual, timeTo);

            if (timeFrom != default && timeTo != default)
            {
                return TableQuery.CombineFilters(filterFrom, TableOperators.And, filterTo);
            }
            if (timeFrom != default) return filterFrom;
            if (timeTo != default) return filterTo;

            return string.Empty;
        }

        public string LogLevelQuery(string columnName, LogLevel level)
        {
            return level == LogLevel.None ?
                TableQuery.GenerateFilterCondition(columnName, QueryComparisons.NotEqual, LogLevel.None.ToString()) :
                TableQuery.GenerateFilterCondition(columnName, QueryComparisons.Equal, level.ToString());
        }

        public string StringQuery()
        {
            return "";
        }
    }
}
