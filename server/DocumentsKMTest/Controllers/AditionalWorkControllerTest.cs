// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net;
// using System.Net.Http;
// using System.Text.Json;
// using System.Threading.Tasks;
// using DocumentsKM.Dtos;
// using DocumentsKM.Models;
// using FluentAssertions;
// using Microsoft.AspNetCore.Authorization.Policy;
// using Microsoft.AspNetCore.TestHost;
// using Microsoft.Extensions.DependencyInjection;
// using Xunit;

// namespace DocumentsKM.Tests
// {
//     // TBD: Update, Delete
//     public class AdditionalWorkControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
//     {
//         private readonly HttpClient _authHttpClient;
//         private readonly HttpClient _httpClient;
//         private readonly Random _rnd = new Random();

//         public AdditionalWorkControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
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
//             var endpoint = $"/api/marks/{markId}/additional-work";

//             // Act
//             var response = await _httpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//             string responseBody = await response.Content.ReadAsStringAsync();

//             var docs = TestData.docs.Where(v => v.Mark.Id == markId);
//             var docsGroupedByCreator = docs.Where
//                 (v => v.Creator != null).GroupBy(d => d.Creator).Select(
//                     g => new Doc
//                     {
//                         Creator = g.First().Creator,
//                         Form = g.Sum(v => v.Form),
//                     });
//             var docsGroupedByNormContr = docs.Where(
//                 v => v.NormContr != null).GroupBy(d => d.NormContr).Select(
//                     g => new Doc
//                     {
//                         NormContr = g.First().NormContr,
//                         Form = g.Sum(v => v.Form),
//                     });


//             var additionalWork = TestData.additionalWork.Where(v => v.Mark.Id == markId)
//                 .Select(v => new AdditionalWorkResponse
//                 {
//                     Id = v.Id,
//                     Employee = new EmployeeBaseResponse
//                     {
//                         Id = v.Employee.Id,
//                         Name = v.Employee.Name,
//                     },
//                     Valuation = v.Valuation,
//                     MetalOrder = v.MetalOrder,
//                     DrawingsCompleted = docsGroupedByCreator.SingleOrDefault(
//                         d => d.Creator.Id == v.Employee.Id)?.Form ?? 0,
//                     DrawingsCheck = docsGroupedByNormContr.SingleOrDefault(
//                         d => d.NormContr.Id == v.Employee.Id)?.Form ?? 0,
//                 }).ToArray();
//             var options = new JsonSerializerOptions()
//             {
//                 PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//             };
//             JsonSerializer.Deserialize<IEnumerable<AdditionalWorkResponse>>(
//                 responseBody, options).Should().BeEquivalentTo(additionalWork);
//         }

//         [Fact]
//         public async Task GetAllByMarkId_ShouldReturnUnauthorized_WhenNoAccessToken()
//         {
//             // Arrange
//             int markId = _rnd.Next(1, TestData.marks.Count());
//             var endpoint = $"/api/marks/{markId}/additional-work";

//             // Act
//             var response = await _authHttpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//         }
//     }
// }
