using BlueValueAssessment.Core.Services;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BlueValueAssessment.DomainServices.Utilities
{
    public class Utility: IUtilities
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;

        public Utility(IHttpClientFactory httpClientFactory, ILogger logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> MakeHttpRequest(object request, string baseAddress, string requestUri,
            HttpMethod method, Dictionary<string, string> headers = null)
        {
            try
            {
                using (var _httpClient = _httpClientFactory.CreateClient())
                {
                    _httpClient.BaseAddress = new Uri(baseAddress);
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    _httpClient.Timeout = TimeSpan.FromSeconds(180);

                    if (headers != null)
                    {
                        foreach (KeyValuePair<string, string> header in headers)
                        {
                            _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }
                    if (method == HttpMethod.Get)
                    {
                        var response = await _httpClient.GetAsync(requestUri);
                        return response;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Http utillities Exception. Ex: {JsonConvert.SerializeObject(ex)}");
                // Log.Error(ex.ToString());
                throw;
            }
        }

    }
}
