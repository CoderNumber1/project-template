using System;
using System.Collections.Generic;
using System.Text;

namespace InsertNamespace.Logging
{
    public class LogEvent<TDetail> : LogEvent
    {
        public TDetail Detail { get; set; }
    }

    public class LogEvent
    {
        public LogLevel LogLevel { get; set; }

        public int NumericLogLevel { get { return (int)LogLevel; } }

        public LogEventClassification Classification { get; set; }

        public LogEventContext Context { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public string Message { get; set; }
    }
}
