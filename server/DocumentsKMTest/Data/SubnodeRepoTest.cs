using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class SubnodeRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<Subnode> subnodes)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "SubnodeTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Nodes.AddRange(TestData.nodes);
            context.Subnodes.AddRange(subnodes);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllByNodeId_ShouldReturnSubnodes()
        {
            // Arrange
            var context = GetContext(TestData.subnodes);
            var repo = new SqlSubnodeRepo(context);

            int nodeId = _rnd.Next(1, TestData.nodes.Count());

            // Act
            var subnodes = repo.GetAllByNodeId(nodeId);

            // Assert
            Assert.Equal(TestData.subnodes.Where(v => v.Node.Id == nodeId), subnodes);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnSubnode()
        {
            // Arrange
            var context = GetContext(TestData.subnodes);
            var repo = new SqlSubnodeRepo(context);

            int id = _rnd.Next(1, TestData.subnodes.Count());

            // Act
            var subnode = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.subnodes.SingleOrDefault(v => v.Id == id), subnode);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            var context = GetContext(TestData.subnodes);
            var repo = new SqlSubnodeRepo(context);
            
            int wrongId = 999;

            // Act
            var subnode = repo.GetById(wrongId);

            // Assert
            Assert.Null(subnode);

            context.Database.EnsureDeleted();
        }
    }
}
