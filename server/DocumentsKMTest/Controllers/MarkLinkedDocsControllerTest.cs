// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net;
// using System.Net.Http;
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
//     public class MarkLinkedDocsControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
//     {
//         private readonly HttpClient _authHttpClient;
//         private readonly HttpClient _httpClient;
//         private readonly Random _rnd = new Random();

//         public MarkLinkedDocsControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
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
//             var endpoint = $"/api/marks/{markId}/mark-linked-docs";

//             // Act
//             var response = await _httpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//             string responseBody = await response.Content.ReadAsStringAsync();

//             var markLinkedDocs = TestData.markLinkedDocs.Where(v => v.Mark.Id == markId)
//                 .Select(v => new MarkLinkedDocResponse
//                 {
//                     Id = v.Id,
//                     LinkedDoc = v.LinkedDoc,
//                 }).ToArray();
//             var options = new JsonSerializerOptions()
//             {
//                 PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//             };
//             JsonSerializer.Deserialize<IEnumerable<MarkLinkedDocResponse>>(
//                 responseBody, options).Should().BeEquivalentTo(markLinkedDocs);
//         }

//         [Fact]
//         public async Task GetAllByMarkId_ShouldReturnUnauthorized_WhenNoAccessToken()
//         {
//             // Arrange
//             int markId = _rnd.Next(1, TestData.marks.Count());
//             var endpoint = $"/api/marks/{markId}/mark-linked-docs";

//             // Act
//             var response = await _authHttpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//         }
//     }
// }