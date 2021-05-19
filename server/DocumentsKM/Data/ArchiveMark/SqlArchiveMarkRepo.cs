using System.Collections.Generic;
using System.Data;
using Dapper;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlArchiveMarkRepo : IArchiveMarkRepo
    {
        private readonly ApplicationContext _context;

        private readonly IDbConnection _dbConnection;

        public SqlArchiveMarkRepo(ApplicationContext context,
            IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
        }

        public IEnumerable<ArchiveMark> GetAllBySubnodeId(int subnodeId)
        {
            const string query = @"select 
                                    [НазвМарки] as Name,
                                    [КодМарки] as Code,
                                    [Отдел] as DepartmentId
                                from [МаркиПроекта] where [Подузел] = @SubnodeId";

            return _dbConnection.Query<ArchiveMark>(query, new { SubnodeId = subnodeId });
        }
    }
}
