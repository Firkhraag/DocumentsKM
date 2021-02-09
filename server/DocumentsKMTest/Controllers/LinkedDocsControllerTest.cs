// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net;
// using System.Net.Http;
// using System.Text.Json;
// using System.Threading.Tasks;
// using DocumentsKM.Models;
// using FluentAssertions;
// using Microsoft.AspNetCore.Authorization.Policy;
// using Microsoft.AspNetCore.TestHost;
// using Microsoft.Extensions.DependencyInjection;
// using Xunit;

// namespace DocumentsKM.Tests
// {
//     public class LinkedDocsControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
//     {
//         private readonly HttpClient _authHttpClient;
//         private readonly HttpClient _httpClient;
//         private readonly Random _rnd = new Random();

//         public LinkedDocsControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
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
//         public async Task GetAllByDocTypeId_ShouldReturnOK_WhenAccessTokenIsProvided()
//         {
//             // Arrange
//             int docTypeId = _rnd.Next(1, TestData.linkedDocTypes.Count());
//             var endpoint = $"/api/linked-docs-types/{docTypeId}/docs";

//             // Act
//             var response = await _httpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//             string responseBody = await response.Content.ReadAsStringAsync();

//             var options = new JsonSerializerOptions()
//             {
//                 PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//             };
//             JsonSerializer.Deserialize<IEnumerable<LinkedDoc>>(
//                 responseBody, options).Should().BeEquivalentTo(TestData.linkedDocs.Where(
//                     v => v.Type.Id == docTypeId));
//         }

//         [Fact]
//         public async Task GetAllByDocTypeId_ShouldReturnUnauthorized_WhenNoAccessToken()
//         {
//             // Arrange
//             int docTypeId = _rnd.Next(1, TestData.linkedDocTypes.Count());
//             var endpoint = $"/api/linked-docs-types/{docTypeId}/docs";

//             // Act
//             var response = await _authHttpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//         }
//     }
// }
