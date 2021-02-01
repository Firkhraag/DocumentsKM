using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;

namespace DocumentsKM.Services
{
    public class ConstructionElementService : IConstructionElementService
    {
        private readonly IConstructionElementRepo _repository;
        private readonly IConstructionRepo _constructionRepo;
        private readonly IProfileClassRepo _profileClassRepo;
        private readonly IProfileTypeRepo _profileTypeRepo;
        private readonly ISteelRepo _steelRepo;

        public ConstructionElementService(
            IConstructionElementRepo constructionElementRepo,
            IConstructionRepo constructionRepo,
            IProfileClassRepo profileClassRepo,
            IProfileTypeRepo profileTypeRepo,
            ISteelRepo steelRepo)
        {
            _repository = constructionElementRepo;
            _constructionRepo = constructionRepo;
            _profileClassRepo = profileClassRepo;
            _profileTypeRepo = profileTypeRepo;
            _steelRepo = steelRepo;
        }

        public IEnumerable<ConstructionElement> GetAllByConstructionId(
            int constructionId)
        {
            return _repository.GetAllByConstructionId(constructionId);
        }

        public void Create(
            ConstructionElement constructionElement,
            int constructionId,
            int profileClassId,
            int profileTypeId,
            int steelId)
        {
            if (constructionElement == null)
                throw new ArgumentNullException(nameof(constructionElement));
            var foundConstruction = _constructionRepo.GetById(constructionId);
            if (foundConstruction == null)
                throw new ArgumentNullException(nameof(foundConstruction));
            var foundProfileClass = _profileClassRepo.GetById(profileClassId);
            if (foundProfileClass == null)
                throw new ArgumentNullException(nameof(foundProfileClass));
            var foundProfileType = _profileTypeRepo.GetById(profileTypeId);
            if (foundProfileType == null)
                throw new ArgumentNullException(nameof(foundProfileType));
            var foundSteel = _steelRepo.GetById(steelId);
            if (foundSteel == null)
                throw new ArgumentNullException(nameof(foundSteel));

            // var uniqueConstraintViolationCheck = _repository.GetByUniqueConstraint(markId, linkedDocId);
            // if (uniqueConstraintViolationCheck != null)
            //     throw new ConflictException(nameof(uniqueConstraintViolationCheck));

            constructionElement.Construction = foundConstruction;
            constructionElement.ProfileClass = foundProfileClass;
            constructionElement.ProfileType = foundProfileType;
            constructionElement.Steel = foundSteel;

            _repository.Add(constructionElement);
        }

        public void Update(int id, ConstructionElementUpdateRequest constructionElementRequest)
        {
            if (constructionElementRequest == null)
                throw new ArgumentNullException(nameof(constructionElementRequest));
            var foundConstructionElement = _repository.GetById(id);
            if (foundConstructionElement == null)
                throw new ArgumentNullException(nameof(foundConstructionElement));

            // var uniqueConstraintViolationCheck = _repository.GetByMarkIdAndLinkedDocId(
            //     foundConstructionElement.Mark.Id, ConstructionElementRequest.LinkedDocId);
            // if (uniqueConstraintViolationCheck != null)
            //     throw new ConflictException(nameof(uniqueConstraintViolationCheck));

            if (constructionElementRequest.ProfileClassId != null)
            {
                var profileClass = _profileClassRepo.GetById(
                    constructionElementRequest.ProfileClassId.GetValueOrDefault());
                if (profileClass == null)
                    throw new ArgumentNullException(nameof(profileClass));
                // var uniqueConstraintViolationCheck = _repository.GetByUniqueConstraint(
                //     foundConstructionElement.Mark.Id, ConstructionElementRequest.LinkedDocId.GetValueOrDefault());
                // if (uniqueConstraintViolationCheck != null)
                //     throw new ConflictException(nameof(uniqueConstraintViolationCheck));
                foundConstructionElement.ProfileClass = profileClass;
            }

            if (constructionElementRequest.ProfileName != null)
                foundConstructionElement.ProfileName = constructionElementRequest.ProfileName;
            if (constructionElementRequest.Symbol != null)
                foundConstructionElement.Symbol = constructionElementRequest.Symbol;
            if (constructionElementRequest.Weight != null)
                foundConstructionElement.Weight = constructionElementRequest.Weight.GetValueOrDefault();
            if (constructionElementRequest.SurfaceArea != null)
                foundConstructionElement.SurfaceArea = constructionElementRequest.SurfaceArea.GetValueOrDefault();
            if (constructionElementRequest.ProfileTypeId != null)
            {
                var profileType = _profileTypeRepo.GetById(
                    constructionElementRequest.ProfileTypeId.GetValueOrDefault());
                if (profileType == null)
                    throw new ArgumentNullException(nameof(profileType));
                foundConstructionElement.ProfileType = profileType;
            }
            if (constructionElementRequest.SteelId != null)
            {
                var steel = _steelRepo.GetById(
                    constructionElementRequest.SteelId.GetValueOrDefault());
                if (steel == null)
                    throw new ArgumentNullException(nameof(steel));
                foundConstructionElement.Steel = steel;
            }
            if (constructionElementRequest.Length != null)
                foundConstructionElement.Length = constructionElementRequest.Length.GetValueOrDefault();
            if (constructionElementRequest.Status != null)
                foundConstructionElement.Status = constructionElementRequest.Status.GetValueOrDefault();

            _repository.Update(foundConstructionElement);
        }

        public void Delete(int id)
        {
            var foundConstructionElement = _repository.GetById(id);
            if (foundConstructionElement == null)
                throw new ArgumentNullException(nameof(foundConstructionElement));
            _repository.Delete(foundConstructionElement);
        }
    }
}
