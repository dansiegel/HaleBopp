using System;
using System.Collections.Generic;

namespace HaleBopp.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Debug(string message, IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public void Info(string message, IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public void Log(string message, IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public void Report(Exception ex, IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public void Report(string message, Exception ex, IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message, IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
