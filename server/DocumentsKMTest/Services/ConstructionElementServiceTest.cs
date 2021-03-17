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
    public class ConstructionElementServiceTest
    {
        private readonly Mock<IConstructionElementRepo> _repository = new Mock<IConstructionElementRepo>();
        private readonly Mock<IConstructionElementRepo> _updateRepository = new Mock<IConstructionElementRepo>();
        private readonly Mock<IMarkRepo> _mockMarkRepo = new Mock<IMarkRepo>();
        private readonly Mock<IConstructionRepo> _mockConstructionRepo = new Mock<IConstructionRepo>();
        private readonly Mock<IProfileRepo> _mockProfileRepo = new Mock<IProfileRepo>();
        private readonly Mock<ISteelRepo> _mockSteelRepo = new Mock<ISteelRepo>();
        private readonly IConstructionElementService _service;
        private readonly IConstructionElementService _updateService;
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
        private readonly List<ProfileClass> _profileClasses;
        private readonly List<ProfileType> _profileTypes;
        private readonly List<Profile> _profiles;
        private readonly List<Steel> _steel;
        private readonly List<ConstructionElement> _constructionElements;

        private readonly List<ConstructionElement> _updateConstructionElements = new List<ConstructionElement> {};
        private readonly int _maxConstructionId = 3;

        public ConstructionElementServiceTest()
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

            foreach (var constructionElement in _constructionElements)
            {
                _repository.Setup(mock =>
                    mock.GetById(constructionElement.Id)).Returns(
                        _constructionElements.SingleOrDefault(v => v.Id == constructionElement.Id));
                _updateRepository.Setup(mock =>
                    mock.GetById(constructionElement.Id)).Returns(
                        _updateConstructionElements.SingleOrDefault(v => v.Id == constructionElement.Id));
            }
            foreach (var construction in _constructions)
            {
                _mockConstructionRepo.Setup(mock =>
                    mock.GetById(construction.Id, false)).Returns(
                        _constructions.SingleOrDefault(v => v.Id == construction.Id));
                _mockConstructionRepo.Setup(mock =>
                    mock.GetById(construction.Id, true)).Returns(
                        _constructions.SingleOrDefault(v => v.Id == construction.Id));

                _repository.Setup(mock =>
                    mock.GetAllByConstructionId(construction.Id)).Returns(
                        _constructionElements.Where(v => v.Construction.Id == construction.Id));
                _updateRepository.Setup(mock =>
                    mock.GetAllByConstructionId(construction.Id)).Returns(
                        _updateConstructionElements.Where(v => v.Construction.Id == construction.Id));

                // foreach (var ConstructionElement in _constructionElements)
                // {
                //     _repository.Setup(mock =>
                //         mock.GetByUniqueKey(mark.Id, ConstructionElement.Designation)).Returns(
                //             _constructionElements.SingleOrDefault(
                //                 v => v.Mark.Id == mark.Id && v.Designation == ConstructionElement.Designation));
                // }
            }
            foreach (var profile in _profiles)
            {
                _mockProfileRepo.Setup(mock =>
                    mock.GetById(profile.Id)).Returns(
                        _profiles.SingleOrDefault(
                            v => v.Id == profile.Id));
            }
            foreach (var steel in _steel)
            {
                _mockSteelRepo.Setup(mock =>
                    mock.GetById(steel.Id)).Returns(
                        _steel.SingleOrDefault(
                            v => v.Id == steel.Id));
            }
            foreach (var mark in _marks)
            {
                _mockMarkRepo.Setup(mock =>
                    mock.GetById(mark.Id)).Returns(
                        _marks.SingleOrDefault(v => v.Id == mark.Id));
            }

            _repository.Setup(mock =>
                mock.Add(It.IsAny<ConstructionElement>())).Verifiable();
            _updateRepository.Setup(mock =>
                mock.Update(It.IsAny<ConstructionElement>())).Verifiable();
            _repository.Setup(mock =>
                mock.Delete(It.IsAny<ConstructionElement>())).Verifiable();

            _service = new ConstructionElementService(
                _repository.Object,
                _mockMarkRepo.Object,
                _mockConstructionRepo.Object,
                _mockProfileRepo.Object,
                _mockSteelRepo.Object);
            _updateService = new ConstructionElementService(
                _updateRepository.Object,
                _mockMarkRepo.Object,
                _mockConstructionRepo.Object,
                _mockProfileRepo.Object,
                _mockSteelRepo.Object);
        }

        [Fact]
        public void GetAllByConstructionId_ShouldReturnConstructionElements()
        {
            // Arrange
            int constructionId = _rnd.Next(1, _maxConstructionId);

            // Act
            var returnedConstructionElements = _service.GetAllByConstructionId(
                constructionId);

            // Assert
            Assert.Equal(_constructionElements.Where(
                v => v.Construction.Id == constructionId), returnedConstructionElements);
        }

        [Fact]
        public void GetAllByConstructionId_ShouldReturnEmptyArray_WhenWrongConstructionId()
        {
            // Act
            var returnedConstructionElements = _service.GetAllByConstructionId(999);

            // Assert
            Assert.Empty(returnedConstructionElements);
        }

        [Fact]
        public void Create_ShouldCreateConstructionElement()
        {
            // Arrange
            int constructionId = _rnd.Next(1, _marks.Count());
            int profileId = _rnd.Next(1, _profiles.Count());
            int steelId = _rnd.Next(1, _steel.Count());

            var newConstructionElement = new ConstructionElement
            {
                Length = 1.0f,
            };

            // Act
            _service.Create(
                newConstructionElement,
                constructionId,
                profileId,
                steelId);

            // Assert
            _repository.Verify(mock => mock.Add(
                It.IsAny<ConstructionElement>()), Times.Once);
            Assert.NotNull(newConstructionElement.Construction);
        }

        [Fact]
        public void Create_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int constructionId = _rnd.Next(1, _marks.Count());
            int profileId = _rnd.Next(1, _profiles.Count());
            int steelId = _rnd.Next(1, _steel.Count());

            var newConstructionElement = new ConstructionElement
            {
                Length = 1.0f,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                null,
                constructionId,
                profileId,
                steelId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newConstructionElement,
                999,
                profileId,
                steelId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newConstructionElement,
                constructionId,
                999,
                steelId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newConstructionElement,
                constructionId,
                profileId,
                999));

            _repository.Verify(mock => mock.Add(It.IsAny<ConstructionElement>()), Times.Never);
        }

        // [Fact]
        // public void Create_ShouldFailWithConflict()
        // {
        //     // Arrange
        //     var conflictConstructionId = _constructionElements[0].Mark.Id;
        //     var conflictDesignation = _constructionElements[0].Designation;

        //     var newConstructionElement = new ConstructionElement
        //     {
        //         Designation = conflictDesignation,
        //         Name = "NewCreate",
        //     };

        //     // Act & Assert
        //     Assert.Throws<ConflictException>(() => _service.Create(newConstructionElement, conflictConstructionId));

        //     _repository.Verify(mock => mock.Add(It.IsAny<ConstructionElement>()), Times.Never);
        // }

        [Fact]
        public void Update_ShouldUpdateConstructionElement()
        {
            // Arrange
            int id = _rnd.Next(1, _updateConstructionElements.Count());
            int profileId = _rnd.Next(1, _profiles.Count());
            short steelId = (Int16)(_rnd.Next(1, _steel.Count()));
            var newFloatValue = 9.0f;

            var newConstructionElementRequest = new ConstructionElementUpdateRequest
            {
                ProfileId = profileId,
                SteelId = steelId,
                Length = newFloatValue,
            };

            // Act
            _updateService.Update(id, newConstructionElementRequest);

            // Assert
            _updateRepository.Verify(mock => mock.Update(It.IsAny<ConstructionElement>()), Times.Once);
            Assert.Equal(_profiles.SingleOrDefault(v => v.Id == profileId), _updateConstructionElements.SingleOrDefault(
                v => v.Id == id).Profile);
            Assert.Equal(_steel.SingleOrDefault(v => v.Id == steelId), _updateConstructionElements.SingleOrDefault(
                v => v.Id == id).Steel);
            Assert.Equal(newFloatValue, _updateConstructionElements.SingleOrDefault(
                v => v.Id == id).Length);
        }

        [Fact]
        public void Update_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int id = _rnd.Next(1, _updateConstructionElements.Count());

            var newConstructionElementRequest = new ConstructionElementUpdateRequest
            {
                Length = 9,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _updateService.Update(id, null));
            Assert.Throws<ArgumentNullException>(() => _updateService.Update(
                999, newConstructionElementRequest));

            _updateRepository.Verify(mock => mock.Update(
                It.IsAny<ConstructionElement>()), Times.Never);
        }

        // [Fact]
        // public void Update_ShouldFailWithConflict()
        // {
        //     // Arrange
        //     var conflictDesignation = _constructionElements[0].Designation;
        //     var id = _constructionElements[3].Id;

        //     var newConstructionElementRequest = new ConstructionElementUpdateRequest
        //     {
        //         Designation = conflictDesignation,
        //         Name = "NewUpdate",
        //     };

        //     // Act & Assert
        //     Assert.Throws<ConflictException>(() => _service.Update(id, newConstructionElementRequest));

        //     _repository.Verify(mock => mock.Update(
        //         It.IsAny<ConstructionElement>()), Times.Never);
        // }

        [Fact]
        public void Delete_ShouldDeleteConstructionElement()
        {
            // Arrange
            int id = _rnd.Next(1, _constructionElements.Count());

            // Act
            _service.Delete(id);

            // Assert
            _repository.Verify(mock => mock.Delete(
                It.IsAny<ConstructionElement>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull_WhenWrongId()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => _service.Delete(999));

            _repository.Verify(mock => mock.Delete(
                It.IsAny<ConstructionElement>()), Times.Never);
        }
    }
}
