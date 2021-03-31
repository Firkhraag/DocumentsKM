using System.Collections.Generic;
using System.Data;
using Dapper;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlProjectRepo : IProjectRepo
    {
        private readonly ApplicationContext _context;

        private readonly IDbConnection _dbConnection;

        public SqlProjectRepo(ApplicationContext context,
            IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
        }

        public IEnumerable<Project> GetAll()
        {
            const string query = @"select 
                                    [Проект] as Id, 
                                    [БазСерия] as BaseSeries,
                                    [Название] as Name,
                                    [ОснНадпСмещ] as Bias
                                from [Проекты] where [Название] is not null
                                and [БазСерия] is not null
                                and [БазСерия] like 'М[1-9]%'";

            return _dbConnection.Query<Project>(query);
        }

        public Project GetById(int id)
        {
            var query = $@"select 
                            [Проект] as Id, 
                            [БазСерия] as BaseSeries,
                            [Название] as Name,
                            [ОснНадпСмещ] as Bias
                        from [Проекты] where [Проект] = {id}";

            return _dbConnection.QuerySingle<Project>(query);
        }

        public Project GetByUniqueKey(string baseSeries)
        {
            var query = $@"select 
                            [Проект] as Id, 
                            [БазСерия] as BaseSeries,
                            [Название] as Name,
                            [ОснНадпСмещ] as Bias
                        from [Проекты] where [БазСерия] = {baseSeries}";

            return _dbConnection.QuerySingle<Project>(query);
        }
    }
}
