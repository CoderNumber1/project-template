using System;
using System.Collections.Generic;
using System.Text;

namespace InsertNamespace.Logging
{
    [Flags]
    public enum LogLevel
    {
        Verbose = 0,
        Debug = 1,
        Information = 2,
        Warning = 4,
        Error = 8,
        Fatal = 16
    }
}
