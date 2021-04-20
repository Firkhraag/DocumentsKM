using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;
using System.Linq;

namespace DocumentsKM.Services
{
    public class SpecificationService : ISpecificationService
    {
        private readonly ISpecificationRepo _repository;
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

        public Specification GetCurrentByMarkId(int markId)
        {
            return _repository.GetCurrentByMarkId(markId);
        }

        public Specification Create(int markId)
        {
            var foundMark = _markRepo.GetById(markId);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));
            var specifications = _repository.GetAllByMarkId(markId);
            foreach (var s in specifications)
            {
                if (s.IsCurrent)
                {
                    s.IsCurrent = false;
                    _repository.Update(s);
                }
            }

            var newSpecification = new Specification
            {
                Mark = foundMark,
                Num = (Int16)(specifications.Count() == 0 ? 1 :
                    specifications.Max(v => v.Num) + 1),
                IsCurrent = true,
            };
            _repository.Add(newSpecification);

            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);

            return newSpecification;
        }

        public void Update(
            int id,
            SpecificationUpdateRequest specification)
        {
            if (specification == null)
                throw new ArgumentNullException(nameof(specification));
            var foundSpecification = _repository.GetById(id);
            if (foundSpecification == null)
                throw new ArgumentNullException(nameof(foundSpecification));

            if (specification.IsCurrent == true)
            {
                var spec = _repository.GetCurrentByMarkId(foundSpecification.Mark.Id);
                spec.IsCurrent = false;
                _repository.Update(spec);
                foundSpecification.IsCurrent = true;
            }
            if (specification.Note != null)
                foundSpecification.Note = specification.Note;
            _repository.Update(foundSpecification);

            var foundMark = _markRepo.GetById(foundSpecification.Mark.Id);
            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
        }

        public void Delete(int id)
        {
            var foundSpecification = _repository.GetById(id, true);
            if (foundSpecification == null)
                throw new ArgumentNullException(nameof(foundSpecification));
            if (foundSpecification.IsCurrent)
                throw new ConflictException(nameof(foundSpecification.IsCurrent));
            var markId = foundSpecification.Mark.Id;
            _repository.Delete(foundSpecification);

            var foundMark = _markRepo.GetById(markId);
            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
        }
    }
}
