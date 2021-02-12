using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ProfileRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<Profile> profiles)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "profileTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.ProfileClasses.AddRange(TestData.profileClasses);
            context.Profiles.AddRange(profiles);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllByProfileClassId_ShouldReturnprofiles()
        {
            // Arrange
            var context = GetContext(TestData.profiles);
            var repo = new SqlProfileRepo(context);

            int classId = _rnd.Next(1, TestData.profileClasses.Count());

            // Act
            var profiles = repo.GetAllByProfileClassId(classId);

            // Assert
            Assert.Equal(TestData.profiles.Where(v => v.Class.Id == classId), profiles);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAllByProjectId_ShouldReturnEmptyArray_WhenWrongProjectId()
        {
            // Arrange
            var context = GetContext(TestData.profiles);
            var repo = new SqlProfileRepo(context);

            // Act
            var profiles = repo.GetAllByProfileClassId(999);

            // Assert
            Assert.Empty(profiles);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnprofile()
        {
            // Arrange
            var context = GetContext(TestData.profiles);
            var repo = new SqlProfileRepo(context);

            int id = _rnd.Next(1, TestData.profiles.Count());

            // Act
            var profile = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.profiles.SingleOrDefault(v => v.Id == id), profile);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext(TestData.profiles);
            var repo = new SqlProfileRepo(context);

            // Act
            var profile = repo.GetById(999);

            // Assert
            Assert.Null(profile);

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
