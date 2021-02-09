using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
    // TBD: Create, Update, Delete
    public class DocsControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
    {
        private readonly HttpClient _authHttpClient;
        private readonly HttpClient _httpClient;
        private readonly Random _rnd = new Random();

        public DocsControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
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
        public async Task GetAllSheetsByMarkId_ShouldReturnOK_WhenAccessTokenIsProvided()
        {
            // Arrange
            int sheetDocTypeId = 1;
            int markId = _rnd.Next(1, TestData.marks.Count());
            var endpoint = $"/api/marks/{markId}/docs/sheets";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string responseBody = await response.Content.ReadAsStringAsync();

            var docs = TestData.docs.Where(
                v => v.Mark.Id == markId && v.Type.Id == sheetDocTypeId)
                    .Select(d => new SheetResponse
                    {
                        Id = d.Id,
                        Num = d.Num,
                        Name = d.Name,
                        Form = d.Form,
                        Creator = d.Creator == null ? null : new EmployeeBaseResponse
                        {
                            Id = d.Creator.Id,
                            Name = d.Creator.Name,
                        },
                        Inspector = d.Inspector == null ? null : new EmployeeBaseResponse
                        {
                            Id = d.Inspector.Id,
                            Name = d.Inspector.Name,
                        },
                        NormContr = d.NormContr == null ? null : new EmployeeBaseResponse
                        {
                            Id = d.NormContr.Id,
                            Name = d.NormContr.Name,
                        },
                        Note = d.Note,
                    }).ToArray();
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            JsonSerializer.Deserialize<IEnumerable<SheetResponse>>(
                responseBody, options).Should().BeEquivalentTo(docs);
        }

        [Fact]
        public async Task GetAllSheetsByMarkId_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            var endpoint = $"/api/marks/{markId}/docs/sheets";

            // Act
            var response = await _authHttpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetAllAttachedByMarkId_ShouldReturnOK_WhenAccessTokenIsProvided()
        {
            // Arrange
            int sheetDocTypeId = 1;
            int markId = _rnd.Next(1, TestData.marks.Count());
            var endpoint = $"/api/marks/{markId}/docs/attached";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            string responseBody = await response.Content.ReadAsStringAsync();

            var docs = TestData.docs.Where(
                v => v.Mark.Id == markId && v.Type.Id != sheetDocTypeId)
                    .Select(d => new DocResponse
                    {
                        Id = d.Id,
                        Num = d.Num,
                        Type = d.Type,
                        Name = d.Name,
                        Form = d.Form,
                        Creator = d.Creator == null ? null : new EmployeeBaseResponse
                        {
                            Id = d.Creator.Id,
                            Name = d.Creator.Name,
                        },
                        Inspector = d.Inspector == null ? null : new EmployeeBaseResponse
                        {
                            Id = d.Inspector.Id,
                            Name = d.Inspector.Name,
                        },
                        NormContr = d.NormContr == null ? null : new EmployeeBaseResponse
                        {
                            Id = d.NormContr.Id,
                            Name = d.NormContr.Name,
                        },
                        ReleaseNum = d.ReleaseNum,
                        NumOfPages = d.NumOfPages,
                        Note = d.Note,
                    }).ToArray();
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            JsonSerializer.Deserialize<IEnumerable<DocResponse>>(
                responseBody, options).Should().BeEquivalentTo(docs);
        }

        [Fact]
        public async Task GetAllAttachedByMarkId_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            var endpoint = $"/api/marks/{markId}/docs/attached";

            // Act
            var response = await _authHttpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
