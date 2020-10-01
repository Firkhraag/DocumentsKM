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
                Code="D1",
                IsActive=true,
            },
            new Department
            {
                Number=111,
                Name="Test department2",
                Code="D2",
                IsActive=true,
            },
            new Department
            {
                Number=112,
                Name="Test department3",
                Code="D3",
                IsActive=false,
            },
        };

        public static readonly List<Position> positions = new List<Position>
        {
            new Position
            {
                Code=1100,
                Name="Test position1",
            },
            new Position
            {
                Code=1185,
                Name="Test position2",
            },
            new Position
            {
                Code=1285,
                Name="Test position3",
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
            },
            new Employee
            {
                Id=1,
                FullName="Test employee2",
                Department=departments[0],
                Position=positions[1],
            },
            new Employee
            {
                Id=2,
                FullName="Test employee3",
                Department=departments[1],
                Position=positions[2],
            },
        };

        public static readonly List<Project> projects = new List<Project>
        {
            new Project
            {
                Id=0,
                BaseSeries="M32788",
            },
            new Project
            {
                Id=1,
                BaseSeries="V14578",
            },
            new Project
            {
                Id=2,
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
                ChiefEngineer=employees[0],
            },
            new Node
            {
                Id=1,
                Project=projects[0],
                Code="222",
                ChiefEngineer=employees[1],
            },
            new Node
            {
                Id=2,
                Project=projects[1],
                Code="333",
                ChiefEngineer=employees[2],
            },
        };

        public static readonly List<Subnode> subnodes = new List<Subnode>
        {
            new Subnode
            {
                Id=0,
                Node=nodes[0],
                Code="Test subnode1",
            },
            new Subnode
            {
                Id=1,
                Node=nodes[1],
                Code="Test subnode2",
            },
            new Subnode
            {
                Id=2,
                Node=nodes[1],
                Code="Test subnode3",
            },
        };

        public static readonly List<Mark> marks = new List<Mark>
        {
            new Mark
            {
                Id=0,
                Subnode=subnodes[0],
                Code="Test mark1",
            },
            new Mark
            {
                Id=1,
                Subnode=subnodes[0],
                Code="Test mark2",
            },
            new Mark
            {
                Id=2,
                Subnode=subnodes[1],
                Code="Test mark3",
            },
        };
    }
}