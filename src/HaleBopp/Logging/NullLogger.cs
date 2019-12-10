using System;
using System.Collections.Generic;

namespace HaleBopp.Logging
{
    public class NullLogger : ILogger
    {
        public void Debug(string message, IDictionary<string, object> parameters) { }

        public void Info(string message, IDictionary<string, object> parameters) { }

        public void Log(string message, IDictionary<string, object> parameters) { }

        public void Report(Exception ex, IDictionary<string, object> parameters) { }

        public void Report(string message, Exception ex, IDictionary<string, object> parameters) { }

        public void Warn(string message, IDictionary<string, object> parameters) { }
    }
}
