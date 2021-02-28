using System.IO;

namespace DocumentsKM.Services
{
    public interface IDocumentService
    {
        // Получить документ общих указаний
        MemoryStream GetGeneralDataDocument(int markId);
        // Получить спецификацию металла
        MemoryStream GetSpecificationDocument(int markId);
        // Получить ведомость металлоконструкций
        MemoryStream GetConstructionDocument(int markId);
        // Получить ведомость высокопрочных болтов
        MemoryStream GetBoltDocument(int markId);
        // Получить задание на смету
        MemoryStream GetEstimateTaskDocument(int markId);
        // Получить лист регистрации проекта
        MemoryStream GetProjectRegistrationDocument(int markId);
    }
}
