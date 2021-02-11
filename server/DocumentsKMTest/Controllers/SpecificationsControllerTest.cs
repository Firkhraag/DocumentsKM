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
//     // TBD: Create, Update, Delete
//     public class SpecificationsControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
//     {
//         private readonly HttpClient _authHttpClient;
//         private readonly HttpClient _httpClient;
//         private readonly Random _rnd = new Random();

//         public SpecificationsControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
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
//         public async Task GetAllByMarkId_ShouldReturnOK_WhenAccessTokenIsProvided()
//         {
//             // Arrange
//             int markId = _rnd.Next(1, TestData.marks.Count());
//             var endpoint = $"/api/marks/{markId}/specifications";

//             // Act
//             var response = await _httpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//             string responseBody = await response.Content.ReadAsStringAsync();

//             var specifications = TestData.specifications.Where(v => v.Mark.Id == markId)
//                 .Select(s => new SpecificationResponse
//                 {
//                     Id = s.Id,
//                     Num = s.Num,
//                     IsCurrent = s.IsCurrent,
//                     Note = s.Note,
//                     CreatedDate = s.CreatedDate,
//                 }).ToArray();
//             var options = new JsonSerializerOptions()
//             {
//                 PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//             };
//             JsonSerializer.Deserialize<IEnumerable<SpecificationResponse>>(
//                 responseBody, options).Should().BeEquivalentTo(specifications);
//         }

//         [Fact]
//         public async Task GetAllByMarkId_ShouldReturnUnauthorized_WhenNoAccessToken()
//         {
//             // Arrange
//             int markId = _rnd.Next(1, TestData.marks.Count());
//             var endpoint = $"/api/marks/{markId}/specifications";

//             // Act
//             var response = await _authHttpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//         }

//         // ------------------------------------DELETE------------------------------------

//         // [Fact]
//         // public async Task Delete_ShouldReturnNoContent()
//         // {
//         //     // Arrange
//         //     int id = 2;
//         //     var endpoint = $"/api/specifications/{id}";

//         //     // Act
//         //     var response = await _httpClient.DeleteAsync(endpoint);

//         //     // Assert
//         //     Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
//         // }

//         // [Fact]
//         // public async Task Delete_ShouldReturnNotFound_WhenWrongId()
//         // {
//         //     // Arrange
//         //     var endpoint = $"/api/specifications/{999}";

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
//         //     var endpoint = $"/api/specifications/{id}";

//         //     // Act
//         //     var response = await _authHttpClient.DeleteAsync(endpoint);

//         //     // Assert
//         //     Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//         // }
//     }
// }