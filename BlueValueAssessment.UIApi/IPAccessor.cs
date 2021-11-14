using BlueValueAssessment.Core.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueValueAssessment.UIApi
{
    public class IPAccessor: IIPAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IPAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetIP()
        {
            var address = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            return address;
        }
    }
}
