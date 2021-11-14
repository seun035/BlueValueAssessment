using System;
using System.Collections.Generic;
using System.Text;

namespace BlueValueAssessment.Core.Documents
{
    public class RequestDocument: BaseDocument
    {
        public string SearchToken { get; set; }

        public string ImdbID { get; set; }

        public long ProcessingTimeMs { get; set; }

        public DateTime TimeStamp { get; set; }

        public string IpAddress { get; set; }
    }
}
