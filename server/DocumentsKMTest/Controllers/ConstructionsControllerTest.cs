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
    public class ConstructionsControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
    {
        private readonly HttpClient _authHttpClient;
        private readonly HttpClient _httpClient;
        private readonly Random _rnd = new Random();

        private class CreateRequest
        {
            public int SpecificationId { set; get; }
            public ConstructionCreateRequest Body { set; get; }
        }

        private class UpdateRequest
        {
            public int Id { set; get; }
            public ConstructionUpdateRequest Body { set; get; }
        }

        public ConstructionsControllerTest(TestWebApplicationFactory<DocumentsKM.Startup> factory)
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
        public async Task GetAllBySpecificationId_ShouldReturnOK()
        {
            // Arrange
            int specificationId = _rnd.Next(1, TestData.specifications.Count());
            var endpoint = $"/api/specifications/{specificationId}/constructions";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllBySpecificationId_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int specificationId = _rnd.Next(1, TestData.specifications.Count());
            var endpoint = $"/api/specifications/{specificationId}/constructions";

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
            int specificationId = 1;
            short typeId = 1;
            short weldingControlId = 1;
            var constructionRequest = new ConstructionCreateRequest
            {
                Name = "NewCreate",
                TypeId = typeId,
                Valuation = "NewCreate",
                StandardAlbumCode = "NewCreate",
                NumOfStandardConstructions = 1,
                HasEdgeBlunting = false,
                HasDynamicLoad = false,
                HasFlangedConnections = false,
                WeldingControlId = weldingControlId,
                PaintworkCoeff = 1.0f,
            };
            string json = JsonSerializer.Serialize(constructionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/specifications/{specificationId}/constructions";

            // Act
            var response = await _httpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenWrongValues()
        {
            // Arrange
            int specificationId = 2;
            short typeId = 1;
            short weldingControlId = 1;
            var wrongConstructionRequests = new List<ConstructionCreateRequest>
            {
                new ConstructionCreateRequest
                {
                    Name = "",
                    TypeId = typeId,
                    Valuation = "NewCreate",
                    StandardAlbumCode = "NewCreate",
                    NumOfStandardConstructions = 1,
                    HasEdgeBlunting = false,
                    HasDynamicLoad = false,
                    HasFlangedConnections = false,
                    WeldingControlId = weldingControlId,
                    PaintworkCoeff = 1.0f,
                },
                new ConstructionCreateRequest
                {
                    Name = "NewCreate",
                    TypeId = typeId,
                    Valuation = "NewCreate",
                    StandardAlbumCode = "NewCreate",
                    NumOfStandardConstructions = -1,
                    HasEdgeBlunting = false,
                    HasDynamicLoad = false,
                    HasFlangedConnections = false,
                    WeldingControlId = weldingControlId,
                    PaintworkCoeff = 1.0f,
                },
                new ConstructionCreateRequest
                {
                    Name = "NewCreate",
                    TypeId = typeId,
                    Valuation = "NewCreate",
                    StandardAlbumCode = "NewCreate",
                    NumOfStandardConstructions = -1,
                    HasEdgeBlunting = false,
                    HasDynamicLoad = false,
                    HasFlangedConnections = false,
                    WeldingControlId = weldingControlId,
                    PaintworkCoeff = -1.0f,
                },
            };

            var endpoint = $"/api/specifications/{specificationId}/constructions";
            foreach (var wrongConstructionRequest in wrongConstructionRequests)
            {
                var json = JsonSerializer.Serialize(wrongConstructionRequest);
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
            int specificationId = 2;
            short typeId = 1;
            short weldingControlId = 1;
            var wrongConstructionRequests = new List<CreateRequest>
            {
                new CreateRequest
                {
                    SpecificationId = 999,
                    Body = new ConstructionCreateRequest
                    {
                        Name = "NewCreate",
                        TypeId = typeId,
                        Valuation = "NewCreate",
                        StandardAlbumCode = "NewCreate",
                        NumOfStandardConstructions = 1,
                        HasEdgeBlunting = false,
                        HasDynamicLoad = false,
                        HasFlangedConnections = false,
                        WeldingControlId = weldingControlId,
                        PaintworkCoeff = 1.0f,
                    },
                },
                new CreateRequest
                {
                    SpecificationId = specificationId,
                    Body = new ConstructionCreateRequest
                    {
                        Name = "NewCreate",
                        TypeId = 999,
                        Valuation = "NewCreate",
                        StandardAlbumCode = "NewCreate",
                        NumOfStandardConstructions = 1,
                        HasEdgeBlunting = false,
                        HasDynamicLoad = false,
                        HasFlangedConnections = false,
                        WeldingControlId = weldingControlId,
                        PaintworkCoeff = 1.0f,
                    },
                },
                new CreateRequest
                {
                    SpecificationId = specificationId,
                    Body = new ConstructionCreateRequest
                    {
                        Name = "NewCreate",
                        TypeId = typeId,
                        SubtypeId = 999,
                        Valuation = "NewCreate",
                        StandardAlbumCode = "NewCreate",
                        NumOfStandardConstructions = 1,
                        HasEdgeBlunting = false,
                        HasDynamicLoad = false,
                        HasFlangedConnections = false,
                        WeldingControlId = weldingControlId,
                        PaintworkCoeff = 1.0f,
                    },
                },
                new CreateRequest
                {
                    SpecificationId = specificationId,
                    Body = new ConstructionCreateRequest
                    {
                        Name = "NewCreate",
                        TypeId = typeId,
                        Valuation = "NewCreate",
                        StandardAlbumCode = "NewCreate",
                        NumOfStandardConstructions = 1,
                        HasEdgeBlunting = false,
                        HasDynamicLoad = false,
                        HasFlangedConnections = false,
                        WeldingControlId = 999,
                        PaintworkCoeff = 1.0f,
                    },
                },
            };

            foreach (var wrongConstructionRequest in wrongConstructionRequests)
            {
                var json = JsonSerializer.Serialize(wrongConstructionRequest.Body);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var endpoint = $"/api/specifications/{wrongConstructionRequest.SpecificationId}/constructions";

                // Act
                var response = await _httpClient.PostAsync(endpoint, httpContent);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task Create_ShouldReturnConflict_WhenConflictValues()
        {
            // Arrange
            int specificationId = 1;
            short typeId = 1;
            short weldingControlId = 1;
            var conflictName = TestData.constructions[0].Name;
            var conflictPaintworkCoeff = TestData.constructions[0].PaintworkCoeff;
            var constructionRequest = new ConstructionCreateRequest
            {
                Name = conflictName,
                TypeId = typeId,
                Valuation = "NewCreate",
                StandardAlbumCode = "NewCreate",
                NumOfStandardConstructions = 1,
                HasEdgeBlunting = false,
                HasDynamicLoad = false,
                HasFlangedConnections = false,
                WeldingControlId = weldingControlId,
                PaintworkCoeff = conflictPaintworkCoeff,
            };
            string json = JsonSerializer.Serialize(constructionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/specifications/{specificationId}/constructions";

            // Act
            var response = await _httpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int specificationId = 1;
            short typeId = 1;
            short weldingControlId = 1;
            var constructionRequest = new ConstructionCreateRequest
            {
                Name = "NewCreate",
                TypeId = typeId,
                Valuation = "NewCreate",
                StandardAlbumCode = "NewCreate",
                NumOfStandardConstructions = 1,
                HasEdgeBlunting = false,
                HasDynamicLoad = false,
                HasFlangedConnections = false,
                WeldingControlId = weldingControlId,
                PaintworkCoeff = 1.0f,
            };
            string json = JsonSerializer.Serialize(constructionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/specifications/{specificationId}/constructions";

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
            int id = 3;
            short typeId = 3;
            short weldingControlId = 3;
            var constructionRequest = new ConstructionUpdateRequest
            {
                Name = "NewUpdate",
                TypeId = typeId,
                Valuation = "NewUpdate",
                StandardAlbumCode = "NewUpdate",
                NumOfStandardConstructions = 2,
                HasEdgeBlunting = true,
                HasDynamicLoad = true,
                HasFlangedConnections = true,
                WeldingControlId = weldingControlId,
                PaintworkCoeff = 2.0f,
            };
            string json = JsonSerializer.Serialize(constructionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/constructions/{id}";

            // Act
            var response = await _httpClient.PatchAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenWrongValues()
        {
            // Arrange
            int id = 2;
            var wrongConstructionRequests = new List<ConstructionUpdateRequest>
            {
                // new ConstructionUpdateRequest
                // {
                //     Name = "",
                // },
                new ConstructionUpdateRequest
                {
                    NumOfStandardConstructions = -1,
                },
                new ConstructionUpdateRequest
                {
                    PaintworkCoeff = -1.0f,
                },
            };

            var endpoint = $"/api/constructions/{id}";
            foreach (var wrongConstructionRequest in wrongConstructionRequests)
            {
                var json = JsonSerializer.Serialize(wrongConstructionRequest);
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
            int id = 2;
            var wrongConstructionRequests = new List<UpdateRequest>
            {
                new UpdateRequest
                {
                    Id = 999,
                    Body = new ConstructionUpdateRequest
                    {
                        Valuation = "NewUpdate",
                    },
                },
                new UpdateRequest
                {
                    Id = id,
                    Body = new ConstructionUpdateRequest
                    {
                        TypeId = 999,
                        Valuation = "NewUpdate",
                    },
                },
                new UpdateRequest
                {
                    Id = id,
                    Body = new ConstructionUpdateRequest
                    {
                        SubtypeId = 999,
                        Valuation = "NewUpdate",
                    },
                },
                new UpdateRequest
                {
                    Id = id,
                    Body = new ConstructionUpdateRequest
                    {
                        Valuation = "NewUpdate",
                        WeldingControlId = 999,
                    },
                },
            };

            foreach (var wrongConstructionRequest in wrongConstructionRequests)
            {
                var json = JsonSerializer.Serialize(wrongConstructionRequest.Body);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var endpoint = $"/api/constructions/{wrongConstructionRequest.Id}";

                // Act
                var response = await _httpClient.PatchAsync(endpoint, httpContent);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task Update_ShouldReturnConflict_WhenConflictValues()
        {
            // Arrange
            int id = 2;
            var conflictName = TestData.constructions[0].Name;
            var conflictPaintworkCoeff = TestData.constructions[0].PaintworkCoeff;
            var constructionRequest = new ConstructionUpdateRequest
            {
                Name = conflictName,
                PaintworkCoeff = conflictPaintworkCoeff,
            };
            string json = JsonSerializer.Serialize(constructionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/constructions/{id}";

            // Act
            var response = await _httpClient.PatchAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int id = 3;
            short typeId = 3;
            short weldingControlId = 3;
            var constructionRequest = new ConstructionUpdateRequest
            {
                Name = "NewUpdate",
                TypeId = typeId,
                Valuation = "NewUpdate",
                StandardAlbumCode = "NewUpdate",
                NumOfStandardConstructions = 2,
                HasEdgeBlunting = true,
                HasDynamicLoad = true,
                HasFlangedConnections = true,
                WeldingControlId = weldingControlId,
                PaintworkCoeff = 2.0f,
            };
            string json = JsonSerializer.Serialize(constructionRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/constructions/{id}";

            // Act
            var response = await _authHttpClient.PatchAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        // ------------------------------------DELETE------------------------------------

        // [Fact]
        // public async Task Delete_ShouldReturnNoContent()
        // {
        //     // Arrange
        //     int id = 2;
        //     var endpoint = $"/api/constructions/{id}";

        //     // Act
        //     var response = await _httpClient.DeleteAsync(endpoint);

        //     // Assert
        //     Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        // }

        // [Fact]
        // public async Task Delete_ShouldReturnNotFound_WhenWrongId()
        // {
        //     // Arrange
        //     var endpoint = $"/api/constructions/{999}";

        //     // Act
        //     var response = await _httpClient.DeleteAsync(endpoint);

        //     // Assert
        //     Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        // }

        [Fact]
        public async Task Delete_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            int id = 2;
            var endpoint = $"/api/constructions/{id}";

            // Act
            var response = await _authHttpClient.DeleteAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
