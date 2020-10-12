using DocumentsKM.Data;
using DocumentsKM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
 
namespace DocumentsKM
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationContext ctx)
        {
            ctx.Database.EnsureCreated();

            List<Department> departments = new List<Department>
            {
                new Department
                {
                    Number=110,
                    Name="Test department1",
                    ShortName="Department1",
                    Code="D1",
                    IsActive=true,
                    IsIndustrial=true,
                },
                new Department
                {
                    Number=111,
                    Name="Test department2",
                    ShortName="Department2",
                    Code="D2",
                    IsActive=true,
                    IsIndustrial=false,
                },
                new Department
                {
                    Number=112,
                    Name="Test department3",
                    ShortName="Department3",
                    Code="D3",
                    IsActive=false,
                    IsIndustrial=false,
                },
            };
            if (!ctx.Departments.Any())
            {
                ctx.Departments.AddRange(departments);
                ctx.SaveChanges();
                ctx.Departments.AttachRange(departments);
            }

            List<Position> positions = new List<Position>
            {
                new Position
                {
                    Code=1077,
                    Name="Test position1",
                    ShortName="Position1",
                },
                new Position
                {
                    Code=1100,
                    Name="Test position2",
                    ShortName="Position2",
                },
                new Position
                {
                    Code=1185,
                    Name="Test position3",
                    ShortName="Position3",
                },
                new Position
                {
                    Code=1285,
                    Name="Test position4",
                    ShortName="Position4",
                },
                new Position
                {
                    Code=1290,
                    Name="Test position5",
                    ShortName="Position5",
                },
            };
            if (!ctx.Positions.Any())
            {
                ctx.Positions.AddRange(positions);
                ctx.SaveChanges();
                ctx.Positions.AttachRange(positions);
            }

            List<Employee> employees = new List<Employee>
            {
                new Employee
                {
                    FullName="Test employee1",
                    Department=departments[0],
                    Position=positions[0],
                    RecruitedDate=DateTime.Parse("2020-09-01"),
                    HasCanteen=false,
                    VacationType=1,
                },
                new Employee
                {
                    FullName="Test employee2",
                    Department=departments[0],
                    Position=positions[1],
                    RecruitedDate=DateTime.Parse("2020-09-01"),
                    HasCanteen=false,
                    VacationType=1,
                },
                new Employee
                {
                    FullName="Test employee3",
                    Department=departments[0],
                    Position=positions[2],
                    RecruitedDate=DateTime.Parse("2020-09-01"),
                    HasCanteen=false,
                    VacationType=1,
                },
                new Employee
                {
                    FullName="Test employee4",
                    Department=departments[0],
                    Position=positions[3],
                    RecruitedDate=DateTime.Parse("2020-09-01"),
                    HasCanteen=false,
                    VacationType=1,
                },
                new Employee
                {
                    FullName="Test employee5",
                    Department=departments[0],
                    Position=positions[0],
                    RecruitedDate=DateTime.Parse("2020-09-01"),
                    HasCanteen=false,
                    VacationType=1,
                },
                new Employee
                {
                    FullName="Test employee6",
                    Department=departments[0],
                    Position=positions[4],
                    RecruitedDate=DateTime.Parse("2020-09-01"),
                    HasCanteen=false,
                    VacationType=1,
                },
                new Employee
                {
                    FullName="Test employee7",
                    Department=departments[1],
                    Position=positions[2],
                    RecruitedDate=DateTime.Parse("2020-09-01"),
                    HasCanteen=false,
                    VacationType=1,
                },
                new Employee
                {
                    FullName="Test employee8",
                    Department=departments[1],
                    Position=positions[2],
                    RecruitedDate=DateTime.Parse("2020-09-01"),
                    HasCanteen=false,
                    VacationType=1,
                },
                new Employee
                {
                    FullName="Test employee9",
                    Department=departments[1],
                    Position=positions[3],
                    RecruitedDate=DateTime.Parse("2020-09-01"),
                    HasCanteen=false,
                    VacationType=1,
                },
                new Employee
                {
                    FullName="Test employee10",
                    Department=departments[1],
                    Position=positions[4],
                    RecruitedDate=DateTime.Parse("2020-09-01"),
                    HasCanteen=false,
                    VacationType=1,
                },
            };
            if (!ctx.Employees.Any())
            {
                ctx.Employees.AddRange(employees);
                ctx.SaveChanges();
                ctx.Employees.AttachRange(employees);
            }

            List<Project> projects = new List<Project>
            {
                new Project
                {
                    Type=0,
                    Name="Project name 1",
                    AdditionalName="Additional name 1",
                    BaseSeries="M32788",
                },
                new Project
                {
                    Type=0,
                    Name="Project name 2",
                    AdditionalName="Additional name 2",
                    BaseSeries="V14578",
                },
                new Project
                {
                    Type=0,
                    Name="Project name 3",
                    AdditionalName="Additional name 3",
                    BaseSeries="G29856",
                },
            };
            if (!ctx.Projects.Any())
            {
                ctx.Projects.AddRange(projects);
                ctx.SaveChanges();
                ctx.Projects.AttachRange(projects);
            }

            List<Node> nodes = new List<Node>
            {
                new Node
                {
                    Project=projects[0],
                    Code="111",
                    Name="Node name 1",
                    AdditionalName="AdditionalName 1",
                    ChiefEngineer=employees[0],
                    ActiveNode="1",
                },
                new Node
                {
                    Project=projects[0],
                    Code="222",
                    Name="Node name 2",
                    AdditionalName="AdditionalName 2",
                    ChiefEngineer=employees[1],
                    ActiveNode="1",
                },
                new Node
                {
                    Project=projects[1],
                    Code="333",
                    Name="Node name 3",
                    AdditionalName="AdditionalName 3",
                    ChiefEngineer=employees[2],
                    ActiveNode="1",
                },
                new Node
                {
                    Project=projects[1],
                    Code="444",
                    Name="Node name 4",
                    AdditionalName="AdditionalName 4",
                    ChiefEngineer=employees[3],
                    ActiveNode="1",
                },
            };
            if (!ctx.Nodes.Any())
            {
                ctx.Nodes.AddRange(nodes);
                ctx.SaveChanges();
                ctx.Nodes.AttachRange(nodes);
            }

            List<Subnode> subnodes = new List<Subnode>
            {
                new Subnode
                {
                    Node=nodes[0],
                    Code="Code1",
                    Name="Subnode name 1",
                    AdditionalName="AdditionalName 1",
                },
                new Subnode
                {
                    Node=nodes[1],
                    Code="Code2",
                    Name="Subnode name 2",
                    AdditionalName="AdditionalName 2",
                },
                new Subnode
                {
                    Node=nodes[1],
                    Code="Code3",
                    Name="Subnode name 3",
                    AdditionalName="AdditionalName 3",
                },
                new Subnode
                {
                    Node=nodes[2],
                    Code="Code4",
                    Name="Subnode name 4",
                    AdditionalName="AdditionalName 4",
                },
                new Subnode
                {
                    Node=nodes[2],
                    Code="Code5",
                    Name="Subnode name 5",
                    AdditionalName="AdditionalName 5",
                },
                new Subnode
                {
                    Node=nodes[3],
                    Code="Code6",
                    Name="Subnode name 6",
                    AdditionalName="AdditionalName 6",
                },
            };
            if (!ctx.Subnodes.Any())
            {
                ctx.Subnodes.AddRange(subnodes);
                ctx.SaveChanges();
                ctx.Subnodes.AttachRange(subnodes);
            }

            List<Mark> marks = new List<Mark>
            {
                new Mark
                {
                    Subnode=subnodes[0],
                    Code="Test mark1",
                    AdditionalCode="C1",
                    Name="Mark name 1",
                    Department=departments[0],
                    MainBuilder=employees[0],
                },
                new Mark
                {
                    Subnode=subnodes[0],
                    Code="Test mark2",
                    AdditionalCode="C2",
                    Name="Mark name 2",
                    Department=departments[1],
                    MainBuilder=employees[1],
                },
                new Mark
                {
                    Subnode=subnodes[1],
                    Code="Test mark3",
                    AdditionalCode="C3",
                    Name="Mark name 3",
                    Department=departments[2],
                    MainBuilder=employees[2],
                },
                new Mark
                {
                    Subnode=subnodes[2],
                    Code="Test mark4",
                    AdditionalCode="C4",
                    Name="Mark name 4",
                    Department=departments[0],
                    MainBuilder=employees[0],
                },
                new Mark
                {
                    Subnode=subnodes[3],
                    Code="Test mark5",
                    AdditionalCode="C5",
                    Name="Mark name 5",
                    Department=departments[1],
                    MainBuilder=employees[3],
                },
                new Mark
                {
                    Subnode=subnodes[4],
                    Code="Test mark6",
                    AdditionalCode="C6",
                    Name="Mark name 6",
                    Department=departments[0],
                    MainBuilder=employees[4],
                },
            };
            if (!ctx.Marks.Any())
            {
                ctx.Marks.AddRange(marks);
                ctx.SaveChanges();
                ctx.Marks.AttachRange(marks);
            }

            List<Specification> specifications = new List<Specification>
            {
                new Specification
                {
                    Mark=marks[0],
                    ReleaseNumber=0,
                },
                new Specification
                {
                    Mark=marks[0],
                    ReleaseNumber=1,
                },
                new Specification
                {
                    Mark=marks[1],
                    ReleaseNumber=0,
                },
                new Specification
                {
                    Mark=marks[1],
                    ReleaseNumber=1,
                },
                new Specification
                {
                    Mark=marks[2],
                    ReleaseNumber=0,
                },
                new Specification
                {
                    Mark=marks[2],
                    ReleaseNumber=1,
                },
                new Specification
                {
                    Mark=marks[3],
                    ReleaseNumber=0,
                },
                new Specification
                {
                    Mark=marks[3],
                    ReleaseNumber=1,
                },
            };
            if (!ctx.Specifications.Any())
            {
                ctx.Specifications.AddRange(specifications);
                ctx.SaveChanges();
                ctx.Specifications.AttachRange(specifications);
            }

            List<DocumentType> documentTypes = new List<DocumentType>
            {
                new DocumentType
                {
                    Type=1,
                    Code="C1",
                    Name="Name 1",
                },
                new DocumentType
                {
                    Type=2,
                    Code="C2",
                    Name="Name 2",
                },
                new DocumentType
                {
                    Type=3,
                    Code="C3",
                    Name="Name 3",
                },
            };
            if (!ctx.DocumentTypes.Any())
            {
                ctx.DocumentTypes.AddRange(documentTypes);
                ctx.SaveChanges();
                ctx.DocumentTypes.AttachRange(documentTypes);
            }

            List<Sheet> sheets = new List<Sheet>
            {
                new Sheet
                {
                    Mark=marks[0],
                    DocumentType=documentTypes[0],
                    Name="Name",
                    Developer=employees[0],
                    NumberOfPages=1,
                },
                new Sheet
                {
                    Mark=marks[1],
                    DocumentType=documentTypes[0],
                    Name="Name",
                    Developer=employees[0],
                    NumberOfPages=1,
                },
                new Sheet
                {
                    Mark=marks[2],
                    DocumentType=documentTypes[0],
                    Name="Name",
                    Developer=employees[0],
                    NumberOfPages=1,
                },
                new Sheet
                {
                    Mark=marks[3],
                    DocumentType=documentTypes[0],
                    Name="Name",
                    Developer=employees[0],
                    NumberOfPages=1,
                },
                new Sheet
                {
                    Mark=marks[4],
                    DocumentType=documentTypes[0],
                    Name="Name",
                    Developer=employees[0],
                    NumberOfPages=1,
                },
            };
            if (!ctx.Sheets.Any())
            {
                ctx.Sheets.AddRange(sheets);
                ctx.SaveChanges();
                ctx.Sheets.AttachRange(sheets);
            }

            List<User> users = new List<User>
            {
                new User
                {
                    Login="1",
                    Password=BCrypt.Net.BCrypt.HashPassword("1"),
                    Employee=employees[0],
                },
                new User
                {
                    Login="2",
                    Password=BCrypt.Net.BCrypt.HashPassword("2"),
                    Employee=employees[1],
                },
                new User
                {
                    Login="3",
                    Password=BCrypt.Net.BCrypt.HashPassword("3"),
                    Employee=employees[2],
                },
            };
            if (!ctx.Users.Any())
            {
                ctx.Users.AddRange(users);
                ctx.SaveChanges();
                ctx.Users.AttachRange(users);
            }
        }
    }
}
