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
    public class ConstructionBoltRepoTest
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
        private readonly List<BoltDiameter> _boltDiameters;
        private readonly List<BoltLength> _boltLengths;
        private readonly List<ConstructionBolt> _constructionBolts;

        private readonly List<ConstructionBolt> _updateConstructionBolts = new List<ConstructionBolt> {};

        public ConstructionBoltRepoTest()
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
            _boltDiameters = new List<BoltDiameter>
            {
                new BoltDiameter
                {
                    Id = 1,
                    Diameter = 1,
                    NutWeight = 1,
                    WasherSteel = "WS1",
                    WasherWeight = 1,
                    WasherThickness = 1,
                    BoltTechSpec = "BTS1",
                    StrengthClass = "S1",
                    NutTechSpec = "NTS1",
                    WasherTechSpec = "WTS1",
                },
                new BoltDiameter
                {
                    Id = 2,
                    Diameter = 2,
                    NutWeight = 2,
                    WasherSteel = "WS2",
                    WasherWeight = 2,
                    WasherThickness = 2,
                    BoltTechSpec = "BTS2",
                    StrengthClass = "S2",
                    NutTechSpec = "NTS2",
                    WasherTechSpec = "WTS2",
                },
                new BoltDiameter
                {
                    Id = 3,
                    Diameter = 3,
                    NutWeight = 3,
                    WasherSteel = "WS3",
                    WasherWeight = 3,
                    WasherThickness = 3,
                    BoltTechSpec = "BTS3",
                    StrengthClass = "S3",
                    NutTechSpec = "NTS3",
                    WasherTechSpec = "WTS3",
                },
            };
            _boltLengths = new List<BoltLength>
            {
                new BoltLength
                {
                    Id = 1,
                    Diameter = _boltDiameters[0],
                    BoltLen = 1,
                    ScrewLen = 1,
                    BoltWeight = 1,
                },
                new BoltLength
                {
                    Id = 2,
                    Diameter = _boltDiameters[1],
                    BoltLen = 2,
                    ScrewLen = 2,
                    BoltWeight = 2,
                },
                new BoltLength
                {
                    Id = 3,
                    Diameter = _boltDiameters[2],
                    BoltLen = 3,
                    ScrewLen = 3,
                    BoltWeight = 3,
                },
            };
            _constructionBolts = new List<ConstructionBolt>
            {
                new ConstructionBolt
                {
                    Id = 1,
                    Construction = _constructions[0],
                    Diameter = _boltDiameters[0],
                    Packet = 1,
                    Num = 1,
                    NutNum = 1,
                    WasherNum = 1,
                },
                new ConstructionBolt
                {
                    Id = 2,
                    Construction = _constructions[0],
                    Diameter = _boltDiameters[1],
                    Packet = 2,
                    Num = 2,
                    NutNum = 2,
                    WasherNum = 2,
                },
                new ConstructionBolt
                {
                    Id = 3,
                    Construction = _constructions[1],
                    Diameter = _boltDiameters[0],
                    Packet = 3,
                    Num = 3,
                    NutNum = 3,
                    WasherNum = 3,
                },
                new ConstructionBolt
                {
                    Id = 4,
                    Construction = _constructions[1],
                    Diameter = _boltDiameters[2],
                    Packet = 4,
                    Num = 4,
                    NutNum = 4,
                    WasherNum = 4,
                },
                new ConstructionBolt
                {
                    Id = 5,
                    Construction = _constructions[2],
                    Diameter = _boltDiameters[1],
                    Packet = 5,
                    Num = 5,
                    NutNum = 5,
                    WasherNum = 5,
                },
            };

            foreach (var cb in _constructionBolts)
            {
                _updateConstructionBolts.Add(new ConstructionBolt
                {
                    Id = cb.Id,
                    Construction = cb.Construction,
                    Diameter = cb.Diameter,
                    Packet = cb.Packet,
                    Num = cb.Num,
                    NutNum = cb.NutNum,
                    WasherNum = cb.WasherNum,
                });
            }
        }

        private ApplicationContext GetContext(bool isUpdate = false)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "ConstructionBoltTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (!isUpdate)
                context.ConstructionBolts.AddRange(_constructionBolts);
            else
                context.ConstructionBolts.AddRange(_updateConstructionBolts);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllByConstructionId_ShouldReturnConstructions()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionBoltRepo(context);

            var constructionId = _rnd.Next(1, _maxConstructionId);

            // Act
            var constructionBolts = repo.GetAllByConstructionId(constructionId);

            // Assert
            Assert.Equal(_constructionBolts.Where(
                v => v.Construction.Id == constructionId), constructionBolts);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAllByConstructionId_ShouldReturnEmptyArray_WhenWrongConstructionId()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionBoltRepo(context);

            // Act
            var constructionBolts = repo.GetAllByConstructionId(999);

            // Assert
            Assert.Empty(constructionBolts);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnConstructionBolt()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionBoltRepo(context);

            int id = _rnd.Next(1, _constructionBolts.Count());

            // Act
            var constructionBolt = repo.GetById(id);

            // Assert
            Assert.Equal(_constructionBolts.SingleOrDefault(v => v.Id == id), constructionBolt);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionBoltRepo(context);

            // Act
            var constructionBolt = repo.GetById(999);

            // Assert
            Assert.Null(constructionBolt);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetByUniqueKey_ShouldReturnConstructionBolt()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionBoltRepo(context);

            var constructionId = _constructionBolts[0].Construction.Id;
            var diameterId = _constructionBolts[0].Diameter.Id;

            // Act
            var constructionBolt = repo.GetByUniqueKey(
                constructionId, diameterId);

            // Assert
            Assert.Equal(_constructionBolts.SingleOrDefault(
                v => v.Construction.Id == constructionId &&
                    v.Diameter.Id == diameterId), constructionBolt);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetByUniqueKey_ShouldReturnNull_WhenWrongKey()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionBoltRepo(context);

            var constructionId = _constructionBolts[0].Construction.Id;
            var diameterId = _constructionBolts[0].Diameter.Id;

            // Act
            var additionalWork1 = repo.GetByUniqueKey(999, diameterId);
            var additionalWork2 = repo.GetByUniqueKey(constructionId, 999);

            // Assert
            Assert.Null(additionalWork1);
            Assert.Null(additionalWork2);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Add_ShouldAddConstruction()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionBoltRepo(context);

            int constructionId = _rnd.Next(1, _marks.Count());
            int diameterId = _rnd.Next(1, _boltDiameters.Count());
            var constructionBolt = new ConstructionBolt
            {
                Construction = _constructions.SingleOrDefault(
                    v => v.Id == constructionId),
                Diameter = _boltDiameters.SingleOrDefault(
                    v => v.Id == diameterId),
                Packet = 5,
                Num = 5,
                NutNum = 5,
                WasherNum = 5,
            };

            // Act
            repo.Add(constructionBolt);

            // Assert
            Assert.NotNull(repo.GetById(constructionBolt.Id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Update_ShouldUpdateConstruction()
        {
            // Arrange
            var context = GetContext(true);
            var repo = new SqlConstructionBoltRepo(context);

            int id = _rnd.Next(1, _updateConstructionBolts.Count());
            var construction = _updateConstructionBolts.FirstOrDefault(v => v.Id == id);
            construction.Packet = 9;

            // Act
            repo.Update(construction);

            // Assert
            Assert.Equal(construction.Packet, repo.GetById(id).Packet);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Delete_ShouldDeleteConstruction()
        {
            // Arrange
            var context = GetContext();
            var repo = new SqlConstructionBoltRepo(context);

            int id = _rnd.Next(1, _constructionBolts.Count());
            var constructionBolt = _constructionBolts.FirstOrDefault(
                v => v.Id == id);

            // Act
            repo.Delete(constructionBolt);

            // Assert
            Assert.Null(repo.GetById(id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
