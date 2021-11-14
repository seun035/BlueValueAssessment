using System;
using System.Collections.Generic;
using System.Text;

namespace BlueValueAssessment.Core.Models
{
    public class BaseResponse<T>
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
