using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class UserRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<User> users)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "UserTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Employees.AddRange(TestData.employees);
            context.Users.AddRange(users);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetById_ShouldReturnUser()
        {
            // Arrange
            var context = GetContext(TestData.users);
            var repo = new SqlUserRepo(context);

            int id = _rnd.Next(1, TestData.users.Count());

            // Act
            var user = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.users.SingleOrDefault(v => v.Id == id), user);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext(TestData.users);
            var repo = new SqlUserRepo(context);

            // Act
            var user = repo.GetById(999);

            // Assert
            Assert.Null(user);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetByLogin_ShouldReturnUser()
        {
            // Arrange
            var context = GetContext(TestData.users);
            var repo = new SqlUserRepo(context);

            var u = TestData.users[0];

            // Act
            var user = repo.GetByLogin(u.Login);

            // Assert
            Assert.Equal(u, user);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetByLogin_ShouldReturnNull_WhenWrongLogin()
        {
            // Arrange
            var context = GetContext(TestData.users);
            var repo = new SqlUserRepo(context);

            // Act
            var user = repo.GetByLogin("wrongLogin");

            // Assert
            Assert.Null(user);

            context.Database.EnsureDeleted();
        }
    }
}
