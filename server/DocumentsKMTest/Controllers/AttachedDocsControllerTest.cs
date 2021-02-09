// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net;
// using System.Net.Http;
// using System.Text;
// using System.Text.Json;
// using System.Threading.Tasks;
// using DocumentsKM.Dtos;
// using FluentAssertions;
// using Microsoft.AspNetCore.Authorization.Policy;
// using Microsoft.AspNetCore.TestHost;
// using Microsoft.Extensions.DependencyInjection;
// using Xunit;

// namespace DocumentsKM.Tests
// {
//     // TBD: Create, Update, Delete
//     public class AttachedDocsControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
//     {
//         private readonly HttpClient _authHttpClient;
//         private readonly HttpClient _httpClient;
//         private readonly Random _rnd = new Random();

//         public AttachedDocsControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
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

//         [Fact]
//         public async Task GetAllByMarkId_ShouldReturnOK_WhenAccessTokenIsProvided()
//         {
//             // Arrange
//             int markId = _rnd.Next(1, TestData.marks.Count());
//             var endpoint = $"/api/marks/{markId}/attached-docs";

//             // Act
//             var response = await _httpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//             string responseBody = await response.Content.ReadAsStringAsync();

//             var attachedDocs = TestData.attachedDocs.Where(
//                 v => v.Mark.Id == markId)
//                     .Select(d => new AttachedDocResponse
//                     {
//                         Id = d.Id,
//                         Designation = d.Designation,
//                         Name = d.Name,
//                         Note = d.Note,
//                     }).ToArray();
//             var options = new JsonSerializerOptions()
//             {
//                 PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//             };
//             JsonSerializer.Deserialize<IEnumerable<AttachedDocResponse>>(
//                 responseBody, options).Should().BeEquivalentTo(attachedDocs);
//         }

//         [Fact]
//         public async Task GetAllByMarkId_ShouldReturnUnauthorized_WhenNoAccessToken()
//         {
//             // Arrange
//             int markId = _rnd.Next(1, TestData.marks.Count());
//             var endpoint = $"/api/marks/{markId}/attached-docs";

//             // Act
//             var response = await _authHttpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//         }

//         // [Fact]
//         // public async Task Create_ShouldReturnOK_WhenAccessTokenIsProvided()
//         // {
//         //     // Arrange
//         //     int markId = _rnd.Next(1, TestData.marks.Count());
//         //     var attachedDocRequest = new AttachedDocCreateRequest
//         //     {
//         //         Designation = "NewCreate",
//         //         Name = "NewCreate",
//         //     };
//         //     string json = JsonSerializer.Serialize(attachedDocRequest);
//         //     var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
//         //     var endpoint = $"/api/marks/{markId}/attached-docs";

//         //     var getEndpoint = $"/api/marks/{markId}/attached-docs";

//         //     // Act
//         //     var response = await _httpClient.PostAsync(endpoint, httpContent);
//         //     var getResponse = await _httpClient.GetAsync(getEndpoint);

//         //     // Assert
//         //     Assert.Equal(HttpStatusCode.Created, response.StatusCode);
//         //     string getResponseBody = await getResponse.Content.ReadAsStringAsync();
//         //     var options = new JsonSerializerOptions()
//         //     {
//         //         PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//         //     };
//         //     Assert.Equal(TestData.attachedDocs.Where(v => v.Mark.Id == markId).Count() + 1,
//         //         JsonSerializer.Deserialize<IEnumerable<AttachedDocResponse>>(getResponseBody, options).Count());
//         // }

//         // [Fact]
//         // public async Task Create_ShouldReturnUnauthorized_WhenNoAccessToken()
//         // {
//         //     // Arrange
//         //     int markId = _rnd.Next(1, TestData.marks.Count());
//         //     var attachedDocRequest = new AttachedDocCreateRequest
//         //     {
//         //         Designation = "NewCreate",
//         //         Name = "NewCreate",
//         //     };
//         //     string json = JsonSerializer.Serialize(attachedDocRequest);
//         //     var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
//         //     var endpoint = $"/api/marks/{markId}/attached-docs";

//         //     // Act
//         //     var response = await _authHttpClient.PostAsync(endpoint, httpContent);

//         //     // Assert
//         //     Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//         // }

