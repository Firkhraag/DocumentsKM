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
//     public class EmployeesControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
//     {
//         private readonly HttpClient _authHttpClient;
//         private readonly HttpClient _httpClient;
//         private readonly Random _rnd = new Random();

//         public EmployeesControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
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
//         public async Task GetAllByDepartmentId_ShouldReturnOK_WhenAccessTokenIsProvided()
//         {
//             // Arrange
//             int departmentId = _rnd.Next(1, TestData.departments.Count());
//             var endpoint = $"/api/departments/{departmentId}/employees";

//             // Act
//             var response = await _httpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//             string responseBody = await response.Content.ReadAsStringAsync();

//             var employees = TestData.employees.Where(v => v.Department.Id == departmentId)
//                 .Select(e => new EmployeeBaseResponse
//                 {
//                     Id = e.Id,
//                     Name = e.Name,
//                 }).ToArray();
//             var options = new JsonSerializerOptions()
//             {
//                 PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//             };
//             JsonSerializer.Deserialize<IEnumerable<EmployeeBaseResponse>>(
//                 responseBody, options).Should().BeEquivalentTo(employees);
//         }

//         [Fact]
//         public async Task GetAllByDepartmentId_ShouldReturnUnauthorized_WhenNoAccessToken()
//         {
//             // Arrange
//             int departmentId = _rnd.Next(1, TestData.departments.Count());
//             var endpoint = $"/api/departments/{departmentId}/employees";

//             // Act
//             var response = await _authHttpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//         }

//         [Fact]
//         public async Task GetMarkApprovalEmployeesByDepartmentId_ShouldReturnOK_WhenAccessTokenIsProvided()
//         {
//             // Arrange
//             int[] approvalPosIds = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
//             int departmentId = _rnd.Next(1, TestData.departments.Count());
//             var endpoint = $"/api/departments/{departmentId}/mark-approval-employees";

//             // Act
//             var response = await _httpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//             string responseBody = await response.Content.ReadAsStringAsync();

//             var employees = TestData.employees.Where(
//                 v => v.Department.Id == departmentId && approvalPosIds.Contains(v.Position.Id))
//                     .Select(e => new EmployeeBaseResponse
//                     {
//                         Id = e.Id,
//                         Name = e.Name,
//                     }).ToArray();
//             var options = new JsonSerializerOptions()
//             {
//                 PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//             };
//             JsonSerializer.Deserialize<IEnumerable<EmployeeBaseResponse>>(
//                 responseBody, options).Should().BeEquivalentTo(employees);
//         }

//         [Fact]
//         public async Task GetMarkApprovalEmployeesByDepartmentId_ShouldReturnUnauthorized_WhenNoAccessToken()
//         {
//             // Arrange
//             int departmentId = _rnd.Next(1, TestData.departments.Count());
//             var endpoint = $"/api/departments/{departmentId}/mark-approval-employees";

//             // Act
//             var response = await _authHttpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//         }

//         [Fact]
//         public async Task GetMarkMainEmployeesByDepartmentId_ShouldReturnOK_WhenAccessTokenIsProvided()
//         {
//             // Arrange
//             int[] approvalPosIds = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
//             int departmentId = _rnd.Next(1, TestData.departments.Count());
//             var endpoint = $"/api/departments/{departmentId}/mark-main-employees";
//             var departmentHeadPosId = 7;
//             var chiefSpecialistPosId = 9;
//             var groupLeaderPosId = 10;
//             var mainBuilderPosId = 4;

//             // Act
//             var response = await _httpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//             string responseBody = await response.Content.ReadAsStringAsync();

//             var employee = TestData.employees.SingleOrDefault(
//                 v => v.Department.Id == departmentId && v.Position.Id == departmentHeadPosId);
//             var departmentHead = new EmployeeBaseResponse
//             {
//                 Id = employee.Id,
//                 Name = employee.Name,
//             };
//             var chiefSpecialists = TestData.employees.Where(
//                 v => v.Department.Id == departmentId && v.Position.Id == chiefSpecialistPosId)
//                     .Select(e => new EmployeeBaseResponse
//                     {
//                         Id = e.Id,
//                         Name = e.Name,
//                     }).ToArray();
//             var groupLeaders = TestData.employees.Where(
//                 v => v.Department.Id == departmentId && v.Position.Id == groupLeaderPosId)
//                     .Select(e => new EmployeeBaseResponse
//                     {
//                         Id = e.Id,
//                         Name = e.Name,
//                     }).ToArray();
//             var mainBuilders = TestData.employees.Where(
//                 v => v.Department.Id == departmentId && v.Position.Id == mainBuilderPosId)
//                     .Select(e => new EmployeeBaseResponse
//                     {
//                         Id = e.Id,
//                         Name = e.Name,
//                     }).ToArray();

//             var options = new JsonSerializerOptions()
//             {
//                 PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//             };
//             var responseModel = JsonSerializer.Deserialize<MarkMainEmployeesResponse>(responseBody, options);
//             Assert.Equal(departmentHead.Id, responseModel.DepartmentHead.Id);
//             responseModel.ChiefSpecialists.Should().BeEquivalentTo(chiefSpecialists);
//             responseModel.GroupLeaders.Should().BeEquivalentTo(groupLeaders);
//             responseModel.MainBuilders.Should().BeEquivalentTo(mainBuilders);
//         }

//         [Fact]
//         public async Task GetMarkMainEmployeesByDepartmentId_ShouldReturnUnauthorized_WhenNoAccessToken()
//         {
//             // Arrange
//             int departmentId = _rnd.Next(1, TestData.departments.Count());
//             var endpoint = $"/api/departments/{departmentId}/mark-main-employees";

//             // Act
//             var response = await _authHttpClient.GetAsync(endpoint);

//             // Assert
//             Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//         }
//     }
// }
