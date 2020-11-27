// using System.Collections.Generic;
// using System.Net;
// using System.Net.Http;
// using System.Text.Json;
// using System.Threading.Tasks;
// using AutoMapper;
// using DocumentsKM.Dtos;
// using DocumentsKM.Models;
// using FluentAssertions;
// using Microsoft.AspNetCore.Authorization.Policy;
// using Microsoft.AspNetCore.TestHost;
// using Microsoft.Extensions.DependencyInjection;
// using Xunit;

// namespace DocumentsKM.Tests
// {
//     public class ProjectsControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
//     {
//         private readonly HttpClient _authHttpClient;
//         private readonly HttpClient _httpClient;
//         private readonly IMapper _mapper;

//         public ProjectsControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory,
//             IMapper mapper)
//         {
            
//             _httpClient = factory.WithWebHostBuilder(builder =>
//             {
//                 builder.ConfigureTestServices(services =>
//                 {
//                     services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
//                 });
//             }).CreateClient();
            
//             _authHttpClient = factory.CreateClient();

//             _mapper = mapper;
//         }

//         [Fact]
//         public async Task GetAll_ShouldReturnOK_WhenAccessTokenIsProvided()
//         {
//             // Arrange
//             var endpoint = "/api/projects";

//             // Act
//             var response = await _httpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//             string responseBody = await response.Content.ReadAsStringAsync();

//             var options = new JsonSerializerOptions()
//             {
//                 PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//             };
//             _mapper.Map<IEnumerable<ProjectBaseResponse>>(TestData.projects).Should().BeEquivalentTo(
//                 JsonSerializer.Deserialize<IEnumerable<Project>>(responseBody, options));
//         }

//         [Fact]
//         public async Task GetAll_ShouldReturnUnauthorized_WhenNoAccessToken()
//         {
//             // Arrange
//             var endpoint = "/api/projects";

//             // Act
//             var response = await _authHttpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//         }
//     }
// }
