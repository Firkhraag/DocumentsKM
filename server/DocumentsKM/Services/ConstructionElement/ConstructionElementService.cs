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
        private readonly IMarkRepo _markRepo;
        private readonly IConstructionRepo _constructionRepo;
        private readonly IProfileClassRepo _profileClassRepo;
        private readonly IProfileRepo _profileRepo;
        private readonly ISteelRepo _steelRepo;

        public ConstructionElementService(
            IConstructionElementRepo constructionElementRepo,
            IMarkRepo markRepo,
            IConstructionRepo constructionRepo,
            IProfileClassRepo profileClassRepo,
            IProfileRepo profileRepo,
            ISteelRepo steelRepo)
        {
            _repository = constructionElementRepo;
            _markRepo = markRepo;
            _constructionRepo = constructionRepo;
            _profileClassRepo = profileClassRepo;
            _profileRepo = profileRepo;
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
            int profileId,
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
            var foundProfile = _profileRepo.GetById(profileId);
            if (foundProfile == null)
                throw new ArgumentNullException(nameof(foundProfile));
            var foundSteel = _steelRepo.GetById(steelId);
            if (foundSteel == null)
                throw new ArgumentNullException(nameof(foundSteel));

            // var uniqueConstraintViolationCheck = _repository.GetByUniqueConstraint(markId, linkedDocId);
            // if (uniqueConstraintViolationCheck != null)
            //     throw new ConflictException(nameof(uniqueConstraintViolationCheck));

            constructionElement.Construction = foundConstruction;
            constructionElement.ProfileClass = foundProfileClass;
            constructionElement.Profile = foundProfile;
            constructionElement.Steel = foundSteel;

            _repository.Add(constructionElement);

            var foundMark = _markRepo.GetById(foundConstruction.Specification.Mark.Id);
            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
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

            if (constructionElementRequest.ProfileId != null)
            {
                var profile = _profileRepo.GetById(
                    constructionElementRequest.ProfileId.GetValueOrDefault());
                if (profile == null)
                    throw new ArgumentNullException(nameof(profile));
                foundConstructionElement.Profile = profile;
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

            _repository.Update(foundConstructionElement);

            var foundMark = _markRepo.GetById(foundConstructionElement.Construction.Specification.Mark.Id);
            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
        }

        public void Delete(int id)
        {
            var foundConstructionElement = _repository.GetById(id);
            if (foundConstructionElement == null)
                throw new ArgumentNullException(nameof(foundConstructionElement));
            var markId = foundConstructionElement.Construction.Specification.Mark.Id;
            _repository.Delete(foundConstructionElement);

            var foundMark = _markRepo.GetById(markId);
            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
        }
    }
}
