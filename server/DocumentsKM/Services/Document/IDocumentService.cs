using System.IO;

namespace DocumentsKM.Services
{
    public interface IDocumentService
    {
        // Получить документ общих указаний
        MemoryStream GetGeneralDataDocument(int markId);
        // Получить спецификацию металла
        MemoryStream GetSpecificationDocument(int markId, int userId);
        // Получить ведомость металлоконструкций
        MemoryStream GetConstructionDocument(int markId, int userId);
        // Получить ведомость высокопрочных болтов
        MemoryStream GetBoltDocument(int markId);
        // Получить задание на смету
        MemoryStream GetEstimateTaskDocument(int markId);
        // Получить лист регистрации проекта
        MemoryStream GetProjectRegistrationDocument(int markId);
        // Получить титульный лист комплекта для рассчета
        MemoryStream GetEstimationDocumentTitle(int markId);
        // Получить страницы комплекта для рассчета
        MemoryStream GetEstimationDocumentPages(int markId, int numOfPages);
    }
}
