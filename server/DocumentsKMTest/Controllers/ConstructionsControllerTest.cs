// using System;
// using System.Linq;
// using System.Net;
// using System.Net.Http;
// using System.Text.Json;
// using System.Threading.Tasks;
// using DocumentsKM.Dtos;
// using Microsoft.AspNetCore.Authorization.Policy;
// using Microsoft.AspNetCore.TestHost;
// using Microsoft.Extensions.DependencyInjection;
// using Xunit;

// namespace DocumentsKM.Tests
// {
//     public class ConstructionsControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
//     {
//         private readonly HttpClient _authHttpClient;
//         private readonly HttpClient _httpClient;
//         private readonly Random _rnd = new Random();

//         public ConstructionsControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
//         {
//             _httpClient = factory.WithWebHostBuilder(builder =>
//             {
//                 builder.ConfigureTestServices(services =>
//                 {
//                     services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
//                 });
//             }).CreateClient();
            
//             _authHttpClient = factory.CreateClient();
//         }

//         // ------------------------------------GET------------------------------------

//         [Fact]
//         public async Task GetAllBySpecificationId_ShouldReturnOK_WhenAccessTokenIsProvided()
//         {
//             // Arrange
//             int specificationId = _rnd.Next(1, TestData.specifications.Count());
//             var endpoint = $"/api/specifications/{specificationId}/constructions";

//             // Act
//             var response = await _httpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//         }

//         [Fact]
//         public async Task GetAllBySpecificationId_ShouldReturnUnauthorized_WhenNoAccessToken()
//         {
//             // Arrange
//             int specificationId = _rnd.Next(1, TestData.specifications.Count());
//             var endpoint = $"/api/specifications/{specificationId}/constructions";

//             // Act
//             var response = await _authHttpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//         }

//         // ------------------------------------POST------------------------------------

//         // [Fact]
//         // public async Task Create_ShouldReturnCreated()
//         // {
//         //     // Arrange
//         //     int specificationId = 3;
//         //     int diameterId = 1;
//         //     var constructionRequest = new ConstructionCreateRequest
//         //     {
//         //         DiameterId = diameterId,
//         //         Packet = 9,
//         //         Num = 9,
//         //         NutNum = 9,
//         //         WasherNum = 9,
//         //     };
//         //     string json = JsonSerializer.Serialize(constructionRequest);
//         //     var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
//         //     var endpoint = $"/api/specifications/{specificationId}/constructions";

//         //     // Act
//         //     var response = await _httpClient.PostAsync(endpoint, httpContent);

//         //     // Assert
//         //     Assert.Equal(HttpStatusCode.Created, response.StatusCode);
//         // }

//         // [Fact]
//         // public async Task Create_ShouldReturnBadRequest_WhenWrongValues()
//         // {
//         //     // Arrange
//         //     int specificationId = 1;
//         //     int diameterId = 3;
//         //     var wrongConstructionRequests = new List<ConstructionCreateRequest>
//         //     {
//         //         new ConstructionCreateRequest
//         //         {
//         //             DiameterId = diameterId,
//         //             Packet = -9,
//         //             Num = 9,
//         //             NutNum = 9,
//         //             WasherNum = 9,
//         //         },
//         //         new ConstructionCreateRequest
//         //         {
//         //             DiameterId = diameterId,
//         //             Packet = 9,
//         //             Num = -9,
//         //             NutNum = 9,
//         //             WasherNum = 9,
//         //         },
//         //         new ConstructionCreateRequest
//         //         {
//         //             DiameterId = diameterId,
//         //             Packet = 9,
//         //             Num = 9,
//         //             NutNum = -9,
//         //             WasherNum = 9,
//         //         },
//         //         new ConstructionCreateRequest
//         //         {
//         //             DiameterId = diameterId,
//         //             Packet = 9,
//         //             Num = 9,
//         //             NutNum = 9,
//         //             WasherNum = -9,
//         //         },
//         //     };

//         //     var endpoint = $"/api/specifications/{specificationId}/constructions";
//         //     foreach (var wrongConstructionRequest in wrongConstructionRequests)
//         //     {
//         //         var json = JsonSerializer.Serialize(wrongConstructionRequest);
//         //         var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

//         //         // Act
//         //         var response = await _httpClient.PostAsync(endpoint, httpContent);

//         //         // Assert
//         //         Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
//         //     }
//         // }

