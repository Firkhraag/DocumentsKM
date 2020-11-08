using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;

namespace DocumentsKM.Services
{
    public class MarkOperatingConditionsService : IMarkOperatingConditionsService
    {
        private IMarkOperatingConditionsRepo _repository;
        private IMarkRepo _markRepository;
        private IOperatingAreaRepo _operatingAreaRepo;
        private IGasGroupRepo _gasGroupRepo;
        private IEnvAggressivenessRepo _envAggressivenessRepo;
        private IConstructionMaterialRepo _constructionMaterialRepo;
        private IPaintworkTypeRepo _paintworkTypeRepo;
        private IHighTensileBoltsTypeRepo _highTensileBoltsTypeRepo;

        public MarkOperatingConditionsService(
            IMarkOperatingConditionsRepo markOperatingConditionsRepo,
            IMarkRepo markRepository,
            IOperatingAreaRepo operatingAreaRepo,
            IGasGroupRepo gasGroupRepo,
            IEnvAggressivenessRepo envAggressivenessRepo,
            IConstructionMaterialRepo constructionMaterialRepo,
            IPaintworkTypeRepo paintworkTypeRepo,
            IHighTensileBoltsTypeRepo highTensileBoltsTypeRepo)
        {
            _repository = markOperatingConditionsRepo;
            _markRepository = markRepository;
            _operatingAreaRepo = operatingAreaRepo;
            _gasGroupRepo = gasGroupRepo;
            _envAggressivenessRepo = envAggressivenessRepo;
            _constructionMaterialRepo = constructionMaterialRepo;
            _paintworkTypeRepo = paintworkTypeRepo;
            _highTensileBoltsTypeRepo = highTensileBoltsTypeRepo;
        }

        public MarkOperatingConditions GetByMarkId(int markId)
        {
            return _repository.GetByMarkId(markId);
        }

        public void Create(MarkOperatingConditions markOperatingConditions,
            int markId,
            int envAggressivenessId,
            int operatingAreaId,
            int gasGroupId,
            int constructionMaterialId,
            int paintworkTypeId,
            int highTensileBoltsTypeId)
        {
            if (markOperatingConditions == null)
                throw new ArgumentNullException(nameof(markOperatingConditions));
            var foundMark = _markRepository.GetById(markId);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));

            var envAggressiveness = _envAggressivenessRepo.GetById(envAggressivenessId);
            if (envAggressiveness == null)
                throw new ArgumentNullException(nameof(envAggressiveness));
            markOperatingConditions.EnvAggressiveness = envAggressiveness;

            var operatingArea = _operatingAreaRepo.GetById(operatingAreaId);
            if (operatingArea == null)
                throw new ArgumentNullException(nameof(operatingArea));
            markOperatingConditions.OperatingArea = operatingArea;
            
            var gasGroup = _gasGroupRepo.GetById(gasGroupId);
            if (gasGroup == null)
                throw new ArgumentNullException(nameof(gasGroup));
            markOperatingConditions.GasGroup = gasGroup;

            var constructionMaterial = _constructionMaterialRepo.GetById(constructionMaterialId);
            if (constructionMaterial == null)
                throw new ArgumentNullException(nameof(constructionMaterial));
            markOperatingConditions.ConstructionMaterial = constructionMaterial;

            var paintworkType = _paintworkTypeRepo.GetById(paintworkTypeId);
            if (paintworkType == null)
                throw new ArgumentNullException(nameof(paintworkType));
            markOperatingConditions.PaintworkType = paintworkType;

            var highTensileBoltsType = _highTensileBoltsTypeRepo.GetById(highTensileBoltsTypeId);
            if (highTensileBoltsType == null)
                throw new ArgumentNullException(nameof(highTensileBoltsType));
            markOperatingConditions.HighTensileBoltsType = highTensileBoltsType;

            _repository.Add(markOperatingConditions);
        }

        public void Update(
            int markId,
            MarkOperatingConditionsUpdateRequest markOperatingConditions)
        {
            if (markOperatingConditions == null)
                throw new ArgumentNullException(nameof(markOperatingConditions));
            var foundMarkOperatingConditions = _repository.GetByMarkId(markId);
            if (foundMarkOperatingConditions == null)
                throw new ArgumentNullException(nameof(foundMarkOperatingConditions));
            
            if (markOperatingConditions.SafetyCoeff != null)
                foundMarkOperatingConditions.SafetyCoeff = markOperatingConditions.SafetyCoeff.GetValueOrDefault();
            if (markOperatingConditions.Temperature != null)
                foundMarkOperatingConditions.Temperature = markOperatingConditions.Temperature.GetValueOrDefault();

            if (markOperatingConditions.EnvAggressivenessId != null)
            {
                var envAggressiveness = _envAggressivenessRepo.GetById(markOperatingConditions.EnvAggressivenessId.GetValueOrDefault());
                if (envAggressiveness == null)
                    throw new ArgumentNullException(nameof(envAggressiveness));
                foundMarkOperatingConditions.EnvAggressiveness = envAggressiveness;
            }
            if (markOperatingConditions.OperatingAreaId != null)
            {
                var operatingArea = _operatingAreaRepo.GetById(markOperatingConditions.OperatingAreaId.GetValueOrDefault());
                if (operatingArea == null)
                    throw new ArgumentNullException(nameof(operatingArea));
                foundMarkOperatingConditions.OperatingArea = operatingArea;
            }
            if (markOperatingConditions.GasGroupId != null)
            {
                var gasGroup = _gasGroupRepo.GetById(markOperatingConditions.GasGroupId.GetValueOrDefault());
                if (gasGroup == null)
                    throw new ArgumentNullException(nameof(gasGroup));
                foundMarkOperatingConditions.GasGroup = gasGroup;
            }
            if (markOperatingConditions.ConstructionMaterialId != null)
            {
                var constructionMaterial = _constructionMaterialRepo.GetById(markOperatingConditions.ConstructionMaterialId.GetValueOrDefault());
                if (constructionMaterial == null)
                    throw new ArgumentNullException(nameof(constructionMaterial));
                foundMarkOperatingConditions.ConstructionMaterial = constructionMaterial;
            }
            if (markOperatingConditions.PaintworkTypeId != null)
            {
                var paintworkType = _paintworkTypeRepo.GetById(markOperatingConditions.PaintworkTypeId.GetValueOrDefault());
                if (paintworkType == null)
                    throw new ArgumentNullException(nameof(paintworkType));
                foundMarkOperatingConditions.PaintworkType = paintworkType;
            }
            if (markOperatingConditions.HighTensileBoltsTypeId != null)
            {
                var highTensileBoltsType = _highTensileBoltsTypeRepo.GetById(markOperatingConditions.HighTensileBoltsTypeId.GetValueOrDefault());
                if (highTensileBoltsType == null)
                    throw new ArgumentNullException(nameof(highTensileBoltsType));
                foundMarkOperatingConditions.HighTensileBoltsType = highTensileBoltsType;
            }
            _repository.Update(foundMarkOperatingConditions);
        }
    }
}
