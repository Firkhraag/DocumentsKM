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
    public class MarkGeneralDataSectionsControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
    {
        private readonly HttpClient _authHttpClient;
        private readonly HttpClient _httpClient;
        private readonly Random _rnd = new Random();

        private readonly int _maxMarkId = 3;

        public MarkGeneralDataSectionsControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
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
        public async Task GetAllBymarkId_ShouldReturnOK()
        {
            // Arrange
            int markId = _rnd.Next(1, _maxMarkId);

            var endpoint = $"/api/marks/{markId}/mark-general-data-sections";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllBymarkId_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int markId = _rnd.Next(1, _maxMarkId);

            var endpoint = $"/api/marks/{markId}/mark-general-data-sections";

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
            int markId = 1;
            var markGeneralDataSectionRequest = new MarkGeneralDataSectionCreateRequest
            {
                Name = "NewCreate",
            };
            string json = JsonSerializer.Serialize(markGeneralDataSectionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/marks/{markId}/mark-general-data-sections";

            // Act
            var response = await _httpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenWrongValues()
        {
            // Arrange
            int markId = 1;
            var wrongMarkGeneralDataSectionRequest = new MarkGeneralDataSectionCreateRequest
            {
                Name = "",
            };

            string json = JsonSerializer.Serialize(wrongMarkGeneralDataSectionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/marks/{markId}/mark-general-data-sections";

            // Act
            var response = await _httpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnNotFound_WhenWrongValue()
        {
            // Arrange
            var markGeneralDataSectionRequest = new MarkGeneralDataSectionCreateRequest
            {
                Name = "NewCreate",
            };
            string json = JsonSerializer.Serialize(markGeneralDataSectionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/marks/{999}/mark-general-data-sections";

            // Act
            var response = await _httpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnConflict_WhenConflictValues()
        {
            // Arrange
            int markId = TestData.markGeneralDataSections[0].Mark.Id;
            string name = TestData.markGeneralDataSections[0].Name;
            var markGeneralDataSectionRequest = new MarkGeneralDataSectionCreateRequest
            {
                Name = name,
            };
            string json = JsonSerializer.Serialize(markGeneralDataSectionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/marks/{markId}/mark-general-data-sections";

            // Act
            var response = await _httpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int markId = 1;
            var markGeneralDataSectionRequest = new MarkGeneralDataSectionCreateRequest
            {
                Name = "NewCreate",
            };
            string json = JsonSerializer.Serialize(markGeneralDataSectionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/marks/{markId}/mark-general-data-sections";

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
            int markId = 1;
            var markGeneralDataSectionRequest = new MarkGeneralDataSectionUpdateRequest
            {
                Name = "NewUpdate",
                OrderNum = 15,
            };
            string json = JsonSerializer.Serialize(markGeneralDataSectionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/marks/{markId}/mark-general-data-sections/{id}";

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
            int markId = 1;
            var markGeneralDataSectionRequest = new MarkGeneralDataSectionUpdateRequest
            {
                Name = "",
            };
            string json = JsonSerializer.Serialize(markGeneralDataSectionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/marks/{markId}/mark-general-data-sections/{id}";

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
            int markId = 1;
            var markGeneralDataSectionRequest = new MarkGeneralDataSectionUpdateRequest
            {
                Name = "NewUpdate",
                OrderNum = 15,
            };
            string json = JsonSerializer.Serialize(markGeneralDataSectionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint1 = $"/api/marks/{999}/mark-general-data-sections/{id}";
            var endpoint2 = $"/api/marks/{markId}/mark-general-data-sections/{999}";

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
            int markId = TestData.markGeneralDataSections.SingleOrDefault(v => v.Id == 4).Mark.Id;
            var markGeneralDataSectionRequest = new MarkGeneralDataSectionUpdateRequest
            {
                Name = TestData.markGeneralDataSections.SingleOrDefault(v => v.Id == 4).Name,
            };
            string json = JsonSerializer.Serialize(markGeneralDataSectionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/marks/{markId}/mark-general-data-sections/{id}";

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
            int markId = 1;
            var markGeneralDataSectionRequest = new MarkGeneralDataSectionUpdateRequest
            {
                Name = "NewUpdate",
                OrderNum = 15,
            };
            string json = JsonSerializer.Serialize(markGeneralDataSectionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/marks/{markId}/mark-general-data-sections/{id}";

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
            int markId = 1;
            int id = 2;
            var endpoint = $"/api/marks/{markId}/mark-general-data-sections/{id}";

            // Act
            var response = await _httpClient.DeleteAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenWrongId()
        {
            // Arrange
            int markId = 1;
            var endpoint = $"/api/marks/{markId}/mark-general-data-sections/{999}";

            // Act
            var response = await _httpClient.DeleteAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int markId = 1;
            int id = 2;
            var endpoint = $"/api/marks/{markId}/mark-general-data-sections/{id}";

            // Act
            var response = await _authHttpClient.DeleteAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
