using System;
using System.Collections.Generic;
using System.Text;

namespace HaleBopp.Logging
{
    public interface ILogger
    {
        void Debug(string message, IDictionary<string, object> parameters);
        void Info(string message, IDictionary<string, object> parameters);
        void Warn(string message, IDictionary<string, object> parameters);
        void Log(string message, IDictionary<string, object> parameters);
        void Report(Exception ex, IDictionary<string, object> parameters);
        void Report(string message, Exception ex, IDictionary<string, object> parameters);
    }
}
