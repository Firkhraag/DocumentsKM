using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    // Entity is involved in One-To-Many relationship
    public class GeneralDataPointServiceTest
    {
        private readonly Mock<IGeneralDataPointRepo> _repository = new Mock<IGeneralDataPointRepo>();
        private readonly IGeneralDataPointService _service;
        private readonly Random _rnd = new Random();
        // private readonly List<GeneralDataPoint> _generalDataPoints = new List<GeneralDataPoint> { };

        private readonly List<Department> _departments;
        private readonly List<Position> _positions;
        private readonly List<Employee> _employees;
        private readonly List<Project> _projects;
        private readonly List<Node> _nodes;
        private readonly List<Subnode> _subnodes;
        private readonly List<Mark> _marks;
        private readonly List<GeneralDataSection> _generalDataSections;
        private readonly List<GeneralDataPoint> _generalDataPoints;
        private readonly List<GeneralDataPoint> _updateGeneralDataPoints = new List<GeneralDataPoint> {};

        public GeneralDataPointServiceTest()
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
            _generalDataSections = new List<GeneralDataSection>
            {
                new GeneralDataSection
                {
                    Id = 1,
                    Name = "S1",
                },
                new GeneralDataSection
                {
                    Id = 2,
                    Name = "S2",
                },
                new GeneralDataSection
                {
                    Id = 3,
                    Name = "S3",
                },
                new GeneralDataSection
                {
                    Id = 4,
                    Name = "S4",
                },
                new GeneralDataSection
                {
                    Id = 5,
                    Name = "S5",
                },
                new GeneralDataSection
                {
                    Id = 6,
                    Name = "S6",
                },
            };

            _generalDataPoints = new List<GeneralDataPoint>
            {
                new GeneralDataPoint
                {
                    Id = 1,
                    Section = _generalDataSections[0],
                    Text = "gdp1",
                    OrderNum = 1,
                },
                new GeneralDataPoint
                {
                    Id = 2,
                    Section = _generalDataSections[1],
                    Text = "gdp2",
                    OrderNum = 2,
                },
                new GeneralDataPoint
                {
                    Id = 3,
                    Section = _generalDataSections[0],
                    Text = "gdp3",
                    OrderNum = 1,
                },
                new GeneralDataPoint
                {
                    Id = 4,
                    Section = _generalDataSections[1],
                    Text = "gdp4",
                    OrderNum = 2,
                },
                new GeneralDataPoint
                {
                    Id = 5,
                    Section = _generalDataSections[0],
                    Text = "gdp5",
                    OrderNum = 1,
                },
                new GeneralDataPoint
                {
                    Id = 6,
                    Section = _generalDataSections[1],
                    Text = "gdp6",
                    OrderNum = 2,
                },
                new GeneralDataPoint
                {
                    Id = 7,
                    Section = _generalDataSections[0],
                    Text = "mgdp7",
                    OrderNum = 2,
                },
            };

            var mockUserRepo = new Mock<IUserRepo>();
            var mockGeneralDataSectionRepo = new Mock<IGeneralDataSectionRepo>();

            foreach (var gdp in _generalDataPoints)
            {
                _updateGeneralDataPoints.Add(new GeneralDataPoint
                {
                    Id = gdp.Id,
                    Section = gdp.Section,
                    Text = gdp.Text,
                    OrderNum = gdp.OrderNum,
                });
            }

            foreach (var generalDataPoint in _generalDataPoints)
            {
                _repository.Setup(mock =>
                    mock.GetById(generalDataPoint.Id)).Returns(
                        _generalDataPoints.SingleOrDefault(v => v.Id == generalDataPoint.Id));
            }
            foreach (var user in TestData.users)
            {
                mockUserRepo.Setup(mock =>
                    mock.GetById(user.Id)).Returns(
                        TestData.users.SingleOrDefault(v => v.Id == user.Id));

                foreach (var generalDataSection in TestData.generalDataSections)
                {
                    foreach (var generalDataPoint in _generalDataPoints)
                    {
                        _repository.Setup(mock =>
                            mock.GetByUniqueKey(
                                generalDataSection.Id, generalDataPoint.Text)).Returns(
                                    _generalDataPoints.SingleOrDefault(
                                        v => v.Section.Id == generalDataSection.Id &&
                                            v.Text == generalDataPoint.Text));
                    }
                }
            }
            foreach (var generalDataSection in TestData.generalDataSections)
            {
                _repository.Setup(mock =>
                    mock.GetAllBySectionId(generalDataSection.Id)).Returns(
                        _generalDataPoints.Where(
                            v => v.Section.Id == generalDataSection.Id));

                mockGeneralDataSectionRepo.Setup(mock =>
                    mock.GetById(generalDataSection.Id, false)).Returns(
                        TestData.generalDataSections.SingleOrDefault(
                            v => v.Id == generalDataSection.Id));
                mockGeneralDataSectionRepo.Setup(mock =>
                    mock.GetById(generalDataSection.Id, true)).Returns(
                        TestData.generalDataSections.SingleOrDefault(
                            v => v.Id == generalDataSection.Id));
            }

            _service = new GeneralDataPointService(
                _repository.Object,
                mockUserRepo.Object,
                mockGeneralDataSectionRepo.Object);
        }

        [Fact]
        public void GetAllBySectionId_ShouldReturnGeneralDataPoints()
        {
            // Arrange
            int sectionId = _rnd.Next(1, TestData.generalDataSections.Count());

            // Act
            var returnedGeneralDataPoints = _service.GetAllBySectionId(sectionId);

            // Assert
            Assert.Equal(_generalDataPoints.Where(
                v => v.Section.Id == sectionId),
                    returnedGeneralDataPoints);
        }

        [Fact]
        public void GetAllBySectionId_ShouldReturnEmptyArray_WhenWrongValues()
        {
            // Arrange
            int sectionId = _rnd.Next(1, TestData.generalDataSections.Count());

            // Act
            var returnedGeneralDataPoints = _service.GetAllBySectionId(999);

            // Assert
            Assert.Empty(returnedGeneralDataPoints);
        }
    }
}
