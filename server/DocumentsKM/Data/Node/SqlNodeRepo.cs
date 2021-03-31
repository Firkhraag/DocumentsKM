using System.Collections.Generic;
using System.Data;
using Dapper;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlNodeRepo : INodeRepo
    {
        private readonly ApplicationContext _context;

        private readonly IDbConnection _dbConnection;

        public SqlNodeRepo(ApplicationContext context,
            IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
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
            return _dbConnection.Query<Node>(query);
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

            return _dbConnection.QuerySingle<Node>(query);
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

            return _dbConnection.QuerySingle<Node>(query);
        }
    }
}
