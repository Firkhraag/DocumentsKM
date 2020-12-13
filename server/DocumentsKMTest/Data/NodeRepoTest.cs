using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class NodeRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<Node> nodes)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "NodeTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Projects.AddRange(TestData.projects);
            context.Nodes.AddRange(nodes);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllByProjectId_ShouldReturnNodes()
        {
            // Arrange
            var context = GetContext(TestData.nodes);
            var repo = new SqlNodeRepo(context);

            int projectId = _rnd.Next(1, TestData.projects.Count());

            // Act
            var nodes = repo.GetAllByProjectId(projectId);

            // Assert
            Assert.Equal(TestData.nodes.Where(v => v.Project.Id == projectId), nodes);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnNode()
        {
            // Arrange
            var context = GetContext(TestData.nodes);
            var repo = new SqlNodeRepo(context);

            int id = _rnd.Next(1, TestData.nodes.Count());

            // Act
            var node = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.nodes.SingleOrDefault(v => v.Id == id), node);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            var context = GetContext(TestData.nodes);
            var repo = new SqlNodeRepo(context);

            int wrongId = 999;

            // Act
            var node = repo.GetById(wrongId);

            // Assert
            Assert.Null(node);

            context.Database.EnsureDeleted();
        }
    }
}
