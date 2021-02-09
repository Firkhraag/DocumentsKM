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
//     public class NodesControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
//     {
//         private readonly HttpClient _authHttpClient;
//         private readonly HttpClient _httpClient;
//         private readonly Random _rnd = new Random();

//         public NodesControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
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
//         public async Task GetAllByProjectId_ShouldReturnOK_WhenAccessTokenIsProvided()
//         {
//             // Arrange
//             int projectId = _rnd.Next(1, TestData.projects.Count());
//             var endpoint = $"/api/projects/{projectId}/nodes";

//             // Act
//             var response = await _httpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//             string responseBody = await response.Content.ReadAsStringAsync();

//             var nodes = TestData.nodes.Where(v => v.Project.Id == projectId)
//                 .Select(s => new NodeResponse
//                 {
//                     Id = s.Id,
//                     Code = s.Code,
//                     Name = s.Name,
//                     ChiefEngineer = new EmployeeBaseResponse
//                     {
//                         Id = s.ChiefEngineer.Id,
//                         Name = s.ChiefEngineer.Name,
//                     }
//                 }).ToArray();
//             var options = new JsonSerializerOptions()
//             {
//                 PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//             };
//             JsonSerializer.Deserialize<IEnumerable<NodeResponse>>(
//                 responseBody, options).Should().BeEquivalentTo(nodes);
//         }

//         [Fact]
//         public async Task GetAllByProjectId_ShouldReturnUnauthorized_WhenNoAccessToken()
//         {
//             // Arrange
//             int projectId = _rnd.Next(1, TestData.projects.Count());
//             var endpoint = $"/api/projects/{projectId}/nodes";

//             // Act
//             var response = await _authHttpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//         }
//     }
// }
