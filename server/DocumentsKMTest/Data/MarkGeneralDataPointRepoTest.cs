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
    public class markGeneralDataPointRepoTest
    {
        private readonly Random _rnd = new Random();
        private readonly int _maxSectionId = 2;

        private readonly List<Department> _departments;
        private readonly List<Position> _positions;
        private readonly List<Employee> _employees;
        private readonly List<Project> _projects;
        private readonly List<Node> _nodes;
        private readonly List<Subnode> _subnodes;
        private readonly List<Mark> _marks;
        private readonly List<MarkGeneralDataSection> _markGeneralDataSections;
        private readonly List<MarkGeneralDataPoint> _markGeneralDataPoints;

        private readonly List<MarkGeneralDataPoint> _updateMarkGeneralDataPoints = new List<MarkGeneralDataPoint> {};

        public markGeneralDataPointRepoTest()
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
            _projects = new List<Project>
            {
                new Project
                {
                    Id = 1,
                    Name = "P1",
                    BaseSeries = "M32787",
                },
                new Project
                {
                    Id = 2,
                    Name = "2",
                    BaseSeries = "M32788",
                },
            };
            _nodes = new List<Node>
            {
                new Node
                {
                    Id = 1,
                    ProjectId = _projects[0].Id,
                    Code = "11",
                    Name = "Name 1",
                    ChiefEngineerName = "CE1",
                },
                new Node
                {
                    Id = 2,
                    ProjectId = _projects[1].Id,
                    Code = "22",
                    Name = "Name 2",
                    ChiefEngineerName = "CE2",
                },
            };
            _subnodes = new List<Subnode>
            {
                new Subnode
                {
                    Id = 1,
                    NodeId = _nodes[0].Id,
                    Code = "Code1",
                    Name = "Name 1",
                },
                new Subnode
                {
                    Id = 2,
                    NodeId = _nodes[1].Id,
                    Code = "Code2",
                    Name = "Name 2",
                },
            };
            _marks = new List<Mark>
            {
                new Mark
                {
                    Id = 1,
                    SubnodeId = _subnodes[0].Id,
                    Code = "KM1",
                    Name = "Name 1",
                    Department = _departments[0],
                    NormContr = _employees[0],
                },
                new Mark
                {
                    Id = 2,
                    SubnodeId = _subnodes[0].Id,
                    Code = "KM2",
                    Name = "Name 2",
                    Department = _departments[0],
                    NormContr = _employees[1],
                },
                new Mark
                {
                    Id = 3,
                    SubnodeId = _subnodes[1].Id,
                    Code = "KM3",
                    Name = "Name 3",
                    Department = _departments[1],
                    NormContr = _employees[2],
                },
                new Mark
                {
                    Id = 4,
                    SubnodeId = _subnodes[1].Id,
                    Code = "KM4",
                    Name = "Name 4",
                    Department = _departments[1],
                    NormContr = _employees[2],
                },
            };
            _markGeneralDataSections = new List<MarkGeneralDataSection>
            {
                new MarkGeneralDataSection
                {
                    Id = 1,
                    Name = "S1",
                    Mark = _marks[0],
                    OrderNum = 1,
                },
                new MarkGeneralDataSection
                {
                    Id = 2,
                    Name = "S2",
                    Mark = _marks[0],
                    OrderNum = 2,
                },
                new MarkGeneralDataSection
                {
                    Id = 3,
                    Name = "S3",
                    Mark = _marks[0],
                    OrderNum = 3,
                },
                new MarkGeneralDataSection
                {
                    Id = 4,
                    Name = "S4",
                    Mark = _marks[1],
                    OrderNum = 4,
                },
                new MarkGeneralDataSection
                {
                    Id = 5,
                    Name = "S5",
                    Mark = _marks[1],
                    OrderNum = 5,
                },
                new MarkGeneralDataSection
                {
                    Id = 6,
                    Name = "S6",
                    Mark = _marks[2],
                    OrderNum = 6,
                },
            };
            _markGeneralDataPoints = new List<MarkGeneralDataPoint>
            {
                new MarkGeneralDataPoint
                {
                    Id = 1,
                    Section = _markGeneralDataSections[0],
                    Text = "gdp1",
                    OrderNum = 1,
                },
                new MarkGeneralDataPoint
                {
                    Id = 2,
                    Section = _markGeneralDataSections[1],
                    Text = "gdp2",
                    OrderNum = 2,
                },
                new MarkGeneralDataPoint
                {
                    Id = 3,
                    Section = _markGeneralDataSections[0],
                    Text = "gdp3",
                    OrderNum = 3,
                },
                new MarkGeneralDataPoint
                {
                    Id = 4,
                    Section = _markGeneralDataSections[1],
                    Text = "gdp4",
                    OrderNum = 4,
                },
                new MarkGeneralDataPoint
                {
                    Id = 5,
                    Section = _markGeneralDataSections[0],
                    Text = "gdp5",
                    OrderNum = 5,
                },
                new MarkGeneralDataPoint
                {
                    Id = 6,
                    Section = _markGeneralDataSections[1],
                    Text = "gdp6",
                    OrderNum = 6,
                },
                new MarkGeneralDataPoint
                {
                    Id = 7,
                    Section = _markGeneralDataSections[0],
                    Text = "mgdp7",
                    OrderNum = 7,
                },
            };

            foreach (var gdp in _markGeneralDataPoints)
            {
                _updateMarkGeneralDataPoints.Add(new MarkGeneralDataPoint
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
            builder.UseInMemoryDatabase(databaseName: "MarkGeneralDataPointTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (!isUpdate)
                context.MarkGeneralDataPoints.AddRange(_markGeneralDataPoints);
            else
                context.MarkGeneralDataPoints.AddRange(_updateMarkGeneralDataPoints);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllBySectionId_ShouldReturnmarkGeneralDataPoints()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlMarkGeneralDataPointRepo(context);

            var sectionId = _rnd.Next(1, _maxSectionId);

            // Act
            var markGeneralDataPoints = repo.GetAllBySectionId(sectionId);

            // Assert
            Assert.Equal(_markGeneralDataPoints.Where(
                v => v.Section.Id == sectionId).OrderBy(
                    v => v.OrderNum), markGeneralDataPoints);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAllBySectionId_ShouldReturnEmptyArray_WhenWrongSectionId()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlMarkGeneralDataPointRepo(context);

            // Act
            var markGeneralDataPoints = repo.GetAllBySectionId(999);

            // Assert
            Assert.Empty(markGeneralDataPoints);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnmarkGeneralDataPoints()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlMarkGeneralDataPointRepo(context);

            var markId = 1;

            // Act
            var markGeneralDataPoints = repo.GetAllByMarkId(markId);

            // Assert
            Assert.Equal(_markGeneralDataPoints.Where(
                v => v.Section.Mark.Id == markId).OrderBy(
                    v => v.Section.Id).ThenBy(
                        v => v.OrderNum), markGeneralDataPoints);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnmarkGeneralDataPoint()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlMarkGeneralDataPointRepo(context);

            int id = _rnd.Next(1, _markGeneralDataPoints.Count());

            // Act
            var markGeneralDataPoint = repo.GetById(id);

            // Assert
            Assert.Equal(_markGeneralDataPoints.SingleOrDefault(v => v.Id == id), markGeneralDataPoint);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlMarkGeneralDataPointRepo(context);

            // Act
            var markGeneralDataPoint = repo.GetById(999);

            // Assert
            Assert.Null(markGeneralDataPoint);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetByUniqueKey_ShouldReturnmarkGeneralDataPoint()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlMarkGeneralDataPointRepo(context);

            var sectionId = _markGeneralDataPoints[0].Section.Id;
            var text = _markGeneralDataPoints[0].Text;

            // Act
            var markGeneralDataPoint = repo.GetByUniqueKey(
                sectionId, text);

            // Assert
            Assert.Equal(_markGeneralDataPoints.SingleOrDefault(
                v => v.Section.Id == sectionId &&
                    v.Text == text), markGeneralDataPoint);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetByUniqueKey_ShouldReturnNull_WhenWrongKey()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlMarkGeneralDataPointRepo(context);

            var sectionId = _markGeneralDataPoints[0].Section.Id;
            var text = _markGeneralDataPoints[0].Text;

            // Act
            var additionalWork1 = repo.GetByUniqueKey(999, text);
            var additionalWork2 = repo.GetByUniqueKey(sectionId, "NotFound");

            // Assert
            Assert.Null(additionalWork1);
            Assert.Null(additionalWork2);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Add_ShouldAddmarkGeneralDataPoint()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlMarkGeneralDataPointRepo(context);

            int sectionId = _rnd.Next(1, _maxSectionId);
            var markGeneralDataPoint = new MarkGeneralDataPoint
            {
                Section = _markGeneralDataSections.SingleOrDefault(v => v.Id == sectionId),
                Text = "NewCreate",
                OrderNum = 9,
            };

            // Act
            repo.Add(markGeneralDataPoint);

            // Assert
            Assert.NotNull(repo.GetById(markGeneralDataPoint.Id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Update_ShouldUpdateMarkGeneralDataPoint()
        {
            // Arrange
            var context = GetContext(true);
            var repo = new SqlMarkGeneralDataPointRepo(context);

            int id = _rnd.Next(1, _updateMarkGeneralDataPoints.Count());
            var markGeneralDataPoint = _updateMarkGeneralDataPoints.FirstOrDefault(v => v.Id == id);
            markGeneralDataPoint.Text = "NewUpdate";

            // Act
            repo.Update(markGeneralDataPoint);

            // Assert
            Assert.Equal(markGeneralDataPoint.Text, repo.GetById(id).Text);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Delete_ShouldDeleteMarkGeneralDataPoint()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlMarkGeneralDataPointRepo(context);

            int id = _rnd.Next(1, _markGeneralDataPoints.Count());
            var markGeneralDataPoint = _markGeneralDataPoints.FirstOrDefault(v => v.Id == id);

            // Act
            repo.Delete(markGeneralDataPoint);

            // Assert
            Assert.Null(repo.GetById(id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
