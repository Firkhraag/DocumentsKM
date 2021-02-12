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
    public class ConstructionServiceTest
    {
        private readonly Mock<IConstructionRepo> _repository = new Mock<IConstructionRepo>();
        private readonly Mock<IConstructionRepo> _updateRepository = new Mock<IConstructionRepo>();
        private readonly Mock<IMarkRepo> _mockMarkRepo = new Mock<IMarkRepo>();
        private readonly Mock<ISpecificationRepo> _mockSpecificationRepo = new Mock<ISpecificationRepo>();
        private readonly Mock<IConstructionTypeRepo> _mockConstructionTypeRepo = new Mock<IConstructionTypeRepo>();
        private readonly Mock<IConstructionSubtypeRepo> _mockConstructionSubtypeRepo = new Mock<IConstructionSubtypeRepo>();
        private readonly Mock<IWeldingControlRepo> _mockWeldingControlRepo = new Mock<IWeldingControlRepo>();
        private readonly IConstructionService _service;
        private readonly IConstructionService _updateService;
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

        private readonly List<Construction> _updateConstructions = new List<Construction> {};
        private readonly int _maxSpecificationId = 3;

        public ConstructionServiceTest()
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

            // foreach (var c in _constructions)
            // {
            //     var spec = c.Specification;
            //     _constructions.Add(new Construction
            //     {
            //         Id = c.Id,
            //         Specification = new Specification
            //         {
            //             Id = spec.Id,
            //             Mark = spec.Mark,
            //             IsCurrent = spec.IsCurrent,
            //             Note = spec.Note,
            //         },
            //         Name = c.Name,
            //         Type = c.Type,
            //         Subtype = c.Subtype,
            //         Valuation = c.Valuation,
            //         NumOfStandardConstructions = c.NumOfStandardConstructions,
            //         HasEdgeBlunting = c.HasEdgeBlunting,
            //         HasDynamicLoad = c.HasDynamicLoad,
            //         HasFlangedConnections = c.HasFlangedConnections,
            //         WeldingControl = c.WeldingControl,
            //         PaintworkCoeff = c.PaintworkCoeff,
            //     });
            // }
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
            foreach (var construction in _constructions)
            {
                _repository.Setup(mock =>
                    mock.GetById(construction.Id, false)).Returns(
                        _constructions.SingleOrDefault(v => v.Id == construction.Id));
                _repository.Setup(mock =>
                    mock.GetById(construction.Id, true)).Returns(
                        _constructions.SingleOrDefault(v => v.Id == construction.Id));

                _updateRepository.Setup(mock =>
                    mock.GetById(construction.Id, false)).Returns(
                        _updateConstructions.SingleOrDefault(v => v.Id == construction.Id));
                _updateRepository.Setup(mock =>
                    mock.GetById(construction.Id, true)).Returns(
                        _updateConstructions.SingleOrDefault(v => v.Id == construction.Id));
            }
            foreach (var specification in _specifications)
            {
                _mockSpecificationRepo.Setup(mock =>
                    mock.GetById(specification.Id, false)).Returns(
                        _specifications.SingleOrDefault(v => v.Id == specification.Id));
                _mockSpecificationRepo.Setup(mock =>
                    mock.GetById(specification.Id, true)).Returns(
                        _specifications.SingleOrDefault(v => v.Id == specification.Id));

                _repository.Setup(mock =>
                    mock.GetAllBySpecificationId(specification.Id)).Returns(
                        _constructions.Where(v => v.Specification.Id == specification.Id));
                _updateRepository.Setup(mock =>
                    mock.GetAllBySpecificationId(specification.Id)).Returns(
                        _updateConstructions.Where(v => v.Specification.Id == specification.Id));

                foreach (var construction in _constructions)
                {
                    _repository.Setup(mock =>
                        mock.GetByUniqueKey(
                            specification.Id, construction.Name, construction.PaintworkCoeff)).Returns(
                                _constructions.SingleOrDefault(
                                    v => v.Specification.Id == specification.Id &&
                                        v.Name == construction.Name &&
                                            v.PaintworkCoeff == construction.PaintworkCoeff));

                    _updateRepository.Setup(mock =>
                        mock.GetByUniqueKey(
                            specification.Id, construction.Name, construction.PaintworkCoeff)).Returns(
                                _updateConstructions.SingleOrDefault(
                                    v => v.Specification.Id == specification.Id &&
                                        v.Name == construction.Name &&
                                            v.PaintworkCoeff == construction.PaintworkCoeff));
                }
            }
            foreach (var type in _constructionTypes)
            {
                _mockConstructionTypeRepo.Setup(mock =>
                    mock.GetById(type.Id)).Returns(
                        _constructionTypes.SingleOrDefault(v => v.Id == type.Id));
            }
            foreach (var subtype in _constructionSubtypes)
            {
                _mockConstructionSubtypeRepo.Setup(mock =>
                    mock.GetById(subtype.Id)).Returns(
                        _constructionSubtypes.SingleOrDefault(v => v.Id == subtype.Id));
            }
            foreach (var weldingControl in _weldingControl)
            {
                _mockWeldingControlRepo.Setup(mock =>
                    mock.GetById(weldingControl.Id)).Returns(
                        _weldingControl.SingleOrDefault(v => v.Id == weldingControl.Id));
            }
            foreach (var mark in _marks)
            {
                _mockMarkRepo.Setup(mock =>
                    mock.GetById(mark.Id)).Returns(
                        _marks.SingleOrDefault(v => v.Id == mark.Id));
            }

            _repository.Setup(mock =>
                mock.Add(It.IsAny<Construction>())).Verifiable();
            _updateRepository.Setup(mock =>
                mock.Update(It.IsAny<Construction>())).Verifiable();
            _repository.Setup(mock =>
                mock.Delete(It.IsAny<Construction>())).Verifiable();

            _service = new ConstructionService(
                _repository.Object,
                _mockMarkRepo.Object,
                _mockSpecificationRepo.Object,
                _mockConstructionTypeRepo.Object,
                _mockConstructionSubtypeRepo.Object,
                _mockWeldingControlRepo.Object);
            _updateService = new ConstructionService(
                _updateRepository.Object,
                _mockMarkRepo.Object,
                _mockSpecificationRepo.Object,
                _mockConstructionTypeRepo.Object,
                _mockConstructionSubtypeRepo.Object,
                _mockWeldingControlRepo.Object);
        }

        [Fact]
        public void GetAllBySpecificationId_ShouldReturnConstructions()
        {
            // Arrange
            int specificationId = _rnd.Next(1, _maxSpecificationId);

            // Act
            var returnedConstructions = _service.GetAllBySpecificationId(
                specificationId);

            // Assert
            Assert.Equal(_constructions.Where(
                v => v.Specification.Id == specificationId), returnedConstructions);
        }

        [Fact]
        public void GetAllBySpecificationId_ShouldReturnEmptyArray_WhenWrongSpecificationId()
        {
            // Act
            var returnedConstructions = _service.GetAllBySpecificationId(999);

            // Assert
            Assert.Empty(returnedConstructions);
        }

        [Fact]
        public void Create_ShouldCreateConstruction()
        {
            // Arrange
            int specificationId = _rnd.Next(1, _specifications.Count());
            int typeId = _rnd.Next(1, _constructionTypes.Count());
            int subtypeId = _rnd.Next(1, _constructionSubtypes.Count());
            int weldingControlId = _rnd.Next(1, _weldingControl.Count());

            var newConstruction = new Construction
            {
                Name = "NewCreate",
                Valuation = "NewCreate",
                NumOfStandardConstructions = 0,
                HasEdgeBlunting = true,
                HasDynamicLoad = false,
                HasFlangedConnections = true,
                PaintworkCoeff = 2,
            };

            // Act
            _service.Create(
                newConstruction,
                specificationId,
                typeId,
                subtypeId,
                weldingControlId);

            // Assert
            _repository.Verify(mock => mock.Add(It.IsAny<Construction>()), Times.Once);
            Assert.NotNull(newConstruction.Specification);
        }

        [Fact]
        public void Create_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int specificationId = _rnd.Next(1, _specifications.Count());
            int typeId = _rnd.Next(1, _constructionTypes.Count());
            int subtypeId = _rnd.Next(1, _constructionSubtypes.Count());
            int weldingControlId = _rnd.Next(1, _weldingControl.Count());

            var newConstruction = new Construction
            {
                Name = "NewCreate",
                Valuation = "NewCreate",
                NumOfStandardConstructions = 0,
                HasEdgeBlunting = true,
                HasDynamicLoad = false,
                HasFlangedConnections = true,
                PaintworkCoeff = 2,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => _service.Create(
                    null, specificationId, typeId, subtypeId, weldingControlId));
            Assert.Throws<ArgumentNullException>(
                () => _service.Create(
                    newConstruction, 999, typeId, subtypeId, weldingControlId));
            Assert.Throws<ArgumentNullException>(
                () => _service.Create(
                    newConstruction, specificationId, 999, subtypeId, weldingControlId));
            Assert.Throws<ArgumentNullException>(
                () => _service.Create(
                    newConstruction, specificationId, typeId, 999, weldingControlId));
            Assert.Throws<ArgumentNullException>(
                () => _service.Create(
                    newConstruction, specificationId, typeId, subtypeId, 999));

            _repository.Verify(mock => mock.Add(It.IsAny<Construction>()), Times.Never);
        }

        [Fact]
        public void Create_ShouldFailWithConflict_WhenConflictValue()
        {
            // Arrange
            int typeId = _rnd.Next(1, _constructionTypes.Count());
            int subtypeId = _rnd.Next(1, _constructionSubtypes.Count());
            int weldingControlId = _rnd.Next(1, _weldingControl.Count());

            var conflictSpecificationId = _constructions[0].Specification.Id;
            var conflictName = _constructions[0].Name;
            var conflictPaintworkCoeff = _constructions[0].PaintworkCoeff;

            var newConstruction = new Construction
            {
                Name = conflictName,
                Valuation = "NewCreate",
                NumOfStandardConstructions = 0,
                HasEdgeBlunting = true,
                HasDynamicLoad = false,
                HasFlangedConnections = true,
                PaintworkCoeff = conflictPaintworkCoeff,
            };

            // Act & Assert
            Assert.Throws<ConflictException>(
                () => _service.Create(
                    newConstruction,
                    conflictSpecificationId,
                    typeId,
                    subtypeId,
                    weldingControlId));

            _repository.Verify(mock => mock.Add(It.IsAny<Construction>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldUpdateConstruction()
        {
            // Arrange
            int id = _rnd.Next(1, _updateConstructions.Count());
            var newStringValue = "NewUpdate";
            var newBoolValue = true;
            var newIntValue = 99;

            var newConstructionRequest = new ConstructionUpdateRequest
            {
                Name = newStringValue,
                Valuation = newStringValue,
                NumOfStandardConstructions = newIntValue,
                HasEdgeBlunting = newBoolValue,
                HasDynamicLoad = newBoolValue,
                HasFlangedConnections = newBoolValue,
                PaintworkCoeff = newIntValue,
            };

            // Act
            _updateService.Update(id, newConstructionRequest);

            // Assert
            _updateRepository.Verify(mock => mock.Update(It.IsAny<Construction>()), Times.Once);
            Assert.Equal(
                newStringValue, _updateConstructions.SingleOrDefault(v => v.Id == id).Name);
            Assert.Equal(
                newStringValue, _updateConstructions.SingleOrDefault(v => v.Id == id).Valuation);
            Assert.Equal(
                newIntValue, _updateConstructions.SingleOrDefault(v => v.Id == id).NumOfStandardConstructions);
            Assert.Equal(
                newBoolValue, _updateConstructions.SingleOrDefault(v => v.Id == id).HasEdgeBlunting);
            Assert.Equal(
                newBoolValue, _updateConstructions.SingleOrDefault(v => v.Id == id).HasDynamicLoad);
            Assert.Equal(
                newBoolValue, _updateConstructions.SingleOrDefault(v => v.Id == id).HasFlangedConnections);
            Assert.Equal(
                newIntValue, _updateConstructions.SingleOrDefault(v => v.Id == id).PaintworkCoeff);
        }

        [Fact]
        public void Update_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int id = _rnd.Next(1, _updateConstructions.Count());

            var newConstructionRequest = new ConstructionUpdateRequest
            {
                Valuation = "NewUpdate",
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _updateService.Update(id, null));
            Assert.Throws<ArgumentNullException>(() => _updateService.Update(
                999, newConstructionRequest));

            _updateRepository.Verify(mock => mock.Update(It.IsAny<Construction>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldFailWithConflict()
        {
            // Arrange
            var id = _updateConstructions[1].Id;
            var conflictName = _updateConstructions[0].Name;
            var conflictPaintworkCoeff = _updateConstructions[0].PaintworkCoeff;

            var newConstructionRequest = new ConstructionUpdateRequest
            {
                Name = conflictName,
                PaintworkCoeff = conflictPaintworkCoeff,
                Valuation = "NewUpdate",
            };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _updateService.Update(id, newConstructionRequest));

            _updateRepository.Verify(mock => mock.Update(It.IsAny<Construction>()), Times.Never);
        }

        [Fact]
        public void Delete_ShouldDeleteConstruction()
        {
            // Arrange
            int id = _rnd.Next(1, _constructions.Count());

            // Act
            _service.Delete(id);

            // Assert
            _repository.Verify(mock => mock.Delete(
                It.IsAny<Construction>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull_WhenWrongId()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(999));

            _repository.Verify(mock => mock.Delete(
                It.IsAny<Construction>()), Times.Never);
        }
    }
}
