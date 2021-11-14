using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BlueValueAssessment.Core.Exceptions
{
    public class ValidationException : AppException
    {
        public ValidationException(string friendlyMessage = null)
            : base((int)HttpStatusCode.Forbidden, friendlyMessage)
        {
        }
        public ValidationException(IDictionary<string, string> errors, string friendlyMessage = null)
            : this(friendlyMessage)
        {
            Errors = errors;
        }

        public IDictionary<string, string> Errors { get; set; }
    }
}
