using System;
using System.Collections.Generic;
using System.Text;

namespace InsertNamespace.Logging
{
    public class LogEventClassification
    {
        public string Category { get; set; }

        public string SubCategory { get; set; }

        public int? EventId { get; set; }
    }
}
