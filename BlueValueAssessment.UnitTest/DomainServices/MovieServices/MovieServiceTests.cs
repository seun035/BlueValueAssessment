using BlueValueAssessment.Core.Configs;
using BlueValueAssessment.Core.Models;
using BlueValueAssessment.Core.Services;
using BlueValueAssessment.DomainServices.Movies;
using Microsoft.Extensions.Options;
using Moq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlueValueAssessment.UnitTest.DomainServices.MovieServices
{
    public class MovieServiceTests
    {
        [Fact]
        public async Task SearchMoviesAsync_Return_Success()
        {
            var searchQuery = new SearchQuery
            {
                Title = "Test"
            };

            var query = $"?apikey=testapikey&t={searchQuery.Title}";

            var ipMock = new Mock<IIPAccessor>();
            ipMock.Setup(i => i.GetIP()).Returns("1.127.01");

            var configMock = new Mock<IOptions<OmdbConfig>>();
            configMock.SetupGet(c => c.Value).Returns(new OmdbConfig {
                ApiKey = "testapikey",
                BaseUrl = "testbaseurl"
            });

            var utilitiesMock = new Mock<IUtilities>();
            utilitiesMock.Setup(u => u.MakeHttpRequest(It.Is<object>(x => x == null), It.Is<string>(x => x == "testbaseurl"), It.Is<string>(x => x == query), It.Is<HttpMethod>(x => x == HttpMethod.Get), It.Is<Dictionary<string, string>>(x => x == null)))
                .ReturnsAsync(TestData.GetSuccessMoviesResponse()).Verifiable();

            var requestServiceMock = new Mock<IRequestService>();
            requestServiceMock.Setup(r => r.SaveRequestAsync(It.Is<Request>(x => x.SearchToken == searchQuery.Title && x.IpAddress == "1.127.01"))).Returns(Task.CompletedTask).Verifiable();

            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(l => l.Information(It.Is<string>(x => x.Contains(searchQuery.Title)))).Verifiable();

            var movieService = new MoviesService(utilitiesMock.Object, configMock.Object, ipMock.Object, requestServiceMock.Object, loggerMock.Object);

            var response = await movieService.SearchMoviesAsync(searchQuery);

            utilitiesMock.Verify();
            requestServiceMock.Verify();
            loggerMock.Verify();

            Assert.True(response.StatusCode == 200);
            Assert.True(response.Data != null);
            Assert.True(response.Message == "Success");
        }

        [Fact]
        public async Task SearchMoviesAsync_Fails()
        {
            var searchQuery = new SearchQuery
            {
                Title = "Test"
            };

            var query = $"?apikey=testapikey&t={searchQuery.Title}";

            var ipMock = new Mock<IIPAccessor>();
            ipMock.Setup(i => i.GetIP()).Returns("1.127.01");

            var configMock = new Mock<IOptions<OmdbConfig>>();
            configMock.SetupGet(c => c.Value).Returns(new OmdbConfig
            {
                ApiKey = "testapikey",
                BaseUrl = "testbaseurl"
            });

            var utilitiesMock = new Mock<IUtilities>();
            utilitiesMock.Setup(u => u.MakeHttpRequest(It.Is<object>(x => x == null), It.Is<string>(x => x == "testbaseurl"), It.Is<string>(x => x == query), It.Is<HttpMethod>(x => x == HttpMethod.Get), It.Is<Dictionary<string, string>>(x => x == null)))
                .ReturnsAsync(TestData.GetFailureMoviesResponse()).Verifiable();

            var requestServiceMock = new Mock<IRequestService>();
            requestServiceMock.Setup(r => r.SaveRequestAsync(It.Is<Request>(x => x.SearchToken == searchQuery.Title && x.IpAddress == "1.127.01"))).Returns(Task.CompletedTask).Verifiable();

            var loggerMock = new Mock<ILogger>();

            var movieService = new MoviesService(utilitiesMock.Object, configMock.Object, ipMock.Object, requestServiceMock.Object, loggerMock.Object);

            var response = await movieService.SearchMoviesAsync(searchQuery);

            utilitiesMock.Verify();
            requestServiceMock.Verify();

            Assert.True(response.StatusCode == 400);
            Assert.True(response.Data == null);
            Assert.True(response.Message == "Fail");
        }
    }

    class TestData
    {
        public static readonly string validResponse = "{\"Title\":\"Shrek\",\"Year\":\"2001\",\"Rated\":\"PG\",\"Released\":\"18 May 2001\",\"Runtime\":\"90 minx\",\"Genre\":\"Animation, Adventure, Comedy\",\"Director\":\"Andrew Adamson, Vicky Jenson\",\"Writer\":\"William Steig, Ted Elliott, Terry Rossio\",\"Actors\":\"Mike Myers, Eddie Murphy, Cameron Diaz\",\"Plot\":\"A mean lord exiles fairytale creatures to the swamp of a grumpy ogre, who must go on a quest and rescue a princess for the lord in order to get his land back.\"}";
        public static HttpResponseMessage GetSuccessMoviesResponse()
        {
            return new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(validResponse),
            };
        }

        public static HttpResponseMessage GetFailureMoviesResponse()
        {
            return new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
            };
        }
    }
}
