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
    public class MarksControllerTest : IClassFixture<TestWebApplicationFactory<DocumentsKM.Startup>>
    {
        private readonly HttpClient _authHttpClient;
        private readonly HttpClient _httpClient;
        private readonly Random _rnd = new Random();

        private class UpdateRequest
        {
            public int Id { set; get; }
            public MarkUpdateRequest Body { set; get; }
        }

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

        // ------------------------------------GET------------------------------------

        [Fact]
        public async Task GetAllBySubnodeId_ShouldReturnOK()
        {
            // Arrange
            int subnodeId = _rnd.Next(1, TestData.subnodes.Count());
            var endpoint = $"/api/subnodes/{subnodeId}/marks";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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
        public async Task GetById_ShouldReturnOK()
        {
            // Arrange
            int id = _rnd.Next(1, TestData.marks.Count());
            var endpoint = $"/api/marks/{id}";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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
        public async Task GetMarkParentResponseById_ShouldReturnOK()
        {
            // Arrange
            int id = _rnd.Next(1, TestData.marks.Count());
            var endpoint = $"/api/marks/{id}/parents";

            // Act
            var response = await _httpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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

        // ------------------------------------POST------------------------------------

        [Fact]
        public async Task Create_ShouldReturnCreated()
        {
            // Arrange
            var userId = 1;
            var subnodeId = 1;
            var departmentId = 1;
            var mainBuilderId = 1;
            var markRequest = new MarkCreateRequest
            {
                SubnodeId = subnodeId,
                Code = "NewCreate",
                Name = "NewCreate",
                DepartmentId = departmentId,
                MainBuilderId = mainBuilderId,
            };
            string json = JsonSerializer.Serialize(markRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/users/{userId}/marks";

            // Act
            var response = await _httpClient.PostAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenWrongValues()
        {
            // Arrange
            var userId = 1;
            var subnodeId = 1;
            var departmentId = 1;
            var mainBuilderId = 1;
            var wrongMarkRequests = new List<MarkCreateRequest>
            {
                new MarkCreateRequest
                {
                    SubnodeId = subnodeId,
                    Code = "",
                    Name = "NewCreate",
                    DepartmentId = departmentId,
                    MainBuilderId = mainBuilderId,
                },
                new MarkCreateRequest
                {
                    SubnodeId = subnodeId,
                    Code = "NewCreate",
                    Name = "",
                    DepartmentId = departmentId,
                    MainBuilderId = mainBuilderId,
                },
            };

            var endpoint = $"/api/users/{userId}/marks";
            foreach (var wrongMarkRequest in wrongMarkRequests)
            {
                var json = JsonSerializer.Serialize(wrongMarkRequest);
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
            var userId = 1;
            var subnodeId = 1;
            var departmentId = 1;
            var mainBuilderId = 1;
            var groupLeaderId = 1;
            var chiefSpecialistId = 1;
            var wrongMarkRequests = new List<MarkCreateRequest>
            {
                new MarkCreateRequest
                {
                    SubnodeId = 999,
                    Code = "NewCreate2",
                    Name = "NewCreate2",
                    DepartmentId = departmentId,
                    ChiefSpecialistId = chiefSpecialistId,
                    GroupLeaderId = groupLeaderId,
                    MainBuilderId = mainBuilderId,
                },
                new MarkCreateRequest
                {
                    SubnodeId = subnodeId,
                    Code = "NewCreate2",
                    Name = "NewCreate2",
                    DepartmentId = 999,
                    ChiefSpecialistId = chiefSpecialistId,
                    GroupLeaderId = groupLeaderId,
                    MainBuilderId = mainBuilderId,
                },
                new MarkCreateRequest
                {
                    SubnodeId = subnodeId,
                    Code = "NewCreate2",
                    Name = "NewCreate2",
                    DepartmentId = departmentId,
                    ChiefSpecialistId = 999,
                    GroupLeaderId = groupLeaderId,
                    MainBuilderId = mainBuilderId,
                },
                new MarkCreateRequest
                {
                    SubnodeId = subnodeId,
                    Code = "NewCreate2",
                    Name = "NewCreate2",
                    DepartmentId = departmentId,
                    ChiefSpecialistId = chiefSpecialistId,
                    GroupLeaderId = 999,
                    MainBuilderId = mainBuilderId,
                },
                 new MarkCreateRequest
                {
                    SubnodeId = subnodeId,
                    Code = "NewCreate2",
                    Name = "NewCreate2",
                    DepartmentId = departmentId,
                    ChiefSpecialistId = chiefSpecialistId,
                    GroupLeaderId = groupLeaderId,
                    MainBuilderId = 999,
                },
            };

            var endpoint = $"/api/users/{userId}/marks";
            foreach (var wrongMarkRequest in wrongMarkRequests)
            {
                var json = JsonSerializer.Serialize(wrongMarkRequest);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                // Act
                var response = await _httpClient.PostAsync(endpoint, httpContent);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task Create_ShouldReturnConflict_WhenEmployeesAndDepartmentDontMatchOrConflictValues()
        {
            // Arrange
            var userId = 1;
            var subnodeId = 1;
            var departmentId = 1;
            var chiefSpecialistId = 1;
            var groupLeaderId = 2;
            var mainBuilderId = 3;

            var wrongMarkRequests = new List<MarkCreateRequest>
            {
                new MarkCreateRequest
                {
                    SubnodeId = TestData.marks[0].Subnode.Id,
                    Code = TestData.marks[0].Code,
                    Name = "NewCreate",
                    DepartmentId = departmentId,
                    ChiefSpecialistId = chiefSpecialistId,
                    GroupLeaderId = groupLeaderId,
                    MainBuilderId = mainBuilderId,
                },
                new MarkCreateRequest
                {
                    SubnodeId = subnodeId,
                    Code = "NewCreate",
                    Name = "NewCreate",
                    DepartmentId = departmentId,
                    ChiefSpecialistId = chiefSpecialistId,
                    GroupLeaderId = groupLeaderId,
                    MainBuilderId = mainBuilderId,
                },
            };

            var endpoint = $"/api/users/{userId}/marks";
            foreach (var wrongMarkRequest in wrongMarkRequests)
            {
                var json = JsonSerializer.Serialize(wrongMarkRequest);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                // Act
                var response = await _httpClient.PostAsync(endpoint, httpContent);

                // Assert
                Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            }
        }

        [Fact]
        public async Task Create_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            var userId = 1;
            var subnodeId = 1;
            var departmentId = 1;
            var mainBuilderId = 1;
            var markRequest = new MarkCreateRequest
            {
                SubnodeId = subnodeId,
                Code = "NewCreate",
                Name = "NewCreate",
                DepartmentId = departmentId,
                MainBuilderId = mainBuilderId,
            };
            string json = JsonSerializer.Serialize(markRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/users/{userId}/marks";

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
            var id = 1;
            var markRequest = new MarkUpdateRequest
            {
                Name = "NewUpdate",
            };
            string json = JsonSerializer.Serialize(markRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/marks/{id}";

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
            var endpoint = $"/api/marks/{id}";

            var httpContent = new StringContent("", Encoding.UTF8, "application/json");

            // Act
            var response = await _httpClient.PatchAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenWrongValues()
        {
            // Arrange
            int id = 1;
            var wrongMarkRequests = new List<UpdateRequest>
            {
                new UpdateRequest
                {
                    Id = 999,
                    Body = new MarkUpdateRequest
                    {
                        Name = "NewUpdate",
                    },
                },
                new UpdateRequest
                {
                    Id = id,
                    Body = new MarkUpdateRequest
                    {
                        SubnodeId = 999,
                    },
                },
                new UpdateRequest
                {
                    Id = id,
                    Body = new MarkUpdateRequest
                    {
                        DepartmentId = 999,
                    },
                },
                new UpdateRequest
                {
                    Id = id,
                    Body = new MarkUpdateRequest
                    {
                        ChiefSpecialistId = 999,
                    },
                },
                new UpdateRequest
                {
                    Id = id,
                    Body = new MarkUpdateRequest
                    {
                        GroupLeaderId = 999,
                    },
                },
                new UpdateRequest
                {
                    Id = id,
                    Body = new MarkUpdateRequest
                    {
                        MainBuilderId = 999,
                    },
                },
            };

            foreach (var wrongMarkRequest in wrongMarkRequests)
            {
                var json = JsonSerializer.Serialize(wrongMarkRequest.Body);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var endpoint = $"/api/marks/{wrongMarkRequest.Id}";

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
            var markRequest = new MarkUpdateRequest
            {
                Code = TestData.marks[0].Code,
            };
            string json = JsonSerializer.Serialize(markRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/marks/{id}";

            // Act
            var response = await _httpClient.PatchAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnUnauthorized_WhenNoAccessToken()
        {
            // Arrange
            var id = 1;
            var markRequest = new MarkUpdateRequest
            {
                Name = "NewUpdate",
            };
            string json = JsonSerializer.Serialize(markRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"/api/marks/{id}";

            // Act
            var response = await _authHttpClient.PatchAsync(endpoint, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}