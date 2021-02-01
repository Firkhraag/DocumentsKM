using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;

namespace DocumentsKM.Services
{
    public class StandardConstructionService : IStandardConstructionService
    {
        private readonly IStandardConstructionRepo _repository;
        private readonly ISpecificationRepo _specificationRepo;

        public StandardConstructionService(
            IStandardConstructionRepo standardConstructionRepo,
            ISpecificationRepo specificationRepo)
        {
            _repository = standardConstructionRepo;
            _specificationRepo = specificationRepo;
        }

        public IEnumerable<StandardConstruction> GetAllBySpecificationId(int specificationId)
        {
            return _repository.GetAllBySpecificationId(specificationId);
        }

        public void Create(
            StandardConstruction standardConstruction,
            int specificationId)
        {
            if (standardConstruction == null)
                throw new ArgumentNullException(nameof(standardConstruction));
            var foundSpecification = _specificationRepo.GetById(specificationId);
            if (foundSpecification == null)
                throw new ArgumentNullException(nameof(foundSpecification));

            // var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
            //     specificationId, standardconstruction.Name, standardconstruction.PaintworkCoeff);
            // if (uniqueConstraintViolationCheck != null)
            //     throw new ConflictException(uniqueConstraintViolationCheck.Id.ToString());

            standardConstruction.Specification = foundSpecification;

            _repository.Add(standardConstruction);
        }

        public void Update(
            int id,
            StandardConstructionUpdateRequest standardConstruction)
        {
            if (standardConstruction == null)
                throw new ArgumentNullException(nameof(standardConstruction));
            var foundStandardConstruction = _repository.GetById(id);
            if (foundStandardConstruction == null)
                throw new ArgumentNullException(nameof(foundStandardConstruction));

            if (standardConstruction.Name != null)
                foundStandardConstruction.Name = standardConstruction.Name;
            if (standardConstruction.Num != null)
                foundStandardConstruction.Num = standardConstruction.Num.GetValueOrDefault();
            if (standardConstruction.Sheet != null)
                foundStandardConstruction.Sheet = standardConstruction.Sheet;
            if (standardConstruction.Weight != null)
                foundStandardConstruction.Weight = standardConstruction.Weight.GetValueOrDefault();

            _repository.Update(foundStandardConstruction);
        }

        public void Delete(int id)
        {
            var foundStandardConstruction = _repository.GetById(id);
            if (foundStandardConstruction == null)
                throw new ArgumentNullException(nameof(foundStandardConstruction));
            _repository.Delete(foundStandardConstruction);
        }
    }
}