//         // [Fact]
//         // public async Task Create_ShouldReturnNotFound_WhenWrongValues()
//         // {
//         //     // Arrange
//         //     int specificationId = 1;
//         //     int diameterId = 3;
//         //     var constructionRequest = new ConstructionCreateRequest
//         //     {
//         //         DiameterId = diameterId,
//         //         Packet = 9,
//         //         Num = 9,
//         //         NutNum = 9,
//         //         WasherNum = 9,
//         //     };
//         //     var wrongConstructionRequest = new ConstructionCreateRequest
//         //     {
//         //         DiameterId = 999,
//         //         Packet = 9,
//         //         Num = 9,
//         //         NutNum = 9,
//         //         WasherNum = 9,
//         //     };
//         //     string json1 = JsonSerializer.Serialize(wrongConstructionRequest);
//         //     string json2 = JsonSerializer.Serialize(constructionRequest);
//         //     var httpContent1 = new StringContent(json1, Encoding.UTF8, "application/json");
//         //     var httpContent2 = new StringContent(json2, Encoding.UTF8, "application/json");
//         //     var endpoint1 = $"/api/specifications/{specificationId}/constructions";
//         //     var endpoint2 = $"/api/specifications/{999}/constructions";

//         //     // Act
//         //     var response1 = await _httpClient.PostAsync(endpoint1, httpContent1);
//         //     var response2 = await _httpClient.PostAsync(endpoint2, httpContent2);

//         //     // Assert
//         //     Assert.Equal(HttpStatusCode.NotFound, response1.StatusCode);
//         //     Assert.Equal(HttpStatusCode.NotFound, response2.StatusCode);
//         // }

//         // [Fact]
//         // public async Task Create_ShouldReturnConflict_WhenConflictValues()
//         // {
//         //     // Arrange
//         //     int specificationId = 3;
//         //     int diameterId = 2;
//         //     var constructionRequest = new ConstructionCreateRequest
//         //     {
//         //         DiameterId = diameterId,
//         //         Packet = 9,
//         //         Num = 9,
//         //         NutNum = 9,
//         //         WasherNum = 9,
//         //     };
//         //     string json = JsonSerializer.Serialize(constructionRequest);
//         //     var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
//         //     var endpoint = $"/api/specifications/{specificationId}/constructions";

//         //     // Act
//         //     var response = await _httpClient.PostAsync(endpoint, httpContent);

//         //     // Assert
//         //     Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
//         // }

//         // [Fact]
//         // public async Task Create_ShouldReturnUnauthorized_WhenNoAccessToken()
//         // {
//         //     // Arrange
//         //     int specificationId = 1;
//         //     int diameterId = 3;
//         //     var constructionRequest = new ConstructionCreateRequest
//         //     {
//         //         DiameterId = diameterId,
//         //         Packet = 9,
//         //         Num = 9,
//         //         NutNum = 9,
//         //         WasherNum = 9,
//         //     };
//         //     string json = JsonSerializer.Serialize(constructionRequest);
//         //     var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
//         //     var endpoint = $"/api/specifications/{specificationId}/constructions";

//         //     // Act
//         //     var response = await _authHttpClient.PostAsync(endpoint, httpContent);

//         //     // Assert
//         //     Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//         // }

//         // // ------------------------------------PATCH------------------------------------

//         // [Fact]
//         // public async Task Update_ShouldReturnNoContent()
//         // {
//         //     // Arrange
//         //     int id = 1;
//         //     int diameterId = 3;
//         //     var constructionRequest = new ConstructionUpdateRequest
//         //     {
//         //         DiameterId = diameterId,
//         //         Packet = 9,
//         //         Num = 9,
//         //         NutNum = 9,
//         //         WasherNum = 9,
//         //     };
//         //     string json = JsonSerializer.Serialize(constructionRequest);
//         //     var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
//         //     var endpoint = $"/api/specifications/{id}";

//         //     // Act
//         //     var response = await _httpClient.PatchAsync(endpoint, httpContent);

//         //     // Assert
//         //     Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
//         // }

//         // [Fact]
//         // public async Task Update_ShouldReturnBadRequest_WhenWrongValues()
//         // {
//         //     // Arrange
//         //     int id = 1;
//         //     int diameterId = 3;
//         //     var wrongConstructionRequests = new List<ConstructionCreateRequest>
//         //     {
//         //         new ConstructionCreateRequest
//         //         {
//         //             DiameterId = diameterId,
//         //             Packet = -9,
//         //             Num = 9,
//         //             NutNum = 9,
//         //             WasherNum = 9,
//         //         },
//         //         new ConstructionCreateRequest
//         //         {
//         //             DiameterId = diameterId,
//         //             Packet = 9,
//         //             Num = -9,
//         //             NutNum = 9,
//         //             WasherNum = 9,
//         //         },
//         //         new ConstructionCreateRequest
//         //         {
//         //             DiameterId = diameterId,
//         //             Packet = 9,
//         //             Num = 9,
//         //             NutNum = -9,
//         //             WasherNum = 9,
//         //         },
//         //         new ConstructionCreateRequest
//         //         {
//         //             DiameterId = diameterId,
//         //             Packet = 9,
//         //             Num = 9,
//         //             NutNum = 9,
//         //             WasherNum = -9,
//         //         },
//         //     };
//         //     var httpContent = new StringContent("", Encoding.UTF8, "application/json");
//         //     var endpoint = $"/api/specifications/{id}";

