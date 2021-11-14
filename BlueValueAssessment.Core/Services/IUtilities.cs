using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlueValueAssessment.Core.Services
{
    public interface IUtilities
    {
        Task<HttpResponseMessage> MakeHttpRequest(object request, string baseAddress, string requestUri, HttpMethod method, Dictionary<string, string> headers = null);
    }
}
