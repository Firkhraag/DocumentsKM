using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IMarkRepo
    {
        IEnumerable<Mark> GetAllMarks();
        Mark GetMarkById(int id);
    }
}