using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DocumentsKM.Dtos;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DocumentsKM.Tests
{
    public class DocsControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
    {
        private readonly HttpClient _authHttpClient;
        private readonly HttpClient _httpClient;
        private readonly Random _rnd = new Random();

        private class CreateRequest
        {
            public int MarkId { set; get; }
            public DocCreateRequest Body { set; get; }
        }

        private class UpdateRequest
        {
            public int Id { set; get; }
            public DocUpdateRequest Body { set; get; }
        }

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

        // ------------------------------------GET------------------------------------

        [Fact]
        public async Task GetAllSheetsByMarkId_ShouldReturnOK_WhenAccessTokenIsProvided()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            var endpoint = $"/api/marks/{markId}/docs/sheets";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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
            int markId = _rnd.Next(1, TestData.marks.Count());
            var endpoint = $"/api/marks/{markId}/docs/attached";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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

        // ------------------------------------POST------------------------------------

        [Fact]
        public async Task Create_ShouldReturnCreated()
        {
            // Arrange
            int markId = 1;
            int docTypeId = 1;
            int creatorId = 1;
            int inspectorId = 2;
            int normContrId = 3;
            var docRequest = new DocCreateRequest
            {
                Name = "NewCreate",
                Form = 9.0f,
                TypeId = docTypeId,
                CreatorId = creatorId,
                InspectorId = inspectorId,
                NormContrId = normContrId,
                ReleaseNum = 9,
                NumOfPages = 9,
                Note = "NewCreate",
            };
            string json = JsonSerializer.Serialize(docRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/marks/{markId}/docs";

            // Act
            var response = await _httpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenWrongValues()
        {
            // Arrange
            int markId = 2;
            int docTypeId = 1;
            int creatorId = 1;
            int inspectorId = 2;
            int normContrId = 3;
            var wrongDocRequests = new List<DocCreateRequest>
            {
                new DocCreateRequest
                {
                    Name = "",
                    Form = 9.0f,
                    TypeId = docTypeId,
                    CreatorId = creatorId,
                    InspectorId = inspectorId,
                    NormContrId = normContrId,
                    ReleaseNum = 9,
                    NumOfPages = 9,
                    Note = "NewCreate",
                },
                new DocCreateRequest
                {
                    Name = "NewCreate",
                    Form = -9.0f,
                    TypeId = docTypeId,
                    CreatorId = creatorId,
                    InspectorId = inspectorId,
                    NormContrId = normContrId,
                    ReleaseNum = 9,
                    NumOfPages = 9,
                    Note = "NewCreate",
                },
                new DocCreateRequest
                {
                    Name = "NewCreate",
                    Form = 9.0f,
                    TypeId = docTypeId,
                    CreatorId = creatorId,
                    InspectorId = inspectorId,
                    NormContrId = normContrId,
                    ReleaseNum = -9,
                    NumOfPages = 9,
                    Note = "NewCreate",
                },
                new DocCreateRequest
                {
                    Name = "NewCreate",
                    Form = 9.0f,
                    TypeId = docTypeId,
                    CreatorId = creatorId,
                    InspectorId = inspectorId,
                    NormContrId = normContrId,
                    ReleaseNum = 9,
                    NumOfPages = -9,
                    Note = "NewCreate",
                },
            };

            var endpoint = $"/api/marks/{markId}/docs";
            foreach (var wrongDocRequest in wrongDocRequests)
            {
                var json = JsonSerializer.Serialize(wrongDocRequest);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                // Act
                var response = await _httpClient.PostAsync(endpoint, httpContent);

                // Assert
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [Fact]
        public async Task Create_ShouldReturnNotFound_WhenWrongValues()
        {
            // Arrange
            int markId = 2;
            int docTypeId = 1;
            int creatorId = 1;
            int inspectorId = 2;
            int normContrId = 3;

            var docRequests = new List<CreateRequest>
            {
                new CreateRequest
                {
                    MarkId = 999,
                    Body = new DocCreateRequest
                    {
                        Name = "NewCreate",
                        Form = 9.0f,
                        TypeId = docTypeId,
                        CreatorId = creatorId,
                        InspectorId = inspectorId,
                        NormContrId = normContrId,
                        ReleaseNum = 9,
                        NumOfPages = 9,
                        Note = "NewCreate",
                    },
                },
                new CreateRequest
                {
                    MarkId = markId,
                    Body = new DocCreateRequest
                    {
                        Name = "NewCreate",
                        Form = 9.0f,
                        TypeId = 999,
                        CreatorId = creatorId,
                        InspectorId = inspectorId,
                        NormContrId = normContrId,
                        ReleaseNum = 9,
                        NumOfPages = 9,
                        Note = "NewCreate",
                    },
                },
                new CreateRequest
                {
                    MarkId = markId,
                    Body = new DocCreateRequest
                    {
                        Name = "NewCreate",
                        Form = 9.0f,
                        TypeId = docTypeId,
                        CreatorId = 999,
                        InspectorId = inspectorId,
                        NormContrId = normContrId,
                        ReleaseNum = 9,
                        NumOfPages = 9,
                        Note = "NewCreate",
                    },
                },
                new CreateRequest
                {
                    MarkId = markId,
                    Body = new DocCreateRequest
                    {
                        Name = "NewCreate",
                        Form = 9.0f,
                        TypeId = docTypeId,
                        CreatorId = creatorId,
                        InspectorId = 999,
                        NormContrId = normContrId,
                        ReleaseNum = 9,
                        NumOfPages = 9,
                        Note = "NewCreate",
                    },
                },
                new CreateRequest
                {
                    MarkId = markId,
                    Body = new DocCreateRequest
                    {
                        Name = "NewCreate",
                        Form = 9.0f,
                        TypeId = docTypeId,
                        CreatorId = creatorId,
                        InspectorId = inspectorId,
                        NormContrId = 999,
                        ReleaseNum = 9,
                        NumOfPages = 9,
                        Note = "NewCreate",
                    },
                },
            };

            foreach (var docRequest in docRequests)
            {
                var json = JsonSerializer.Serialize(docRequest.Body);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var endpoint = $"/api/marks/{docRequest.MarkId}/docs";

                // Act
                var response = await _httpClient.PostAsync(endpoint, httpContent);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task Create_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int markId = 1;
            int docTypeId = 1;
            int creatorId = 1;
            int inspectorId = 2;
            int normContrId = 3;
            var docRequest = new DocCreateRequest
            {
                Name = "NewCreate",
                Form = 9.0f,
                TypeId = docTypeId,
                CreatorId = creatorId,
                InspectorId = inspectorId,
                NormContrId = normContrId,
                ReleaseNum = 9,
                NumOfPages = 9,
                Note = "NewCreate",
            };
            string json = JsonSerializer.Serialize(docRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/marks/{markId}/docs";

            // Act
            var response = await _authHttpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        // ------------------------------------PATCH------------------------------------

        [Fact]
        public async Task Update_ShouldReturnNoContent()
        {
            // Arrange
            int id = 1;
            var docRequest = new DocUpdateRequest
            {
                Name = "NewUpdate",
                ReleaseNum = 9,
                NumOfPages = 9,
                Note = "NewUpdate",
            };
            string json = JsonSerializer.Serialize(docRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/docs/{id}";

            // Act
            var response = await _httpClient.PatchAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenWrongValues()
        {
            // Arrange
            int id = 1;
            var wrongDocRequests = new List<DocUpdateRequest>
            {
                new DocUpdateRequest
                {
                    Form = -9.0f,
                },
                new DocUpdateRequest
                {
                    ReleaseNum = -9,
                },
                new DocUpdateRequest
                {
                    NumOfPages = -9,
                },
            };

            var endpoint = $"/api/docs/{id}";

            foreach (var wrongDocRequest in wrongDocRequests)
            {
                var json = JsonSerializer.Serialize(wrongDocRequest);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                // Act
                var response = await _httpClient.PatchAsync(endpoint, httpContent);

                // Assert
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenWrongValues()
        {
            // Arrange
            int id = 1;

            var docRequests = new List<UpdateRequest>
            {
                new UpdateRequest
                {
                    Id = 999,
                    Body = new DocUpdateRequest
                    {
                        Note = "NewUpdate",
                    },
                },
                new UpdateRequest
                {
                    Id = id,
                    Body = new DocUpdateRequest
                    {
                        TypeId = 999,
                    },
                },
                new UpdateRequest
                {
                    Id = id,
                    Body = new DocUpdateRequest
                    {
                        CreatorId = 999,
                    },
                },
                new UpdateRequest
                {
                    Id = id,
                    Body = new DocUpdateRequest
                    {
                        InspectorId = 999,
                    },
                },
                new UpdateRequest
                {
                    Id = id,
                    Body = new DocUpdateRequest
                    {
                        NormContrId = 999,
                    },
                },
            };

            foreach (var docRequest in docRequests)
            {
                var json = JsonSerializer.Serialize(docRequest.Body);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var endpoint = $"/api/docs/{docRequest.Id}";

                // Act
                var response = await _httpClient.PatchAsync(endpoint, httpContent);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task Update_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int id = 1;
            var docRequest = new DocUpdateRequest
            {
                Name = "NewUpdate",
            };
            string json = JsonSerializer.Serialize(docRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/docs/{id}";

            // Act
            var response = await _authHttpClient.PatchAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        // ------------------------------------DELETE------------------------------------

        [Fact]
        public async Task Delete_ShouldReturnNoContent()
        {
            // Arrange
            int id = 2;
            var endpoint = $"/api/docs/{id}";

            // Act
            var response = await _httpClient.DeleteAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenWrongId()
        {
            // Arrange
            var endpoint = $"/api/docs/{999}";

            // Act
            var response = await _httpClient.DeleteAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int id = 2;
            var endpoint = $"/api/docs/{id}";

            // Act
            var response = await _authHttpClient.DeleteAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
