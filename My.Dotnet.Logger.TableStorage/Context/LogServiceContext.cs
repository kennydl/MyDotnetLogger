﻿namespace My.Dotnet.Logger.TableStorage.Context
{
    public interface ILogServiceContext
    {
        string TableName { get; set; } 
    }

    public class LogServiceContext : ILogServiceContext
    {
        public string TableName {get; set;}
    }
}