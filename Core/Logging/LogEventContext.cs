using System;
using System.Collections.Generic;
using System.Text;

namespace InsertNamespace.Logging
{
    public class LogEventContext
    {
        // Server Information
        public string MachineName { get; set; }

        public string MachineOS { get; set; }

        public string MachineIpAddress { get; set; }

        // User Information
        public string SubjectId { get; set; }

        // User Agent Information
        public string UserIpAddress { get; set; }

        public string UserAgent { get; set; }
    }
}
