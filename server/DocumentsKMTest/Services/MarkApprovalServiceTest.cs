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
    public class MarkApprovalServiceTest
    {
        private readonly Mock<IMarkApprovalRepo> _repository = new Mock<IMarkApprovalRepo>();
        private readonly Mock<IMarkRepo> _mockMarkRepo = new Mock<IMarkRepo>();
        private readonly Mock<IEmployeeRepo> _mockEmployeeRepo = new Mock<IEmployeeRepo>();
        private readonly IMarkApprovalService _service;
        private readonly Random _rnd = new Random();
        private readonly List<MarkApproval> _markApprovals = new List<MarkApproval>{};
        private readonly int _maxMarkId = 3;

        public MarkApprovalServiceTest()
        {
            // Arrange
            foreach (var markApproval in TestData.markApprovals)
            {
                _markApprovals.Add(new MarkApproval{
                    Mark = markApproval.Mark,
                    Employee = markApproval.Employee,
                });
            }
            foreach (var mark in TestData.marks)
            {
                _mockMarkRepo.Setup(mock=>
                    mock.GetById(mark.Id)).Returns(
                        TestData.marks.SingleOrDefault(v => v.Id == mark.Id));

                _repository.Setup(mock=>
                    mock.GetAllByMarkId(mark.Id)).Returns(
                        _markApprovals.Where(v => v.Mark.Id == mark.Id));
            }
            foreach (var employee in TestData.employees)
            {
                _mockEmployeeRepo.Setup(mock=>
                    mock.GetById(employee.Id)).Returns(
                        TestData.employees.SingleOrDefault(v => v.Id == employee.Id));
            }

            _repository.Setup(mock=>
                mock.Add(It.IsAny<MarkApproval>())).Verifiable();
            _repository.Setup(mock=>
                mock.Delete(It.IsAny<MarkApproval>())).Verifiable();

            _service = new MarkApprovalService(
                _repository.Object,
                _mockMarkRepo.Object,
                _mockEmployeeRepo.Object);
        }

        [Fact]
        public void GetAllEmployeesByMarkId_ShouldReturnEmployees()
        {
            // Arrange
            int markId = _rnd.Next(1, _maxMarkId);
            
            // Act
            var employees = _service.GetAllEmployeesByMarkId(markId);

            // Assert
            Assert.Equal(_markApprovals.Where(
                v => v.Mark.Id == markId).Select(v => v.Employee), employees);
        }

        [Fact]
        public void GetAllEmployeesByMarkId_ShouldReturnEmptyArray_WhenWrongMarkId()
        {
            // Arrange
            int wrongMarkId = 999;
            
            // Act
            var employees = _service.GetAllEmployeesByMarkId(wrongMarkId);

            // Assert
            Assert.Empty(employees);
        }
        
        [Fact]
        public void Update_ShouldUpdateMarkApproval()
        {
            // Arrange
            int markId = 1;
            List<int> employeeIds = new List<int>{2, 3};
            
            // Act
            _service.Update(markId, employeeIds);

            // Assert
            _repository.Verify(mock => mock.Add(It.IsAny<MarkApproval>()), Times.Once);
            _repository.Verify(mock => mock.Delete(It.IsAny<MarkApproval>()), Times.Once);
        }

        [Fact]
        public void Update_ShouldFailWithNull()
        {
            // Arrange
            int markId = _rnd.Next(1, _maxMarkId);
            int wrongMarkId = 999;
            List<int> employeeIds = new List<int>{_rnd.Next(1, TestData.employees.Count())};
            List<int> wrongEmployeeIds = new List<int>{999};
            
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(markId, null));
            Assert.Throws<ArgumentNullException>(() => _service.Update(wrongMarkId, employeeIds));
            Assert.Throws<ArgumentNullException>(() => _service.Update(markId, wrongEmployeeIds));

            _repository.Verify(mock => mock.Add(It.IsAny<MarkApproval>()), Times.Never);
            _repository.Verify(mock => mock.Delete(It.IsAny<MarkApproval>()), Times.Never);
        }
    }
}








// using System.Collections.Generic;
// using DocumentsKM.Models;
// using DocumentsKM.Data;
// using System;

// namespace DocumentsKM.Services
// {
//     public class MarkApprovalService : IMarkApprovalService
//     {
//         private IMarkApprovalRepo _repository;
//         private readonly IMarkRepo _markRepo;
//         private readonly IEmployeeRepo _employeeRepo;

//         public MarkApprovalService(
//             IMarkApprovalRepo markApprovalRepo,
//             IMarkRepo markRepo,
//             IEmployeeRepo employeeRepo)
//         {
//             _repository = markApprovalRepo;
//             _markRepo = markRepo;
//             _employeeRepo = employeeRepo;
//         }

//         public IEnumerable<Employee> GetAllEmployeesByMarkId(int markId)
//         {
//             var markApprovals = _repository.GetAllByMarkId(markId);
//             var employees = new List<Employee>{};
//             foreach (var ma in markApprovals)
//             {
//                 employees.Add(ma.Employee);
//             }
//             return employees;
//         }

//         public void Update(
//             int markId,
//             List<int> employeeIds)
//         {
//             var foundMark = _markRepo.GetById(markId);
//             if (foundMark == null)
//                 throw new ArgumentNullException(nameof(foundMark));
//             var employees = new List<Employee>{};

//             foreach (var id in employeeIds)
//             {
//                 var employee = _employeeRepo.GetById(id);
//                 if (employee == null)
//                     throw new ArgumentNullException(nameof(employee));
//                 employees.Add(employee);
//             }

//             var markApprovals = _repository.GetAllByMarkId(markId);
//             var currentEmployeeIds = new List<int>{};
//             foreach (var ma in markApprovals)
//             {
//                 if (!employeeIds.Contains(ma.Employee.Id))
//                     _repository.Delete(ma);
//                 currentEmployeeIds.Add(ma.Employee.Id);
//             }

//             foreach (var (id, i) in employeeIds.WithIndex())
//                 if (!currentEmployeeIds.Contains(id))
//                     _repository.Add(
//                         new MarkApproval
//                         {
//                             Mark=foundMark,
//                             Employee=employees[i],
//                         });
//         }
//     }
// }