//         //     // Act
//         //     var response = await _httpClient.PatchAsync(endpoint, httpContent);

//         //     // Assert
//         //     Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

//         //     foreach (var wrongConstructionRequest in wrongConstructionRequests)
//         //     {
//         //         var json = JsonSerializer.Serialize(wrongConstructionRequest);
//         //         httpContent = new StringContent(json, Encoding.UTF8, "application/json");

//         //         // Act
//         //         response = await _httpClient.PatchAsync(endpoint, httpContent);

//         //         // Assert
//         //         Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
//         //     }
//         // }

//         // [Fact]
//         // public async Task Update_ShouldReturnNotFound_WhenWrongValues()
//         // {
//         //     // Arrange
//         //     int id = 1;
//         //     int diameterId = 3;
//         //     var constructionRequest = new ConstructionUpdateRequest
//         //     {
//         //         DiameterId = diameterId,
//         //         Packet = 9,
//         //         Num = 9,
//         //         NutNum = 9,
//         //         WasherNum = 9,
//         //     };
//         //     var wrongConstructionRequest = new ConstructionUpdateRequest
//         //     {
//         //         DiameterId = 999,
//         //         Packet = 9,
//         //         Num = 9,
//         //         NutNum = 9,
//         //         WasherNum = 9,
//         //     };
//         //     string json1 = JsonSerializer.Serialize(wrongConstructionRequest);
//         //     string json2 = JsonSerializer.Serialize(constructionRequest);
//         //     var httpContent1 = new StringContent(json1, Encoding.UTF8, "application/json");
//         //     var httpContent2 = new StringContent(json2, Encoding.UTF8, "application/json");
//         //     var endpoint1 = $"/api/specifications/{id}";
//         //     var endpoint2 = $"/api/specifications/{999}";

//         //     // Act
//         //     var response1 = await _httpClient.PatchAsync(endpoint1, httpContent1);
//         //     var response2 = await _httpClient.PatchAsync(endpoint2, httpContent2);

//         //     // Assert
//         //     Assert.Equal(HttpStatusCode.NotFound, response1.StatusCode);
//         //     Assert.Equal(HttpStatusCode.NotFound, response2.StatusCode);
//         // }

//         // [Fact]
//         // public async Task Update_ShouldReturnConflict_WhenConflictValues()
//         // {
//         //     // Arrange
//         //     int id = 3;
//         //     int diameterId = 3;
//         //     var constructionRequest = new ConstructionUpdateRequest
//         //     {
//         //         DiameterId = diameterId,
//         //         Packet = 9,
//         //         Num = 9,
//         //         NutNum = 9,
//         //         WasherNum = 9,
//         //     };
//         //     string json = JsonSerializer.Serialize(constructionRequest);
//         //     var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
//         //     var endpoint = $"/api/specifications/{id}";

//         //     // Act
//         //     var response = await _httpClient.PatchAsync(endpoint, httpContent);

//         //     // Assert
//         //     Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
//         // }

//         // [Fact]
//         // public async Task Update_ShouldReturnUnauthorized_WhenNoAccessToken()
//         // {
//         //     // Arrange
//         //     int id = 1;
//         //     int diameterId = 3;
//         //     var constructionRequest = new ConstructionUpdateRequest
//         //     {
//         //         DiameterId = diameterId,
//         //         Packet = 9,
//         //         Num = 9,
//         //         NutNum = 9,
//         //         WasherNum = 9,
//         //     };
//         //     string json = JsonSerializer.Serialize(constructionRequest);
//         //     var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
//         //     var endpoint = $"/api/specifications/{id}";

//         //     // Act
//         //     var response = await _authHttpClient.PatchAsync(endpoint, httpContent);

//         //     // Assert
//         //     Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//         // }

//         // ------------------------------------DELETE------------------------------------

//         // [Fact]
//         // public async Task Delete_ShouldReturnNoContent()
//         // {
//         //     // Arrange
//         //     int id = 2;
//         //     var endpoint = $"/api/constructions/{id}";

//         //     // Act
//         //     var response = await _httpClient.DeleteAsync(endpoint);

//         //     // Assert
//         //     Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
//         // }

//         // [Fact]
//         // public async Task Delete_ShouldReturnNotFound_WhenWrongId()
//         // {
//         //     // Arrange
//         //     var endpoint = $"/api/constructions/{999}";

//         //     // Act
//         //     var response = await _httpClient.DeleteAsync(endpoint);

//         //     // Assert
//         //     Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
//         // }

//         // [Fact]
//         // public async Task Delete_ShouldReturnUnauthorized_WhenNoAccessToken()
//         // {
//         //     // Arrange
//         //     int id = 2;
//         //     var endpoint = $"/api/constructions/{id}";

//         //     // Act
//         //     var response = await _authHttpClient.DeleteAsync(endpoint);

//         //     // Assert
//         //     Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//         // }
//     }
// }
