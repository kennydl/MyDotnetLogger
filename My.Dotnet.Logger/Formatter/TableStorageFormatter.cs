using System;
using System.Globalization;

namespace My.Dotnet.Logger.Formatter
{
    public class TableStorageFormatter : IFormatProvider
    {
        private readonly IFormatProvider _basedOn;
        private readonly string _shortDatePattern;
        public TableStorageFormatter(string shortDatePattern, IFormatProvider basedOn = null)
        {
            _shortDatePattern = shortDatePattern;
            _basedOn = basedOn ?? CultureInfo.CurrentCulture;
        }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(DateTimeFormatInfo))
            {
                var basedOnFormatInfo = (DateTimeFormatInfo)_basedOn.GetFormat(formatType);
                var dateFormatInfo = (DateTimeFormatInfo)basedOnFormatInfo.Clone();
                dateFormatInfo.ShortDatePattern = _shortDatePattern;
                return dateFormatInfo;
            }
            return _basedOn.GetFormat(formatType);
        }
    }
}