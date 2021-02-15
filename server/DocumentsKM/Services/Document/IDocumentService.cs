using System.IO;

namespace DocumentsKM.Services
{
    public interface IDocumentService
    {
        // Получить документ общих указаний
        MemoryStream GetGeneralDataDocument(int markId);
        // Получить ведомость высокопрочных болтов
        MemoryStream GetBoltDocument(int markId);
    }
}
