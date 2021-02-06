using System.IO;

namespace DocumentsKM.Services
{
    public interface IGeneralDataDocService
    {
        // Получить документ общих указаний
        MemoryStream GetDocByMarkId(int markId);
    }
}
