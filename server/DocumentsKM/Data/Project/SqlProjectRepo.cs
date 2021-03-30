using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using Dapper;
using DocumentsKM.Helpers;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlProjectRepo : IProjectRepo
    {
        private readonly ApplicationContext _context;

        public SqlProjectRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Project> GetAll()
        {
            const string query = @"select 
                                    [Проект] as Id, 
                                    [БазСерия] as BaseSeries,
                                    [Название] as Name
                                from [Проекты] where [Название] is not null and [БазСерия] is not null";

            using(IDbConnection db = new SqlConnection(Secrets.ARCHIVE_CONNECTION_STRING))
            {
                var projects = db.Query<Project>(query);
                projects = projects.Where(v => Regex.IsMatch(v.Name, "^М[1-9]*"));
                return projects;
            }
        }

        public Project GetById(int id)
        {
            var query = $@"select 
                            [Проект] as Id, 
                            [БазСерия] as BaseSeries,
                            [Название] as Name
                        from [Проекты] where [Проект] = {id}";

            using(IDbConnection db = new SqlConnection(Secrets.ARCHIVE_CONNECTION_STRING))
            {
                return db.QuerySingle<Project>(query);
            }
        }

        public Project GetByUniqueKey(string baseSeries)
        {
            var query = $@"select 
                            [Проект] as Id, 
                            [БазСерия] as BaseSeries,
                            [Название] as Name
                        from [Проекты] where [БазСерия] = {baseSeries}";

            using(IDbConnection db = new SqlConnection(Secrets.ARCHIVE_CONNECTION_STRING))
            {
                return db.QuerySingle<Project>(query);
            }
        }
    }
}
