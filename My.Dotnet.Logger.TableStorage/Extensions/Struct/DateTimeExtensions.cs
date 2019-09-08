using System;

namespace My.Dotnet.Logger.TableStorage.Extensions.Struct
{
    internal static class DateTimeExtensions
    {
        public static string ToDateString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        public static string ToTimeString(this DateTime date)
        {
            return date.ToString("HH:mm:ss.ffff");
        }
    }
}
