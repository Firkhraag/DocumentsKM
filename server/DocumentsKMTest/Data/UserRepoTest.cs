using System;
using System.Linq;
using DocumentsKM.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class UserRepoTest : IDisposable
    {
        private readonly IUserRepo _repo;
        private readonly Random _rnd = new Random();

        public UserRepoTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "UserTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Users.AddRange(TestData.users);
            context.SaveChanges();
            _repo = new SqlUserRepo(context);
        }

        public void Dispose()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "UserTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnUser()
        {
            // Arrange
            int id = _rnd.Next(1, TestData.users.Count());

            // Act
            var user = _repo.GetById(id);

            // Assert
            Assert.Equal(TestData.users.SingleOrDefault(v => v.Id == id), user);
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            int wrongId = 999;

            // Act
            var user = _repo.GetById(wrongId);

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public void GetByLogin_ShouldReturnUser()
        {
            // Arrange
            int id = _rnd.Next(1, TestData.users.Count());
            var u = TestData.users.SingleOrDefault(v => v.Id == id);
            var login = u.Login;

            // Act
            var user = _repo.GetByLogin(login);

            // Assert
            Assert.Equal(u, user);
        }

        [Fact]
        public void GetByLogin_ShouldReturnNull()
        {
            // Arrange
            var wrongLogin = "wrongLogin";

            // Act
            var user = _repo.GetByLogin(wrongLogin);

            // Assert
            Assert.Null(user);
        }
    }
}
