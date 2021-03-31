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
    public class StandardConstructionRepoTest
    {
        private readonly Random _rnd = new Random();
        private readonly int _maxSpecificationId = 3;

        private readonly List<Department> _departments;
        private readonly List<Position> _positions;
        private readonly List<Employee> _employees;
        private readonly List<Project> _projects;
        private readonly List<Node> _nodes;
        private readonly List<Subnode> _subnodes;
        private readonly List<Mark> _marks;
        private readonly List<Specification> _specifications;
        private readonly List<StandardConstruction> _standardConstructions;

        private readonly List<StandardConstruction> _updateStandardConstructions = new List<StandardConstruction> {};

        public StandardConstructionRepoTest()
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
            _specifications = new List<Specification>
            {
                new Specification
                {
                    Id = 1,
                    Mark = _marks[0],
                    Num = 0,
                    IsCurrent = true,
                },
                new Specification
                {
                    Id = 2,
                    Mark = _marks[1],
                    Num = 0,
                    IsCurrent = false,
                },
                new Specification
                {
                    Id = 3,
                    Mark = _marks[1],
                    Num = 1,
                    IsCurrent = true,
                },
                new Specification
                {
                    Id = 4,
                    Mark = _marks[2],
                    Num = 0,
                    IsCurrent = true,
                },
            };
            _standardConstructions = new List<StandardConstruction>
            {
                new StandardConstruction
                {
                    Id = 1,
                    Specification = _specifications[0],
                    Name = "N1",
                    Num = 1,
                    Sheet = "S1",
                    Weight = 1.0f,
                },
                new StandardConstruction
                {
                    Id = 2,
                    Specification = _specifications[1],
                    Name = "N2",
                    Num = 2,
                    Sheet = "S2",
                    Weight = 2.0f,
                },
                new StandardConstruction
                {
                    Id = 3,
                    Specification = _specifications[2],
                    Name = "N3",
                    Num = 3,
                    Sheet = "S3",
                    Weight = 3.0f,
                },
            };

            foreach (var sc in _standardConstructions)
            {
                _updateStandardConstructions.Add(new StandardConstruction
                {
                    Id = sc.Id,
                    Specification = sc.Specification,
                    Name = sc.Name,
                    Num = sc.Num,
                    Sheet = sc.Sheet,
                    Weight = sc.Weight,
                });
            }
        }

        private ApplicationContext GetContext(bool isUpdate = false)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "StandardConstructionTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (!isUpdate)
                context.StandardConstructions.AddRange(_standardConstructions);
            else
                context.StandardConstructions.AddRange(_updateStandardConstructions);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllBySpecificationId_ShouldReturnstandardConstructions()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlStandardConstructionRepo(context);

            var specificationId = _rnd.Next(1, _maxSpecificationId);

            // Act
            var standardConstructions = repo.GetAllBySpecificationId(specificationId);

            // Assert
            Assert.Equal(_standardConstructions.Where(
                v => v.Specification.Id == specificationId), standardConstructions);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAllBySpecificationId_ShouldReturnEmptyArray_WhenWrongSpecificationId()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlStandardConstructionRepo(context);

            // Act
            var standardConstructions = repo.GetAllBySpecificationId(999);

            // Assert
            Assert.Empty(standardConstructions);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnstandardConstruction()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlStandardConstructionRepo(context);

            int id = _rnd.Next(1, _standardConstructions.Count());

            // Act
            var standardConstruction = repo.GetById(id);

            // Assert
            Assert.Equal(_standardConstructions.SingleOrDefault(v => v.Id == id), standardConstruction);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlStandardConstructionRepo(context);

            // Act
            var standardConstruction = repo.GetById(999);

            // Assert
            Assert.Null(standardConstruction);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetByUniqueKey_ShouldReturnstandardConstruction()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlStandardConstructionRepo(context);

            var specificationId = _standardConstructions[0].Specification.Id;
            var name = _standardConstructions[0].Name;

            // Act
            var standardConstruction = repo.GetByUniqueKey(specificationId, name);

            // Assert
            Assert.Equal(_standardConstructions.SingleOrDefault(
                v => v.Specification.Id == specificationId &&
                    v.Name == name), standardConstruction);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetByUniqueKey_ShouldReturnNull_WhenWrongKey()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlStandardConstructionRepo(context);

            var specificationId = _standardConstructions[0].Specification.Id;
            var name = _standardConstructions[0].Name;

            // Act
            var additionalWork1 = repo.GetByUniqueKey(999, name);
            var additionalWork2 = repo.GetByUniqueKey(specificationId, "NotFound");

            // Assert
            Assert.Null(additionalWork1);
            Assert.Null(additionalWork2);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Add_ShouldAddstandardConstruction()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlStandardConstructionRepo(context);

            int specificationId = _rnd.Next(1, _specifications.Count());
            var standardConstruction = new StandardConstruction
            {
                Specification = _specifications.SingleOrDefault(v => v.Id == specificationId),
                Name = "NewCreate",
                Num = 1,
                Sheet = "NewCreate",
                Weight = 1.0f,
            };

            // Act
            repo.Add(standardConstruction);

            // Assert
            Assert.NotNull(repo.GetById(standardConstruction.Id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Update_ShouldUpdatestandardConstruction()
        {
            // Arrange
            var context = GetContext(true);
            var repo = new SqlStandardConstructionRepo(context);

            int id = _rnd.Next(1, _updateStandardConstructions.Count());
            var standardConstruction = _updateStandardConstructions.FirstOrDefault(v => v.Id == id);
            standardConstruction.Name = "NewUpdate";

            // Act
            repo.Update(standardConstruction);

            // Assert
            Assert.Equal(standardConstruction.Name, repo.GetById(id).Name);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Delete_ShouldDeletestandardConstruction()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlStandardConstructionRepo(context);

            int id = _rnd.Next(1, _standardConstructions.Count());
            var standardConstruction = _standardConstructions.FirstOrDefault(v => v.Id == id);

            // Act
            repo.Delete(standardConstruction);

            // Assert
            Assert.Null(repo.GetById(id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
