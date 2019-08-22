using System;
using System.Collections.Generic;
using System.Text;

namespace My.Dotnet.Logger.Data.Context
{
    public interface ILogServiceContext
    {
        string TableName { get; }
    }

    public class LogServiceContext : ILogServiceContext
    {
        public string TableName {get; set;}
    }
}
