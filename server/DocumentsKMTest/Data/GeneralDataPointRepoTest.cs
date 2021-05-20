using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    // Entity is involved in One-To-Many relationship
    public class GeneralDataPointRepoTest
    {
        private readonly Random _rnd = new Random();
        private readonly int _maxSectionId = 2;

        private readonly List<Department> _departments;
        private readonly List<Position> _positions;
        private readonly List<Employee> _employees;
        private readonly List<GeneralDataSection> _generalDataSections;
        private readonly List<GeneralDataPoint> _generalDataPoints;

        private readonly List<GeneralDataPoint> _updateGeneralDataPoints = new List<GeneralDataPoint> {};

        public GeneralDataPointRepoTest()
        {
            // Arrange
            _departments = new List<Department>
            {
                new Department
                {
                    Id = 1,
                    Name = "D1",
                },
                new Department
                {
                    Id = 2,
                    Name = "D2",
                },
            };
            _positions = new List<Position>
            {
                new Position
                {
                    Id = 1,
                    Name = "P1",
                },
                new Position
                {
                    Id = 2,
                    Name = "P2",
                },
                new Position
                {
                    Id = 3,
                    Name = "P3",
                },
                new Position
                {
                    Id = 4,
                    Name = "P4",
                },
                new Position
                {
                    Id = 7,
                    Name = "P7",
                },
                new Position
                {
                    Id = 9,
                    Name = "P9",
                },
                new Position
                {
                    Id = 10,
                    Name = "P10",
                },
            };
            _employees = new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    Name = "E1",
                    Department = _departments[0],
                    Position = _positions[0],
                },
                new Employee
                {
                    Id = 2,
                    Name = "E2",
                    Department = _departments[1],
                    Position = _positions[1],
                },
                new Employee
                {
                    Id = 3,
                    Name = "E3",
                    Department = _departments[0],
                    Position = _positions[2],
                },
                new Employee
                {
                    Id = 4,
                    Name = "E4",
                    Department = _departments[1],
                    Position = _positions[1],
                },
                new Employee
                {
                    Id = 5,
                    Name = "E5",
                    Department = _departments[0],
                    Position = _positions[4],
                },
                new Employee
                {
                    Id = 6,
                    Name = "E6",
                    Department = _departments[1],
                    Position = _positions[4],
                },
                new Employee
                {
                    Id = 7,
                    Name = "E7",
                    Department = _departments[0],
                    Position = _positions[5],
                },
                new Employee
                {
                    Id = 8,
                    Name = "E8",
                    Department = _departments[1],
                    Position = _positions[6],
                },
            };
            _generalDataSections = new List<GeneralDataSection>
            {
                new GeneralDataSection
                {
                    Id = 1,
                    Name = "S1",
                },
                new GeneralDataSection
                {
                    Id = 2,
                    Name = "S2",
                },
                new GeneralDataSection
                {
                    Id = 3,
                    Name = "S3",
                },
                new GeneralDataSection
                {
                    Id = 4,
                    Name = "S4",
                },
                new GeneralDataSection
                {
                    Id = 5,
                    Name = "S5",
                },
                new GeneralDataSection
                {
                    Id = 6,
                    Name = "S6",
                },
            };
            _generalDataPoints = new List<GeneralDataPoint>
            {
                new GeneralDataPoint
                {
                    Id = 1,
                    Section = _generalDataSections[0],
                    Text = "gdp1",
                    OrderNum = 1,
                },
                new GeneralDataPoint
                {
                    Id = 2,
                    Section = _generalDataSections[1],
                    Text = "gdp2",
                    OrderNum = 2,
                },
                new GeneralDataPoint
                {
                    Id = 3,
                    Section = _generalDataSections[0],
                    Text = "gdp3",
                    OrderNum = 3,
                },
                new GeneralDataPoint
                {
                    Id = 4,
                    Section = _generalDataSections[1],
                    Text = "gdp4",
                    OrderNum = 4,
                },
                new GeneralDataPoint
                {
                    Id = 5,
                    Section = _generalDataSections[0],
                    Text = "gdp5",
                    OrderNum = 5,
                },
                new GeneralDataPoint
                {
                    Id = 6,
                    Section = _generalDataSections[1],
                    Text = "gdp6",
                    OrderNum = 6,
                },
                new GeneralDataPoint
                {
                    Id = 7,
                    Section = _generalDataSections[0],
                    Text = "mgdp7",
                    OrderNum = 7,
                },
            };

            foreach (var gdp in _generalDataPoints)
            {
                _updateGeneralDataPoints.Add(new GeneralDataPoint
                {
                    Id = gdp.Id,
                    Section = gdp.Section,
                    Text = gdp.Text,
                    OrderNum = gdp.OrderNum,
                });
            }
        }

        private ApplicationContext GetContext(bool isUpdate = false)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "GeneralDataPointTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (!isUpdate)
                context.GeneralDataPoints.AddRange(_generalDataPoints);
            else
                context.GeneralDataPoints.AddRange(_updateGeneralDataPoints);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllBySectionId_ShouldReturnGeneralDataPoints()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlGeneralDataPointRepo(context);

            var sectionId = _rnd.Next(1, _maxSectionId);

            // Act
            var generalDataPoints = repo.GetAllBySectionId(sectionId);

            // Assert
            Assert.Equal(_generalDataPoints.Where(
                v => v.Section.Id == sectionId).OrderBy(
                    v => v.OrderNum), generalDataPoints);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAllBySectionId_ShouldReturnEmptyArray_WhenWrongSectionId()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlGeneralDataPointRepo(context);

            // Act
            var generalDataPoints = repo.GetAllBySectionId(999);

            // Assert
            Assert.Empty(generalDataPoints);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAll_ShouldReturnGeneralDataPoints()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlGeneralDataPointRepo(context);

            // Act
            var generalDataPoints = repo.GetAll();

            // Assert
            Assert.Equal(_generalDataPoints.OrderBy(
                    v => v.OrderNum), generalDataPoints);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnGeneralDataPoint()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlGeneralDataPointRepo(context);

            int id = _rnd.Next(1, _generalDataPoints.Count());

            // Act
            var generalDataPoint = repo.GetById(id);

            // Assert
            Assert.Equal(_generalDataPoints.SingleOrDefault(v => v.Id == id), generalDataPoint);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlGeneralDataPointRepo(context);

            // Act
            var generalDataPoint = repo.GetById(999);

            // Assert
            Assert.Null(generalDataPoint);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetByUniqueKey_ShouldReturnGeneralDataPoint()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlGeneralDataPointRepo(context);

            var sectionId = _generalDataPoints[0].Section.Id;
            var text = _generalDataPoints[0].Text;

            // Act
            var generalDataPoint = repo.GetByUniqueKey(
                sectionId, text);

            // Assert
            Assert.Equal(_generalDataPoints.SingleOrDefault(
                v => v.Section.Id == sectionId &&
                    v.Text == text), generalDataPoint);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetByUniqueKey_ShouldReturnNull_WhenWrongKey()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlGeneralDataPointRepo(context);

            var sectionId = _generalDataPoints[0].Section.Id;
            var text = _generalDataPoints[0].Text;

            // Act
            var additionalWork1 = repo.GetByUniqueKey(999, text);
            var additionalWork2 = repo.GetByUniqueKey(sectionId, "NotFound");

            // Assert
            Assert.Null(additionalWork1);
            Assert.Null(additionalWork2);

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
