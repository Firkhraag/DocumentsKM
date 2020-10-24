using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;

namespace DocumentsKM.Services
{
    public class SpecificationService : ISpecificationService
    {
        private ISpecificationRepo _repository;
        private readonly IMarkRepo _markRepo;

        public SpecificationService(
            ISpecificationRepo specificationRepo,
            IMarkRepo markRepo)
        {
            _repository = specificationRepo;
            _markRepo = markRepo;
        }

        public IEnumerable<Specification> GetAllByMarkId(int markId)
        {
            return _repository.GetAllByMarkId(markId);
        }

        public Specification Create(int markId)
        {
            var foundMark = _markRepo.GetById(markId);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));
            var specifications = _repository.GetAllByMarkId(markId);
            byte maxNum = 0;
            foreach (var s in specifications)
                if (s.ReleaseNumber > maxNum)
                    maxNum = s.ReleaseNumber;
            var newSpecification = new Specification{
                Mark = foundMark,
                ReleaseNumber = Convert.ToByte(maxNum + 1),
            };
            _repository.Add(newSpecification);
            foundMark.CurrentSpecification = newSpecification;
            _markRepo.Update(foundMark);
            return newSpecification;
        }

        public void Delete(int markId, int id)
        {
            var foundMark = _markRepo.GetById(markId);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));
            if (foundMark.CurrentSpecification.Id == id)
                throw new ConflictException(nameof(foundMark));

            var specification = _repository.GetById(id);
            if (specification == null)
                throw new ArgumentNullException(nameof(specification));

            _repository.Delete(specification);
        }
    }
}
