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
    public class GeneralDataSectionsControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
    {
        private readonly HttpClient _authHttpClient;
        private readonly HttpClient _httpClient;
        private readonly Random _rnd = new Random();

        private readonly int _maxUserId = 3;

        public GeneralDataSectionsControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
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
        public async Task GetAllByUserId_ShouldReturnOK()
        {
            // Arrange
            int userId = _rnd.Next(1, _maxUserId);

            var endpoint = $"/api/users/{userId}/general-data-sections";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllByUserId_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int userId = _rnd.Next(1, _maxUserId);

            var endpoint = $"/api/users/{userId}/general-data-sections";

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
            int userId = 1;
            var generalDataSectionRequest = new GeneralDataSectionCreateRequest
            {
                Name = "NewCreate",
            };
            string json = JsonSerializer.Serialize(generalDataSectionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/users/{userId}/general-data-sections";

            // Act
            var response = await _httpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenWrongValues()
        {
            // Arrange
            int userId = 1;
            var wrongGeneralDataSectionRequest = new GeneralDataSectionCreateRequest
            {
                Name = "",
            };

            string json = JsonSerializer.Serialize(wrongGeneralDataSectionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/users/{userId}/general-data-sections";

            // Act
            var response = await _httpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnNotFound_WhenWrongValue()
        {
            // Arrange
            var generalDataSectionRequest = new GeneralDataSectionCreateRequest
            {
                Name = "NewCreate",
            };
            string json = JsonSerializer.Serialize(generalDataSectionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/users/{999}/general-data-sections";

            // Act
            var response = await _httpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnConflict_WhenConflictValues()
        {
            // Arrange
            int userId = TestData.generalDataSections[0].User.Id;
            string name = TestData.generalDataSections[0].Name;
            var generalDataSectionRequest = new GeneralDataSectionCreateRequest
            {
                Name = name,
            };
            string json = JsonSerializer.Serialize(generalDataSectionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/users/{userId}/general-data-sections";

            // Act
            var response = await _httpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int userId = 1;
            var generalDataSectionRequest = new GeneralDataSectionCreateRequest
            {
                Name = "NewCreate",
            };
            string json = JsonSerializer.Serialize(generalDataSectionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/users/{userId}/general-data-sections";

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
            int userId = 1;
            var generalDataSectionRequest = new GeneralDataSectionUpdateRequest
            {
                Name = "NewUpdate",
                OrderNum = 15,
            };
            string json = JsonSerializer.Serialize(generalDataSectionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/users/{userId}/general-data-sections/{id}";

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
            int userId = 1;
            var generalDataSectionRequest = new GeneralDataSectionUpdateRequest
            {
                Name = "",
            };
            string json = JsonSerializer.Serialize(generalDataSectionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/users/{userId}/general-data-sections/{id}";

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
            int userId = 1;
            var generalDataSectionRequest = new GeneralDataSectionUpdateRequest
            {
                Name = "NewUpdate",
                OrderNum = 15,
            };
            string json = JsonSerializer.Serialize(generalDataSectionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint1 = $"/api/users/{999}/general-data-sections/{id}";
            var endpoint2 = $"/api/users/{userId}/general-data-sections/{999}";

            // Act
            var response1 = await _httpClient.PatchAsync(endpoint1, httpContent);
            var response2 = await _httpClient.PatchAsync(endpoint2, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response1.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response2.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnConflict_WhenConflictValues()
        {
            // Arrange
            int id = 5;
            int userId = TestData.generalDataSections.SingleOrDefault(v => v.Id == 4).User.Id;
            var generalDataSectionRequest = new GeneralDataSectionUpdateRequest
            {
                Name = TestData.generalDataSections.SingleOrDefault(v => v.Id == 4).Name,
            };
            string json = JsonSerializer.Serialize(generalDataSectionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/users/{userId}/general-data-sections/{id}";

            // Act
            var response = await _httpClient.PatchAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int id = 1;
            int userId = 1;
            var generalDataSectionRequest = new GeneralDataSectionUpdateRequest
            {
                Name = "NewUpdate",
                OrderNum = 15,
            };
            string json = JsonSerializer.Serialize(generalDataSectionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/users/{userId}/general-data-sections/{id}";

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
            int userId = 1;
            int id = 2;
            var endpoint = $"/api/users/{userId}/general-data-sections/{id}";

            // Act
            var response = await _httpClient.DeleteAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenWrongId()
        {
            // Arrange
            int userId = 1;
            var endpoint = $"/api/users/{userId}/general-data-sections/{999}";

            // Act
            var response = await _httpClient.DeleteAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int userId = 1;
            int id = 2;
            var endpoint = $"/api/users/{userId}/general-data-sections/{id}";

            // Act
            var response = await _authHttpClient.DeleteAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
