using System;
using System.Linq;
using DocumentsKM.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class SubnodeRepoTest : IDisposable
    {
        private readonly ISubnodeRepo _repo;
        private readonly Random _rnd = new Random();

        public SubnodeRepoTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "SubnodeTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Subnodes.AddRange(TestData.subnodes);
            context.SaveChanges();
            _repo = new SqlSubnodeRepo(context);
        }

        public void Dispose()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "SubnodeTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAllByNodeId_ShouldReturnAllSubnodesWithGivenNodeId()
        {
            // Arrange
            int nodeId = _rnd.Next(1, TestData.nodes.Count());

            // Act
            var subnodes = _repo.GetAllByNodeId(nodeId);

            // Assert
            Assert.Equal(TestData.subnodes.Where(v => v.Node.Id == nodeId), subnodes);
        }

        [Fact]
        public void GetById_ShouldReturnSubnode()
        {
            // Arrange
            int id = _rnd.Next(1, TestData.subnodes.Count());

            // Act
            var subnode = _repo.GetById(id);

            // Assert
            Assert.Equal(TestData.subnodes.SingleOrDefault(v => v.Id == id), subnode);
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            int wrongId = 999;

            // Act
            var subnode = _repo.GetById(wrongId);

            // Assert
            Assert.Null(subnode);
        }
    }
}
