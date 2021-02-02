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
    // TBD: Create, Update, Delete
    public class ConstructionsControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
    {
        private readonly HttpClient _authHttpClient;
        private readonly HttpClient _httpClient;
        private readonly Random _rnd = new Random();

        public ConstructionsControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
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
        public async Task GetAllBySpecificationId_ShouldReturnOK_WhenAccessTokenIsProvided()
        {
            // Arrange
            int specificationId = _rnd.Next(1, TestData.specifications.Count());
            var endpoint = $"/api/specifications/{specificationId}/constructions";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string responseBody = await response.Content.ReadAsStringAsync();

            var constructions = TestData.constructions.Where(
                v => v.Specification.Id == specificationId)
                .Select(v => new ConstructionResponse
                {
                    Id = v.Id,
                    Name = v.Name,
                    Type = v.Type,
                    Subtype = v.Subtype,
                    Valuation = v.Valuation,
                    StandardAlbumCode = v.StandardAlbumCode,
                    NumOfStandardConstructions = v.NumOfStandardConstructions,
                    HasEdgeBlunting = v.HasEdgeBlunting,
                    HasDynamicLoad = v.HasDynamicLoad,
                    HasFlangedConnections = v.HasFlangedConnections,
                    WeldingControl = v.WeldingControl,
                    PaintworkCoeff = v.PaintworkCoeff,
                }).ToArray();
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            JsonSerializer.Deserialize<IEnumerable<ConstructionResponse>>(
                responseBody, options).Should().BeEquivalentTo(constructions);
        }

        [Fact]
        public async Task GetAllBySpecificationId_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int specificationId = _rnd.Next(1, TestData.specifications.Count());
            var endpoint = $"/api/specifications/{specificationId}/constructions";

            // Act
            var response = await _authHttpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
