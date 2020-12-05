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
    public class MarkServiceTest
    {
        private readonly Mock<IMarkRepo> _repository = new Mock<IMarkRepo>();
        private readonly Mock<ISubnodeRepo> _mockSubnodeRepo = new Mock<ISubnodeRepo>();
        private readonly Mock<IDepartmentRepo> _mockDepartmentRepo = new Mock<IDepartmentRepo>();
        private readonly Mock<IEmployeeRepo> _mockEmployeeRepo = new Mock<IEmployeeRepo>();
        private readonly Mock<ISpecificationService> _mockSpecificationService = new Mock<ISpecificationService>();
        private readonly IMarkService _service;
        private readonly Random _rnd = new Random();
        private readonly List<Mark> _marks = new List<Mark>{};

        public MarkServiceTest()
        {
            // Arrange
            foreach (var mark in TestData.marks)
            {
                _marks.Add(new Mark
                {
                    Id = mark.Id,
                    Subnode = mark.Subnode,
                    Code = mark.Code,
                    Name = mark.Name,
                    Department = mark.Department,
                    ChiefSpecialist = mark.ChiefSpecialist,
                    GroupLeader = mark.GroupLeader,
                    MainBuilder = mark.MainBuilder,
                });
            }
            foreach (var mark in _marks)
            {
                _repository.Setup(mock=>
                    mock.GetById(mark.Id)).Returns(
                        _marks.SingleOrDefault(v => v.Id == mark.Id));
            }
            foreach (var subnode in TestData.subnodes)
            {
                _mockSubnodeRepo.Setup(mock=>
                    mock.GetById(subnode.Id)).Returns(
                        TestData.subnodes.SingleOrDefault(v => v.Id == subnode.Id));
                _repository.Setup(mock=>
                    mock.GetAllBySubnodeId(subnode.Id)).Returns(
                        _marks.Where(v => v.Subnode.Id == subnode.Id));

                foreach (var mark in _marks)
                {
                    _repository.Setup(mock=>
                        mock.GetBySubnodeIdAndCode(subnode.Id, mark.Code)).Returns(
                            _marks.SingleOrDefault(v => v.Subnode.Id == subnode.Id &&
                            v.Code == mark.Code));
                }
            }
            foreach (var department in TestData.departments)
            {
                _mockDepartmentRepo.Setup(mock=>
                    mock.GetById(department.Id)).Returns(
                        TestData.departments.SingleOrDefault(v => v.Id == department.Id));
            }
            foreach (var employee in TestData.employees)
            {
                _mockEmployeeRepo.Setup(mock=>
                    mock.GetById(employee.Id)).Returns(
                        TestData.employees.SingleOrDefault(v => v.Id == employee.Id));
            }

            _repository.Setup(mock=>
                mock.Add(It.IsAny<Mark>())).Verifiable();
            _repository.Setup(mock=>
                mock.Update(It.IsAny<Mark>())).Verifiable();

            _service = new MarkService(
                _repository.Object,
                _mockSubnodeRepo.Object,
                _mockDepartmentRepo.Object,
                _mockEmployeeRepo.Object,
                _mockSpecificationService.Object);
        }

        [Fact]
        public void GetAllBySubnodeId_ShouldReturnMarks()
        {
            // Arrange
            int subnodeId = _rnd.Next(1, TestData.subnodes.Count());
            
            // Act
            var returnedMarks = _service.GetAllBySubnodeId(subnodeId);

            // Assert
            Assert.Equal(_marks.Where(
                v => v.Subnode.Id == subnodeId), returnedMarks);
        }

        [Fact]
        public void GetById_ShouldReturnMark()
        {
            // Arrange
            int markId = _rnd.Next(1, _marks.Count());
            
            // Act
            var returnedMark = _service.GetById(markId);

            // Assert
            Assert.Equal(_marks.SingleOrDefault(
                v => v.Id == markId), returnedMark);
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            int wrongMarkId = 999;
            
            // Act
            var returnedMark = _service.GetById(wrongMarkId);

            // Assert
            Assert.Null(returnedMark);
        }

        [Fact]
        public void Create_ShouldCreateMark()
        {
            // Arrange
            int subnodeId = _rnd.Next(1, TestData.subnodes.Count());
            int departmentId = _rnd.Next(1, TestData.departments.Count());
            int mainBuilderId = _rnd.Next(1, TestData.employees.Count());
            while (TestData.employees.SingleOrDefault(v => v.Id == mainBuilderId).Department.Id != departmentId)
            {
                mainBuilderId = _rnd.Next(1, TestData.employees.Count());
            }
            int chiefSpecialistId = _rnd.Next(1, TestData.employees.Count());
            while (TestData.employees.SingleOrDefault(v => v.Id == chiefSpecialistId).Department.Id != departmentId)
            {
                chiefSpecialistId = _rnd.Next(1, TestData.employees.Count());
            }
            int groupLeaderId = _rnd.Next(1, TestData.employees.Count());
            while (TestData.employees.SingleOrDefault(v => v.Id == groupLeaderId).Department.Id != departmentId)
            {
                groupLeaderId = _rnd.Next(1, TestData.employees.Count());
            }

            var newMark = new Mark
            {
                Name="NewCreate",
                Code="NewCreate",
            };
            
            // Act
            _service.Create(newMark,
                subnodeId,
                departmentId,
                mainBuilderId,
                chiefSpecialistId,
                groupLeaderId);

            // Assert
            _repository.Verify(mock => mock.Add(It.IsAny<Mark>()), Times.Once);
            Assert.NotNull(newMark.Subnode);
            Assert.NotNull(newMark.Department);
            Assert.NotNull(newMark.MainBuilder);
            Assert.NotNull(newMark.ChiefSpecialist);
            Assert.NotNull(newMark.GroupLeader);
        }

        [Fact]
        public void Create_ShouldFailWithNull()
        {
            // Arrange
            int subnodeId = _rnd.Next(1, TestData.subnodes.Count());
            int wrongSubnodeId = 999;
            int departmentId = _rnd.Next(1, TestData.departments.Count());
            int wrongDepartmentId = 999;
            int mainBuilderId = _rnd.Next(1, TestData.employees.Count());
            while (TestData.employees.SingleOrDefault(v => v.Id == mainBuilderId).Department.Id != departmentId)
            {
                mainBuilderId = _rnd.Next(1, TestData.employees.Count());
            }
            int wrongMainBuilderId = 999;
            int chiefSpecialistId = _rnd.Next(1, TestData.employees.Count());
            while (TestData.employees.SingleOrDefault(v => v.Id == chiefSpecialistId).Department.Id != departmentId)
            {
                chiefSpecialistId = _rnd.Next(1, TestData.employees.Count());
            }
            int wrongChiefSpecialistId = 999;
            int groupLeaderId = _rnd.Next(1, TestData.employees.Count());
            while (TestData.employees.SingleOrDefault(v => v.Id == groupLeaderId).Department.Id != departmentId)
            {
                groupLeaderId = _rnd.Next(1, TestData.employees.Count());
            }
            int wrongGroupLeaderId = 999;

            var newMark = new Mark
            {
                Name="NewCreate",
                Code="NewCreate",
            };
            
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(null,
                subnodeId,
                departmentId,
                mainBuilderId,
                chiefSpecialistId,
                groupLeaderId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(null,
                wrongSubnodeId,
                departmentId,
                mainBuilderId,
                chiefSpecialistId,
                groupLeaderId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(null,
                subnodeId,
                wrongDepartmentId,
                mainBuilderId,
                chiefSpecialistId,
                groupLeaderId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(null,
                subnodeId,
                departmentId,
                wrongMainBuilderId,
                chiefSpecialistId,
                groupLeaderId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(null,
                subnodeId,
                departmentId,
                mainBuilderId,
                wrongChiefSpecialistId,
                groupLeaderId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(null,
                subnodeId,
                departmentId,
                mainBuilderId,
                chiefSpecialistId,
                wrongGroupLeaderId));
            _repository.Verify(mock => mock.Add(It.IsAny<Mark>()), Times.Never);
        }

        [Fact]
        public void Create_ShouldFailWithConflict()
        {
            // Arrange
            int subnodeId = _marks[0].Subnode.Id;
            var conflictCode = _marks[0].Code;
            int departmentId = _rnd.Next(1, TestData.departments.Count());
            int mainBuilderId = _rnd.Next(1, TestData.employees.Count());
            while (TestData.employees.SingleOrDefault(v => v.Id == mainBuilderId).Department.Id != departmentId)
            {
                mainBuilderId = _rnd.Next(1, TestData.employees.Count());
            }
            int chiefSpecialistId = _rnd.Next(1, TestData.employees.Count());
            while (TestData.employees.SingleOrDefault(v => v.Id == chiefSpecialistId).Department.Id != departmentId)
            {
                chiefSpecialistId = _rnd.Next(1, TestData.employees.Count());
            }
            int groupLeaderId = _rnd.Next(1, TestData.employees.Count());
            while (TestData.employees.SingleOrDefault(v => v.Id == groupLeaderId).Department.Id != departmentId)
            {
                groupLeaderId = _rnd.Next(1, TestData.employees.Count());
            }

            var newMark = new Mark
            {
                Name="NewCreate",
                Code=conflictCode,
            };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Create(newMark,
                subnodeId,
                departmentId,
                mainBuilderId,
                chiefSpecialistId,
                groupLeaderId));

            _repository.Verify(mock => mock.Add(It.IsAny<Mark>()), Times.Never);
        }
        
        [Fact]
        public void Update_ShouldUpdateMark()
        {
            // Arrange
            int id = _rnd.Next(1, _marks.Count());
            int newGroupLeaderId = _rnd.Next(1, TestData.employees.Count());
            while (TestData.employees.SingleOrDefault(v => v.Id == newGroupLeaderId).Department.Id !=
                _marks.SingleOrDefault(v => v.Id == id).Department.Id)
            {
                newGroupLeaderId = _rnd.Next(1, TestData.employees.Count());
            }

            var newMarkRequest = new MarkUpdateRequest
            {
                GroupLeaderId = newGroupLeaderId,
            };
            
            // Act
            _service.Update(id,
                newMarkRequest);

            // Assert
            _repository.Verify(mock => mock.Update(It.IsAny<Mark>()), Times.Once);
            Assert.Equal(newGroupLeaderId, _marks.SingleOrDefault(v => v.Id == id).GroupLeader.Id);
        }

        [Fact]
        public void Update_ShouldFailWithNull()
        {
            // Arrange
            int id = _rnd.Next(1, _marks.Count());
            int wrongId = 999;
            int newGroupLeaderId = _rnd.Next(1, TestData.employees.Count());
            int wrongGroupLeaderId = 999;

            var newMarkRequest = new MarkUpdateRequest
            {
                GroupLeaderId = newGroupLeaderId,
            };
            var wrongMarkRequest = new MarkUpdateRequest
            {
                GroupLeaderId = wrongGroupLeaderId,
            };
            
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(wrongId, newMarkRequest));
            Assert.Throws<ArgumentNullException>(() => _service.Update(id, null));
            Assert.Throws<ArgumentNullException>(() => _service.Update(id, wrongMarkRequest));
            _repository.Verify(mock => mock.Update(It.IsAny<Mark>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldFailWithConflict()
        {
            int id = 1;
            var conflictCode = _marks[1].Code;

            var newMarkRequest = new MarkUpdateRequest
            {
                Code = conflictCode,
            };
            
            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Update(id,
                newMarkRequest));

            _repository.Verify(mock => mock.Add(It.IsAny<Mark>()), Times.Never);
        }
    }
}
