using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class SqlMarkRepo : IMarkRepo
    {
        private readonly ApplicationContext _context;

        public SqlMarkRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Mark> GetAll()
        {
            return _context.Marks.ToList();
        }

        public IEnumerable<Mark> GetAllBySubnodeId(int subnodeId)
        {
            return _context.Marks.Where(m => m.Subnode.Id == subnodeId);
        }

        public Mark GetById(int id)
        {
            return _context.Marks.FirstOrDefault(m => m.Id == id);
        }

        public void Create(Mark mark)
        {
            _context.Marks.Add(mark);
            _context.SaveChanges();
        }

        public void Update(Mark mark)
        {
            _context.Entry(mark).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
