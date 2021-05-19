using System.Collections.Generic;
using System.Data;
using Dapper;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlSubnodeRepo : ISubnodeRepo
    {
        private readonly ApplicationContext _context;

        private readonly IDbConnection _dbConnection;

        public SqlSubnodeRepo(ApplicationContext context,
            IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
        }

        public IEnumerable<Subnode> GetAllByNodeId(int nodeId)
        {
            var query = $@"select
                            [Подузел] as Id,
                            [КодПодуз] as Code,
                            [НазвПодузла] as Name,
                            [Узел] as NodeId
                        from [Подузлы] where [КодПодуз] is not null and [Узел] = @NodeId";
            return _dbConnection.Query<Subnode>(query, new { NodeId = nodeId });
        }

        public Subnode GetById(int id)
        {
            var query = $@"select
                            [Подузел] as Id,
                            [КодПодуз] as Code,
                            [НазвПодузла] as Name,
                            [Узел] as NodeId
                        from [Подузлы] where [Подузел] = @Id";

            return _dbConnection.QuerySingle<Subnode>(query, new { Id = id });
        }

        public Subnode GetByUniqueKey(int nodeId, string code)
        {
            var query = $@"select
                            [Подузел] as Id,
                            [КодПодуз] as Code,
                            [НазвПодузла] as Name,
                            [Узел] as NodeId
                        from [Подузлы] where [Узел] = @NodeId and [КодПодуз] = @Code";

            return _dbConnection.QuerySingle<Subnode>(query, new { NodeId = nodeId, Code = code });
        }
    }
}
