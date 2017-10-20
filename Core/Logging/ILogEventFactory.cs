using System;
using System.Collections.Generic;
using System.Text;

namespace InsertNamespace.Logging
{
    public interface ILogEventFactory
    {
        LogEvent CreateEvent(LogLevel level, string message = null, LogEventClassification classification = null, LogEventContext context = null);

        LogEvent CreateEvent<TDetail>(
            LogLevel level, 
            TDetail detail, 
            string message = null, 
            LogEventClassification classification = null, 
            LogEventContext context = null);

        LogEventClassification CreateClassification(string category, string subCategory = null, int? eventId = null);

        LogEventContext CreateContext();
    }
}
