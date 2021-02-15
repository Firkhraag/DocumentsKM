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
    public class SpecificationServiceTest
    {
        private readonly Mock<ISpecificationRepo> _repository = new Mock<ISpecificationRepo>();
        private readonly Mock<ISpecificationRepo> _updateRepository = new Mock<ISpecificationRepo>();
        private readonly Mock<IMarkRepo> _mockMarkRepo = new Mock<IMarkRepo>();
        private readonly ISpecificationService _service;
        private readonly ISpecificationService _updateService;
        private readonly Random _rnd = new Random();

        private readonly List<Department> _departments;
        private readonly List<Position> _positions;
        private readonly List<Employee> _employees;
        private readonly List<Project> _projects;
        private readonly List<Node> _nodes;
        private readonly List<Subnode> _subnodes;
        private readonly List<Mark> _marks;
        private readonly List<Specification> _specifications;

        private readonly List<Specification> _updateSpecifications = new List<Specification> {};
        private readonly int _maxMarkId = 3;

        public SpecificationServiceTest()
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
            foreach (var spec in _specifications)
            {
                _updateSpecifications.Add(new Specification
                {
                    Id = spec.Id,
                    Mark = spec.Mark,
                    IsCurrent = spec.IsCurrent,
                    Note = spec.Note,
                });
            }
            foreach (var specification in _specifications)
            {
                _repository.Setup(mock =>
                    mock.GetById(specification.Id, false)).Returns(
                        _specifications.SingleOrDefault(v => v.Id == specification.Id));
                _repository.Setup(mock =>
                    mock.GetById(specification.Id, true)).Returns(
                        _specifications.SingleOrDefault(v => v.Id == specification.Id));
                    
                _updateRepository.Setup(mock =>
                    mock.GetById(specification.Id, false)).Returns(
                        _updateSpecifications.SingleOrDefault(v => v.Id == specification.Id));
                _updateRepository.Setup(mock =>
                    mock.GetById(specification.Id, true)).Returns(
                        _updateSpecifications.SingleOrDefault(v => v.Id == specification.Id));
            }
            foreach (var mark in _marks)
            {
                _mockMarkRepo.Setup(mock =>
                    mock.GetById(mark.Id)).Returns(
                        _marks.SingleOrDefault(v => v.Id == mark.Id));

                _repository.Setup(mock =>
                    mock.GetAllByMarkId(mark.Id)).Returns(
                        _specifications.Where(v => v.Mark.Id == mark.Id));
                _updateRepository.Setup(mock =>
                    mock.GetAllByMarkId(mark.Id)).Returns(
                        _updateSpecifications.Where(v => v.Mark.Id == mark.Id));
            }

            _repository.Setup(mock =>
                mock.Add(It.IsAny<Specification>())).Verifiable();
            _updateRepository.Setup(mock =>
                mock.Update(It.IsAny<Specification>())).Verifiable();
            _repository.Setup(mock =>
                mock.Delete(It.IsAny<Specification>())).Verifiable();

            _service = new SpecificationService(
                _repository.Object,
                _mockMarkRepo.Object);
            _updateService = new SpecificationService(
                _updateRepository.Object,
                _mockMarkRepo.Object);
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnSpecifications()
        {
            // Arrange
            int markId = _rnd.Next(1, _maxMarkId);

            // Act
            var returnedSpecifications = _service.GetAllByMarkId(markId);

            // Assert
            Assert.Equal(_specifications.Where(
                v => v.Mark.Id == markId), returnedSpecifications);
        }

        [Fact]
        public void Create_ShouldCreateSpecification()
        {
            // Arrange
            int markId = _rnd.Next(1, _marks.Count());

            // Act
            var specification = _service.Create(markId);

            // Assert
            _repository.Verify(
                mock => mock.Add(It.IsAny<Specification>()), Times.Once);
            Assert.NotNull(specification.Mark);

            int maxNum = 0;
            foreach (var s in _specifications.Where(v => v.Mark.Id == markId))
            {
                if (s.Num > maxNum)
                    maxNum = s.Num;
            }
            Assert.Equal(maxNum + 1, specification.Num);
        }

        [Fact]
        public void Create_ShouldFailWithNull_WhenWrongMarkId()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(999));

            _repository.Verify(
                mock => mock.Add(It.IsAny<Specification>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldUpdateSpecification()
        {
            // Arrange
            var id = 2;
            var spec = _updateSpecifications.SingleOrDefault(v => v.Id == id);
            var newNote = "NewUpdate";
            var newSpecificationRequest = new SpecificationUpdateRequest
            {
                IsCurrent = true,
                Note = newNote,
            };

            // Act
            _updateService.Update(id, newSpecificationRequest);

            // Assert
            _updateRepository.Verify(
                mock => mock.Update(It.IsAny<Specification>()), Times.Exactly(2));
            Assert.Equal(newNote, _updateSpecifications.SingleOrDefault(v => v.Id == id).Note);
            Assert.Single(_updateSpecifications.Where(
                v => v.Mark.Id == spec.Mark.Id && v.IsCurrent));
        }

        [Fact]
        public void Update_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int id = _rnd.Next(1, _updateSpecifications.Count());

            var newSpecificationRequest = new SpecificationUpdateRequest
            {
                Note = "NewUpdate",
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => _updateService.Update(id, null));
            Assert.Throws<ArgumentNullException>(
                () => _updateService.Update(999, newSpecificationRequest));

            _updateRepository.Verify(
                mock => mock.Update(It.IsAny<Specification>()), Times.Never);
        }

        [Fact]
        public void Delete_ShouldDeleteSpecification()
        {
            // Arrange
            int id = _rnd.Next(1, _specifications.Count());
            while (_specifications.SingleOrDefault(v => v.Id == id).IsCurrent)
            {
                id = _rnd.Next(1, _specifications.Count());
            }

            // Act
            _service.Delete(id);

            // Assert
            _repository.Verify(
                mock => mock.Delete(It.IsAny<Specification>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(999));

            _repository.Verify(
                mock => mock.Delete(It.IsAny<Specification>()), Times.Never);
        }

        [Fact]
        public void Delete_ShouldFailWithConflict_WhenIsCurrent()
        {
            // Arrange
            int id = _rnd.Next(1, _specifications.Count());
            while (_specifications.SingleOrDefault(v => v.Id == id).IsCurrent == false)
            {
                id = _rnd.Next(1, _specifications.Count());
            }

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Delete(id));

            _repository.Verify(
                mock => mock.Delete(It.IsAny<Specification>()), Times.Never);
        }
    }
}
