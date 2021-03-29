using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Dtos;
using DocumentsKM.Models;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    // Entity is involved in One-To-Many relationship
    public class ConstructionBoltServiceTest
    {
        private readonly Mock<IConstructionBoltRepo> _repository = new Mock<IConstructionBoltRepo>();
        private readonly Mock<IConstructionBoltRepo> _updateRepository = new Mock<IConstructionBoltRepo>();
        private readonly IConstructionBoltService _service;
        private readonly IConstructionBoltService _updateService;
        private readonly Random _rnd = new Random();

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
        private readonly int _maxConstructionId = 3;

        public ConstructionBoltServiceTest()
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
                    ChiefEngineer = "CE1",
                },
                new Node
                {
                    Id = 2,
                    Project = _projects[1],
                    Code = "22",
                    Name = "Name 2",
                    ChiefEngineer = "CE2",
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
                    NormContr = _employees[0],
                },
                new Mark
                {
                    Id = 2,
                    Subnode = _subnodes[0],
                    Code = "KM2",
                    Name = "Name 2",
                    Department = _departments[0],
                    NormContr = _employees[1],
                },
                new Mark
                {
                    Id = 3,
                    Subnode = _subnodes[1],
                    Code = "KM3",
                    Name = "Name 3",
                    Department = _departments[1],
                    NormContr = _employees[2],
                },
                new Mark
                {
                    Id = 4,
                    Subnode = _subnodes[1],
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
                    Length = 1,
                    ScrewLength = 1,
                    Weight = 1,
                },
                new BoltLength
                {
                    Id = 2,
                    Diameter = _boltDiameters[1],
                    Length = 2,
                    ScrewLength = 2,
                    Weight = 2,
                },
                new BoltLength
                {
                    Id = 3,
                    Diameter = _boltDiameters[2],
                    Length = 3,
                    ScrewLength = 3,
                    Weight = 3,
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

            var mockMarkRepo = new Mock<IMarkRepo>();
            var mockConstructionRepo = new Mock<IConstructionRepo>();
            var mockBoltDiameterRepo = new Mock<IBoltDiameterRepo>();

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

            foreach (var constructionBolt in _constructionBolts)
            {
                _repository.Setup(mock =>
                    mock.GetById(constructionBolt.Id)).Returns(
                        _constructionBolts.SingleOrDefault(v => v.Id == constructionBolt.Id));
                _updateRepository.Setup(mock =>
                    mock.GetById(constructionBolt.Id)).Returns(
                        _updateConstructionBolts.SingleOrDefault(v => v.Id == constructionBolt.Id));
            }
            foreach (var construction in _constructions)
            {
                mockConstructionRepo.Setup(mock =>
                    mock.GetById(construction.Id, false)).Returns(
                        _constructions.SingleOrDefault(v => v.Id == construction.Id));
                mockConstructionRepo.Setup(mock =>
                    mock.GetById(construction.Id, true)).Returns(
                        _constructions.SingleOrDefault(v => v.Id == construction.Id));

                _repository.Setup(mock =>
                    mock.GetAllByConstructionId(construction.Id)).Returns(
                        _constructionBolts.Where(v => v.Construction.Id == construction.Id));
                _updateRepository.Setup(mock =>
                    mock.GetAllByConstructionId(construction.Id)).Returns(
                        _updateConstructionBolts.Where(v => v.Construction.Id == construction.Id));

                foreach (var constructionBolt in _constructionBolts)
                {
                    _repository.Setup(mock =>
                        mock.GetByUniqueKey(construction.Id, constructionBolt.Diameter.Id)).Returns(
                            _constructionBolts.SingleOrDefault(
                                v => v.Construction.Id == construction.Id &&
                                    v.Diameter.Id == constructionBolt.Diameter.Id));
                    _updateRepository.Setup(mock =>
                        mock.GetByUniqueKey(construction.Id, constructionBolt.Diameter.Id)).Returns(
                            _updateConstructionBolts.SingleOrDefault(
                                v => v.Construction.Id == construction.Id &&
                                    v.Diameter.Id == constructionBolt.Diameter.Id));
                }
            }
            foreach (var diameter in _boltDiameters)
            {
                mockBoltDiameterRepo.Setup(mock =>
                    mock.GetById(diameter.Id)).Returns(
                        _boltDiameters.SingleOrDefault(
                            v => v.Id == diameter.Id));
            }
            foreach (var mark in _marks)
            {
                mockMarkRepo.Setup(mock =>
                    mock.GetById(mark.Id)).Returns(
                        _marks.SingleOrDefault(v => v.Id == mark.Id));
            }

            _repository.Setup(mock =>
                mock.Add(It.IsAny<ConstructionBolt>())).Verifiable();
            _updateRepository.Setup(mock =>
                mock.Update(It.IsAny<ConstructionBolt>())).Verifiable();
            _repository.Setup(mock =>
                mock.Delete(It.IsAny<ConstructionBolt>())).Verifiable();

            _service = new ConstructionBoltService(
                _repository.Object,
                mockMarkRepo.Object,
                mockConstructionRepo.Object,
                mockBoltDiameterRepo.Object);
            _updateService = new ConstructionBoltService(
                _updateRepository.Object,
                mockMarkRepo.Object,
                mockConstructionRepo.Object,
                mockBoltDiameterRepo.Object);
        }

        [Fact]
        public void GetAllByConstructionId_ShouldReturnConstructionBolts()
        {
            // Arrange
            int constructionId = _rnd.Next(1, _maxConstructionId);

            // Act
            var returnedConstructionBolts = _service.GetAllByConstructionId(
                constructionId);

            // Assert
            Assert.Equal(_constructionBolts.Where(
                v => v.Construction.Id == constructionId), returnedConstructionBolts);
        }

        [Fact]
        public void GetAllByConstructionId_ShouldReturnEmptyArray_WhenWrongConstructionId()
        {
            // Act
            var returnedConstructionBolts = _service.GetAllByConstructionId(999);

            // Assert
            Assert.Empty(returnedConstructionBolts);
        }

        [Fact]
        public void Create_ShouldCreateConstructionBolt()
        {
            // Arrange
            int constructionId = 1;
            int diameterId = 3;

            var newConstructionBolt = new ConstructionBolt
            {
                Packet = 5,
                Num = 5,
                NutNum = 5,
                WasherNum = 5,
            };

            // Act
            _service.Create(newConstructionBolt, constructionId, diameterId);

            // Assert
            _repository.Verify(mock => mock.Add(
                It.IsAny<ConstructionBolt>()), Times.Once);
            Assert.NotNull(newConstructionBolt.Construction);
        }

        [Fact]
        public void Create_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int constructionId = _rnd.Next(1, _marks.Count());
            int diameterId = _rnd.Next(1, _boltDiameters.Count());

            var newConstructionBolt = new ConstructionBolt
            {
                Packet = 5,
                Num = 5,
                NutNum = 5,
                WasherNum = 5,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                null, constructionId, diameterId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newConstructionBolt, 999, diameterId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newConstructionBolt, constructionId, 999));

            _repository.Verify(mock => mock.Add(It.IsAny<ConstructionBolt>()), Times.Never);
        }

        [Fact]
        public void Create_ShouldFailWithConflict()
        {
            // Arrange
            int conflictConstructionId = _constructionBolts[0].Construction.Id;
            int conflictDiameterId = _constructionBolts[0].Diameter.Id;

            var newConstructionBolt = new ConstructionBolt
            {
                Packet = 5,
                Num = 5,
                NutNum = 5,
                WasherNum = 5,
            };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Create(
                newConstructionBolt, conflictConstructionId, conflictDiameterId));

            _repository.Verify(mock => mock.Add(It.IsAny<ConstructionBolt>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldUpdateConstructionBolt()
        {
            // Arrange
            int id = _rnd.Next(1, _updateConstructionBolts.Count());
            short newNumber = 6;

            var newConstructionBoltRequest = new ConstructionBoltUpdateRequest
            {
                Packet = newNumber,
                Num = newNumber,
                NutNum = newNumber,
                WasherNum = newNumber,
            };

            // Act
            _updateService.Update(id, newConstructionBoltRequest);

            // Assert
            _updateRepository.Verify(mock => mock.Update(It.IsAny<ConstructionBolt>()), Times.Once);
            Assert.Equal(newNumber, _updateConstructionBolts.SingleOrDefault(
                v => v.Id == id).Packet);
            Assert.Equal(newNumber, _updateConstructionBolts.SingleOrDefault(
                v => v.Id == id).Num);
            Assert.Equal(newNumber, _updateConstructionBolts.SingleOrDefault(
                v => v.Id == id).NutNum);
            Assert.Equal(newNumber, _updateConstructionBolts.SingleOrDefault(
                v => v.Id == id).WasherNum);
        }

        [Fact]
        public void Update_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int id = _rnd.Next(1, _updateConstructionBolts.Count());
            int wrongId = 999;
            short newNumber = 6;

            var newConstructionBoltRequest = new ConstructionBoltUpdateRequest
            {
                Packet = newNumber,
                Num = newNumber,
                NutNum = newNumber,
                WasherNum = newNumber,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _updateService.Update(id, null));
            Assert.Throws<ArgumentNullException>(() => _updateService.Update(
                wrongId, newConstructionBoltRequest));

            _updateRepository.Verify(mock => mock.Update(
                It.IsAny<ConstructionBolt>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldFailWithConflict()
        {
            // Arrange
            short conflictDiameterId = 2;
            var id = 1;

            var newConstructionBoltRequest = new ConstructionBoltUpdateRequest
            {
                DiameterId = conflictDiameterId,
            };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _updateService.Update(id, newConstructionBoltRequest));

            _updateRepository.Verify(mock => mock.Update(
                It.IsAny<ConstructionBolt>()), Times.Never);
        }

        [Fact]
        public void Delete_ShouldDeleteConstructionBolt()
        {
            // Arrange
            int id = _rnd.Next(1, _constructionBolts.Count());

            // Act
            _service.Delete(id);

            // Assert
            _repository.Verify(mock => mock.Delete(
                It.IsAny<ConstructionBolt>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull_WhenWrongId()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => _service.Delete(999));

            _repository.Verify(mock => mock.Delete(
                It.IsAny<ConstructionBolt>()), Times.Never);
        }
    }
}
