using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace DocumentsKM.Tests
{
    public class MarksControllerTest : IClassFixture<WebApplicationFactory<DocumentsKM.Startup>>
    {
        private readonly HttpClient httpClient;

        public MarksControllerTest(WebApplicationFactory<DocumentsKM.Startup> factory)
        {
            httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllMarks_ReturnsUnauthorized()
        {
            // Arrange
            var endpoint = "/api/subnodes/0/marks";

            // Act
            var response = await httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
