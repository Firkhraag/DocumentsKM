using DocumentsKM.Dtos;
using DocumentsKM.Models;
using System;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IArchiveMarkService
    {
        // Получить все марки по id подузла
        IEnumerable<ArchiveMark> GetAllBySubnodeId(int subnodeId);
    }
}
