using System;
using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Tests
{
    public static class TestData
    {
        public static readonly List<Department> departments = new List<Department>
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

        public static readonly List<Position> positions = new List<Position>
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

        public static readonly List<Employee> employees = new List<Employee>
        {
            new Employee
            {
                Id=0,
                FullName="Test employee1",
                Department=departments[0],
                Position=positions[0],
                RecruitedDate=DateTime.Parse("2020-09-01"),
                HasCanteen=false,
                VacationType=1,
            },
            new Employee
            {
                Id=1,
                FullName="Test employee2",
                Department=departments[0],
                Position=positions[1],
                RecruitedDate=DateTime.Parse("2020-09-01"),
                HasCanteen=false,
                VacationType=1,
            },
            new Employee
            {
                Id=2,
                FullName="Test employee3",
                Department=departments[1],
                Position=positions[2],
                RecruitedDate=DateTime.Parse("2020-09-01"),
                HasCanteen=false,
                VacationType=1,
            },
        };

        public static readonly List<Project> projects = new List<Project>
        {
            new Project
            {
                Id=0,
                Type=0,
                Name="Name 1",
                AdditionalName="Additional name 1",
                BaseSeries="M32788",
            },
            new Project
            {
                Id=1,
                Type=0,
                Name="Name 2",
                AdditionalName="Additional name 2",
                BaseSeries="V14578",
            },
            new Project
            {
                Id=2,
                Type=0,
                Name="Name 3",
                AdditionalName="Additional name 3",
                BaseSeries="G29856",
            },
        };

        public static readonly List<Node> nodes = new List<Node>
        {
            new Node
            {
                Id=0,
                Project=projects[0],
                Code="111",
                Name="Name 1",
                AdditionalName="AdditionalName 1",
                ChiefEngineer=employees[0],
                ActiveNode="1",
                Created=DateTime.Parse("2020-09-01"),
            },
            new Node
            {
                Id=1,
                Project=projects[0],
                Code="222",
                Name="Name 2",
                AdditionalName="AdditionalName 2",
                ChiefEngineer=employees[1],
                ActiveNode="1",
                Created=DateTime.Parse("2020-09-01"),
            },
            new Node
            {
                Id=2,
                Project=projects[1],
                Code="333",
                Name="Name 3",
                AdditionalName="AdditionalName 3",
                ChiefEngineer=employees[2],
                ActiveNode="1",
                Created=DateTime.Parse("2020-09-01"),
            },
        };

        public static readonly List<Subnode> subnodes = new List<Subnode>
        {
            new Subnode
            {
                Id=0,
                Node=nodes[0],
                Code="Test subnode1",
                Name="Name 1",
                AdditionalName="AdditionalName 1",
                Created=DateTime.Parse("2020-09-01"),
            },
            new Subnode
            {
                Id=1,
                Node=nodes[1],
                Code="Test subnode2",
                Name="Name 2",
                AdditionalName="AdditionalName 2",
                Created=DateTime.Parse("2020-09-01"),
            },
            new Subnode
            {
                Id=2,
                Node=nodes[1],
                Code="Test subnode3",
                Name="Name 3",
                AdditionalName="AdditionalName 3",
                Created=DateTime.Parse("2020-09-01"),
            },
        };

        public static readonly List<Mark> marks = new List<Mark>
        {
            new Mark
            {
                Id=0,
                Subnode=subnodes[0],
                Code="Test mark1",
                AdditionalCode="C1",
                Name="Name 1",
                Department=departments[0],
                MainBuilder=employees[0],
            },
            new Mark
            {
                Id=1,
                Subnode=subnodes[0],
                Code="Test mark2",
                AdditionalCode="C2",
                Name="Name 2",
                Department=departments[1],
                MainBuilder=employees[1],
            },
            new Mark
            {
                Id=2,
                Subnode=subnodes[1],
                Code="Test mark3",
                AdditionalCode="C3",
                Name="Name 3",
                Department=departments[2],
                MainBuilder=employees[2],
            },
        };

        public static readonly List<User> users = new List<User>
        {
            new User
            {
                Id=0,
                Login="1",
                Password=BCrypt.Net.BCrypt.HashPassword("1"),
                Employee=employees[0],
            },
            new User
            {
                Id=1,
                Login="2",
                Password=BCrypt.Net.BCrypt.HashPassword("2"),
                Employee=employees[1],
            },
            new User
            {
                Id=2,
                Login="3",
                Password=BCrypt.Net.BCrypt.HashPassword("3"),
                Employee=employees[2],
            },
        };
    }
}