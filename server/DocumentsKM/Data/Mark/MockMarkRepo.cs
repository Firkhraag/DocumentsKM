using System;
using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    // Мок репозитория для тестирования
    public class MockMarkRepo : IMarkRepo
    {
        private readonly List<Mark> _marks;

        public MockMarkRepo(ISubnodeRepo subnodeRepo)
        {
            // Начальные значения
            _marks = new List<Mark>
            {
                new Mark
                {
                    Id=0,
                    Subnode=subnodeRepo.GetById(0),
                    Code="Test mark1",
                },
                new Mark
                {
                    Id=1,
                    Subnode=subnodeRepo.GetById(0),
                    Code="Test mark2",
                },
                new Mark
                {
                    Id=2,
                    Subnode=subnodeRepo.GetById(1),
                    Code="Test mark3",
                },
            };
        }

        public IEnumerable<Mark> GetAll()
        {
            return _marks;
        }

        public IEnumerable<Mark> GetAllBySubnodeId(int subnodeId)
        {
            var marksToReturn = new List<Mark>();
            foreach (Mark mark in _marks)
            {
                if (mark.Subnode.Id == subnodeId)
                    marksToReturn.Add(mark);
            }
            return marksToReturn;
        }

        // public IEnumerable<Mark> GetUserRecentMarks()
        // {
        //     return _marks;
        // }

        public Mark GetById(int id)
        {
            try
            {
                return _marks[id];
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        public void Create(Mark mark)
        {
            if (mark == null)
            {
                throw new ArgumentNullException(nameof(mark));
            }
            _marks.Add(mark);
        }

        public void Update(Mark mark)
        {
            // Not implemented
        }

        public bool SaveChanges()
        {
            return true;
        }
    }
}
