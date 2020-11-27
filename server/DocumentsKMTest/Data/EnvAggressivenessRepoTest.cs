using System;
using System.Linq;
using DocumentsKM.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class EnvAggressivenessRepoTest : IDisposable
    {
        private readonly IEnvAggressivenessRepo _repo;

        public EnvAggressivenessRepoTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "EnvAggressivenessTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.EnvAggressiveness.AddRange(TestData.envAggressiveness);
            context.SaveChanges();
            _repo = new SqlEnvAggressivenessRepo(context);
        }

        public void Dispose()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "EnvAggressivenessTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAll_ShouldReturnAllEnvAggressiveness()
        {
            // Act
            var envAggressiveness = _repo.GetAll();

            // Assert
            Assert.Equal(TestData.envAggressiveness, envAggressiveness);
        }

        [Fact]
        public void GetById_ShouldReturnEnvAggressiveness()
        {
            // Arrange
            var rnd = new Random();
            int id = rnd.Next(1, TestData.envAggressiveness.Count());

            // Act
            var envAggressiveness = _repo.GetById(id);

            // Assert
            Assert.Equal(TestData.envAggressiveness.SingleOrDefault(v => v.Id == id),
                envAggressiveness);
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            int wrongId = 999;

            // Act
            var envAggressiveness = _repo.GetById(wrongId);

            // Assert
            Assert.Null(envAggressiveness);
        }
    }
}
