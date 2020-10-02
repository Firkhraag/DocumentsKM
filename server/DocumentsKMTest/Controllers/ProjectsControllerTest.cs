using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ProjectsControllerTest : IClassFixture<WebApplicationFactory<DocumentsKM.Startup>>
    {
        private readonly HttpClient httpClient;

        public ProjectsControllerTest(WebApplicationFactory<DocumentsKM.Startup> factory)
        {
            httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            var endpoint = "/api/projects";

            // Act
            var response = await httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
