using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DocumentsKM.Tests
{
    public class EmployeesControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
    {
        private readonly HttpClient _authHttpClient;
        private readonly HttpClient _httpClient;
        private readonly Random _rnd = new Random();

        public EmployeesControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
        {
            _httpClient = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();
            
            _authHttpClient = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllByDepartmentId_ShouldReturnOK()
        {
            // Arrange
            int departmentId = _rnd.Next(1, TestData.departments.Count());
            var endpoint = $"/api/departments/{departmentId}/employees";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllByDepartmentId_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int departmentId = _rnd.Next(1, TestData.departments.Count());
            var endpoint = $"/api/departments/{departmentId}/employees";

            // Act
            var response = await _authHttpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetMarkApprovalEmployeesByDepartmentId_ShouldReturnOK()
        {
            // Arrange
            int[] approvalPosIds = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
            int departmentId = _rnd.Next(1, TestData.departments.Count());
            var endpoint = $"/api/departments/{departmentId}/mark-approval-employees";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetMarkApprovalEmployeesByDepartmentId_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int departmentId = _rnd.Next(1, TestData.departments.Count());
            var endpoint = $"/api/departments/{departmentId}/mark-approval-employees";

            // Act
            var response = await _authHttpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetMarkMainEmployeesByDepartmentId_ShouldReturnOK()
        {
            // Arrange
            int[] approvalPosIds = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
            int departmentId = _rnd.Next(1, TestData.departments.Count());
            var endpoint = $"/api/departments/{departmentId}/mark-main-employees";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetMarkMainEmployeesByDepartmentId_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int departmentId = _rnd.Next(1, TestData.departments.Count());
            var endpoint = $"/api/departments/{departmentId}/mark-main-employees";

            // Act
            var response = await _authHttpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
