using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class SqlMarkOperatingConditionsRepo : IMarkOperatingConditionsRepo
    {
        private readonly ApplicationContext _context;

        public SqlMarkOperatingConditionsRepo(ApplicationContext context)
        {
            _context = context;
        }

        public MarkOperatingConditions GetByMarkId(int markId)
        {
            // return _context.MarkOperatingConditions.Include(
            //     v => v.Mark).Include(
            //         v => v.EnvAggressiveness).Include(
            //             v => v.OperatingArea).Include(
            //                 v => v.GasGroup).Include(
            //                     v => v.ConstructionMaterial).Include(
            //                         v => v.PaintworkType).Include(
            //                             v => v.HighTensileBoltsType).SingleOrDefault(
            //                                 v => v.Mark.Id == markId);
            return _context.MarkOperatingConditions.SingleOrDefault(
                v => v.Mark.Id == markId);
        }

        public void Add(MarkOperatingConditions markOperatingConditions)
        {
            _context.MarkOperatingConditions.Add(markOperatingConditions);
            _context.SaveChanges();
        }

        public void Update(MarkOperatingConditions markOperatingConditions)
        {
            _context.Entry(markOperatingConditions).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
