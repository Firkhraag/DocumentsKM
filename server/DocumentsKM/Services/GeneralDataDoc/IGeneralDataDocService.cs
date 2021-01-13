using System.IO;

namespace DocumentsKM.Services
{
    public interface IGeneralDataDocService
    {
        MemoryStream GetDocByMarkId(int markId);
    }
}
