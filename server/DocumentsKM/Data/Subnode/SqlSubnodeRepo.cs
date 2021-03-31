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
                        from [Подузлы] where [КодПодуз] is not null and [Узел] = {nodeId}";
            return _dbConnection.Query<Subnode>(query);
        }

        public Subnode GetById(int id)
        {
            var query = $@"select 
                            [Подузел] as Id, 
                            [КодПодуз] as Code,
                            [НазвПодузла] as Name,
                            [Узел] as NodeId
                        from [Подузлы] where [Подузел] = {id}";

            return _dbConnection.QuerySingle<Subnode>(query);
        }

        public Subnode GetByUniqueKey(int nodeId, string code)
        {
            var query = $@"select 
                            [Подузел] as Id, 
                            [КодПодуз] as Code,
                            [НазвПодузла] as Name,
                            [Узел] as NodeId
                        from [Подузлы] where [Узел] = {nodeId} and [КодПодуз] = {code}";

            return _dbConnection.QuerySingle<Subnode>(query);
        }
    }
}
