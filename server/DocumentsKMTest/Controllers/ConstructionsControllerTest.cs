using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DocumentsKM.Tests
{
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

        // ------------------------------------GET------------------------------------

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

        // ------------------------------------DELETE------------------------------------

        // [Fact]
        // public async Task Delete_ShouldReturnNoContent()
        // {
        //     // Arrange
        //     int id = 2;
        //     var endpoint = $"/api/constructions/{id}";

        //     // Act
        //     var response = await _httpClient.DeleteAsync(endpoint);

        //     // Assert
        //     Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        // }

        // [Fact]
        // public async Task Delete_ShouldReturnNotFound_WhenWrongId()
        // {
        //     // Arrange
        //     var endpoint = $"/api/constructions/{999}";

        //     // Act
        //     var response = await _httpClient.DeleteAsync(endpoint);

        //     // Assert
        //     Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        // }

        // [Fact]
        // public async Task Delete_ShouldReturnUnauthorized_WhenNoAccessToken()
        // {
        //     // Arrange
        //     int id = 2;
        //     var endpoint = $"/api/constructions/{id}";

        //     // Act
        //     var response = await _authHttpClient.DeleteAsync(endpoint);

        //     // Assert
        //     Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        // }
    }
}
