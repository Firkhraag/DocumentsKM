using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Model;

namespace DocumentsKM.Data
{
    public class SqlMarkRepo : IMarkRepo
    {
        private readonly MarkContext _context;

        public SqlMarkRepo(MarkContext context)
        {
            _context = context;
        }

        // GetAllSubnodeMarks returns the list of marks for the subnode
        public IEnumerable<Mark> GetAllSubnodeMarks(ulong subnodeId)
        {
            // TBD
            return _context.Marks.ToList();
        }

        // GetAllSubnodeMarks returns the list of marks for the subnode
        public IEnumerable<Mark> GetUserRecentMarks()
        {
            //TBD
            return _context.Marks.ToList();
        }

        public Mark GetMarkById(ulong id)
        {
            return _context.Marks.FirstOrDefault(m => m.Id == id);
        }

        public void CreateMark(Mark mark)
        {
            if (mark == null)
            {
                throw new ArgumentNullException(nameof(mark));
            }
            _context.Marks.Add(mark);
        }

        public IEnumerable<Mark> GetAllMarks()
        {
            return _context.Marks.ToList();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateMark(Mark mark)
        {
            // Nothing
        }
    }
}
