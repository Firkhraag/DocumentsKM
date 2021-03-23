using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class CorrProtGeneralDataPointService : ICorrProtGeneralDataPointService
    {
        private string protectionString = "";
        private string paintworkString = "";
        private string cleaningString = "";
        private string installationString = "";
        private string factoryString = "";

        private readonly IMarkOperatingConditionsRepo _markOperatingConditionsRepo;
        private readonly ICorrProtVariantRepo _corrProtVariantRepo;
        private readonly ICorrProtMethodRepo _corrProtMethodRepo;
        private readonly ICorrProtCleaningDegreeRepo _corrProtCleaningDegreeRepo;
        private readonly ICorrProtCoatingRepo _corrProtCoatingRepo;
        private readonly IPrimerRepo _primerRepo;

        public CorrProtGeneralDataPointService(
            IMarkOperatingConditionsRepo markOperatingConditionsRepo,
            ICorrProtVariantRepo corrProtVariantRepo,
            ICorrProtMethodRepo corrProtMethodRepo,
            ICorrProtCleaningDegreeRepo corrProtCleaningDegreeRepo,
            ICorrProtCoatingRepo corrProtCoatingRepo,
            IPrimerRepo primerRepo)
        {
            _markOperatingConditionsRepo = markOperatingConditionsRepo;
            _corrProtVariantRepo = corrProtVariantRepo;
            _corrProtMethodRepo = corrProtMethodRepo;
            _corrProtCleaningDegreeRepo = corrProtCleaningDegreeRepo;
            _corrProtCoatingRepo = corrProtCoatingRepo;
            _primerRepo = primerRepo;
        }

        public string GetWholeString(int markId)
        {
            Process(markId);
            return protectionString + "\n" + paintworkString + "\n" + installationString + "\n" + factoryString + "\n" + cleaningString;
        }

        public void Process(int markId)
        {
            var conditions = _markOperatingConditionsRepo.GetByMarkId(markId);
            var corrProtMethod = _corrProtMethodRepo.GetByAggressivenessAndMaterialId(
                conditions.EnvAggressiveness.Id,
                conditions.ConstructionMaterial.Id);
            if (corrProtMethod.Status == 3)
            {
                protectionString = "# Нет способов защиты при заданной агрессивности и материале конструкций";
                return;
            }
            protectionString = "# Защита металлоконструкций от коррозии осуществляется " + corrProtMethod.Name;
            NextStep(conditions);
        }

        public void NextStep(MarkOperatingConditions conditions)
        {
            var corrProtVariant = _corrProtVariantRepo.GetByOperatingConditionIds(
                conditions.EnvAggressiveness.Id,
                conditions.ConstructionMaterial.Id,
                conditions.GasGroup.Id,
                conditions.OperatingArea.Id);
            if (corrProtVariant.Status == 2)
            {
                paintworkString = "# Окраска лакокрасочными материалами не требуется.";
	            cleaningString = "# Степень очистки поверхности стальных конструкций от окислов - " + corrProtVariant.CleaningDegree.Name + ".";
                return;
            } else if (corrProtVariant.Status == 3) {
                paintworkString = "# При заданной зоне эксплуатации, группе газов, агрессивности и материале конструкций окраска лакокрасочными материалами не возможна";
                return;
            }
            FinalStep(conditions);
        }

        public void FinalStep(MarkOperatingConditions conditions)
        {
            var corrProtVariant = _corrProtVariantRepo.GetByOperatingConditionIdsWithPaintwork(
                conditions.EnvAggressiveness.Id,
                conditions.ConstructionMaterial.Id,
                conditions.GasGroup.Id,
                conditions.OperatingArea.Id,
                conditions.PaintworkType.Id);
            var corrProtCoating = _corrProtCoatingRepo.GetByFastnessTypeAndGroup(
                corrProtVariant.PaintworkFastness.Id,
                corrProtVariant.PaintworkType.Id,
                corrProtVariant.PaintworkGroup.GetValueOrDefault());
            var primer = _primerRepo.GetByGroup(corrProtCoating.PrimerGroup);
            protectionString = protectionString + " группы " + corrProtVariant.PaintworkGroup + ":";
            installationString = "- на монтаже - " + corrProtCoating.Name + " в " + corrProtVariant.PaintworkNumOfLayers + " сл. толщиной " + corrProtVariant.PaintworkPrimerThickness + " мкм;";
            factoryString= "- на заводе - " + primer.Name + " в " + corrProtVariant.PrimerNumOfLayers + " сл.";
            cleaningString = "Степень очистки поверхности стальных конструкций от окислов перед окраской - " + corrProtVariant.CleaningDegree.Name + ".";
        }
    }
}
