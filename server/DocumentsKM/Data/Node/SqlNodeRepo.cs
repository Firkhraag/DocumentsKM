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
                        from [Узлы] where [КодУзла] is not null and [Проект] = @ProjectId";
            return _dbConnection.Query<Node>(query, new { ProjectId = projectId });
        }

        public Node GetById(int id)
        {
            var query = $@"select
                            [Узел] as Id,
                            [КодУзла] as Code, 
                            [НазвУзла] as Name,
                            [Проект] as ProjectId,
                            [ГИП] as ChiefEngineerName
                        from [Узлы] where [Узел] = @Id";

            return _dbConnection.QuerySingle<Node>(query, new { Id = id });
        }

        public Node GetByUniqueKey(int projectId, string code)
        {
            var query = $@"select
                            [Узел] as Id,
                            [КодУзла] as Code, 
                            [НазвУзла] as Name,
                            [Проект] as ProjectId,
                            [ГИП] as ChiefEngineerName
                        from [Узлы] where [Проект] = @ProjectId and [КодУзла] = @Code";

            return _dbConnection.QuerySingle<Node>(query, new { ProjectId = projectId, Code = code });
        }
    }
}
