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
    public class GeneralDataPointsControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
    {
        private readonly HttpClient _authHttpClient;
        private readonly HttpClient _httpClient;
        private readonly Random _rnd = new Random();
        private readonly int _maxSectionId = 3;

        public GeneralDataPointsControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
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
        public async Task GetAllBySectionId_ShouldReturnOK()
        {
            // Arrange
            int sectionId = _rnd.Next(1, _maxSectionId);
            var endpoint = $"/api/general-data-sections/{sectionId}/general-data-points";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllBySectionId_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int sectionId = _rnd.Next(1, _maxSectionId);
            var endpoint = $"/api/general-data-sections/{sectionId}/general-data-points";

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
            int sectionId = 1;
            var generalDataPointRequest = new GeneralDataPointCreateRequest
            {
                Text = "NewCreate",
            };
            string json = JsonSerializer.Serialize(generalDataPointRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/general-data-sections/{sectionId}/general-data-points";

            // Act
            var response = await _httpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenWrongValues()
        {
            // Arrange
            int sectionId = 1;
            var wrongGeneralDataPointRequest = new GeneralDataPointCreateRequest
            {
                Text = "",
            };

            string json = JsonSerializer.Serialize(wrongGeneralDataPointRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/general-data-sections/{sectionId}/general-data-points";

            // Act
            var response = await _httpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnNotFound_WhenWrongValue()
        {
            // Arrange
            var generalDataPointRequest = new GeneralDataPointCreateRequest
            {
                Text = "NewCreate",
            };
            string json = JsonSerializer.Serialize(generalDataPointRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/general-data-sections/{999}/general-data-points";

            // Act
            var response = await _httpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnConflict_WhenConflictValues()
        {
            // Arrange
            int sectionId = TestData.generalDataPoints[0].Section.Id;
            string text = TestData.generalDataPoints[0].Text;
            var generalDataPointRequest = new GeneralDataPointCreateRequest
            {
                Text = text,
            };
            string json = JsonSerializer.Serialize(generalDataPointRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/general-data-sections/{sectionId}/general-data-points";

            // Act
            var response = await _httpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int sectionId = 1;
            var generalDataPointRequest = new GeneralDataPointCreateRequest
            {
                Text = "NewCreate",
            };
            string json = JsonSerializer.Serialize(generalDataPointRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/general-data-sections/{sectionId}/general-data-points";

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
            int sectionId = 1;
            var generalDataPointRequest = new GeneralDataPointUpdateRequest
            {
                Text = "NewUpdate",
                OrderNum = 15,
            };
            string json = JsonSerializer.Serialize(generalDataPointRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/general-data-sections/{sectionId}/general-data-points/{id}";

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
            int sectionId = 1;
            var generalDataPointRequest = new GeneralDataPointUpdateRequest
            {
                Text = "",
            };
            string json = JsonSerializer.Serialize(generalDataPointRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/general-data-sections/{sectionId}/general-data-points/{id}";

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
            int sectionId = 1;
            var generalDataPointRequest = new GeneralDataPointUpdateRequest
            {
                Text = "NewUpdate",
                OrderNum = 15,
            };
            string json = JsonSerializer.Serialize(generalDataPointRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint1 = $"/api/general-data-sections/{999}/general-data-points/{id}";
            var endpoint2 = $"/api/general-data-sections/{sectionId}/general-data-points/{999}";

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
            int id = 6;
            int sectionId = TestData.generalDataPoints.SingleOrDefault(v => v.Id == 4).Section.Id;
            var generalDataPointRequest = new GeneralDataPointUpdateRequest
            {
                Text = TestData.generalDataPoints.SingleOrDefault(v => v.Id == 4).Text,
            };
            string json = JsonSerializer.Serialize(generalDataPointRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/general-data-sections/{sectionId}/general-data-points/{id}";

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
            int sectionId = 1;
            var generalDataPointRequest = new GeneralDataPointUpdateRequest
            {
                Text = "NewUpdate",
                OrderNum = 15,
            };
            string json = JsonSerializer.Serialize(generalDataPointRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/general-data-sections/{sectionId}/general-data-points/{id}";

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
            int sectionId = 1;
            int id = 2;
            var endpoint = $"/api/general-data-sections/{sectionId}/general-data-points/{id}";

            // Act
            var response = await _httpClient.DeleteAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenWrongId()
        {
            // Arrange
            int sectionId = 1;
            var endpoint = $"/api/general-data-sections/{sectionId}/general-data-points/{999}";

            // Act
            var response = await _httpClient.DeleteAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int sectionId = 1;
            int id = 2;
            var endpoint = $"/api/general-data-sections/{sectionId}/general-data-points/{id}";

            // Act
            var response = await _authHttpClient.DeleteAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
