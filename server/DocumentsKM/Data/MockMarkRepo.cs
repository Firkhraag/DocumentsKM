using System;
using System.Collections.Generic;
using DocumentsKM.Model;

namespace DocumentsKM.Data
{
    public class MockMarkRepo : IMarkRepo
    {

        private List<Mark> _marks = new List<Mark>();

        public void CreateMark(Mark mark)
        {
            if (mark == null)
            {
                throw new ArgumentNullException(nameof(mark));
            }
            _marks.Add(mark);
        }

        public IEnumerable<Mark> GetAllMarks()
        {
            var project = new Project{
                Id=0,
                BaseSeries="M32788"
            };
            var node = new Node{
                Id=0,
                Project=project,
                Code="327"
            };
            var subnode = new Subnode{
                Id=0,
                Node=node,
                Code="111"
            };
            var mark1 = new Mark{
                Id=0,
                Subnode=subnode,
                Code="RTY 6"
            };
            var mark2 = new Mark{
                Id=1,
                Subnode=subnode,
                Code="AVS 1"
            };

            return new List<Mark>
            {
                mark1,
                mark2
            };
        }

        public Mark GetMarkById(ulong id)
        {
            var project = new Project{
                Id=0,
                BaseSeries="M32788"
            };
            var node = new Node{
                Id=0,
                Project=project,
                Code="327"
            };
            var subnode = new Subnode{
                Id=0,
                Node=node,
                Code="111"
            };
            return new Mark{
                Id=id,
                Subnode=subnode,
                Code="RTY 6"
            };
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