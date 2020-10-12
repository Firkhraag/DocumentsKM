// using System.Collections.Generic;
// using System.Linq;
// using DocumentsKM.Models;

// namespace DocumentsKM.Data
// {
//     public class SqlPositionRepo : IPositionRepo
//     {
//         private readonly ApplicationContext _context;

//         public SqlPositionRepo(ApplicationContext context)
//         {
//             _context = context;
//         }

//         public Position GetByCode(int code)
//         {
//             return _context.Positions.FirstOrDefault(p => p.Code== code);
//         }
//     }
// }
