using System;
using System.Collections.Generic;
using DocumentsKM.Model;

namespace DocumentsKM.Data
{
    // Repo implementation for testing purposes
    public class MockMarkRepo : IMarkRepo
    {
        private readonly List<Mark> _marks;

        public MockMarkRepo(ISubnodeRepo subnodeRepo)
        {
            _marks = new List<Mark>{
                new Mark{
                    Id=0,
                    Subnode=subnodeRepo.GetSubnodeById(0),
                    Code="111"
                },
                new Mark{
                    Id=1,
                    Subnode=subnodeRepo.GetSubnodeById(0),
                    Code="222"
                },
                new Mark{
                    Id=2,
                    Subnode=subnodeRepo.GetSubnodeById(1),
                    Code="333"
                }
            };
        }

        // GetAllSubnodeMarks returns the list of marks for the subnode
        public IEnumerable<Mark> GetAllSubnodeMarks(ulong subnodeId)
        {
            var marksToReturn = new List<Mark>();
            foreach (Mark mark in _marks) {
                if (mark.Subnode.Id == subnodeId)
                {
                    marksToReturn.Add(mark);
                }
            }
            return marksToReturn;
        }

        public Mark GetMarkById(ulong id)
        {
            try {
                var mark = _marks[Convert.ToInt32(id)];
                return mark;
            } catch (ArgumentOutOfRangeException) {
                return null;
            }
        }

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
            return _marks;
        }

        // SaveChanges is a method that is used only in sql repo realization
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
