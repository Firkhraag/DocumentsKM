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
//     public class MarkGeneralDataPointsControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
//     {
//         private readonly HttpClient _authHttpClient;
//         private readonly HttpClient _httpClient;
//         private readonly Random _rnd = new Random();

//         private readonly int _maxMarkId = 3;
//         private readonly int _maxSectionId = 3;

//         public MarkGeneralDataPointsControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
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
//             int markId = _rnd.Next(1, _maxMarkId);
//             int sectionId = _rnd.Next(1, _maxSectionId);
//             var endpoint = $"/api/marks/{markId}/general-data-sections/{sectionId}/general-data-points";

//             // Act
//             var response = await _httpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//             string responseBody = await response.Content.ReadAsStringAsync();

//             var markGeneralDataPoints = TestData.markGeneralDataPoints.Where(
//                 v => v.Mark.Id == markId && v.Section.Id == sectionId)
//                 .Select(v => new MarkGeneralDataPointResponse
//                 {
//                     Id = v.Id,
//                     Text = v.Text,
//                     OrderNum = v.OrderNum,
//                 }).ToArray();
//             var options = new JsonSerializerOptions()
//             {
//                 PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//             };
//             JsonSerializer.Deserialize<IEnumerable<MarkGeneralDataPointResponse>>(
//                 responseBody, options).Should().BeEquivalentTo(markGeneralDataPoints);
//         }

//         [Fact]
//         public async Task GetAllByMarkId_ShouldReturnUnauthorized_WhenNoAccessToken()
//         {
//             // Arrange
//             int markId = _rnd.Next(1, _maxMarkId);
//             int sectionId = _rnd.Next(1, _maxSectionId);
//             var endpoint = $"/api/marks/{markId}/general-data-sections/{sectionId}/general-data-points";

//             // Act
//             var response = await _authHttpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//         }
//     }
// }
