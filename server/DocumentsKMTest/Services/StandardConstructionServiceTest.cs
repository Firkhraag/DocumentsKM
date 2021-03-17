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
    public class StandardConstructionServiceTest
    {
        private readonly Mock<IStandardConstructionRepo> _repository = new Mock<IStandardConstructionRepo>();
        private readonly Mock<IStandardConstructionRepo> _updateRepository = new Mock<IStandardConstructionRepo>();
        private readonly Mock<IMarkRepo> _mockMarkRepo = new Mock<IMarkRepo>();
        private readonly Mock<ISpecificationRepo> _mockSpecificationRepo = new Mock<ISpecificationRepo>();
        private readonly IStandardConstructionService _service;
        private readonly IStandardConstructionService _updateService;
        private readonly Random _rnd = new Random();

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
        private readonly int _maxSpecificationId = 3;

        public StandardConstructionServiceTest()
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
                new StandardConstruction
                {
                    Id = 4,
                    Specification = _specifications[0],
                    Name = "N4",
                    Num = 4,
                    Sheet = "S4",
                    Weight = 4.0f,
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

            foreach (var standardConstruction in _standardConstructions)
            {
                _repository.Setup(mock =>
                    mock.GetById(standardConstruction.Id)).Returns(
                        _standardConstructions.SingleOrDefault(v => v.Id == standardConstruction.Id));
                _updateRepository.Setup(mock =>
                    mock.GetById(standardConstruction.Id)).Returns(
                        _updateStandardConstructions.SingleOrDefault(v => v.Id == standardConstruction.Id));
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
                        _standardConstructions.Where(v => v.Specification.Id == specification.Id));
                _updateRepository.Setup(mock =>
                    mock.GetAllBySpecificationId(specification.Id)).Returns(
                        _updateStandardConstructions.Where(v => v.Specification.Id == specification.Id));

                foreach (var standardConstruction in _standardConstructions)
                {
                    _repository.Setup(mock =>
                        mock.GetByUniqueKey(
                            specification.Id, standardConstruction.Name)).Returns(
                                _standardConstructions.SingleOrDefault(
                                    v => v.Specification.Id == specification.Id &&
                                        v.Name == standardConstruction.Name));
                }
            }
            foreach (var mark in _marks)
            {
                _mockMarkRepo.Setup(mock =>
                    mock.GetById(mark.Id)).Returns(
                        _marks.SingleOrDefault(v => v.Id == mark.Id));
            }

            _repository.Setup(mock =>
                mock.Add(It.IsAny<StandardConstruction>())).Verifiable();
            _updateRepository.Setup(mock =>
                mock.Update(It.IsAny<StandardConstruction>())).Verifiable();
            _repository.Setup(mock =>
                mock.Delete(It.IsAny<StandardConstruction>())).Verifiable();

            _service = new StandardConstructionService(
                _repository.Object,
                _mockMarkRepo.Object,
                _mockSpecificationRepo.Object);
            _updateService = new StandardConstructionService(
                _updateRepository.Object,
                _mockMarkRepo.Object,
                _mockSpecificationRepo.Object);
        }

        [Fact]
        public void GetAllBySpecificationId_ShouldReturnStandardConstructions()
        {
            // Arrange
            int specificationId = _rnd.Next(1, _maxSpecificationId);

            // Act
            var returnedStandardConstructions = _service.GetAllBySpecificationId(
                specificationId);

            // Assert
            Assert.Equal(_standardConstructions.Where(
                v => v.Specification.Id == specificationId), returnedStandardConstructions);
        }

        [Fact]
        public void GetAllBySpecificationId_ShouldReturnEmptyArray_WhenWrongSpecificationId()
        {
            // Act
            var returnedstandardConstructions = _service.GetAllBySpecificationId(999);

            // Assert
            Assert.Empty(returnedstandardConstructions);
        }

        [Fact]
        public void Create_ShouldCreatestandardConstruction()
        {
            // Arrange
            int specificationId = _rnd.Next(1, _specifications.Count());

            var newStandardConstruction = new StandardConstruction
            {
                Name = "NewCreate",
                Num = 9,
                Sheet = "NewCreate",
                Weight = 9.0f,
            };

            // Act
            _service.Create(
                newStandardConstruction,
                specificationId);

            // Assert
            _repository.Verify(mock => mock.Add(It.IsAny<StandardConstruction>()), Times.Once);
            Assert.NotNull(newStandardConstruction.Specification);
        }

        [Fact]
        public void Create_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int specificationId = _rnd.Next(1, _specifications.Count());

            var newStandardConstruction = new StandardConstruction
            {
                Name = "NewCreate",
                Num = 9,
                Sheet = "NewCreate",
                Weight = 9.0f,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => _service.Create(
                    null, specificationId));
            Assert.Throws<ArgumentNullException>(
                () => _service.Create(
                    newStandardConstruction, 999));

            _repository.Verify(mock => mock.Add(It.IsAny<StandardConstruction>()), Times.Never);
        }

        [Fact]
        public void Create_ShouldFailWithConflict_WhenConflictValue()
        {
            // Arrange
            var specificationId = _standardConstructions[0].Specification.Id;
            var name = _standardConstructions[0].Name;

            var newStandardConstruction = new StandardConstruction
            {
                Name = name,
                Num = 9,
                Sheet = "NewCreate",
                Weight = 9.0f,
            };

            // Act & Assert
            Assert.Throws<ConflictException>(
                () => _service.Create(
                    newStandardConstruction,
                    specificationId));

            _repository.Verify(mock => mock.Add(It.IsAny<StandardConstruction>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldUpdateStandardConstruction()
        {
            // Arrange
            var id = _rnd.Next(1, _updateStandardConstructions.Count());
            var newStringValue = "NewUpdate";
            short newIntValue = 99;
            var newFloatValue = 9.0f;

            var newStandardConstructionRequest = new StandardConstructionUpdateRequest
            {
                Name = newStringValue,
                Num = newIntValue,
                Sheet = newStringValue,
                Weight = newFloatValue,
            };

            // Act
            _updateService.Update(id, newStandardConstructionRequest);

            // Assert
            _updateRepository.Verify(mock => mock.Update(It.IsAny<StandardConstruction>()), Times.Once);
            Assert.Equal(
                newStringValue, _updateStandardConstructions.SingleOrDefault(v => v.Id == id).Name);
            Assert.Equal(
                newIntValue, _updateStandardConstructions.SingleOrDefault(v => v.Id == id).Num);
            Assert.Equal(
                newStringValue, _updateStandardConstructions.SingleOrDefault(v => v.Id == id).Sheet);
            Assert.Equal(
                newFloatValue, _updateStandardConstructions.SingleOrDefault(v => v.Id == id).Weight);
        }

        [Fact]
        public void Update_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int id = _rnd.Next(1, _updateStandardConstructions.Count());

            var newStandardConstructionRequest = new StandardConstructionUpdateRequest
            {
                Sheet = "NewUpdate",
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _updateService.Update(id, null));
            Assert.Throws<ArgumentNullException>(() => _updateService.Update(
                999, newStandardConstructionRequest));

            _updateRepository.Verify(mock => mock.Update(It.IsAny<StandardConstruction>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldFailWithConflict()
        {
            // Arrange
            var id = 4;
            var conflictName = _standardConstructions[0].Name;

            var newStandardConstructionRequest = new StandardConstructionUpdateRequest
            {
                Name = conflictName,
            };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Update(id, newStandardConstructionRequest));

            _repository.Verify(mock => mock.Update(It.IsAny<StandardConstruction>()), Times.Never);
        }

        [Fact]
        public void Delete_ShouldDeleteStandardConstruction()
        {
            // Arrange
            int id = _rnd.Next(1, _standardConstructions.Count());

            // Act
            _service.Delete(id);

            // Assert
            _repository.Verify(mock => mock.Delete(
                It.IsAny<StandardConstruction>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull_WhenWrongId()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(999));

            _repository.Verify(mock => mock.Delete(
                It.IsAny<StandardConstruction>()), Times.Never);
        }
    }
}
