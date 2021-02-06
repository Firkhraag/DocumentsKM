using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;

namespace DocumentsKM.Services
{
    public class ConstructionCopyService : IConstructionCopyService
    {
        private readonly IConstructionRepo _constructionRepo;
        private readonly IMarkRepo _markRepo;
        private readonly ISpecificationRepo _specificationRepo;
        private readonly IConstructionElementRepo _constructionElementRepo;
        private readonly IConstructionBoltRepo _constructionBoltRepo;

        public ConstructionCopyService(
            IConstructionRepo constructionRepo,
            IMarkRepo markRepo,
            ISpecificationRepo specificationRepo,
            IConstructionElementRepo constructionElementRepo,
            IConstructionBoltRepo constructionBoltRepo)
        {
            _constructionRepo = constructionRepo;
            _markRepo = markRepo;
            _specificationRepo = specificationRepo;
            _constructionElementRepo = constructionElementRepo;
            _constructionBoltRepo = constructionBoltRepo;
        }

        public void Copy(
            int constructionId,
            int specificationId)
        {
            var foundConstruction = _constructionRepo.GetById(constructionId);
            if (foundConstruction == null)
                throw new ArgumentNullException(nameof(foundConstruction));
            var foundSpecification = _specificationRepo.GetById(specificationId);
            if (foundSpecification == null)
                throw new ArgumentNullException(nameof(foundSpecification));

            var uniqueConstraintViolationCheck = _constructionRepo.GetByUniqueKey(
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
            };
            _constructionRepo.Add(construction);

            var constructionElements = _constructionElementRepo.GetAllByConstructionId(
                constructionId);
            foreach (var ce in constructionElements)
            {
                var constructionElement = new ConstructionElement
                {
                    Construction = construction,
                    ProfileClass = ce.ProfileClass,
                    ProfileName = ce.ProfileName,
                    Symbol = ce.Symbol,
                    Weight = ce.Weight,
                    SurfaceArea = ce.SurfaceArea,
                    ProfileType = ce.ProfileType,
                    Steel = ce.Steel,
                    Length = ce.Length,
                    Status = ce.Status,
                };
                _constructionElementRepo.Add(constructionElement);
            }

            var constructionBolts = _constructionBoltRepo.GetAllByConstructionId(
                constructionId);
            foreach (var cb in constructionBolts)
            {
                var constructionBolt = new ConstructionBolt
                {
                    Construction = construction,
                    Diameter = cb.Diameter,
                    Packet = cb.Packet,
                    Num = cb.Num,
                    NutNum = cb.NutNum,
                    WasherNum = cb.WasherNum,
                };
                _constructionBoltRepo.Add(constructionBolt);
            }

            var foundMark = _markRepo.GetById(foundSpecification.Mark.Id);
            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
        }
    }
}
