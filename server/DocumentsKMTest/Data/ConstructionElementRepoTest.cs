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
    public class ConstructionElementRepoTest
    {
        private readonly Random _rnd = new Random();
        private readonly int _maxConstructionId = 3;

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
        private readonly List<ProfileClass> _profileClasses;
        private readonly List<ProfileType> _profileTypes;
        private readonly List<Profile> _profiles;
        private readonly List<Steel> _steel;
        private readonly List<ConstructionElement> _constructionElements;

        private readonly List<ConstructionElement> _updateConstructionElements = new List<ConstructionElement> {};

        public ConstructionElementRepoTest()
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
            _steel = new List<Steel>
            {
                new Steel
                {
                    Id = 1,
                    Name = "N1",
                    Standard = "S1",
                    Strength = 1,
                },
                new Steel
                {
                    Id = 2,
                    Name = "N2",
                    Standard = "S2",
                    Strength = 2,
                },
                new Steel
                {
                    Id = 3,
                    Name = "N3",
                    Standard = "S3",
                },
            };
            _profileClasses = new List<ProfileClass>
            {
                new ProfileClass
                {
                    Id = 1,
                    Name = "N1",
                    Note = "N1",
                },
                new ProfileClass
                {
                    Id = 2,
                    Name = "N2",
                },
                new ProfileClass
                {
                    Id = 3,
                    Name = "N3",
                },
            };
            _profileTypes = new List<ProfileType>
            {
                new ProfileType
                {
                    Id = 1,
                    Name = "N1",
                },
                new ProfileType
                {
                    Id = 2,
                    Name = "N2",
                },
                new ProfileType
                {
                    Id = 3,
                    Name = "N3",
                },
            };
            _profiles = new List<Profile>
            {
                new Profile
                {
                    Id = 1,
                    Class = _profileClasses[0],
                    Name = "P1",
                    Symbol = "S1",
                    Weight = 1,
                    Area = 1,
                    Type = _profileTypes[0],

                },
                new Profile
                {
                    Id = 2,
                    Class = _profileClasses[1],
                    Name = "P2",
                    Symbol = "S2",
                    Weight = 2,
                    Area = 2,
                    Type = _profileTypes[1],
                },
                new Profile
                {
                    Id = 3,
                    Class = _profileClasses[2],
                    Name = "P3",
                    Symbol = "S3",
                    Weight = 3,
                    Area = 3,
                    Type = _profileTypes[2],
                },
            };
            _constructionElements = new List<ConstructionElement>
            {
                new ConstructionElement
                {
                    Id = 1,
                    Construction = _constructions[0],
                    Profile = _profiles[0],
                    Steel = _steel[0],
                    Length = 1.0f,
                },
                new ConstructionElement
                {
                    Id = 2,
                    Construction = _constructions[0],
                    Profile = _profiles[1],
                    Steel = _steel[1],
                    Length = 1.0f,
                },
                new ConstructionElement
                {
                    Id = 3,
                    Construction = _constructions[0],
                    Profile = _profiles[2],
                    Steel = _steel[2],
                    Length = 1.0f,
                },
                new ConstructionElement
                {
                    Id = 4,
                    Construction = _constructions[1],
                    Profile = _profiles[0],
                    Steel = _steel[0],
                    Length = 1.0f,
                },
                new ConstructionElement
                {
                    Id = 5,
                    Construction = _constructions[1],
                    Profile = _profiles[1],
                    Steel = _steel[1],
                    Length = 1.0f,
                },
                new ConstructionElement
                {
                    Id = 6,
                    Construction = _constructions[1],
                    Profile = _profiles[2],
                    Steel = _steel[2],
                    Length = 1.0f,
                },
                new ConstructionElement
                {
                    Id = 7,
                    Construction = _constructions[2],
                    Profile = _profiles[0],
                    Steel = _steel[0],
                    Length = 1.0f,
                },
                new ConstructionElement
                {
                    Id = 8,
                    Construction = _constructions[2],
                    Profile = _profiles[1],
                    Steel = _steel[1],
                    Length = 1.0f,
                },
                new ConstructionElement
                {
                    Id = 9,
                    Construction = _constructions[2],
                    Profile = _profiles[2],
                    Steel = _steel[2],
                    Length = 1.0f,
                },
            };

            foreach (var ce in _constructionElements)
            {
                _updateConstructionElements.Add(new ConstructionElement
                {
                    Id = ce.Id,
                    Construction = ce.Construction,
                    Profile = ce.Profile,
                    Steel = ce.Steel,
                    Length = ce.Length,
                });
            }
        }

        private ApplicationContext GetContext(bool isUpdate = false)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "ConstructionElementTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (!isUpdate)
                context.ConstructionElements.AddRange(_constructionElements);
            else
                context.ConstructionElements.AddRange(_updateConstructionElements);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllByConstructionId_ShouldReturnConstructions()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionElementRepo(context);

            var constructionId = _rnd.Next(1, _maxConstructionId);

            // Act
            var constructionElements = repo.GetAllByConstructionId(constructionId);

            // Assert
            Assert.Equal(_constructionElements.Where(
                v => v.Construction.Id == constructionId), constructionElements);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAllByConstructionId_ShouldReturnEmptyArray_WhenWrongConstructionId()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionElementRepo(context);

            // Act
            var constructionElements = repo.GetAllByConstructionId(999);

            // Assert
            Assert.Empty(constructionElements);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnConstructionElement()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionElementRepo(context);

            int id = _rnd.Next(1, _constructionElements.Count());

            // Act
            var constructionElement = repo.GetById(id);

            // Assert
            Assert.Equal(_constructionElements.SingleOrDefault(v => v.Id == id), constructionElement);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionElementRepo(context);

            // Act
            var constructionElement = repo.GetById(999);

            // Assert
            Assert.Null(constructionElement);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Add_ShouldAddConstruction()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionElementRepo(context);

            int constructionId = _rnd.Next(1, _specifications.Count());
            int profileId = _rnd.Next(1, _profiles.Count());
            int steelId = _rnd.Next(1, _steel.Count());
            var constructionElement = new ConstructionElement
            {
                Construction = _constructions.SingleOrDefault(
                    v => v.Id == constructionId),
                Profile = _profiles.SingleOrDefault(
                    v => v.Id == profileId),
                Steel = _steel.SingleOrDefault(
                    v => v.Id == steelId),
                Length = 1.0f,
            };

            // Act
            repo.Add(constructionElement);

            // Assert
            Assert.NotNull(repo.GetById(constructionElement.Id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Update_ShouldUpdateConstruction()
        {
            // Arrange
            var context = GetContext(true);
            var repo = new SqlConstructionElementRepo(context);

            int id = _rnd.Next(1, _updateConstructionElements.Count());
            var construction = _updateConstructionElements.FirstOrDefault(v => v.Id == id);
            construction.Length = 9.0f;

            // Act
            repo.Update(construction);

            // Assert
            Assert.Equal(construction.Length, repo.GetById(id).Length);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Delete_ShouldDeleteConstruction()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionElementRepo(context);

            int id = _rnd.Next(1, _constructionElements.Count());
            var constructionElement = _constructionElements.FirstOrDefault(
                v => v.Id == id);

            // Act
            repo.Delete(constructionElement);

            // Assert
            Assert.Null(repo.GetById(id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
