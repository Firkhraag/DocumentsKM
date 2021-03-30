using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using DocumentsKM.Helpers;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlNodeRepo : INodeRepo
    {
        private readonly ApplicationContext _context;

        public SqlNodeRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Node> GetAllByProjectId(int projectId)
        {
            var query = $@"select 
                            [Узел] as Id, 
                            [КодУзла] as Code, 
                            [НазвУзла] as Name,
                            [Проект] as ProjectId,
                            [ГИП] as ChiefEngineerName
                        from [Узлы] where [КодУзла] is not null and [Проект] = {projectId}";
            using(IDbConnection db = new SqlConnection(Secrets.ARCHIVE_CONNECTION_STRING))
            {
                return db.Query<Node>(query);
            }
        }

        public Node GetById(int id)
        {
            var query = $@"select 
                            [Узел] as Id, 
                            [КодУзла] as Code, 
                            [НазвУзла] as Name,
                            [Проект] as ProjectId,
                            [ГИП] as ChiefEngineerName
                        from [Узлы] where [Узел] = {id}";

            using(IDbConnection db = new SqlConnection(Secrets.ARCHIVE_CONNECTION_STRING))
            {
                return db.QuerySingle<Node>(query);
            }
        }

        public Node GetByUniqueKey(int projectId, string code)
        {
            var query = $@"select 
                            [Узел] as Id, 
                            [КодУзла] as Code, 
                            [НазвУзла] as Name,
                            [Проект] as ProjectId,
                            [ГИП] as ChiefEngineerName
                        from [Узлы] where [Проект] = {projectId} and [КодУзла] = {code}";

            using(IDbConnection db = new SqlConnection(Secrets.ARCHIVE_CONNECTION_STRING))
            {
                return db.QuerySingle<Node>(query);
            }
        }
    }
}
