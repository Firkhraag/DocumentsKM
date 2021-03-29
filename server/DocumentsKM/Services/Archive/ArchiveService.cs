using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using DocumentsKM.Helpers;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public class ArchiveService : IArchiveService
    {
        public IEnumerable<Project> GetProjects()
        {
            const string query = @"select 
                                    [Проект] as Id, 
                                    [БазСерия] as BaseSeries,
                                    [Название] as Name
                                from [Проекты]";

            using(IDbConnection db = new SqlConnection(Secrets.ARCHIVE_CONNECTION_STRING))
            {
                var projects = db.Query<Project>(query);
                return projects;
            }
        }

        public IEnumerable<Node> GetNodes()
        {
            const string query = @"select 
                                    [Узел] as Id, 
                                    [КодУзла] as Code, 
                                    [НазвУзла] as Name,
                                    [Проект] as ProjectId,
                                    [ГИП] as ChiefEngineer
                                from [Узлы]";
            using(IDbConnection db = new SqlConnection(Secrets.ARCHIVE_CONNECTION_STRING))
            {
                var nodes = db.Query<Node>(query);
                return nodes;
            }
        }

        public IEnumerable<Subnode> GetSubnodes()
        {
            const string query = @"select 
                                    [Подузел] as Id, 
                                    [КодПодуз] as Code,
                                    [НазвПодузла] as Name,
                                    [Узел] as NodeId
                                from [Подузлы]";
            using(IDbConnection db = new SqlConnection(Secrets.ARCHIVE_CONNECTION_STRING))
            {
                var subnodes = db.Query<Subnode>(query);
                return subnodes;
            }
        }
    }
}
