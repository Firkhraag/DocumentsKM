using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class MarkGeneralDataSectionRepoTest
    {
        private readonly Random _rnd = new Random();
        private readonly int _maxMarkId = 3;

        private ApplicationContext GetContext(List<MarkGeneralDataSection> MarkGeneralDataSections)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "MarkGeneralDataSectionTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.MarkGeneralDataSections.AddRange(MarkGeneralDataSections);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnMarkGeneralDataSections()
        {
            // Assert
            var context = GetContext(TestData.markGeneralDataSections);
            var repo = new SqlMarkGeneralDataSectionRepo(context);

            int markId = _rnd.Next(1, _maxMarkId);

            // Act
            var markGeneralDataSections = repo.GetAllByMarkId(markId);

            // Assert
            Assert.Equal(TestData.markGeneralDataSections.Where(
                v => v.Mark.Id == markId), markGeneralDataSections);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnMarkGeneralDataSection()
        {
            // Arrange
            var context = GetContext(TestData.markGeneralDataSections);
            var repo = new SqlMarkGeneralDataSectionRepo(context);

            int id = _rnd.Next(1, TestData.markGeneralDataSections.Count());

            // Act
            var markGeneralDataSection = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.markGeneralDataSections.SingleOrDefault(v => v.Id == id),
                markGeneralDataSection);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext(TestData.markGeneralDataSections);
            var repo = new SqlMarkGeneralDataSectionRepo(context);

            // Act
            var markGeneralDataSection = repo.GetById(999);

            // Assert
            Assert.Null(markGeneralDataSection);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Add_ShouldAddMarkGeneralDataSection()
        {
            // Arrange
            var context = GetContext(TestData.markGeneralDataSections);
            var repo = new SqlMarkGeneralDataSectionRepo(context);

            int markId = _rnd.Next(1, TestData.marks.Count());
            var markGeneralDataSection = new MarkGeneralDataSection
            {
                Name = "NewCreate",
                Mark = TestData.marks.SingleOrDefault(v => v.Id == markId),
                OrderNum = 9,
            };

            // Act
            repo.Add(markGeneralDataSection);

            // Assert
            Assert.NotNull(repo.GetById(markGeneralDataSection.Id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Update_ShouldUpdateMarkGeneralDataSection()
        {
            // Arrange
            var markGeneralDataSections = new List<MarkGeneralDataSection> { };
            foreach (var mgds in TestData.markGeneralDataSections)
            {
                markGeneralDataSections.Add(new MarkGeneralDataSection
                {
                    Id = mgds.Id,
                    Name = mgds.Name,
                    Mark = mgds.Mark,
                    OrderNum = mgds.OrderNum,
                });
            }
            var context = GetContext(markGeneralDataSections);
            var repo = new SqlMarkGeneralDataSectionRepo(context);

            int id = _rnd.Next(1, markGeneralDataSections.Count());
            var markGeneralDataSection = markGeneralDataSections.FirstOrDefault(v => v.Id == id);
            markGeneralDataSection.Name = "NewUpdate";

            // Act
            repo.Update(markGeneralDataSection);

            // Assert
            Assert.Equal(markGeneralDataSection.Name, repo.GetById(id).Name);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Delete_ShouldDeleteMarkGeneralDataSection()
        {
            // Arrange
            var context = GetContext(TestData.markGeneralDataSections);
            var repo = new SqlMarkGeneralDataSectionRepo(context);

            int id = _rnd.Next(1, TestData.markGeneralDataSections.Count());
            var markGeneralDataSection = TestData.markGeneralDataSections.FirstOrDefault(v => v.Id == id);

            // Act
            repo.Delete(markGeneralDataSection);

            // Assert
            Assert.Null(repo.GetById(id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
