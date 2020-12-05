using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class EnvAggressivenessRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<EnvAggressiveness> envAggressiveness)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "EnvAggressivenessTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.EnvAggressiveness.AddRange(envAggressiveness);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAll_ShouldReturnEnvAggressiveness()
        {
            // Arrange
            var context = GetContext(TestData.envAggressiveness);
            var repo = new SqlEnvAggressivenessRepo(context);

            // Act
            var envAggressiveness = repo.GetAll();

            // Assert
            Assert.Equal(TestData.envAggressiveness, envAggressiveness);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnEnvAggressiveness()
        {
            // Arrange
            var context = GetContext(TestData.envAggressiveness);
            var repo = new SqlEnvAggressivenessRepo(context);

            int id = _rnd.Next(1, TestData.envAggressiveness.Count());

            // Act
            var envAggressiveness = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.envAggressiveness.SingleOrDefault(v => v.Id == id),
                envAggressiveness);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            var context = GetContext(TestData.envAggressiveness);
            var repo = new SqlEnvAggressivenessRepo(context);

            int wrongId = 999;

            // Act
            var envAggressiveness = repo.GetById(wrongId);

            // Assert
            Assert.Null(envAggressiveness);

            context.Database.EnsureDeleted();
        }
    }
}
