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
    public class ConstructionSubtypesControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
    {
        private readonly HttpClient _authHttpClient;
        private readonly HttpClient _httpClient;
        private readonly Random _rnd = new Random();

        public ConstructionSubtypesControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
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
        public async Task GetAllByTypeId_ShouldReturnOK_WhenAccessTokenIsProvided()
        {
            // Arrange
            int typeId = _rnd.Next(1, TestData.constructionTypes.Count());
            var endpoint = $"/api/construction-types/{typeId}/construction-subtypes";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string responseBody = await response.Content.ReadAsStringAsync();

            var subtypes = TestData.constructionSubtypes.Where(v => v.Type.Id == typeId)
                .Select(s => new ConstructionSubtypeResponse{
                    Id = s.Id,
                    Name = s.Name,
                    Valuation = s.Valuation,
                }).ToArray();
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            subtypes.Should().BeEquivalentTo(
                JsonSerializer.Deserialize<IEnumerable<ConstructionSubtypeResponse>>(responseBody, options));
        }

        [Fact]
        public async Task GetAllByTypeId_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int typeId = _rnd.Next(1, TestData.constructionTypes.Count());
            var endpoint = $"/api/construction-types/{typeId}/construction-subtypes";

            // Act
            var response = await _authHttpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
