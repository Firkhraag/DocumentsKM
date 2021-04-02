using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;
using System.Linq;

namespace DocumentsKM.Services
{
    public class ConstructionService : IConstructionService
    {
        private readonly IConstructionRepo _repository;
        private readonly IMarkRepo _markRepo;
        private readonly ISpecificationRepo _specificationRepo;
        private readonly IConstructionTypeRepo _constructionTypeRepo;
        private readonly IConstructionSubtypeRepo _constructionSubtypeRepo;
        private readonly IWeldingControlRepo _weldingControlRepo;

        public ConstructionService(
            IConstructionRepo constructionRepo,
            IMarkRepo markRepo,
            ISpecificationRepo specificationRepo,
            IConstructionTypeRepo constructionTypeRepo,
            IConstructionSubtypeRepo constructionSubtypeRepo,
            IWeldingControlRepo weldingControlRepo)
        {
            _repository = constructionRepo;
            _markRepo = markRepo;
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

            var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
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

            var foundMark = _markRepo.GetById(foundSpecification.Mark.Id);
            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
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

            var isUniqueKeyChanged = false;
            if (construction.Name != null)
            {
                // var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
                //     foundConstruction.Specification.Id, construction.Name, construction.PaintworkCoeff);
                // if (uniqueConstraintViolationCheck != null && uniqueConstraintViolationCheck.Id != id)
                //     throw new ConflictException(uniqueConstraintViolationCheck.Id.ToString());
                foundConstruction.Name = construction.Name;
                isUniqueKeyChanged = true;
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
                    foundConstruction.SubtypeId = null;
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
                isUniqueKeyChanged = true;
            }

            if (isUniqueKeyChanged)
            {
                var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
                    foundConstruction.Specification.Id, foundConstruction.Name, foundConstruction.PaintworkCoeff);
                if (uniqueConstraintViolationCheck != null && uniqueConstraintViolationCheck.Id != id)
                    throw new ConflictException(uniqueConstraintViolationCheck.Id.ToString());
            }

            _repository.Update(foundConstruction);

            var foundMark = _markRepo.GetById(foundConstruction.Specification.Mark.Id);
            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
        }

        public void Delete(int id)
        {
            var foundConstruction = _repository.GetById(id, true);
            if (foundConstruction == null)
                throw new ArgumentNullException(nameof(foundConstruction));

            var markId = foundConstruction.Specification.Mark.Id;
            _repository.Delete(foundConstruction);

            var foundMark = _markRepo.GetById(markId);
            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
        }

        public Construction Copy(
            int constructionId,
            int specificationId)
        {
            var foundConstruction = _repository.GetById(constructionId);
            if (foundConstruction == null)
                throw new ArgumentNullException(nameof(foundConstruction));
            var foundSpecification = _specificationRepo.GetById(specificationId);
            if (foundSpecification == null)
                throw new ArgumentNullException(nameof(foundSpecification));

            var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
                specificationId, foundConstruction.Name, foundConstruction.PaintworkCoeff);
            if (uniqueConstraintViolationCheck != null)
                throw new ConflictException(uniqueConstraintViolationCheck.Id.ToString());

            var construction = new Construction
            {
                Specification = foundSpecification,
                Name = foundConstruction.Name,
                Type = foundConstruction.Type,
                Subtype = foundConstruction.Subtype,
                Valuation = foundConstruction.Valuation,
                NumOfStandardConstructions = foundConstruction.NumOfStandardConstructions,
                HasEdgeBlunting = foundConstruction.HasEdgeBlunting,
                HasDynamicLoad = foundConstruction.HasDynamicLoad,
                HasFlangedConnections = foundConstruction.HasFlangedConnections,
                WeldingControl = foundConstruction.WeldingControl,
                PaintworkCoeff = foundConstruction.PaintworkCoeff,
                ConstructionElements = foundConstruction.ConstructionElements.Select(ce => new ConstructionElement
                {
                    Profile = ce.Profile,
                    Steel = ce.Steel,
                    Length = ce.Length,
                }).ToList(),
                ConstructionBolts = foundConstruction.ConstructionBolts.Select(cb => new ConstructionBolt
                {
                    Diameter = cb.Diameter,
                    Packet = cb.Packet,
                    Num = cb.Num,
                    NutNum = cb.NutNum,
                    WasherNum = cb.WasherNum,
                }).ToList(),
            };
            _repository.Add(construction);

            var foundMark = _markRepo.GetById(foundSpecification.Mark.Id);
            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);

            return construction;
        }
    }
}
