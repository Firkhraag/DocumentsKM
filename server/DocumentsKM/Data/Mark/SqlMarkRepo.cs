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

        public IEnumerable<Mark> GetAllByIds(List<int> ids)
        {
            return _context.Marks.Where(v => ids.Contains(v.Id)).ToList();
        }

        public IEnumerable<Mark> GetAllBySubnodeId(int subnodeId)
        {
            return _context.Marks.Where(
                v => v.SubnodeId == subnodeId).ToList();
        }

        public Mark GetById(int id)
        {
            return _context.Marks.SingleOrDefault(v => v.Id == id);
        }

        public Mark GetByUniqueKey(int subnodeId, string code)
        {
            return _context.Marks.SingleOrDefault(
                v => v.SubnodeId == subnodeId && v.Code == code);
        }

        public void Add(Mark mark)
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
