using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class SqlPositionRepo : IPositionRepo
    {
        private readonly ApplicationContext _context;

        public SqlPositionRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Position> GetAll()
        {
            return _context.Positions.ToList();
        }

        public Position GetById(int id)
        {
            return _context.Positions.SingleOrDefault(v => v.Id == id);
        }

        public void Add(Position position)
        {
            _context.Positions.Add(position);
            _context.SaveChanges();
        }

        public void Update(Position position)
        {
            _context.Entry(position).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Position position)
        {
            _context.Positions.Remove(position);
            _context.SaveChanges();
        }
    }
}
