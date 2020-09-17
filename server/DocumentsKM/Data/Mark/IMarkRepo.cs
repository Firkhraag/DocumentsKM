using System.Collections.Generic;
using DocumentsKM.Model;

namespace DocumentsKM.Data
{
    public interface IMarkRepo
    {
        bool SaveChanges();

        IEnumerable<Mark> GetAllSubnodeMarks(ulong subnodeId);
        IEnumerable<Mark> GetUserRecentMarks();
        Mark GetMarkById(ulong id);


        IEnumerable<Mark> GetAllMarks();
        void CreateMark(Mark mark);
        void UpdateMark(Mark mark);
    }
}
