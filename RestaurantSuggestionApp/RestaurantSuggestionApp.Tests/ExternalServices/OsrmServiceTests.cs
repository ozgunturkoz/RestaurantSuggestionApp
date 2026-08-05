using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using RestaurantSuggestionApp.Business.ExternalServices;
using RestaurantSuggestionApp.Core.Entities;

namespace RestaurantSuggestionApp.Tests.ExternalServices
{
    [TestFixture]
    public class OsrmServiceTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private Mock<IConfiguration> _mockConfiguration;
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _mockConfiguration = new Mock<IConfiguration>();

            _mockConfiguration.Setup(c => c["OSRM:BaseUrl"]).Returns("http://router.project-osrm.org");
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }

        [Test]
        public async Task GetDistancesFromOsrmAsync_SuccessResponse_ReturnsDistancesAndDurations()
        {
            var candidates = new List<Restaurant>
            {
                new Restaurant(1, "Rest 1", "Kebap", 41.1M, 29.1M, 100M)
            };

            var jsonResponse = @"{
                ""code"": ""Ok"",
                ""distances"": [
                    [0, 100]
                ],
                ""durations"": [
                    [0, 120]
                ]
            }";

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
                });

            var osrmService = new OsrmService(_httpClient, _mockConfiguration.Object);

            var result = await osrmService.GetDistancesFromOsrmAsync(candidates, 41.0, 29.0);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Id, Is.EqualTo(1));
            Assert.That(result[0].WalkingDistanceInMeters, Is.EqualTo(100));
            Assert.That(result[0].WalkingDurationInSeconds, Is.EqualTo(120));
        }

        [Test]
        public async Task GetDistancesFromOsrmAsync_ApiFails_ReturnsEmptyList()
        {
            var candidates = new List<Restaurant>
            {
                new Restaurant(1, "Rest 1", "Kebap", 41.1M, 29.1M, 100M)
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            var osrmService = new OsrmService(_httpClient, _mockConfiguration.Object);

            var result = await osrmService.GetDistancesFromOsrmAsync(candidates, 41.0, 29.0);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}
