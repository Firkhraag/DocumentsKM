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
//     public class SubnodesControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
//     {
//         private readonly HttpClient _authHttpClient;
//         private readonly HttpClient _httpClient;
//         private readonly Random _rnd = new Random();

//         public SubnodesControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
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
//         public async Task GetAllByNodeId_ShouldReturnOK()
//         {
//             // Arrange
//             int nodeId = _rnd.Next(1, TestData.nodes.Count());
//             var endpoint = $"/api/nodes/{nodeId}/subnodes";

//             // Act
//             var response = await _httpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//         }

//         [Fact]
//         public async Task GetAllByNodeId_ShouldReturnUnauthorized_WhenNoAccessToken()
//         {
//             // Arrange
//             int nodeId = _rnd.Next(1, TestData.nodes.Count());
//             var endpoint = $"/api/nodes/{nodeId}/subnodes";

//             // Act
//             var response = await _authHttpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//         }
//     }
// }
