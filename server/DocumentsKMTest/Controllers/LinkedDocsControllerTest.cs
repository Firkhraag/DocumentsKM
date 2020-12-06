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
//             TestData.linkedDocs.Where(v => v.Type.Id == docTypeId).Should().BeEquivalentTo(
//                 JsonSerializer.Deserialize<IEnumerable<LinkedDoc>>(responseBody, options));
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












// using System;
// using System.Collections.Generic;
// using System.Net.Mime;
// using AutoMapper;
// using DocumentsKM.Dtos;
// using DocumentsKM.Models;
// using DocumentsKM.Services;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;

// namespace DocumentsKM.Controllers
// {
//     [Route("api")]
//     [Authorize]
//     [ApiController]
//     [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//     public class LinkedDocsController : ControllerBase
//     {
//         private readonly ILinkedDocService _service;

//         public LinkedDocsController(ILinkedDocService linkedDocService)
//         {
//             _service = linkedDocService;
//         }

//         [HttpGet, Route("linked-docs-types/{docTypeId}/docs")]
//         [ProducesResponseType(StatusCodes.Status200OK)]
//         public ActionResult<IEnumerable<LinkedDoc>> GetAllByDocTypeId(int docTypeId)
//         {
//             var linkedDocs = _service.GetAllByDocTypeId(docTypeId);
//             return Ok(linkedDocs);
//         }
//     }
// }
