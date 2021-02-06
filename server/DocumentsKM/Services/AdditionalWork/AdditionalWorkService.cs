using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;
using System.Linq;

namespace DocumentsKM.Services
{
    public class AdditionalWorkService : IAdditionalWorkService
    {
        private readonly IAdditionalWorkRepo _repository;
        private readonly IMarkRepo _markRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IDocRepo _docRepo;

        public AdditionalWorkService(
            IAdditionalWorkRepo additionalWorkRepo,
            IMarkRepo markRepo,
            IEmployeeRepo employeeRepo,
            IDocRepo docRepo)
        {
            _repository = additionalWorkRepo;
            _markRepo = markRepo;
            _employeeRepo = employeeRepo;
            _docRepo = docRepo;
        }

        public IEnumerable<AdditionalWorkResponse> GetAllByMarkId(int markId)
        {
            var docs = _docRepo.GetAllByMarkId(markId);
            var docsGroupedByCreator = docs.Where
                (v => v.Creator != null).GroupBy(d => d.Creator).Select(
                    g => new Doc
                    {
                        Creator = g.First().Creator,
                        Form = g.Sum(v => v.Form),
                    });
            var docsGroupedByNormContr = docs.Where(
                v => v.NormContr != null).GroupBy(d => d.NormContr).Select(
                    g => new Doc
                    {
                        NormContr = g.First().NormContr,
                        Form = g.Sum(v => v.Form),
                    });

            var addWork = _repository.GetAllByMarkId(markId).Select(v =>
                new AdditionalWorkResponse
                {
                    Id = v.Id,
                    Employee = new EmployeeBaseResponse
                    {
                        Id = v.Employee.Id,
                        Name = v.Employee.Name,
                    },
                    Valuation = v.Valuation,
                    MetalOrder = v.MetalOrder,
                    DrawingsCompleted = docsGroupedByCreator.SingleOrDefault(
                        d => d.Creator.Id == v.Employee.Id)?.Form ?? 0,
                    DrawingsCheck = docsGroupedByNormContr.SingleOrDefault(
                        d => d.NormContr.Id == v.Employee.Id)?.Form ?? 0,
                });

            return addWork;
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

            var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
                markId, employeeId);
            if (uniqueConstraintViolationCheck != null)
                throw new ConflictException(
                    uniqueConstraintViolationCheck.Id.ToString());

            additionalWork.Mark = foundMark;
            additionalWork.Employee = foundEmployee;
            _repository.Add(additionalWork);

            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
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

                var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
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

            var foundMark = _markRepo.GetById(foundAdditionalWork.Mark.Id);
            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
        }

        public void Delete(int id)
        {
            var foundAdditionalWork = _repository.GetById(id);
            if (foundAdditionalWork == null)
                throw new ArgumentNullException(nameof(foundAdditionalWork));
            var markId = foundAdditionalWork.Mark.Id;
            _repository.Delete(foundAdditionalWork);

            var foundMark = _markRepo.GetById(markId);
            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
        }
    }
}