//         // [Fact]
//         // public async Task Create_ShouldReturnBadRequest_WhenAccessTokenIsProvided()
//         // {
//         //     // Arrange
//         //     int markId = _rnd.Next(1, TestData.marks.Count());
//         //     var wrongAttachedDocRequest1 = new AttachedDocCreateRequest
//         //     {
//         //         Name = "NewCreate",
//         //     };
//         //     var wrongAttachedDocRequest2 = new AttachedDocCreateRequest
//         //     {
//         //         Designation = "NewCreate",
//         //     };
//         //     string json1 = JsonSerializer.Serialize(wrongAttachedDocRequest1);
//         //     string json2 = JsonSerializer.Serialize(wrongAttachedDocRequest2);
//         //     var httpContent1 = new StringContent(json1, Encoding.UTF8, "application/json");
//         //     var httpContent2 = new StringContent(json2, Encoding.UTF8, "application/json");
//         //     var endpoint = $"/api/marks/{markId}/attached-docs";

//         //     // Act
//         //     var response1 = await _httpClient.PostAsync(endpoint, httpContent1);
//         //     var response2 = await _httpClient.PostAsync(endpoint, httpContent2);

//         //     // Assert
//         //     Assert.Equal(HttpStatusCode.BadRequest, response1.StatusCode);
//         //     Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
//         // }

//         // [Fact]
//         // public async Task Update_ShouldReturnNoContent_WhenAccessTokenIsProvided()
//         // {
//         //     // Arrange
//         //     int id = _rnd.Next(1, TestData.attachedDocs.Count());
//         //     var attachedDocRequest = new AttachedDocUpdateRequest
//         //     {
//         //         Name = "NewUpdate",
//         //     };
//         //     string json = JsonSerializer.Serialize(attachedDocRequest);
//         //     var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
//         //     var endpoint = $"/api/attached-docs/{id}";

//         //     // Act
//         //     var response = await _httpClient.PatchAsync(endpoint, httpContent);

//         //     // Assert
//         //     Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
//         // }

//         // [Fact]
//         // public async Task Update_ShouldReturnUnauthorized_WhenNoAccessToken()
//         // {
//         //     // Arrange
//         //     int id = _rnd.Next(1, TestData.attachedDocs.Count());
//         //     var attachedDocRequest = new AttachedDocUpdateRequest
//         //     {
//         //         Name = "NewUpdate",
//         //     };
//         //     string json = JsonSerializer.Serialize(attachedDocRequest);
//         //     var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
//         //     var endpoint = $"/api/attached-docs/{id}";

//         //     // Act
//         //     var response = await _authHttpClient.PatchAsync(endpoint, httpContent);

//         //     // Assert
//         //     Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//         // }

//         // [Fact]
//         // public async Task Delete_ShouldReturnNoContent_WhenAccessTokenIsProvided()
//         // {
//         //     // Arrange
//         //     int id = _rnd.Next(1, TestData.attachedDocs.Count());
//         //     var endpoint = $"/api/attached-docs/{id}";

//         //     int markId = TestData.attachedDocs.FirstOrDefault(v => v.Id == id).Mark.Id;
//         //     var getEndpoint = $"/api/marks/{markId}/attached-docs";

//         //     // Act
//         //     var response = await _httpClient.DeleteAsync(endpoint);
//         //     var getResponse = await _httpClient.GetAsync(getEndpoint);

//         //     // Assert
//         //     Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

//         //     string getResponseBody = await getResponse.Content.ReadAsStringAsync();
//         //     var options = new JsonSerializerOptions()
//         //     {
//         //         PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//         //     };
//         //     Assert.Equal(TestData.attachedDocs.Where(v => v.Mark.Id == markId).Count() - 1,
//         //         JsonSerializer.Deserialize<IEnumerable<AttachedDocResponse>>(getResponseBody, options).Count());
//         // }

//         // [Fact]
//         // public async Task Delete_ShouldReturnUnauthorized_WhenNoAccessToken()
//         // {
//         //     // Arrange
//         //     int id = _rnd.Next(1, TestData.attachedDocs.Count());
//         //     var endpoint = $"/api/attached-docs/{id}";

//         //     // Act
//         //     var response = await _authHttpClient.DeleteAsync(endpoint);

//         //     // Assert
//         //     Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//         // }
//     }
// }
