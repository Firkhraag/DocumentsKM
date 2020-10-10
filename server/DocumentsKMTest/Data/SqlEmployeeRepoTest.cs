// using System.Collections.Generic;
// using System.Linq;
// using DocumentsKM.Models;

// namespace DocumentsKM.Data
// {
//     public class SqlEmployeeRepo : IEmployeeRepo
//     {
//         private readonly ApplicationContext _context;

//         public SqlEmployeeRepo(ApplicationContext context)
//         {
//             _context = context;
//         }

//         public IEnumerable<Employee> GetAllByDepartmentNumberAndPositionRange(
//             int departmentNumber,
//             int minPosCode,
//             int maxPosCode)
//         {
//             return _context.Employees.Where(e => (e.Department.Number == departmentNumber) &&
//                 (e.Position.Code >= minPosCode) && (e.Position.Code <= maxPosCode));
//         }

//         public IEnumerable<Employee> GetAllByDepartmentNumberAndPosition(
//             int departmentNumber,
//             int posCode)
//         {
//             return _context.Employees.Where(e => (e.Department.Number == departmentNumber) &&
//                 (e.Position.Code == posCode));
//         }

//         public Employee GetById(int id)
//         {
//             return _context.Employees.FirstOrDefault(m => m.Id == id);
//         }
//     }
// }
