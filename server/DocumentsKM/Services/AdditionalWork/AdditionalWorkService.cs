using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;

namespace DocumentsKM.Services
{
    public class AdditionalWorkService : IAdditionalWorkService
    {
        private IAdditionalWorkRepo _repository;
        private readonly IMarkRepo _markRepo;
        private readonly IEmployeeRepo _employeeRepo;

        public AdditionalWorkService(
            IAdditionalWorkRepo AdditionalWorkRepo,
            IMarkRepo markRepo,
            IEmployeeRepo employeeRepo)
        {
            _repository = AdditionalWorkRepo;
            _markRepo = markRepo;
            _employeeRepo = employeeRepo;
        }

        public IEnumerable<AdditionalWork> GetAllByMarkId(int markId)
        {
            return _repository.GetAllByMarkId(markId);
        }

        public void Create(
            AdditionalWork additionalWork,
            int markId,
            int employeeId)
        {
            if (additionalWork == null)
                throw new ArgumentNullException(nameof(AdditionalWork));
            var foundMark = _markRepo.GetById(markId);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));
            var foundEmployee = _employeeRepo.GetById(employeeId);
            if (foundEmployee == null)
                throw new ArgumentNullException(nameof(foundEmployee));

            var uniqueConstraintViolationCheck = _repository.GetByUniqueKeyValues(
                markId, employeeId);
            if (uniqueConstraintViolationCheck != null)
                throw new ConflictException(uniqueConstraintViolationCheck.Id.ToString());

            additionalWork.Mark = foundMark;
            additionalWork.Employee = foundEmployee;
            _repository.Add(additionalWork);
        }

        public void Update(
            int id,
            AdditionalWorkUpdateRequest additionalWork)
        {
            if (additionalWork == null)
                throw new ArgumentNullException(nameof(additionalWork));
            var foundAdditionalWork = _repository.GetById(id);
            if (foundAdditionalWork == null)
                throw new ArgumentNullException(nameof(foundAdditionalWork));

            if (additionalWork.EmployeeId != null)
            {
                var employeeId = additionalWork.EmployeeId.GetValueOrDefault();
                var employee = _employeeRepo.GetById(employeeId);
                if (employee == null)
                    throw new ArgumentNullException(nameof(employee));

                var uniqueConstraintViolationCheck = _repository.GetByUniqueKeyValues(
                    foundAdditionalWork.Mark.Id, employeeId);
                if (uniqueConstraintViolationCheck != null && uniqueConstraintViolationCheck.Id != id)
                    throw new ConflictException(uniqueConstraintViolationCheck.Id.ToString());
                foundAdditionalWork.Employee = employee;
            }
            if (additionalWork.Valuation != null)
                foundAdditionalWork.Valuation = additionalWork.Valuation.GetValueOrDefault();
            if (additionalWork.MetalOrder != null)
                foundAdditionalWork.MetalOrder = additionalWork.MetalOrder.GetValueOrDefault();

            _repository.Update(foundAdditionalWork);
        }

        public void Delete(int id)
        {
            var foundAdditionalWork = _repository.GetById(id);
            if (foundAdditionalWork == null)
                throw new ArgumentNullException(nameof(foundAdditionalWork));
            _repository.Delete(foundAdditionalWork);
        }
    }
}
