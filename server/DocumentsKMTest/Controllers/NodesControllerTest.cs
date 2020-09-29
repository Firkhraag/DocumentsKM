using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace DocumentsKM.Tests
{
    public class NodesControllerTest : IClassFixture<WebApplicationFactory<DocumentsKM.Startup>>
    {
        private readonly HttpClient httpClient;

        public NodesControllerTest(WebApplicationFactory<DocumentsKM.Startup> factory)
        {
            httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllNodes_ReturnsUnauthorized()
        {
            // Arrange
            var endpoint = "/api/projects/0/nodes";

            // Act
            var response = await httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
