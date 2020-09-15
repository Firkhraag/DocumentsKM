using System.Collections.Generic;
using DocumentsKM.Model;

namespace DocumentsKM.Data
{
    public interface IMarkRepo
    {
        bool SaveChanges();

        IEnumerable<Mark> GetAllMarks();
        Mark GetMarkById(ulong id);
        void CreateMark(Mark mark);
        void UpdateMark(Mark mark);
    }
}