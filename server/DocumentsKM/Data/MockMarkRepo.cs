using System;
using System.Collections.Generic;
using DocumentsKM.ProjectKM;

namespace DocumentsKM.Data
{
    public class MockMarkRepo : IMarkRepo
    {
        public void CreateMark(Mark mark)
        {
            if (mark == null)
            {
                throw new ArgumentNullException(nameof(mark));
            }
            // Nothing
        }

        public IEnumerable<Mark> GetAllMarks()
        {
            return new List<Mark>
            {
                new Mark(),
            };
        }

        public Mark GetMarkById(ulong id)
        {
            return new Mark();
        }

        public bool SaveChanges()
        {
            return true;
        }

        public void UpdateMark(Mark mark)
        {
            // Nothing
        }
    }
}