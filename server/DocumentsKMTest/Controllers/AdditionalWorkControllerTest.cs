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
    // TBD: Update, Delete
    public class AdditionalWorkControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
    {
        private readonly HttpClient _authHttpClient;
        private readonly HttpClient _httpClient;
        private readonly Random _rnd = new Random();

        public AdditionalWorkControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
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
        public async Task GetAllByMarkId_ShouldReturnOK_WhenAccessTokenIsProvided()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            var endpoint = $"/api/marks/{markId}/additional-work";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllByMarkId_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            var endpoint = $"/api/marks/{markId}/additional-work";

            // Act
            var response = await _authHttpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnCreated()
        {
            // Arrange
            int markId = 1;
            int employeeId = 3;
            var additionalWorkRequest = new AdditionalWorkCreateRequest
            {
                EmployeeId = employeeId,
                Valuation = 9,
                MetalOrder = 9,
            };
            string json = JsonSerializer.Serialize(additionalWorkRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/marks/{markId}/additional-work";

            // Act
            var response = await _httpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        // [Fact]
        // public async Task Create_ShouldReturnBadRequest_WhenWrongValues()
        // {
        //     // Arrange
        //     int markId = _rnd.Next(1, TestData.marks.Count());
        //     var wrongAttachedDocRequest1 = new AttachedDocCreateRequest
        //     {
        //         Name = "NewCreate",
        //     };
        //     var wrongAttachedDocRequest2 = new AttachedDocCreateRequest
        //     {
        //         Designation = "NewCreate",
        //     };
        //     string json1 = JsonSerializer.Serialize(wrongAttachedDocRequest1);
        //     string json2 = JsonSerializer.Serialize(wrongAttachedDocRequest2);
        //     var httpContent1 = new StringContent(json1, Encoding.UTF8, "application/json");
        //     var httpContent2 = new StringContent(json2, Encoding.UTF8, "application/json");
        //     var endpoint = $"/api/marks/{markId}/attached-docs";

        //     // Act
        //     var response1 = await _httpClient.PostAsync(endpoint, httpContent1);
        //     var response2 = await _httpClient.PostAsync(endpoint, httpContent2);

        //     // Assert
        //     Assert.Equal(HttpStatusCode.BadRequest, response1.StatusCode);
        //     Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
        // }

        // [Fact]
        // public async Task Create_ShouldReturnUnauthorized_WhenNoAccessToken()
        // {
        //     // Arrange
        //     int markId = _rnd.Next(1, TestData.marks.Count());
        //     var attachedDocRequest = new AttachedDocCreateRequest
        //     {
        //         Designation = "NewCreate",
        //         Name = "NewCreate",
        //     };
        //     string json = JsonSerializer.Serialize(attachedDocRequest);
        //     var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        //     var endpoint = $"/api/marks/{markId}/attached-docs";

        //     // Act
        //     var response = await _authHttpClient.PostAsync(endpoint, httpContent);

        //     // Assert
        //     Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        // }

        // [Fact]
        // public async Task Update_ShouldReturnNoContent()
        // {
        //     // Arrange
        //     int id = 1;
        //     var attachedDocRequest = new AttachedDocUpdateRequest
        //     {
        //         Name = "NewUpdate",
        //     };
        //     string json = JsonSerializer.Serialize(attachedDocRequest);
        //     var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        //     var endpoint = $"/api/attached-docs/{id}";

        //     // Act
        //     var response = await _httpClient.PatchAsync(endpoint, httpContent);

        //     // Assert
        //     Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        // }

        // [Fact]
        // public async Task Update_ShouldReturnBadRequest_WhenEmptyString()
        // {
        //     // Arrange
        //     int id = 1;
        //     var httpContent = new StringContent("", Encoding.UTF8, "application/json");
        //     var endpoint = $"/api/attached-docs/{id}";

        //     // Act
        //     var response = await _httpClient.PatchAsync(endpoint, httpContent);

        //     // Assert
        //     Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        // }

        // [Fact]
        // public async Task Update_ShouldReturnUnauthorized_WhenNoAccessToken()
        // {
        //     // Arrange
        //     int id = 1;
        //     var attachedDocRequest = new AttachedDocUpdateRequest
        //     {
        //         Name = "NewUpdate",
        //     };
        //     string json = JsonSerializer.Serialize(attachedDocRequest);
        //     var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        //     var endpoint = $"/api/attached-docs/{id}";

        //     // Act
        //     var response = await _authHttpClient.PatchAsync(endpoint, httpContent);

        //     // Assert
        //     Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        // }

        // [Fact]
        // public async Task Delete_ShouldReturnNoContent_WhenAccessTokenIsProvided()
        // {
        //     // Arrange
        //     int id = 2;
        //     var endpoint = $"/api/attached-docs/{id}";

        //     int markId = TestData.attachedDocs.FirstOrDefault(v => v.Id == id).Mark.Id;

        //     // Act
        //     var response = await _httpClient.DeleteAsync(endpoint);

        //     // Assert
        //     Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        // }

        // [Fact]
        // public async Task Delete_ShouldReturnUnauthorized_WhenNoAccessToken()
        // {
        //     // Arrange
        //     int id = 2;
        //     var endpoint = $"/api/attached-docs/{id}";

        //     // Act
        //     var response = await _authHttpClient.DeleteAsync(endpoint);

        //     // Assert
        //     Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        // }
    }
}
