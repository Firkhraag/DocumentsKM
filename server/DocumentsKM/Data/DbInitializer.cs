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
                    Code=1100,
                    Name="Test position1",
                    ShortName="Position3",
                },
                new Position
                {
                    Code=1185,
                    Name="Test position2",
                    ShortName="Position2",
                },
                new Position
                {
                    Code=1285,
                    Name="Test position3",
                    ShortName="Position3",
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
                    Department=departments[1],
                    Position=positions[2],
                    RecruitedDate=DateTime.Parse("2020-09-01"),
                    HasCanteen=false,
                    VacationType=1,
                },
            };
            if (!ctx.Employees.Any())
            {
                // ctx.Departments.AttachRange(departments);
                // ctx.Positions.AttachRange(positions);
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
                    Created=DateTime.Parse("2020-09-01"),
                },
                new Node
                {
                    Project=projects[0],
                    Code="222",
                    Name="Node name 2",
                    AdditionalName="AdditionalName 2",
                    ChiefEngineer=employees[1],
                    ActiveNode="1",
                    Created=DateTime.Parse("2020-09-01"),
                },
                new Node
                {
                    Project=projects[1],
                    Code="333",
                    Name="Node name 3",
                    AdditionalName="AdditionalName 3",
                    ChiefEngineer=employees[2],
                    ActiveNode="1",
                    Created=DateTime.Parse("2020-09-01"),
                },
            };
            if (!ctx.Nodes.Any())
            {
                // ctx.Projects.AttachRange(projects);
                // ctx.Employees.AttachRange(employees);
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
                    Created=DateTime.Parse("2020-09-01"),
                },
                new Subnode
                {
                    Node=nodes[1],
                    Code="Code2",
                    Name="Subnode name 2",
                    AdditionalName="AdditionalName 2",
                    Created=DateTime.Parse("2020-09-01"),
                },
                new Subnode
                {
                    Node=nodes[1],
                    Code="Code3",
                    Name="Subnode name 3",
                    AdditionalName="AdditionalName 3",
                    Created=DateTime.Parse("2020-09-01"),
                },
            };
            if (!ctx.Subnodes.Any())
            {
                // ctx.Nodes.AttachRange(nodes);
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
            };
            if (!ctx.Marks.Any())
            {
                // ctx.Subnodes.AttachRange(subnodes);
                // ctx.Departments.AttachRange(departments);
                // ctx.Employees.AttachRange(employees);
                ctx.Marks.AddRange(marks);
                ctx.SaveChanges();
                ctx.Marks.AttachRange(marks);
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
                // ctx.Employees.AttachRange(employees);
                ctx.Users.AddRange(users);
                ctx.SaveChanges();
                ctx.Users.AttachRange(users);
            }
        }
    }
}
