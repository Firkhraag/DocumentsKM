using System;
using System.Linq;
using DocumentsKM.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class NodeRepoTest : IDisposable
    {
        private readonly INodeRepo _repo;
        private readonly Random _rnd = new Random();

        public NodeRepoTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "NodeTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Nodes.AddRange(TestData.nodes);
            context.SaveChanges();
            _repo = new SqlNodeRepo(context);
        }

        public void Dispose()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "NodeTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAllByProjectId_ShouldReturnAllNodesWithGivenProjectId()
        {
            // Arrange
            int projectId = _rnd.Next(1, TestData.projects.Count());

            // Act
            var nodes = _repo.GetAllByProjectId(projectId);

            // Assert
            Assert.Equal(TestData.nodes.Where(v => v.Project.Id == projectId), nodes);
        }

        [Fact]
        public void GetById_ShouldReturnNode()
        {
            // Arrange
            int id = _rnd.Next(1, TestData.nodes.Count());

            // Act
            var node = _repo.GetById(id);

            // Assert
            Assert.Equal(TestData.nodes.SingleOrDefault(v => v.Id == id), node);
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            int wrongId = 999;

            // Act
            var node = _repo.GetById(wrongId);

            // Assert
            Assert.Null(node);
        }
    }
}
