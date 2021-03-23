using System;
using System.Collections.Generic;
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

        private class CreateRequest
        {
            public int ConstructionId { set; get; }
            public ConstructionElementCreateRequest Body { set; get; }
        }

        private class UpdateRequest
        {
            public int Id { set; get; }
            public ConstructionElementUpdateRequest Body { set; get; }
        }

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
        public async Task GetAllByConstructionId_ShouldReturnOK()
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
            short steelId = 3;
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
            short steelId = 3;
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
            short steelId = 3;
            var constructionElementRequests = new List<CreateRequest>
            {
                new CreateRequest
                {
                    ConstructionId = 999,
                    Body = new ConstructionElementCreateRequest
                    {
                        ProfileId = profileId,
                        SteelId = steelId,
                        Length = 9,
                    },
                },
                new CreateRequest
                {
                    ConstructionId = constructionId,
                    Body = new ConstructionElementCreateRequest
                    {
                        ProfileId = 999,
                        SteelId = steelId,
                        Length = 9,
                    },
                },
                new CreateRequest
                {
                    ConstructionId = constructionId,
                    Body = new ConstructionElementCreateRequest
                    {
                        ProfileId = 999,
                        SteelId = steelId,
                        Length = 9,
                    },
                },
            };

            foreach (var req in constructionElementRequests)
            {
                var json = JsonSerializer.Serialize(req.Body);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var endpoint = $"/api/constructions/{req.ConstructionId}/elements";

                // Act
                var response = await _httpClient.PostAsync(endpoint, httpContent);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task Create_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int constructionId = 1;
            int profileId = 3;
            short steelId = 3;
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
            short steelId = 2;
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
            var wrongConstructionElementRequest = new ConstructionElementUpdateRequest
            {
                Length = -9.0f,
            };
            var json = JsonSerializer.Serialize(wrongConstructionElementRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/construction-elements/{id}";

            // Act
            var response = await _httpClient.PatchAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenWrongValues()
        {
            // Arrange
            int id = 1;

            var constructionElementRequests = new List<UpdateRequest>
            {
                new UpdateRequest
                {
                    Id = 999,
                    Body = new ConstructionElementUpdateRequest
                    {
                        Length = 9,
                    },
                },
                new UpdateRequest
                {
                    Id = id,
                    Body = new ConstructionElementUpdateRequest
                    {
                        ProfileId = 999,
                    },
                },
                new UpdateRequest
                {
                    Id = id,
                    Body = new ConstructionElementUpdateRequest
                    {
                        SteelId = 999,
                    },
                },
            };

            foreach (var req in constructionElementRequests)
            {
                var json = JsonSerializer.Serialize(req.Body);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var endpoint = $"/api/construction-elements/{req.Id}";

                // Act
                var response = await _httpClient.PatchAsync(endpoint, httpContent);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task Update_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int id = 1;
            var constructionElementRequest = new ConstructionElementUpdateRequest
            {
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
