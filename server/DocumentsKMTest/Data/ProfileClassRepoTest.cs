using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ProfileClassRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<ProfileClass> profileClasses)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "ProfileClassTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.ProfileClasses.AddRange(profileClasses);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAll_ShouldReturnProfileClasss()
        {
            // Arrange
            var context = GetContext(TestData.profileClasses);
            var repo = new SqlProfileClassRepo(context);

            // Act
            var profileClasses = repo.GetAll();

            // Assert
            Assert.Equal(TestData.profileClasses, profileClasses);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnProfileClass()
        {
            // Arrange
            var context = GetContext(TestData.profileClasses);
            var repo = new SqlProfileClassRepo(context);

            var id = _rnd.Next(1, TestData.profileClasses.Count());

            // Act
            var profileClass = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.profileClasses.SingleOrDefault(v => v.Id == id),
                profileClass);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext(TestData.profileClasses);
            var repo = new SqlProfileClassRepo(context);

            // Act
            var ProfileClass = repo.GetById(999);

            // Assert
            Assert.Null(ProfileClass);

            context.Database.EnsureDeleted();
        }
    }
}
