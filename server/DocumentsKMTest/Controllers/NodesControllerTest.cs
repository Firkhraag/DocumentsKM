// using System;
// using System.Linq;
// using System.Net;
// using System.Net.Http;
// using System.Threading.Tasks;
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
//         public async Task GetAllByProjectId_ShouldReturnOK()
//         {
//             // Arrange
//             int projectId = _rnd.Next(1, TestData.projects.Count());
//             var endpoint = $"/api/projects/{projectId}/nodes";

//             // Act
//             var response = await _httpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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
