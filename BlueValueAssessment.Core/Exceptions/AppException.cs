using System;
using System.Collections.Generic;
using System.Text;

namespace BlueValueAssessment.Core.Exceptions
{
    public class AppException : ApplicationException
    {
        public AppException(int statusCode, string friendlyMessage)
        {
            StatusCode = statusCode;
            FriendlyMessage = friendlyMessage;
        }

        public int StatusCode { get; set; }

        public object FriendlyMessage { get; set; }
    }
}
