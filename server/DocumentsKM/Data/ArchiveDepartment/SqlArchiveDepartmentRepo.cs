using System.Collections.Generic;
using System.Data;
using Dapper;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlArchiveDepartmentRepo : IArchiveDepartmentRepo
    {
        private readonly ApplicationContext _context;

        private readonly IDbConnection _dbConnection;

        public SqlArchiveDepartmentRepo(ApplicationContext context,
            IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
        }

        public ArchiveDepartment GetById(int id)
        {
            const string query = @"select 
                                    [Шифр_отд] as Code
                                from [Отделы] where [Код] = @Id";

            return _dbConnection.QuerySingle<ArchiveDepartment>(query, new { Id = id });
        }
    }
}
