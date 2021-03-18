// using System;
// using System.Collections.Generic;
// using System.Linq;

// namespace Gipromez.JobLog.External.Archive
// {
//     public class ArchiveRepository : IArchiveRepository
//     {
//         private readonly string _connectionStringArchive;
//         public ArchiveRepository(string connectionStringArchive)
//         {
//             _connectionStringArchive = connectionStringArchive ?? throw new ArgumentNullException(nameof(connectionStringArchive));
//         }

//         public IEnumerable<Project> GetProjects(string param)
//         {
//             const string query = @"select 
//                                     [Проект] as Id, 
//                                     [БазСерия] as Title,
//                                     [Название] as Name,
//                                     [Название_кр] as NameShort
//                                 from [Проекты] 
//                                 where [БазСерия] like @param or [название] like @param
//                                 order by Title";

//             using (IDbManager dbManager = new DbManager(new SqlConnection(_connectionStringArchive)))
//             {
//                 return dbManager.SetCommand(query)
//                                 .SetParameter("param", "%" + param + "%")
//                                 .ExecuteList<Project>();
//             }
//         }

//         public Project GetProject(int id)
//         {
//             const string query = @"select 
//                                     [Проект] as Id, 
//                                     [БазСерия] as Title,
//                                     [Название] as Name,
//                                     [Название_кр] as NameShort
//                                 from [Проекты] 
//                                 where [Проект] = @id";

//             using (IDbManager dbManager = new DbManager(new SqlConnection(_connectionStringArchive)))
//             {
//                 return dbManager.SetCommand(query)
//                                 .SetParameter("id", id)
//                                 .ExecuteObject<Project>();
//             }
//         }

//         public IEnumerable<Node> GetNodes(int projectId)
//         {
//             const string query = @"select 
//                                     [Узел] as Id, 
//                                     [КодУзла] as Title, 
//                                     [НазвУзла] as Name,
//                                     [Проект] as ParentId
//                                 from [Узлы]
//                                 where [Проект] = @projectId
//                                 order by Title";
//             using (IDbManager dbManager = new DbManager(new SqlConnection(_connectionStringArchive)))
//             {
//                 return dbManager.SetCommand(query)
//                                 .SetParameter("projectId", projectId)
//                                 .ExecuteList<Node>();
//             }
//         }

//         public Node GetNode(int nodeId)
//         {
//             const string query = @"select 
//                                     [Узел] as Id, 
//                                     [КодУзла] as Title, 
//                                     [НазвУзла] as Name,
//                                     [Проект] as ParentId
//                                 from [Узлы]
//                                 where [Узел] = @nodeId";
//             using (IDbManager dbManager = new DbManager(new SqlConnection(_connectionStringArchive)))
//             {
//                 return dbManager.SetCommand(query)
//                                 .SetParameter("nodeId", nodeId)
//                                 .ExecuteObject<Node>();
//             }
//         }

//         public IEnumerable<Node> GetSubnodes(int nodeId)
//         {
//             const string query = @"select 
//                                     [Подузел] as Id, 
//                                     [КодПодуз] as Title,
//                                     [НазвПодузла] as Name,
//                                     [Узел] as ParentId
//                                 from [Подузлы]
//                                 where [Узел] = @nodeId
//                                 order by Title";
//             using (IDbManager dbManager = new DbManager(new SqlConnection(_connectionStringArchive)))
//             {
//                 return dbManager.SetCommand(query)
//                                 .SetParameter("nodeId", nodeId)
//                                 .ExecuteList<Node>();
//             }
//         }

//         public Node GetSubnode(int subnodeId)
//         {
//             const string query = @"select
//                                     [Подузел] as Id,
//                                     [КодПодуз] as Title,
//                                     [НазвПодузла] as Name,
//                                     [Узел] as ParentId
//                                 from [Подузлы]
//                                 where [Подузел] = @subnodeId";

//             using (IDbManager dbManager = new DbManager(new SqlConnection(_connectionStringArchive)))
//             {
//                 return dbManager.SetCommand(query)
//                                 .SetParameter("subnodeId", subnodeId)
//                                 .ExecuteObject<Node>();
//             }
//         }

//         public IEnumerable<Mark> GetMarks(int subnodeId, string departmentId = null)
//         {
//             if (departmentId == "409") departmentId = null;

//             var query = @"select 
//                             [Марка] as Id,
//                             [КодМарки] as Title, 
//                             [НазвМарки] as Name, 
//                             [Шифр_отд] as DepartmentId,
//                             [Подузел] as ParentId
//                         from [МаркиПроекта], [Отделы]
//                         where ([МаркиПроекта].[Отдел] = [Отделы].[Код]) and ([Подузел] = @subnodeId)";

//             if (departmentId != null)
//                         query += @" and ([Отделы].[Шифр_отд] = @departmentId)";
//             query += " order by Title";
//             using (IDbManager dbManager = new DbManager(new SqlConnection(_connectionStringArchive)))
//             {
//                 var command = dbManager.SetCommand(query)
//                                 .SetParameter("subnodeId", subnodeId);
//                 if (query.Contains("@departmentId"))
//                     command = command.SetParameter("departmentId", departmentId);
//                 return command.ExecuteList<Mark>();
//             }
//         }

