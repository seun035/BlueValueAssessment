using BlueValueAssessment.Core.Models;
using BlueValueAssessment.Core.Services;
using BlueValueAssessment.UIApi.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueValueAssessment.UIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly IValidatorFactory _validationFactory;

        public AdminController(IRequestService requestService, IValidatorFactory validationFactory)
        {
            _requestService = requestService;
            _validationFactory = validationFactory;
        }

        [HttpGet("request/{requestId}")]
        public async Task<IActionResult> GetRequest(Guid requestId)
        {
            return Ok(await _requestService.GetRequestAsync(requestId));
        }

        [HttpGet("requests")]
        public async Task<IActionResult> GetRequests()
        {
            return Ok(await _requestService.GetRequestsAsync());
        }

        [HttpGet("requests-by-date")]
        public async Task<IActionResult> GetRequests([FromQuery]DateRange dateRange)
        {
            await _validationFactory.ValidateAsync(dateRange);

            return Ok(await _requestService.GetAllByDateAsync(dateRange));
        }

        [HttpGet("requests-overview")]
        public async Task<IActionResult> GetRequests([FromQuery] string date)
        {
            await _validationFactory.ValidateAsync(date);

            return Ok(await _requestService.GetCountByDateAsync(DateTime.ParseExact(date, "dd-MM-yyyy", null)));
        }

        [HttpDelete("request/{requestId}/delete")]
        public async Task<IActionResult> Delete(Guid requestId)
        {
            return Ok(await _requestService.DeleteAsync(requestId));
        }
    }
}
