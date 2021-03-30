using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using DocumentsKM.Helpers;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlSubnodeRepo : ISubnodeRepo
    {
        private readonly ApplicationContext _context;

        public SqlSubnodeRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Subnode> GetAllByNodeId(int nodeId)
        {
            var query = $@"select 
                            [Подузел] as Id, 
                            [КодПодуз] as Code,
                            [НазвПодузла] as Name,
                            [Узел] as NodeId
                        from [Подузлы] where [КодПодуз] is not null and [Узел] = {nodeId}";
            using(IDbConnection db = new SqlConnection(Secrets.ARCHIVE_CONNECTION_STRING))
            {
                var subnodes = db.Query<Subnode>(query);
                return subnodes;
            }
        }

        public Subnode GetById(int id)
        {
            var query = $@"select 
                            [Подузел] as Id, 
                            [КодПодуз] as Code,
                            [НазвПодузла] as Name,
                            [Узел] as NodeId
                        from [Подузлы] where [Подузел] = {id}";

            using(IDbConnection db = new SqlConnection(Secrets.ARCHIVE_CONNECTION_STRING))
            {
                return db.QuerySingle<Subnode>(query);
            }
        }

        public Subnode GetByUniqueKey(int nodeId, string code)
        {
            var query = $@"select 
                            [Подузел] as Id, 
                            [КодПодуз] as Code,
                            [НазвПодузла] as Name,
                            [Узел] as NodeId
                        from [Подузлы] where [Узел] = {nodeId} and [КодПодуз] = {code}";

            using(IDbConnection db = new SqlConnection(Secrets.ARCHIVE_CONNECTION_STRING))
            {
                return db.QuerySingle<Subnode>(query);
            }
        }
    }
}
