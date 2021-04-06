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
    public class MarkGeneralDataPointServiceTest
    {
        private readonly Mock<IMarkGeneralDataPointRepo> _repository = new Mock<IMarkGeneralDataPointRepo>();
        private readonly Mock<IMarkGeneralDataPointRepo> _updateRepository = new Mock<IMarkGeneralDataPointRepo>();
        private readonly IMarkGeneralDataPointService _service;
        private readonly IMarkGeneralDataPointService _updateService;
        private readonly Random _rnd = new Random();

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

        public MarkGeneralDataPointServiceTest()
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
                },
                new MarkGeneralDataSection
                {
                    Id = 2,
                    Name = "S2",
                    Mark = _marks[1],
                },
                new MarkGeneralDataSection
                {
                    Id = 3,
                    Name = "S3",
                    Mark = _marks[2],
                },
            };

            _markGeneralDataPoints = new List<MarkGeneralDataPoint>
            {
                new MarkGeneralDataPoint
                {
                    Id = 1,
                    Section = _markGeneralDataSections[0],
                    Text = "mgdp1",
                    OrderNum = 1,
                },
                new MarkGeneralDataPoint
                {
                    Id = 2,
                    Section = _markGeneralDataSections[1],
                    Text = "mgdp2",
                    OrderNum = 2,
                },
                new MarkGeneralDataPoint
                {
                    Id = 3,
                    Section = _markGeneralDataSections[0],
                    Text = "mgdp3",
                    OrderNum = 1,
                },
                new MarkGeneralDataPoint
                {
                    Id = 4,
                    Section = _markGeneralDataSections[1],
                    Text = "mgdp4",
                    OrderNum = 2,
                },
                new MarkGeneralDataPoint
                {
                    Id = 5,
                    Section = _markGeneralDataSections[0],
                    Text = "mgdp5",
                    OrderNum = 1,
                },
                new MarkGeneralDataPoint
                {
                    Id = 6,
                    Section = _markGeneralDataSections[1],
                    Text = "mgdp6",
                    OrderNum = 2,
                },
                new MarkGeneralDataPoint
                {
                    Id = 7,
                    Section = _markGeneralDataSections[0],
                    Text = "mgdp7",
                    OrderNum = 2,
                },
            };

            var mockMarkRepo = new Mock<IMarkRepo>();
            var mockMarkGeneralDataSectionRepo = new Mock<IMarkGeneralDataSectionRepo>();
            var mockGeneralDataPointRepo = new Mock<IGeneralDataPointRepo>();
            var mockGeneralDataSectionRepo = new Mock<IGeneralDataSectionRepo>();

            foreach (var mgdp in TestData.markGeneralDataPoints)
            {
                _updateMarkGeneralDataPoints.Add(new MarkGeneralDataPoint
                {
                    Id = mgdp.Id,
                    Section = mgdp.Section,
                    Text = mgdp.Text,
                    OrderNum = mgdp.OrderNum,
                });
            }

            foreach (var markGeneralDataPoint in _markGeneralDataPoints)
            {
                _repository.Setup(mock =>
                    mock.GetById(markGeneralDataPoint.Id)).Returns(
                        _markGeneralDataPoints.SingleOrDefault(v => v.Id == markGeneralDataPoint.Id));

                _updateRepository.Setup(mock =>
                    mock.GetById(markGeneralDataPoint.Id)).Returns(
                        _updateMarkGeneralDataPoints.SingleOrDefault(v => v.Id == markGeneralDataPoint.Id));
            }
            foreach (var mark in TestData.marks)
            {
                mockMarkRepo.Setup(mock =>
                    mock.GetById(mark.Id)).Returns(
                        TestData.marks.SingleOrDefault(v => v.Id == mark.Id));

                _repository.Setup(mock =>
                    mock.GetAllByMarkId(mark.Id)).Returns(
                        _markGeneralDataPoints.Where(v => v.Section.Mark.Id == mark.Id));

                _updateRepository.Setup(mock =>
                    mock.GetAllByMarkId(mark.Id)).Returns(
                        _updateMarkGeneralDataPoints.Where(v => v.Section.Mark.Id == mark.Id));
            }
            foreach (var markGeneralDataSection in TestData.markGeneralDataSections)
            {
                mockMarkGeneralDataSectionRepo.Setup(mock =>
                    mock.GetById(markGeneralDataSection.Id, true)).Returns(
                        TestData.markGeneralDataSections.SingleOrDefault(
                            v => v.Id == markGeneralDataSection.Id));
                mockMarkGeneralDataSectionRepo.Setup(mock =>
                    mock.GetById(markGeneralDataSection.Id, false)).Returns(
                        TestData.markGeneralDataSections.SingleOrDefault(
                            v => v.Id == markGeneralDataSection.Id));

                _repository.Setup(mock =>
                    mock.GetAllBySectionId(markGeneralDataSection.Id)).Returns(
                        _markGeneralDataPoints.Where(
                            v => v.Section.Id == markGeneralDataSection.Id));

                _updateRepository.Setup(mock =>
                    mock.GetAllBySectionId(markGeneralDataSection.Id)).Returns(
                        _updateMarkGeneralDataPoints.Where(
                            v => v.Section.Id == markGeneralDataSection.Id));

                foreach (var markGeneralDataPoint in _markGeneralDataPoints)
                {
                    _repository.Setup(mock =>
                        mock.GetByUniqueKey(
                            markGeneralDataSection.Id, markGeneralDataPoint.Text)).Returns(
                                _markGeneralDataPoints.SingleOrDefault(
                                    v => v.Section.Id == markGeneralDataSection.Id &&
                                        v.Text == markGeneralDataPoint.Text));

                    _updateRepository.Setup(mock =>
                        mock.GetByUniqueKey(
                            markGeneralDataSection.Id, markGeneralDataPoint.Text)).Returns(
                                _updateMarkGeneralDataPoints.SingleOrDefault(
                                    v => v.Section.Id == markGeneralDataSection.Id &&
                                        v.Text == markGeneralDataPoint.Text));
                }
            }
            foreach (var generalDataPoint in TestData.generalDataPoints)
            {
                mockGeneralDataPointRepo.Setup(mock =>
                    mock.GetById(generalDataPoint.Id)).Returns(
                        TestData.generalDataPoints.SingleOrDefault(
                            v => v.Id == generalDataPoint.Id));
            }

            _repository.Setup(mock =>
                mock.Add(It.IsAny<MarkGeneralDataPoint>())).Verifiable();
            _updateRepository.Setup(mock =>
                mock.Update(It.IsAny<MarkGeneralDataPoint>())).Verifiable();
            _repository.Setup(mock =>
                mock.Delete(It.IsAny<MarkGeneralDataPoint>())).Verifiable();

            _service = new MarkGeneralDataPointService(
                _repository.Object,
                mockMarkRepo.Object,
                mockMarkGeneralDataSectionRepo.Object,
                mockGeneralDataPointRepo.Object,
                mockGeneralDataSectionRepo.Object);
            _updateService = new MarkGeneralDataPointService(
                _updateRepository.Object,
                mockMarkRepo.Object,
                mockMarkGeneralDataSectionRepo.Object,
                mockGeneralDataPointRepo.Object,
                mockGeneralDataSectionRepo.Object);
        }

        [Fact]
        public void GetAllBySectionId_ShouldReturnMarkGeneralDataPoints()
        {
            // Arrange
            int sectionId = _rnd.Next(1, TestData.markGeneralDataSections.Count());

            // Act
            var returnedMarkGeneralDataPoints = _service.GetAllBySectionId(sectionId);

            // Assert
            Assert.Equal(_markGeneralDataPoints.Where(
                v => v.Section.Id == sectionId), returnedMarkGeneralDataPoints);
        }

        [Fact]
        public void GetAllBySectionId_ShouldReturnEmptyArray_WhenWrongValues()
        {
            // Arrange
            int sectionId = _rnd.Next(1, TestData.markGeneralDataSections.Count());

            // Act
            var returnedMarkGeneralDataPoints = _service.GetAllBySectionId(999);

            // Assert
            Assert.Empty(returnedMarkGeneralDataPoints);
        }

        [Fact]
        public void Create_ShouldCreateMarkGeneralDataPoint()
        {
            // Arrange
            int sectionId = _rnd.Next(1, TestData.markGeneralDataSections.Count());

            var newMarkGeneralDataPoint = new MarkGeneralDataPoint
            {
                Text = "NewCreate",
            };

            // Act
            _service.Create(newMarkGeneralDataPoint, sectionId);

            // Assert
            _repository.Verify(mock => mock.Add(It.IsAny<MarkGeneralDataPoint>()), Times.Once);
            Assert.NotNull(newMarkGeneralDataPoint.Section);
        }

        [Fact]
        public void Create_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int sectionId = _rnd.Next(1, TestData.markGeneralDataSections.Count());

            var newmarkGeneralDataPoint = new MarkGeneralDataPoint
            {
                Text = "NewCreate",
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(null, sectionId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(newmarkGeneralDataPoint, 999));

            _repository.Verify(mock => mock.Add(It.IsAny<MarkGeneralDataPoint>()), Times.Never);
        }

        [Fact]
        public void Create_ShouldFailWithConflict_WhenConflictValues()
        {
            // Arrange
            int conflictSectionId = _markGeneralDataPoints[0].Section.Id;

            var newMarkGeneralDataPoint = new MarkGeneralDataPoint
            {
                Text = _markGeneralDataPoints[0].Text,
            };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Create(newMarkGeneralDataPoint, conflictSectionId));

            _repository.Verify(mock => mock.Add(It.IsAny<MarkGeneralDataPoint>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldUpdatemarkGeneralDataPoint()
        {
            // Arrange
            int id = _updateMarkGeneralDataPoints[0].Id;
            int sectionId = _updateMarkGeneralDataPoints[0].Section.Id;
            var newStringValue = "NewUpdate";

            var newMarkGeneralDataPointRequest = new MarkGeneralDataPointUpdateRequest
            {
                Text = newStringValue,
            };

            // Act
            _updateService.Update(
                id, sectionId, newMarkGeneralDataPointRequest);

            // Assert
            _updateRepository.Verify(mock => mock.Update(It.IsAny<MarkGeneralDataPoint>()), Times.Once);
            var v = _updateMarkGeneralDataPoints.SingleOrDefault(v => v.Id == id);
            Assert.Equal(newStringValue, v.Text);
        }

        [Fact]
        public void Update_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int id = _updateMarkGeneralDataPoints[0].Id;
            int sectionId = _updateMarkGeneralDataPoints[0].Section.Id;
            var newStringValue = "NewUpdate";

            var newMarkGeneralDataPointRequest = new MarkGeneralDataPointUpdateRequest
            {
                Text = newStringValue,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _updateService.Update(
                id, sectionId, null));
            Assert.Throws<ArgumentNullException>(() => _updateService.Update(
                999, sectionId, newMarkGeneralDataPointRequest));
            Assert.Throws<ArgumentNullException>(() => _updateService.Update(
                id, 999, newMarkGeneralDataPointRequest));


            _updateRepository.Verify(mock => mock.Update(It.IsAny<MarkGeneralDataPoint>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldFailWithConflict_WhenConflictValues()
        {
            // Arrange
            int id = _updateMarkGeneralDataPoints[0].Id;
            int sectionId = _updateMarkGeneralDataPoints[0].Section.Id;

            var newMarkGeneralDataPointRequest = new MarkGeneralDataPointUpdateRequest
            {
                Text = _updateMarkGeneralDataPoints[6].Text,
            };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _updateService.Update(
                id, sectionId, newMarkGeneralDataPointRequest));

            _updateRepository.Verify(mock => mock.Update(It.IsAny<MarkGeneralDataPoint>()), Times.Never);
        }

        [Fact]
        public void Delete_ShouldDeletemarkGeneralDataPoint()
        {
            // Arrange
            int id = _markGeneralDataPoints[0].Id;
            int sectionId = _markGeneralDataPoints[0].Section.Id;

            // Act
            _service.Delete(id, sectionId);

            // Assert
            _repository.Verify(mock => mock.Delete(It.IsAny<MarkGeneralDataPoint>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull_WhenWrongId()
        {
            // Arrange
            int id = _markGeneralDataPoints[0].Id;
            int sectionId = _markGeneralDataPoints[0].Section.Id;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(999, sectionId));
            Assert.Throws<ArgumentNullException>(() => _service.Delete(id, 999));

            _repository.Verify(mock => mock.Delete(It.IsAny<MarkGeneralDataPoint>()), Times.Never);
        }
    }
}
