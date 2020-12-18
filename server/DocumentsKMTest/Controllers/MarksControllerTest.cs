using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DocumentsKM.Dtos;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DocumentsKM.Tests
{
    public class MarksControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
    {
        private readonly HttpClient _authHttpClient;
        private readonly HttpClient _httpClient;
        private readonly Random _rnd = new Random();

        public MarksControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
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

        // Added get new mark code endpoint

        [Fact]
        public async Task GetAllBySubnodeId_ShouldReturnOK_WhenAccessTokenIsProvided()
        {
            // Arrange
            int subnodeId = _rnd.Next(1, TestData.subnodes.Count());
            var endpoint = $"/api/subnodes/{subnodeId}/marks";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string responseBody = await response.Content.ReadAsStringAsync();

            var marks = TestData.marks.Where(v => v.Subnode.Id == subnodeId)
                .Select(v => new MarkBaseResponse
                {
                    Id = v.Id,
                    Code = v.Code,
                    Department = v.Department,
                }).ToArray();
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            JsonSerializer.Deserialize<IEnumerable<MarkBaseResponse>>(
                responseBody, options).Should().BeEquivalentTo(marks);
        }

        [Fact]
        public async Task GetAllBySubnodeId_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            var endpoint = $"/api/marks/{markId}/mark-linked-docs";

            // Act
            var response = await _authHttpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetById_ShouldReturnOK_WhenAccessTokenIsProvided()
        {
            // Arrange
            int id = _rnd.Next(1, TestData.marks.Count());
            var endpoint = $"/api/marks/{id}";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string responseBody = await response.Content.ReadAsStringAsync();

            var foundMark = TestData.marks.SingleOrDefault(v => v.Id == id);
            var mark = new MarkBaseResponse
            {
                Id = foundMark.Id,
                Code = foundMark.Code,
                Department = foundMark.Department,
            };
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            JsonSerializer.Deserialize<MarkBaseResponse>(
                responseBody, options).Should().BeEquivalentTo(mark);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenWrongId()
        {
            // Arrange
            int wrongId = 999;
            var endpoint = $"/api/marks/{wrongId}";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetById_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int id = _rnd.Next(1, TestData.marks.Count());
            var endpoint = $"/api/marks/{id}/parents";

            // Act
            var response = await _authHttpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetMarkParentResponseById_ShouldReturnOK_WhenAccessTokenIsProvided()
        {
            // Arrange
            int id = _rnd.Next(1, TestData.marks.Count());
            var endpoint = $"/api/marks/{id}/parents";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string responseBody = await response.Content.ReadAsStringAsync();

            var foundMark = TestData.marks.SingleOrDefault(v => v.Id == id);
            var mark = new MarkBaseResponse
            {
                Id = foundMark.Id,
                Code = foundMark.Code,
                Department = foundMark.Department,
            };
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            JsonSerializer.Deserialize<MarkBaseResponse>(
                responseBody, options).Should().BeEquivalentTo(mark);
        }

        [Fact]
        public async Task GetMarkParentResponseById_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int id = _rnd.Next(1, TestData.marks.Count());
            var endpoint = $"/api/marks/{id}/parents";

            // Act
            var response = await _authHttpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}