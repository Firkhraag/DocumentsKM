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
    public class ConstructionRepoTest
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
        private readonly List<ConstructionType> _constructionTypes;
        private readonly List<ConstructionSubtype> _constructionSubtypes;
        private readonly List<WeldingControl> _weldingControl;
        private readonly List<Construction> _constructions;

        private readonly List<Construction> _updateConstructions = new List<Construction> {};

        public ConstructionRepoTest()
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
                    Project = _projects[0],
                    Code = "11",
                    Name = "Name 1",
                    ChiefEngineer = _employees[0],
                },
                new Node
                {
                    Id = 2,
                    Project = _projects[1],
                    Code = "22",
                    Name = "Name 2",
                    ChiefEngineer = _employees[1],
                },
            };
            _subnodes = new List<Subnode>
            {
                new Subnode
                {
                    Id = 1,
                    Node = _nodes[0],
                    Code = "Code1",
                    Name = "Name 1",
                },
                new Subnode
                {
                    Id = 2,
                    Node = _nodes[1],
                    Code = "Code2",
                    Name = "Name 2",
                },
            };
            _marks = new List<Mark>
            {
                new Mark
                {
                    Id = 1,
                    Subnode = _subnodes[0],
                    Code = "KM1",
                    Name = "Name 1",
                    Department = _departments[0],
                    MainBuilder = _employees[0],
                },
                new Mark
                {
                    Id = 2,
                    Subnode = _subnodes[0],
                    Code = "KM2",
                    Name = "Name 2",
                    Department = _departments[0],
                    MainBuilder = _employees[1],
                },
                new Mark
                {
                    Id = 3,
                    Subnode = _subnodes[1],
                    Code = "KM3",
                    Name = "Name 3",
                    Department = _departments[1],
                    MainBuilder = _employees[2],
                },
                new Mark
                {
                    Id = 4,
                    Subnode = _subnodes[1],
                    Code = "KM4",
                    Name = "Name 4",
                    Department = _departments[1],
                    MainBuilder = _employees[2],
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
            _constructionTypes = new List<ConstructionType>
            {
                new ConstructionType
                {
                    Id = 1,
                    Name = "T1",
                },
                new ConstructionType
                {
                    Id = 2,
                    Name = "T2",
                },
                new ConstructionType
                {
                    Id = 3,
                    Name = "T3",
                },
            };
            _constructionSubtypes = new List<ConstructionSubtype>
            {
                new ConstructionSubtype
                {
                    Id = 1,
                    Type = _constructionTypes[0],
                    Name = "S1",
                    Valuation = "V1",
                },
                new ConstructionSubtype
                {
                    Id = 2,
                    Type = _constructionTypes[1],
                    Name = "S2",
                    Valuation = "V2",
                },
                new ConstructionSubtype
                {
                    Id = 3,
                    Type = _constructionTypes[2],
                    Name = "S3",
                    Valuation = "V3",
                },
            };
            _weldingControl = new List<WeldingControl>
            {
                new WeldingControl
                {
                    Id = 1,
                    Name = "WC1",
                },
                new WeldingControl
                {
                    Id = 2,
                    Name = "WC2",
                },
                new WeldingControl
                {
                    Id = 3,
                    Name = "WC3",
                },
            };
            _constructions = new List<Construction>
            {
                new Construction
                {
                    Id = 1,
                    Specification = _specifications[0],
                    Name = "N1",
                    Type = _constructionTypes[0],
                    Subtype = _constructionSubtypes[0],
                    Valuation = "1701",
                    NumOfStandardConstructions = 1,
                    StandardAlbumCode = "C1",
                    HasEdgeBlunting = true,
                    HasDynamicLoad = false,
                    HasFlangedConnections = true,
                    WeldingControl = _weldingControl[0],
                    PaintworkCoeff = 1,
                },
                new Construction
                {
                    Id = 2,
                    Specification = _specifications[0],
                    Name = "N2",
                    Type = _constructionTypes[1],
                    Valuation = "1702",
                    NumOfStandardConstructions = 1,
                    StandardAlbumCode = "C1",
                    HasEdgeBlunting = true,
                    HasDynamicLoad = false,
                    HasFlangedConnections = true,
                    WeldingControl = _weldingControl[1],
                    PaintworkCoeff = 1,
                },
                new Construction
                {
                    Id = 3,
                    Specification = _specifications[1],
                    Name = "N3",
                    Type = _constructionTypes[0],
                    Valuation = "1703",
                    NumOfStandardConstructions = 0,
                    HasEdgeBlunting = true,
                    HasDynamicLoad = false,
                    HasFlangedConnections = true,
                    WeldingControl = _weldingControl[1],
                    PaintworkCoeff = 1,
                },
                new Construction
                {
                    Id = 4,
                    Specification = _specifications[1],
                    Name = "N4",
                    Type = _constructionTypes[2],
                    Valuation = "1704",
                    NumOfStandardConstructions = 0,
                    HasEdgeBlunting = true,
                    HasDynamicLoad = false,
                    HasFlangedConnections = true,
                    WeldingControl = _weldingControl[2],
                    PaintworkCoeff = 2,
                },
                new Construction
                {
                    Id = 5,
                    Specification = _specifications[2],
                    Name = "N5",
                    Type = _constructionTypes[0],
                    Valuation = "1705",
                    NumOfStandardConstructions = 0,
                    HasEdgeBlunting = true,
                    HasDynamicLoad = false,
                    HasFlangedConnections = true,
                    WeldingControl = _weldingControl[0],
                    PaintworkCoeff = 2,
                },
            };

            foreach (var c in _constructions)
            {
                _updateConstructions.Add(new Construction
                {
                    Id = c.Id,
                    Specification = c.Specification,
                    Name = c.Name,
                    Type = c.Type,
                    Subtype = c.Subtype,
                    Valuation = c.Valuation,
                    NumOfStandardConstructions = c.NumOfStandardConstructions,
                    HasEdgeBlunting = c.HasEdgeBlunting,
                    HasDynamicLoad = c.HasDynamicLoad,
                    HasFlangedConnections = c.HasFlangedConnections,
                    WeldingControl = c.WeldingControl,
                    PaintworkCoeff = c.PaintworkCoeff,
                });
            }
        }

        private ApplicationContext GetContext(bool isUpdate = false)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "ConstructionTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (!isUpdate)
                context.Constructions.AddRange(_constructions);
            else
                context.Constructions.AddRange(_updateConstructions);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllBySpecificationId_ShouldReturnConstructions()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionRepo(context);

            var specificationId = _rnd.Next(1, _maxSpecificationId);

            // Act
            var constructions = repo.GetAllBySpecificationId(specificationId);

            // Assert
            Assert.Equal(_constructions.Where(
                v => v.Specification.Id == specificationId), constructions);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAllBySpecificationId_ShouldReturnEmptyArray_WhenWrongSpecificationId()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionRepo(context);

            // Act
            var constructions = repo.GetAllBySpecificationId(999);

            // Assert
            Assert.Empty(constructions);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnConstruction()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionRepo(context);

            int id = _rnd.Next(1, _constructions.Count());

            // Act
            var construction = repo.GetById(id);

            // Assert
            Assert.Equal(_constructions.SingleOrDefault(v => v.Id == id), construction);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionRepo(context);

            // Act
            var construction = repo.GetById(999);

            // Assert
            Assert.Null(construction);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetByUniqueKey_ShouldReturnConstruction()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionRepo(context);

            var specificationId = _constructions[0].Specification.Id;
            var name = _constructions[0].Name;
            var paintworkCoeff = _constructions[0].PaintworkCoeff;

            // Act
            var construction = repo.GetByUniqueKey(
                specificationId, name, paintworkCoeff);

            // Assert
            Assert.Equal(_constructions.SingleOrDefault(
                v => v.Specification.Id == specificationId &&
                    v.Name == name && v.PaintworkCoeff == paintworkCoeff), construction);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetByUniqueKey_ShouldReturnNull_WhenWrongKey()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionRepo(context);

            var specificationId = _constructions[0].Specification.Id;
            var name = _constructions[0].Name;
            var paintworkCoeff = _constructions[0].PaintworkCoeff;

            // Act
            var additionalWork1 = repo.GetByUniqueKey(999, name, paintworkCoeff);
            var additionalWork2 = repo.GetByUniqueKey(specificationId, "NotFound", paintworkCoeff);
            var additionalWork3 = repo.GetByUniqueKey(specificationId, name, -1);

            // Assert
            Assert.Null(additionalWork1);
            Assert.Null(additionalWork2);
            Assert.Null(additionalWork3);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Add_ShouldAddConstruction()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionRepo(context);

            int specificationId = _rnd.Next(1, _specifications.Count());
            int typeId = _rnd.Next(1, _constructionTypes.Count());
            int subtypeId = _rnd.Next(1, _constructionSubtypes.Count());
            int weldingControlId = _rnd.Next(1, _weldingControl.Count());
            var construction = new Construction
            {
                Specification = _specifications.SingleOrDefault(v => v.Id == specificationId),
                Name = "NewCreate",
                Type = _constructionTypes.SingleOrDefault(v => v.Id == typeId),
                Subtype = _constructionSubtypes.SingleOrDefault(v => v.Id == subtypeId),
                Valuation = "1700",
                NumOfStandardConstructions = 0,
                HasEdgeBlunting = false,
                HasDynamicLoad = false,
                HasFlangedConnections = false,
                WeldingControl = _weldingControl.SingleOrDefault(v => v.Id == weldingControlId),
                PaintworkCoeff = 1,
            };

            // Act
            repo.Add(construction);

            // Assert
            Assert.NotNull(repo.GetById(construction.Id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Update_ShouldUpdateConstruction()
        {
            // Arrange
            var context = GetContext(true);
            var repo = new SqlConstructionRepo(context);

            int id = _rnd.Next(1, _updateConstructions.Count());
            var construction = _updateConstructions.FirstOrDefault(v => v.Id == id);
            construction.Name = "NewUpdate";

            // Act
            repo.Update(construction);

            // Assert
            Assert.Equal(construction.Name, repo.GetById(id).Name);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Delete_ShouldDeleteConstruction()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionRepo(context);

            int id = _rnd.Next(1, _constructions.Count());
            var construction = _constructions.FirstOrDefault(v => v.Id == id);

            // Act
            repo.Delete(construction);

            // Assert
            Assert.Null(repo.GetById(id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
