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
    public class DocsControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
    {
        private readonly HttpClient _authHttpClient;
        private readonly HttpClient _httpClient;
        private readonly Random _rnd = new Random();

        public DocsControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
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
        public async Task GetAllSheetsByMarkId_ShouldReturnOK_WhenAccessTokenIsProvided()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            var endpoint = $"/api/marks/{markId}/docs/sheets";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllSheetsByMarkId_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            var endpoint = $"/api/marks/{markId}/docs/sheets";

            // Act
            var response = await _authHttpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetAllAttachedByMarkId_ShouldReturnOK_WhenAccessTokenIsProvided()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            var endpoint = $"/api/marks/{markId}/docs/attached";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllAttachedByMarkId_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            var endpoint = $"/api/marks/{markId}/docs/attached";

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
            int docTypeId = 1;
            int creatorId = 1;
            int inspectorId = 2;
            int normContrId = 3;
            var docRequest = new DocCreateRequest
            {
                Name = "NewCreate",
                Form = 9.0f,
                TypeId = docTypeId,
                CreatorId = creatorId,
                InspectorId = inspectorId,
                NormContrId = normContrId,
                ReleaseNum = 9,
                NumOfPages = 9,
                Note = "NewCreate",
            };
            string json = JsonSerializer.Serialize(docRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/marks/{markId}/docs";

            // Act
            var response = await _httpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        // [Fact]
        // public async Task Create_ShouldReturnBadRequest_WhenWrongValues()
        // {
        //     // Arrange
        //     int markId = 2;
        //     var wrongdocRequests = new List<DocCreateRequest>
        //     {
        //         new DocCreateRequest
        //         {
        //             Name = "NewCreate",
        //         },
        //         new DocCreateRequest
        //         {
        //             Designation = "NewCreate",
        //         },
        //         new DocCreateRequest
        //         {
        //             Designation = "",
        //             Name = "NewCreate",
        //         },
        //         new DocCreateRequest
        //         {
        //             Designation = "NewCreate",
        //             Name = "",
        //         },
        //     };

        //     var endpoint = $"/api/marks/{markId}/docs";
        //     foreach (var wrongdocRequest in wrongdocRequests)
        //     {
        //         var json = JsonSerializer.Serialize(wrongdocRequest);
        //         var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        //         // Act
        //         var response = await _httpClient.PostAsync(endpoint, httpContent);

        //         // Assert
        //         Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //     }
        // }

        // [Fact]
        // public async Task Create_ShouldReturnNotFound_WhenWrongValues()
        // {
        //     // Arrange
        //     var docRequest = new DocCreateRequest
        //     {
        //         Designation = "NewCreate",
        //         Name = "NewCreate",
        //     };
        //     string json = JsonSerializer.Serialize(docRequest);
        //     var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        //     var endpoint = $"/api/marks/{999}/docs";

        //     // Act
        //     var response = await _httpClient.PostAsync(endpoint, httpContent);

        //     // Assert
        //     Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        // }

        // [Fact]
        // public async Task Create_ShouldReturnUnauthorized_WhenNoAccessToken()
        // {
        //     // Arrange
        //     int markId = 2;
        //     var docRequest = new DocCreateRequest
        //     {
        //         Designation = "NewCreate",
        //         Name = "NewCreate",
        //     };
        //     string json = JsonSerializer.Serialize(docRequest);
        //     var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        //     var endpoint = $"/api/marks/{markId}/docs";

        //     // Act
        //     var response = await _authHttpClient.PostAsync(endpoint, httpContent);

        //     // Assert
        //     Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        // }

        // // ------------------------------------PATCH------------------------------------

        // [Fact]
        // public async Task Update_ShouldReturnNoContent()
        // {
        //     // Arrange
        //     int id = 1;
        //     var docRequest = new DocUpdateRequest
        //     {
        //         Name = "NewUpdate",
        //     };
        //     string json = JsonSerializer.Serialize(docRequest);
        //     var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        //     var endpoint = $"/api/docs/{id}";

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
        //     var endpoint = $"/api/docs/{id}";

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
        //     var docRequest = new DocUpdateRequest
        //     {
        //         Name = "NewUpdate",
        //     };
        //     string json = JsonSerializer.Serialize(docRequest);
        //     var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        //     var endpoint = $"/api/docs/{id}";

        //     // Act
        //     var response = await _authHttpClient.PatchAsync(endpoint, httpContent);

        //     // Assert
        //     Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        // }

        // ------------------------------------DELETE------------------------------------

        [Fact]
        public async Task Delete_ShouldReturnNoContent()
        {
            // Arrange
            int id = 2;
            var endpoint = $"/api/docs/{id}";

            // Act
            var response = await _httpClient.DeleteAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenWrongId()
        {
            // Arrange
            var endpoint = $"/api/docs/{999}";

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
            var endpoint = $"/api/docs/{id}";

            // Act
            var response = await _authHttpClient.DeleteAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
