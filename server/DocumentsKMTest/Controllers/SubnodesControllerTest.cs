using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace DocumentsKM.Tests
{
    public class SubnodesControllerTest : IClassFixture<WebApplicationFactory<DocumentsKM.Startup>>
    {
        private readonly HttpClient httpClient;

        public SubnodesControllerTest(WebApplicationFactory<DocumentsKM.Startup> factory)
        {
            httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllSubnodes_ReturnsUnauthorized()
        {
            // Arrange
            var endpoint = "/api/nodes/0/subnodes";

            // Act
            var response = await httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
