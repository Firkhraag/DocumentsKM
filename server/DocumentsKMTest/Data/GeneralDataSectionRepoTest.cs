using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class GeneralDataSectionRepoTest
    {
        private readonly Random _rnd = new Random();

        private readonly int _maxUserId = 3;

        private ApplicationContext GetContext(List<GeneralDataSection> generalDataSections)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "GeneralDataSectionTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.GeneralDataSections.AddRange(generalDataSections);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllByUserId_ShouldReturnGeneralDataSections()
        {
            // Assert
            var context = GetContext(TestData.generalDataSections);
            var repo = new SqlGeneralDataSectionRepo(context);

            int userId = _rnd.Next(1, _maxUserId);

            // Act
            var generalDataSections = repo.GetAllByUserId(userId);

            // Assert
            Assert.Equal(TestData.generalDataSections.Where(v => v.User.Id == userId), generalDataSections);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnGeneralDataSection()
        {
            // Arrange
            var context = GetContext(TestData.generalDataSections);
            var repo = new SqlGeneralDataSectionRepo(context);

            int id = _rnd.Next(1, TestData.generalDataSections.Count());

            // Act
            var generalDataSection = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.generalDataSections.SingleOrDefault(v => v.Id == id),
                generalDataSection);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext(TestData.generalDataSections);
            var repo = new SqlGeneralDataSectionRepo(context);

            // Act
            var generalDataSection = repo.GetById(999);

            // Assert
            Assert.Null(generalDataSection);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Add_ShouldAddgeneralDataSection()
        {
            // Arrange
            var context = GetContext(TestData.generalDataSections);
            var repo = new SqlGeneralDataSectionRepo(context);

            int userId = _rnd.Next(1, TestData.users.Count());
            var generalDataSection = new GeneralDataSection
            {
                Name = "NewCreate",
                User = TestData.users.SingleOrDefault(v => v.Id == userId),
                OrderNum = 9,
            };

            // Act
            repo.Add(generalDataSection);

            // Assert
            Assert.NotNull(repo.GetById(generalDataSection.Id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Update_ShouldUpdategeneralDataSection()
        {
            // Arrange
            var generalDataSections = new List<GeneralDataSection> { };
            foreach (var mgds in TestData.generalDataSections)
            {
                generalDataSections.Add(new GeneralDataSection
                {
                    Id = mgds.Id,
                    Name = mgds.Name,
                    User = mgds.User,
                    OrderNum = mgds.OrderNum,
                });
            }
            var context = GetContext(generalDataSections);
            var repo = new SqlGeneralDataSectionRepo(context);

            int id = _rnd.Next(1, generalDataSections.Count());
            var generalDataSection = generalDataSections.FirstOrDefault(v => v.Id == id);
            generalDataSection.Name = "NewUpdate";

            // Act
            repo.Update(generalDataSection);

            // Assert
            Assert.Equal(generalDataSection.Name, repo.GetById(id).Name);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Delete_ShouldDeletegeneralDataSection()
        {
            // Arrange
            var context = GetContext(TestData.generalDataSections);
            var repo = new SqlGeneralDataSectionRepo(context);

            int id = _rnd.Next(1, TestData.generalDataSections.Count());
            var generalDataSection = TestData.generalDataSections.FirstOrDefault(v => v.Id == id);

            // Act
            repo.Delete(generalDataSection);

            // Assert
            Assert.Null(repo.GetById(id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
