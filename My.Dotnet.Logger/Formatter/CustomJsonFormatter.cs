using My.Dotnet.Logger.Utilities;
using System;

namespace My.Dotnet.Logger.Formatter
{
    public class CustomJsonFormatter : IFormatProvider
    {
        public object GetFormat(Type formatType)
        {
            return this;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            return JsonSerializer.SerializeObject(arg);
        }
    }
}
