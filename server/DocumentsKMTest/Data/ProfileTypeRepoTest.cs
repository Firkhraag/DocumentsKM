using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ProfileTypeRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<ProfileType> profileTypes)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "ProfileTypeTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.ProfileTypes.AddRange(profileTypes);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAll_ShouldReturnProfileTypes()
        {
            // Arrange
            var context = GetContext(TestData.profileTypes);
            var repo = new SqlProfileTypeRepo(context);

            // Act
            var profileTypes = repo.GetAll();

            // Assert
            Assert.Equal(TestData.profileTypes, profileTypes);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnProfileType()
        {
            // Arrange
            var context = GetContext(TestData.profileTypes);
            var repo = new SqlProfileTypeRepo(context);

            var id = _rnd.Next(1, TestData.profileTypes.Count());

            // Act
            var profileType = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.profileTypes.SingleOrDefault(v => v.Id == id),
                profileType);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext(TestData.profileTypes);
            var repo = new SqlProfileTypeRepo(context);

            // Act
            var profileType = repo.GetById(999);

            // Assert
            Assert.Null(profileType);

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
