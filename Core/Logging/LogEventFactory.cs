using System;
using System.Collections.Generic;
using System.Text;

namespace InsertNamespace.Logging
{
    public class LogEventFactory : ILogEventFactory
    {
        private IContextProvider _contextProvider;

        public LogEventFactory(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public LogEventClassification CreateClassification(string category, string subCategory = null, int? eventId = null)
        {
            return new LogEventClassification()
            {
                Category = category,
                SubCategory = subCategory,
                EventId = eventId
            };
        }

        public LogEventContext CreateContext()
        {
            return new LogEventContext()
            {
                MachineIpAddress = _contextProvider.GetMachineIpAddress(),
                MachineName = _contextProvider.GetMachineName(),
                MachineOS = _contextProvider.GetMachineOS(),
                SubjectId = _contextProvider.GetSubjectId(),
                UserAgent = _contextProvider.GetUserAgent(),
                UserIpAddress = _contextProvider.GetUserIpAddress()
            };
        }

        public LogEvent CreateEvent(LogLevel level, string message = null, LogEventClassification classification = null, LogEventContext context = null)
        {
            return new LogEvent()
            {
                LogLevel = level,
                Message = message,
                Timestamp = DateTimeOffset.UtcNow,
                Classification = classification ?? CreateClassification(null),
                Context = CreateContext()
            };
        }

        public LogEvent CreateEvent<TDetail>(LogLevel level, TDetail detail, string message = null, LogEventClassification classification = null, LogEventContext context = null)
        {
            return new LogEvent<TDetail>()
            {
                LogLevel = level,
                Detail = detail,
                Message = message,
                Timestamp = DateTimeOffset.UtcNow,
                Classification = classification ?? CreateClassification(null),
                Context = CreateContext()
            };
        }
    }
}
