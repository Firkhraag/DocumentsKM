using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class MarkGeneralDataPointRepoTest
    {
        private readonly Random _rnd = new Random();
        private readonly int _maxMarkId = 3;
        private readonly int _maxSectionId = 3;

        private ApplicationContext GetContext(List<MarkGeneralDataPoint> markGeneralDataPoints)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "MarkGeneralDataPointTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Marks.AddRange(TestData.marks);
            context.GeneralDataSections.AddRange(TestData.generalDataSections);
            context.MarkGeneralDataPoints.AddRange(markGeneralDataPoints);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllByMarkAndSectionId_ShouldReturnMarkGeneralDataPoints()
        {
            // Arrange
            var context = GetContext(TestData.markGeneralDataPoints);
            var repo = new SqlMarkGeneralDataPointRepo(context);

            var markId = _rnd.Next(1, _maxMarkId);
            var sectionId = _rnd.Next(1, _maxSectionId);

            // Act
            var markGeneralDataPoints = repo.GetAllByMarkAndSectionId(markId, sectionId);

            // Assert
            Assert.Equal(TestData.markGeneralDataPoints.Where(
                v => v.Mark.Id == markId && v.Section.Id == sectionId),
                markGeneralDataPoints);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAllByMarkAndSectionId_ShouldReturnEmptyArray_WhenWrongMarkOrSectionId()
        {
            // Arrange
            var context = GetContext(TestData.markGeneralDataPoints);
            var repo = new SqlMarkGeneralDataPointRepo(context);

            var markId = _rnd.Next(1, _maxMarkId);
            var wrongMarkId = 999;
            var sectionId = _rnd.Next(1, _maxSectionId);
            var wrongSectionId = 999;

            // Act
            var markGeneralDataPoints1 = repo.GetAllByMarkAndSectionId(
                wrongMarkId, sectionId);
            var markGeneralDataPoints2 = repo.GetAllByMarkAndSectionId(
                markId, wrongSectionId);

            // Assert
            Assert.Empty(markGeneralDataPoints1);
            Assert.Empty(markGeneralDataPoints2);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnMarkGeneralDataPoint()
        {
            // Arrange
            var context = GetContext(TestData.markGeneralDataPoints);
            var repo = new SqlMarkGeneralDataPointRepo(context);

            int id = _rnd.Next(1, TestData.markGeneralDataPoints.Count());

            // Act
            var markGeneralDataPoint = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.markGeneralDataPoints.SingleOrDefault(v => v.Id == id),
                markGeneralDataPoint);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext(TestData.markGeneralDataPoints);
            var repo = new SqlMarkGeneralDataPointRepo(context);

            // Act
            var markGeneralDataPoint = repo.GetById(999);

            // Assert
            Assert.Null(markGeneralDataPoint);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetByUniqueKey_ShouldReturnMarkGeneralDataPoint()
        {
            // Arrange
            var context = GetContext(TestData.markGeneralDataPoints);
            var repo = new SqlMarkGeneralDataPointRepo(context);

            int id = _rnd.Next(1, TestData.markGeneralDataPoints.Count());
            var foundmarkGeneralDataPoint = TestData.markGeneralDataPoints.FirstOrDefault(
                v => v.Id == id);
            var markId = foundmarkGeneralDataPoint.Mark.Id;
            var sectionId = foundmarkGeneralDataPoint.Section.Id;
            var text = foundmarkGeneralDataPoint.Text;

            // Act
            var markGeneralDataPoint = repo.GetByUniqueKey(
                markId, sectionId, text);

            // Assert
            Assert.Equal(id, markGeneralDataPoint.Id);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetByUniqueKey_ShouldReturnNull()
        {
            // Arrange
            var context = GetContext(TestData.markGeneralDataPoints);
            var repo = new SqlMarkGeneralDataPointRepo(context);

            var markId = TestData.marks[0].Id;
            var sectionId = TestData.generalDataSections[0].Id;
            var text = TestData.markGeneralDataPoints[0].Text;

            // Act
            var markGeneralDataPoint1 = repo.GetByUniqueKey(
                999, sectionId, text);
            var markGeneralDataPoint2 = repo.GetByUniqueKey(
                markId, 999, text);
            var markGeneralDataPoint3 = repo.GetByUniqueKey(
                markId, sectionId, "NotFound");

            // Assert
            Assert.Null(markGeneralDataPoint1);
            Assert.Null(markGeneralDataPoint2);
            Assert.Null(markGeneralDataPoint3);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void Add_ShouldAddMarkGeneralDataPoint()
        {
            // Arrange
            var context = GetContext(TestData.markGeneralDataPoints);
            var repo = new SqlMarkGeneralDataPointRepo(context);

            int markId = _rnd.Next(1, TestData.marks.Count());
            int sectionId = _rnd.Next(1, TestData.generalDataSections.Count());
            var markGeneralDataPoint = new MarkGeneralDataPoint
            {
                Mark = TestData.marks.SingleOrDefault(v => v.Id == markId),
                Section = TestData.generalDataSections.SingleOrDefault(
                    v => v.Id == sectionId),
                Text = "NewCreate",
                OrderNum = 3,
            };

            // Act
            repo.Add(markGeneralDataPoint);

            // Assert
            Assert.NotNull(repo.GetById(markGeneralDataPoint.Id));

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void Update_ShouldUpdateMarkGeneralDataPoint()
        {
            // Arrange
            var markGeneralDataPoints = new List<MarkGeneralDataPoint> { };
            foreach (var gdp in TestData.markGeneralDataPoints)
            {
                markGeneralDataPoints.Add(new MarkGeneralDataPoint
                {
                    Id = gdp.Id,
                    Mark = gdp.Mark,
                    Section = gdp.Section,
                    Text = gdp.Text,
                    OrderNum = gdp.OrderNum,
                });
            }
            var context = GetContext(markGeneralDataPoints);
            var repo = new SqlMarkGeneralDataPointRepo(context);

            int id = _rnd.Next(1, markGeneralDataPoints.Count());
            var markGeneralDataPoint = markGeneralDataPoints.FirstOrDefault(
                v => v.Id == id);
            markGeneralDataPoint.Text = "NewUpdate";

            // Act
            repo.Update(markGeneralDataPoint);

            // Assert
            Assert.Equal(markGeneralDataPoint.Text, repo.GetById(id).Text);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void Delete_ShouldDeleteMarkGeneralDataPoint()
        {
            // Arrange
            var context = GetContext(TestData.markGeneralDataPoints);
            var repo = new SqlMarkGeneralDataPointRepo(context);

            int id = _rnd.Next(1, TestData.markGeneralDataPoints.Count());
            var markGeneralDataPoint = TestData.markGeneralDataPoints.FirstOrDefault(
                v => v.Id == id);

            // Act
            repo.Delete(markGeneralDataPoint);

            // Assert
            Assert.Null(repo.GetById(id));

            context.Database.EnsureDeleted();
        }
    }
}