//         public Mark[] GetMarks(IReadOnlyList<int> marksId)
//         {
//             const string query = @"select 
//                             [Марка] as Id,
//                             [КодМарки] as Title, 
//                             [НазвМарки] as Name,
//                             [Подузел] as ParentId
//                         from [МаркиПроекта]
//                         where ([Марка] in @marksId)
//                         order by Title";

//             using (IDbManager dbManager = new DbManager(new SqlConnection(_connectionStringArchive)))
//             {
//                 var command = dbManager.SetCommand(query).SetParameter("marksId", marksId);
//                 var result = command.ExecuteList<Mark>();
//                 return result.ToArray();
//             }
//         }

//         public IEnumerable<LocalEstimate> GetLocalEstimates(int markId)
//         {
//             const string query = @"select [НомерСвязки] as Id
//                                       ,[Марка] as MarkId
//                                       ,[НаОсновании] as Basis
//                                       ,[ДопИнф] as Additional
//                                       ,[ОбПрилСмет] as Title
//                                       ,[НомИзмСпС] as PermissionStart
//                                       ,[НомИзмСпПо] as PermissionEnd
//                                       ,[ЛистСпец] as SheetSp
//                                       ,[ЛистСметы] as SheetEstimate
//                                       ,[Зарезервировал] as WhoReserved
//                                       ,[РукГр] as GroupLeader
//                                       ,[Исполнитель] as ExecutorDirect
//                                       ,[Дата_резервир] as DateReservation
//                                       ,[Дата_нач] as DataStart
//                                       ,[Дата_исполн] as DataExecution
//                                       ,[Разрешение] as Permission
//                                       ,[Примечание] as Note
//                                   from [ArchDocT].[dbo].[S_Листы]
//                                   where [ВидЛиста]=5 and [Марка] = @markId";

//             using (IDbManager dbManager = new DbManager(new SqlConnection(_connectionStringArchive)))
//             {
//                 var command = dbManager.SetCommand(query).SetParameter("markId", markId);
//                 var result = command.ExecuteList<LocalEstimate>();
//                 return result;
//             }
//         }
//     }
// }



using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public class ArchiveService
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        // public List<User> GetUsers()
        // {
        //     List<User> users = new List<User>();
        //     using(IDbConnection db = new SqlConnection(connectionString))
        //     {
        //         users = db.Query<User>("SELECT * FROM Users").ToList();
        //     }
        //     return users;
        // }

        // public User Get(int id)
        // {
        //     User user = null;
        //     using (IDbConnection db = new SqlConnection(connectionString))
        //     {
        //         user = db.Query<User>("SELECT * FROM Users WHERE Id = @id", new { id }).FirstOrDefault();
        //     }
        //     return user;
        // }

        // public User Create(User user)
        // {
        //     using (IDbConnection db = new SqlConnection(connectionString))
        //     {
        //         var sqlQuery = "INSERT INTO Users (Name, Age) VALUES(@Name, @Age); SELECT CAST(SCOPE_IDENTITY() as int)";
        //         int? userId = db.Query<int>(sqlQuery, user).FirstOrDefault();
        //         user.Id = userId;
        //     }
        //     return user;
        // }

        // public void Update(User user)
        // {
        //     using (IDbConnection db = new SqlConnection(connectionString))
        //     {
        //         var sqlQuery = "UPDATE Users SET Name = @Name, Age = @Age WHERE Id = @Id";
        //         db.Execute(sqlQuery, user);
        //     }
        // }

        // public void Delete(int id)
        // {
        //      using (IDbConnection db = new SqlConnection(connectionString))
        //      {
        //          var sqlQuery = "DELETE FROM Users WHERE Id = @id";
        //          db.Execute(sqlQuery, new { id });
        //      }
        // }


        public IEnumerable<ArchiveProject> GetProjects(string param)
        {
            const string query = @"select 
                                    [Проект] as Id, 
                                    [БазСерия] as Title,
                                    [Название] as Name,
                                    [Название_кр] as NameShort
                                from [Проекты] 
                                where [БазСерия] like @param or [название] like @param
                                order by Title";

            // using (IDbManager dbManager = new DbManager(new SqlConnection(_connectionStringArchive)))
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                return db.SetCommand(query)
                                .SetParameter("param", "%" + param + "%")
                                .ExecuteList<ArchiveProject>();
            }
        }

        public IEnumerable<ArchiveNode> GetNodes(int projectId)
        {
            const string query = @"select 
                                    [Узел] as Id, 
                                    [КодУзла] as Title, 
                                    [НазвУзла] as Name,
                                    [Проект] as ParentId
                                from [Узлы]
                                where [Проект] = @projectId
                                order by Title";
            using (IDbManager dbManager = new DbManager(new SqlConnection(_connectionStringArchive)))
            {
                return dbManager.SetCommand(query)
                                .SetParameter("projectId", projectId)
                                .ExecuteList<ArchiveNode>();
            }
        }

        public IEnumerable<ArchiveNode> GetSubnodes(int nodeId)
        {
            const string query = @"select 
                                    [Подузел] as Id, 
                                    [КодПодуз] as Title,
                                    [НазвПодузла] as Name,
                                    [Узел] as ParentId
                                from [Подузлы]
                                where [Узел] = @nodeId
                                order by Title";
            using (IDbManager dbManager = new DbManager(new SqlConnection(_connectionStringArchive)))
            {
                return dbManager.SetCommand(query)
                                .SetParameter("nodeId", nodeId)
                                .ExecuteList<ArchiveNode>();
            }
        }
    }
}

