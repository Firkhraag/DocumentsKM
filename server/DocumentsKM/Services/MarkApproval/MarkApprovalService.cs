using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;

namespace DocumentsKM.Services
{
    public class MarkApprovalService : IMarkApprovalService
    {
        private IMarkApprovalRepo _repository;
        private readonly IMarkRepo _markRepo;
        private readonly IEmployeeRepo _employeeRepo;

        public MarkApprovalService(
            IMarkApprovalRepo markApprovalRepo,
            IMarkRepo markRepo,
            IEmployeeRepo employeeRepo)
        {
            _repository = markApprovalRepo;
            _markRepo = markRepo;
            _employeeRepo = employeeRepo;
        }

        public IEnumerable<Employee> GetAllEmployeesByMarkId(int markId)
        {
            var markApprovals = _repository.GetAllByMarkId(markId);
            var employees = new List<Employee>{};
            foreach (var ma in markApprovals)
            {
                employees.Add(ma.Employee);
            }
            return employees;
        }

        public void Update(
            int markId,
            List<int> employeeIds)
        {
            if (employeeIds == null)
                throw new ArgumentNullException(nameof(employeeIds));
            var foundMark = _markRepo.GetById(markId);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));
            var employees = new List<Employee>{};

            foreach (var id in employeeIds)
            {
                var employee = _employeeRepo.GetById(id);
                if (employee == null)
                    throw new ArgumentNullException(nameof(employee));
                employees.Add(employee);
            }

            var markApprovals = _repository.GetAllByMarkId(markId);
            var currentEmployeeIds = new List<int>{};
            foreach (var ma in markApprovals)
            {
                if (!employeeIds.Contains(ma.Employee.Id))
                    _repository.Delete(ma);
                currentEmployeeIds.Add(ma.Employee.Id);
            }

            foreach (var (id, i) in employeeIds.WithIndex())
                if (!currentEmployeeIds.Contains(id))
                    _repository.Add(
                        new MarkApproval
                        {
                            Mark=foundMark,
                            Employee=employees[i],
                        });
        }
    }
}
