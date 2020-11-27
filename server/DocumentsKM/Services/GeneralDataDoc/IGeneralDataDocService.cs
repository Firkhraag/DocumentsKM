using System.IO;
using System.Threading.Tasks;

namespace DocumentsKM.Services
{
    public interface IGeneralDataDocService
    {
        Task<MemoryStream> GetDocByMarkId(int markId);
    }
}
