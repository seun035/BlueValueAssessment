using BlueValueAssessment.Core.Services;
using BlueValueAssessment.UIApi.Dtos;
using BlueValueAssessment.UIApi.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueValueAssessment.UIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;
        private readonly IValidatorFactory _validationFactory;
        private readonly ILogger _logger;

        public MoviesController(IMoviesService moviesService, IValidatorFactory validationFactory, ILogger logger)
        {
            _moviesService = moviesService;
            _validationFactory = validationFactory;
            _logger = logger;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery]SearchQueryDto searchQueryDto)
        {
            _logger.Information($"Search begins at {DateTime.UtcNow}, Movie title {searchQueryDto.Title}");
            await _validationFactory.ValidateAsync(searchQueryDto);
            _logger.Information($"Validation Passed fro title {searchQueryDto.Title}");

            var response = await _moviesService.SearchMoviesAsync(new Core.Models.SearchQuery { Title = searchQueryDto.Title });

            _logger.Information($"Search response for title {searchQueryDto.Title} Res: {JsonConvert.SerializeObject(response)} at {DateTime.UtcNow}");
            return Ok(response);
        }
    }
}
