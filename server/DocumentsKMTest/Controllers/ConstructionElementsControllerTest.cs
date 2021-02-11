using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DocumentsKM.Dtos;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ConstructionElementsControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
    {
        private readonly HttpClient _authHttpClient;
        private readonly HttpClient _httpClient;
        private readonly Random _rnd = new Random();

        public ConstructionElementsControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
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
        public async Task GetAllByConstructionId_ShouldReturnOK_WhenAccessTokenIsProvided()
        {
            // Arrange
            int constructionId = _rnd.Next(1, TestData.constructions.Count());
            var endpoint = $"/api/constructions/{constructionId}/elements";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllByConstructionId_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int constructionId = _rnd.Next(1, TestData.constructions.Count());
            var endpoint = $"/api/constructions/{constructionId}/elements";

            // Act
            var response = await _authHttpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        // ------------------------------------POST------------------------------------

        [Fact]
        public async Task Create_ShouldReturnCreated()
        {
            // Arrange
            int constructionId = 1;
            int profileId = 3;
            int steelId = 3;
            var constructionElementRequest = new ConstructionElementCreateRequest
            {
                ProfileId = profileId,
                SteelId = steelId,
                Length = 9,
            };
            string json = JsonSerializer.Serialize(constructionElementRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/constructions/{constructionId}/elements";

            // Act
            var response = await _httpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenWrongValues()
        {
            // Arrange
            int constructionId = 1;
            int profileId = 3;
            int steelId = 3;
            var wrongConstructionElementRequest = new ConstructionElementCreateRequest
            {
                ProfileId = profileId,
                SteelId = steelId,
                Length = -9,
            };

            var endpoint = $"/api/constructions/{constructionId}/elements";
            var json = JsonSerializer.Serialize(wrongConstructionElementRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _httpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnNotFound_WhenWrongValues()
        {
            // Arrange
            int constructionId = 1;
            int profileId = 3;
            int steelId = 3;
            var constructionElementRequest = new ConstructionElementCreateRequest
            {
                ProfileId = profileId,
                SteelId = steelId,
                Length = 9,
            };
            var wrongConstructionElementRequest1 = new ConstructionElementCreateRequest
            {
                ProfileId = 999,
                SteelId = steelId,
                Length = 9,
            };
            var wrongConstructionElementRequest2 = new ConstructionElementCreateRequest
            {
                ProfileId = profileId,
                SteelId = 999,
                Length = 9,
            };
            string json1 = JsonSerializer.Serialize(wrongConstructionElementRequest1);
            string json2 = JsonSerializer.Serialize(wrongConstructionElementRequest2);
            string json3 = JsonSerializer.Serialize(constructionElementRequest);
            var httpContent1 = new StringContent(json1, Encoding.UTF8, "application/json");
            var httpContent2 = new StringContent(json2, Encoding.UTF8, "application/json");
            var httpContent3 = new StringContent(json3, Encoding.UTF8, "application/json");
            var endpoint1 = $"/api/constructions/{constructionId}/elements";
            var endpoint2 = $"/api/constructions/{constructionId}/elements";
            var endpoint3 = $"/api/constructions/{999}/elements";

            // Act
            var response1 = await _httpClient.PostAsync(endpoint1, httpContent1);
            var response2 = await _httpClient.PostAsync(endpoint2, httpContent2);
            var response3 = await _httpClient.PostAsync(endpoint3, httpContent2);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response1.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response2.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response3.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int constructionId = 1;
            int profileId = 3;
            int steelId = 3;
            var constructionElementRequest = new ConstructionElementCreateRequest
            {
                ProfileId = profileId,
                SteelId = steelId,
                Length = 9,
            };
            string json = JsonSerializer.Serialize(constructionElementRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/constructions/{constructionId}/elements";

            // Act
            var response = await _authHttpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        // ------------------------------------PATCH------------------------------------

        [Fact]
        public async Task Update_ShouldReturnNoContent()
        {
            // Arrange
            int id = 1;
            int profileId = 2;
            int steelId = 2;
            var constructionElementRequest = new ConstructionElementUpdateRequest
            {
                ProfileId = profileId,
                SteelId = steelId,
                Length = 9,
            };
            string json = JsonSerializer.Serialize(constructionElementRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/construction-elements/{id}";

            // Act
            var response = await _httpClient.PatchAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenWrongValues()
        {
            // Arrange
            int id = 1;
            int profileId = 3;
            int steelId = 3;
            var wrongConstructionElementRequest = new ConstructionElementUpdateRequest
            {
                ProfileId = profileId,
                SteelId = steelId,
                Length = -9.0f,
            };
            var json = JsonSerializer.Serialize(wrongConstructionElementRequest);
            var httpContent1 = new StringContent(json, Encoding.UTF8, "application/json");
            var httpContent2 = new StringContent("", Encoding.UTF8, "application/json");
            var endpoint = $"/api/construction-elements/{id}";

            // Act
            var response1 = await _httpClient.PatchAsync(endpoint, httpContent1);
            var response2 = await _httpClient.PatchAsync(endpoint, httpContent2);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response1.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenWrongValues()
        {
            // Arrange
            int id = 1;
            int profileId = 3;
            int steelId = 3;
            var constructionElementRequest = new ConstructionElementUpdateRequest
            {
                ProfileId = profileId,
                SteelId = steelId,
                Length = 9,
            };
            var wrongConstructionElementRequest1 = new ConstructionElementUpdateRequest
            {
                ProfileId = 999,
                SteelId = steelId,
                Length = 9,
            };
            var wrongConstructionElementRequest2 = new ConstructionElementUpdateRequest
            {
                ProfileId = profileId,
                SteelId = 999,
                Length = 9,
            };
            string json1 = JsonSerializer.Serialize(wrongConstructionElementRequest1);
            string json2 = JsonSerializer.Serialize(wrongConstructionElementRequest2);
            string json3 = JsonSerializer.Serialize(constructionElementRequest);
            var httpContent1 = new StringContent(json1, Encoding.UTF8, "application/json");
            var httpContent2 = new StringContent(json2, Encoding.UTF8, "application/json");
            var httpContent3 = new StringContent(json3, Encoding.UTF8, "application/json");
            var endpoint1 = $"/api/construction-elements/{id}";
            var endpoint2 = $"/api/construction-elements/{id}";
            var endpoint3 = $"/api/construction-elements/{999}";

            // Act
            var response1 = await _httpClient.PatchAsync(endpoint1, httpContent1);
            var response2 = await _httpClient.PatchAsync(endpoint2, httpContent2);
            var response3 = await _httpClient.PatchAsync(endpoint3, httpContent2);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response1.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response2.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response3.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int id = 1;
            int profileId = 2;
            int steelId = 2;
            var constructionElementRequest = new ConstructionElementUpdateRequest
            {
                ProfileId = profileId,
                SteelId = steelId,
                Length = 9,
            };
            string json = JsonSerializer.Serialize(constructionElementRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/construction-elements/{id}";

            // Act
            var response = await _authHttpClient.PatchAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        // ------------------------------------DELETE------------------------------------

        [Fact]
        public async Task Delete_ShouldReturnNoContent()
        {
            // Arrange
            int id = 2;
            var endpoint = $"/api/construction-elements/{id}";

            // Act
            var response = await _httpClient.DeleteAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenWrongId()
        {
            // Arrange
            var endpoint = $"/api/construction-elements/{999}";

            // Act
            var response = await _httpClient.DeleteAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int id = 2;
            var endpoint = $"/api/construction-elements/{id}";

            // Act
            var response = await _authHttpClient.DeleteAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
