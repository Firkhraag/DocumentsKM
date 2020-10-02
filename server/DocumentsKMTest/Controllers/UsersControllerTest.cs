using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DocumentsKM.Controllers;
using DocumentsKM.Dtos;
using DocumentsKM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class UsersControllerTest : IClassFixture<WebApplicationFactory<DocumentsKM.Startup>>
    {
        private readonly HttpClient httpClient;

        public UsersControllerTest(WebApplicationFactory<DocumentsKM.Startup> factory)
        {
            httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task Login_ReturnsBadRequest_WhenModelStateIsInvalid()
        {

        }

        [Fact]
        public async Task Login_ReturnsOkResult_WhenModelStateIsValid()
        {

        }
    }
}
