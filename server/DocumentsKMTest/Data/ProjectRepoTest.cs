using System;
using System.Linq;
using DocumentsKM.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ProjectRepoTest : IDisposable
    {
        private readonly IProjectRepo _repo;

        public ProjectRepoTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "ProjectTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Projects.AddRange(TestData.projects);
            context.SaveChanges();
            _repo = new SqlProjectRepo(context);
        }

        public void Dispose()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "ProjectTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAll_ShouldReturnAllProjects()
        {
            // Act
            var projects = _repo.GetAll();

            // Assert
            Assert.Equal(TestData.projects, projects);
        }

        [Fact]
        public void GetById_ShouldReturnProject()
        {
            // Arrange
            var rnd = new Random();
            int id = rnd.Next(1, TestData.projects.Count());

            // Act
            var project = _repo.GetById(id);

            // Assert
            Assert.Equal(TestData.projects.SingleOrDefault(v => v.Id == id), project);
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            int wrongId = 999;

            // Act
            var project = _repo.GetById(wrongId);

            // Assert
            Assert.Null(project);
        }
    }
}
