using System;
using System.Collections.Generic;
using System.Text;

namespace InsertNamespace.Logging
{
    public class Log : ILog
    {
        private ILogEventFactory _eventFactory;

        public Log(ILogEventFactory eventFactory)
        {
            _eventFactory = eventFactory;
        }

        private Serilog.Events.LogEventLevel ToSerilogLevel(LogLevel level)
        {
            switch(level)
            {
                case LogLevel.Verbose:
                    return Serilog.Events.LogEventLevel.Verbose;
                case LogLevel.Debug:
                    return Serilog.Events.LogEventLevel.Debug;
                case LogLevel.Information:
                    return Serilog.Events.LogEventLevel.Information;
                case LogLevel.Warning:
                    return Serilog.Events.LogEventLevel.Warning;
                case LogLevel.Error:
                    return Serilog.Events.LogEventLevel.Error;
                case LogLevel.Fatal:
                    return Serilog.Events.LogEventLevel.Fatal;
                default:
                    return Serilog.Events.LogEventLevel.Verbose;
            }
        }

        public void Write(LogLevel level, string message, string category)
        {
            Serilog.Log.Write(
                ToSerilogLevel(level),
                "{@Event}",
                _eventFactory.CreateEvent(level, message, _eventFactory.CreateClassification(category), _eventFactory.CreateContext()));
        }

        public void Write<TDetail>(LogLevel level, TDetail detail, string category, string message = null)
        {
            Serilog.Log.Write(
                ToSerilogLevel(level),
                "{@Event}",
                _eventFactory.CreateEvent(level, detail, message, _eventFactory.CreateClassification(category), _eventFactory.CreateContext()));
        }

        public void Write(LogLevel level, string message, LogEventClassification classification)
        {
            Serilog.Log.Write(
                ToSerilogLevel(level),
                "{@Event}",
                _eventFactory.CreateEvent(level, message, classification, _eventFactory.CreateContext()));
        }

        public void Write<TDetail>(LogLevel level, TDetail detail, LogEventClassification classification, string message = null)
        {
            Serilog.Log.Write(
                ToSerilogLevel(level),
                "{@Event}",
                _eventFactory.CreateEvent(level, detail, message, classification, _eventFactory.CreateContext()));
        }
    }
}
