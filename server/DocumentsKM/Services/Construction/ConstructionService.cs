using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;

namespace DocumentsKM.Services
{
    public class ConstructionService : IConstructionService
    {
        private IConstructionRepo _repository;
        private readonly ISpecificationRepo _specificationRepo;
        private readonly IConstructionTypeRepo _constructionTypeRepo;
        private readonly IConstructionSubtypeRepo _constructionSubtypeRepo;
        private readonly IWeldingControlRepo _weldingControlRepo;

        public ConstructionService(
            IConstructionRepo constructionRepo,
            ISpecificationRepo specificationRepo,
            IConstructionTypeRepo constructionTypeRepo,
            IConstructionSubtypeRepo constructionSubtypeRepo,
            IWeldingControlRepo weldingControlRepo)
        {
            _repository = constructionRepo;
            _specificationRepo = specificationRepo;
            _constructionTypeRepo = constructionTypeRepo;
            _constructionSubtypeRepo = constructionSubtypeRepo;
            _weldingControlRepo = weldingControlRepo;
        }

        public IEnumerable<Construction> GetAllBySpecificationId(int specificationId)
        {
            return _repository.GetAllBySpecificationId(specificationId);
        }

        public void Create(
            Construction construction,
            int specificationId,
            int typeId,
            int? subtypeId,
            int weldingControlId)
        {
            if (construction == null)
                throw new ArgumentNullException(nameof(construction));
            var foundSpecification = _specificationRepo.GetById(specificationId);
            if (foundSpecification == null)
                throw new ArgumentNullException(nameof(foundSpecification));

            var uniqueConstraintViolationCheck = _repository.GetByUniqueKeyValues(
                specificationId, construction.Name, construction.PaintworkCoeff);
            if (uniqueConstraintViolationCheck != null)
                throw new ConflictException(uniqueConstraintViolationCheck.Id.ToString());

            construction.Specification = foundSpecification;

            var foundType = _constructionTypeRepo.GetById(typeId);
            if (foundType == null)
                throw new ArgumentNullException(nameof(foundType));
            construction.Type = foundType;
            if (subtypeId != null)
            {
                var subtype = _constructionSubtypeRepo.GetById(subtypeId.GetValueOrDefault());
                if (subtype == null)
                    throw new ArgumentNullException(nameof(subtype));
                construction.Subtype = subtype;
            }
            var foundWeldingControl = _weldingControlRepo.GetById(weldingControlId);
            if (foundWeldingControl == null)
                throw new ArgumentNullException(nameof(foundWeldingControl));
            construction.WeldingControl = foundWeldingControl;

            _repository.Add(construction);
        }

        public void Update(
            int id,
            ConstructionUpdateRequest construction)
        {
            if (construction == null)
                throw new ArgumentNullException(nameof(Construction));
            var foundConstruction = _repository.GetById(id);
            if (foundConstruction == null)
                throw new ArgumentNullException(nameof(foundConstruction));

            if (construction.Name != null)
            {
                // var uniqueConstraintViolationCheck = _repository.GetByUniqueKeyValues(
                //     foundConstruction.Specification.Id, construction.Name, construction.PaintworkCoeff);
                // if (uniqueConstraintViolationCheck != null && uniqueConstraintViolationCheck.Id != id)
                //     throw new ConflictException(uniqueConstraintViolationCheck.Id.ToString());
                foundConstruction.Name = construction.Name;
            }
            if (construction.TypeId != null)
            {
                var typeId = construction.TypeId.GetValueOrDefault();
                var type = _constructionTypeRepo.GetById(typeId);
                if (type == null)
                    throw new ArgumentNullException(nameof(type));
                foundConstruction.Type = type;
            }
            if (construction.SubtypeId != null)
            {
                var subtypeId = construction.SubtypeId.GetValueOrDefault();
                if (subtypeId == -1)
                    foundConstruction.Subtype = null;
                else
                {
                    var subtype = _constructionSubtypeRepo.GetById(subtypeId);
                    if (subtype == null)
                        throw new ArgumentNullException(nameof(subtype));
                    foundConstruction.Subtype = subtype;
                }
            }
            if (construction.Valuation != null)
                foundConstruction.Valuation = construction.Valuation;
            if (construction.StandardAlbumCode != null)
                foundConstruction.StandardAlbumCode = construction.StandardAlbumCode;
            if (construction.NumOfStandardConstructions != null)
            {
                foundConstruction.NumOfStandardConstructions =
                    construction.NumOfStandardConstructions.GetValueOrDefault();
            }
            if (construction.HasEdgeBlunting != null)
            {
                foundConstruction.HasEdgeBlunting =
                    construction.HasEdgeBlunting.GetValueOrDefault();
            }
            if (construction.HasDynamicLoad != null)
            {
                foundConstruction.HasDynamicLoad =
                    construction.HasDynamicLoad.GetValueOrDefault();
            }
            if (construction.HasFlangedConnections != null)
            {
                foundConstruction.HasFlangedConnections =
                    construction.HasFlangedConnections.GetValueOrDefault();
            }
            if (construction.WeldingControlId != null)
            {
                var weldingControlId = construction.WeldingControlId.GetValueOrDefault();
                var weldingControl = _weldingControlRepo.GetById(weldingControlId);
                if (weldingControl == null)
                    throw new ArgumentNullException(nameof(weldingControl));
                foundConstruction.WeldingControl = weldingControl;
            }
            if (construction.PaintworkCoeff != null)
            {
                foundConstruction.PaintworkCoeff =
                    construction.PaintworkCoeff.GetValueOrDefault();
            }

            _repository.Update(foundConstruction);
        }

        public void Delete(int id)
        {
            var foundConstruction = _repository.GetById(id);
            if (foundConstruction == null)
                throw new ArgumentNullException(nameof(foundConstruction));
            _repository.Delete(foundConstruction);
        }
    }
}
