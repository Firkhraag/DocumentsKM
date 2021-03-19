using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using DocumentsKM.Helpers;
using DocumentsKM.Dtos;

namespace DocumentsKM.Services
{
    public class ArchiveService : IArchiveService
    {
        public IEnumerable<ArchiveProject> GetProjects()
        {
            // const string query = @"select 
            //                         [Проект] as Id, 
            //                         [БазСерия] as Title,
            //                         [Название] as Name,
            //                         [Название_кр] as NameShort
            //                     from [Проекты] 
            //                     where [БазСерия] like @Pattern
            //                     order by Title";
            const string query = @"select 
                                    [Проект] as Id, 
                                    [БазСерия] as Title,
                                    [Название] as Name,
                                    [Название_кр] as NameShort
                                from [Проекты]";

            using(IDbConnection db = new SqlConnection(Secrets.ARCHIVE_CONNECTION_STRING))
            {
                var projects = db.Query<ArchiveProject>(query);
                return projects;
            }
        }

        public IEnumerable<ArchiveNode> GetNodes()
        {
            // const string query = @"select 
            //                         [Узел] as Id, 
            //                         [КодУзла] as Title, 
            //                         [НазвУзла] as Name,
            //                         [Проект] as ParentId
            //                     from [Узлы]
            //                     where [Проект] = @ProjectId
            //                     order by Title";
            const string query = @"select 
                                    [Узел] as Id, 
                                    [КодУзла] as Title, 
                                    [НазвУзла] as Name,
                                    [Проект] as ParentId
                                from [Узлы]";
            using(IDbConnection db = new SqlConnection(Secrets.ARCHIVE_CONNECTION_STRING))
            {
                var nodes = db.Query<ArchiveNode>(query);
                return nodes;
            }
        }

        public IEnumerable<ArchiveNode> GetSubnodes()
        {
            // const string query = @"select 
            //                         [Подузел] as Id, 
            //                         [КодПодуз] as Title,
            //                         [НазвПодузла] as Name,
            //                         [Узел] as ParentId
            //                     from [Подузлы]
            //                     where [Узел] = @NodeId
            //                     order by Title";
            const string query = @"select 
                                    [Подузел] as Id, 
                                    [КодПодуз] as Title,
                                    [НазвПодузла] as Name,
                                    [Узел] as ParentId
                                from [Подузлы]";
            using(IDbConnection db = new SqlConnection(Secrets.ARCHIVE_CONNECTION_STRING))
            {
                var subnodes = db.Query<ArchiveNode>(query);
                return subnodes;
            }
        }
    }
}
