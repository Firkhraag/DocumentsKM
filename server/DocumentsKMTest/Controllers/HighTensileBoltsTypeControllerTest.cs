using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DocumentsKM.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DocumentsKM.Tests
{
    public class HighTensileBoltsTypesControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
    {
        private readonly HttpClient _authHttpClient;
        private readonly HttpClient _httpClient;

        public HighTensileBoltsTypesControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
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
        public async Task GetAll_ShouldReturnOK_WhenAccessTokenIsProvided()
        {
            // Arrange
            var endpoint = "/api/high-tensile-bolts-types";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string responseBody = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            TestData.highTensileBoltsTypes.Should().BeEquivalentTo(
                JsonSerializer.Deserialize<IEnumerable<HighTensileBoltsType>>(responseBody, options));
        }

        [Fact]
        public async Task GetAll_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            var endpoint = "/api/high-tensile-bolts-types";

            // Act
            var response = await _authHttpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
