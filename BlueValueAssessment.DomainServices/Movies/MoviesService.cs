using BlueValueAssessment.Core.Configs;
using BlueValueAssessment.Core.Helpers;
using BlueValueAssessment.Core.Models;
using BlueValueAssessment.Core.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlueValueAssessment.DomainServices.Movies
{
    public class MoviesService: IMoviesService
    {
        private readonly IUtilities _utilities;
        private readonly IOptions<OmdbConfig> _omdbConfig;
        private readonly IIPAccessor _iPAccessor;
        private readonly IRequestService _requestService;
        private readonly ILogger _logger;

        public MoviesService(IUtilities utilities, IOptions<OmdbConfig> omdbConfig, IIPAccessor iPAccessor, IRequestService requestService, ILogger logger)
        {
            _utilities = utilities;
            _omdbConfig = omdbConfig;
            _iPAccessor = iPAccessor;
            _requestService = requestService;
            _logger = logger;
        }

        public async Task<BaseResponse<SearchResult>> SearchMoviesAsync(SearchQuery searchQuery)
        {
            var response = new BaseResponse<SearchResult>();
            var ip = _iPAccessor.GetIP();
            var request = new Request
            {
                IpAddress = ip,
                SearchToken = searchQuery.Title,
                TimeStamp = DateTime.UtcNow
            };

            try
            {
                var query = $"?apikey={_omdbConfig.Value.ApiKey}&t={searchQuery.Title}";

                var httpResponse = await _utilities.MakeHttpRequest(null, _omdbConfig.Value.BaseUrl, query, HttpMethod.Get);

                if (httpResponse != null && httpResponse.IsSuccessStatusCode)
                {
                    var respAsString = await httpResponse.Content.ReadAsStringAsync();

                    _logger.Information($"Request title: {searchQuery.Title} || Response: {respAsString}");

                    var result = JsonConvert.DeserializeObject<SearchResult>(respAsString);
                    response.Data = result;
                    response.StatusCode = 200;
                    response.Message = ResponseMessage.OperationSuccessful;

                    request.ImdbID = result.ImdbID;
                }
                else
                {
                    response.StatusCode = (int)httpResponse.StatusCode;
                    response.Message = ResponseMessage.OperationFailed;
                }
                
            }
            catch (Exception ex)
            {
                throw;
            }

            request.ProcessingTimeMs = Convert.ToInt64(DateTime.UtcNow.Subtract(request.TimeStamp).TotalMilliseconds);
            await _requestService.SaveRequestAsync(request);
           
            return response;
        }
    }
}
