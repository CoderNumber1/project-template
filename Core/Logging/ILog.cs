using System;
using System.Collections.Generic;
using System.Text;

namespace InsertNamespace.Logging
{
    public interface ILog
    {
        void Write(LogLevel level, string message, string category);

        void Write<TDetail>(LogLevel level, TDetail detail, string category, string message = null);

        void Write(LogLevel level, string message, LogEventClassification classification);

        void Write<TDetail>(LogLevel level, TDetail detail, LogEventClassification classification, string message = null);
    }
}
