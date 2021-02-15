using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DocumentsKM.Dtos;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DocumentsKM.Tests
{
    public class MarkOperatingConditionsControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
    {
        private readonly HttpClient _authHttpClient;
        private readonly HttpClient _httpClient;
        private readonly Random _rnd = new Random();

        private readonly int _maxMarkId = 2;

        public MarkOperatingConditionsControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
        {
            _httpClient = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();
            
            _authHttpClient = factory.CreateClient();
        }

        [Fact]
        public async Task GetByMarkId_ShouldReturnOK()
        {
            // Arrange
            int markId = _rnd.Next(1, _maxMarkId);
            var endpoint = $"/api/marks/{markId}/mark-operating-conditions";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string responseBody = await response.Content.ReadAsStringAsync();

            var foundMarkOperatingConditions = TestData.markOperatingConditions.SingleOrDefault(
                v => v.Mark.Id == markId);
            var markOperatingConditions = new MarkOperatingConditionsResponse
            {
                SafetyCoeff = foundMarkOperatingConditions.SafetyCoeff,
                EnvAggressiveness = foundMarkOperatingConditions.EnvAggressiveness,
                Temperature = foundMarkOperatingConditions.Temperature,
                OperatingArea = foundMarkOperatingConditions.OperatingArea,
                GasGroup = foundMarkOperatingConditions.GasGroup,
                ConstructionMaterial = foundMarkOperatingConditions.ConstructionMaterial,
                PaintworkType = foundMarkOperatingConditions.PaintworkType,
                HighTensileBoltsType = foundMarkOperatingConditions.HighTensileBoltsType,
            };
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            JsonSerializer.Deserialize<MarkOperatingConditionsResponse>(
                responseBody, options).Should().BeEquivalentTo(markOperatingConditions);
        }

        [Fact]
        public async Task GetByMarkId_ShouldReturnNotFound_WhenWrongId()
        {
            // Arrange
            int wrongId = 999;
            var endpoint = $"/api/marks/{wrongId}/mark-operating-conditions";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetByMarkOperatingConditionId_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int markId = _rnd.Next(1, _maxMarkId);
            var endpoint = $"/api/marks/{markId}/mark-operating-conditions";

            // Act
            var response = await _authHttpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}