using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IArchiveMarkRepo
    {
        // Получить все марки из архива по id подузла
        IEnumerable<ArchiveMark> GetAllBySubnodeId(int subnodeId);
    }
}
