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
                Id=1,
                Name="D1",
            },
            new Department
            {
                Id=2,
                Name="D2",
            },
        };

        public static readonly List<Position> positions = new List<Position>
        {
            new Position
            {
                Id=1,
                Name="P1",
            },
            new Position
            {
                Id=2,
                Name="P2",
            },
            new Position
            {
                Id=3,
                Name="P3",
            },
        };

        public static readonly List<Employee> employees = new List<Employee>
        {
            new Employee
            {
                Id=1,
                Name="E1",
                Department=departments[0],
                Position=positions[0],
            },
            new Employee
            {
                Id=2,
                Name="E2",
                Position=positions[1],
            },
            new Employee
            {
                Id=3,
                Name="E3",
                Department=departments[0],
                Position=positions[2],
            },
        };

        public static readonly List<Project> projects = new List<Project>
        {
            new Project
            {
                Id=1,
                Name="P1",
                BaseSeries="M32787",
            },
            new Project
            {
                Id=2,
                Name="2",
                BaseSeries="M32788",
            },
        };

        public static readonly List<Node> nodes = new List<Node>
        {
            new Node
            {
                Id=1,
                Project=projects[0],
                Code="11",
                Name="Name 1",
                ChiefEngineer=employees[0],
            },
            new Node
            {
                Id=2,
                Project=projects[1],
                Code="22",
                Name="Name 2",
                ChiefEngineer=employees[1],
            },
        };

        public static readonly List<Subnode> subnodes = new List<Subnode>
        {
            new Subnode
            {
                Id=1,
                Node=nodes[0],
                Code="Code1",
                Name="Name 1",
            },
            new Subnode
            {
                Id=2,
                Node=nodes[1],
                Code="Code2",
                Name="Name 2",
            },
        };

        public static readonly List<Mark> marks = new List<Mark>
        {
            new Mark
            {
                Id=1,
                Subnode=subnodes[0],
                Code="KM1",
                Name="Name 1",
                Department=departments[0],
                MainBuilder=employees[0],
            },
            new Mark
            {
                Id=2,
                Subnode=subnodes[0],
                Code="KM2",
                Name="Name 2",
                Department=departments[0],
                MainBuilder=employees[1],
            },
            new Mark
            {
                Id=3,
                Subnode=subnodes[1],
                Code="KM3",
                Name="Name 3",
                Department=departments[1],
                MainBuilder=employees[2],
            },
            new Mark
            {
                Id=4,
                Subnode=subnodes[1],
                Code="KM4",
                Name="Name 4",
                Department=departments[1],
                MainBuilder=employees[2],
            },
        };

        public static readonly List<Specification> specifications = new List<Specification>
        {
            new Specification
            {
                Id=1,
                Mark=marks[0],
                Num=0,
                IsCurrent=true,
            },
            new Specification
            {
                Id=2,
                Mark=marks[1],
                Num=0,
                IsCurrent=false,
            },
            new Specification
            {
                Id=3,
                Mark=marks[1],
                Num=1,
                IsCurrent=true,
            },
            new Specification
            {
                Id=4,
                Mark=marks[2],
                Num=0,
                IsCurrent=true,
            },
        };

        public static readonly List<DocType> docTypes = new List<DocType>
        {
            new DocType
            {
                Id=1,
                Code="C1",
                Name="DT1",
            },
            new DocType
            {
                Id=2,
                Code="C2",
                Name="DT2",
            },
            new DocType
            {
                Id=3,
                Code="C3",
                Name="DT3",
            },
        };

        public static readonly List<Doc> docs = new List<Doc>
        {
            new Doc
            {
                Id=1,
                Mark=marks[0],
                Type=docTypes[0],
                Name="Name 1",
                Creator=employees[0],
                NumOfPages=1,
                Form=1.0f,
            },
            new Doc
            {
                Id=2,
                Mark=marks[1],
                Type=docTypes[0],
                Name="Name 2",
                Creator=employees[0],
                NumOfPages=1,
                Form=1.0f,
            },
            new Doc
            {
                Id=3,
                Mark=marks[2],
                Type=docTypes[0],
                Name="Name 3",
                Creator=employees[0],
                NumOfPages=1,
                Form=1.0f,
            },
        };

        public static readonly List<User> users = new List<User>
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

        public static readonly List<ConstructionMaterial> constructionMaterials = new List<ConstructionMaterial>
        {
            new ConstructionMaterial
            {
                Id=1,
                Name="CM1",
            },
            new ConstructionMaterial
            {
                Id=2,
                Name="CM2",
            },
            new ConstructionMaterial
            {
                Id=3,
                Name="CM3",
            },
        };

        public static readonly List<AttachedDoc> attachedDocs = new List<AttachedDoc>
        {
            new AttachedDoc
            {
                Id=1,
                Mark=marks[0],
                Designation="D1",
                Name="AD1",
            },
            new AttachedDoc
            {
                Id=2,
                Mark=marks[1],
                Designation="D2",
                Name="AD2",
            },
            new AttachedDoc
            {
                Id=3,
                Mark=marks[2],
                Designation="D3",
                Name="AD3",
            },
            // For Conflict Exception
            new AttachedDoc
            {
                Id=4,
                Mark=marks[0],
                Designation="D4",
                Name="AD4",
            },
        };

        // public static readonly List<Position> positions = new List<Position>
        // {
        //     new Position
        //     {
        //         Id=1,
        //         Name="Test position1",
        //     },
        //     new Position
        //     {
        //         Id=2,
        //         Name="Test position2",
        //     },
        //     new Position
        //     {
        //         Id=3,
        //         Name="Test position3",
        //     },
        // };

        // public static readonly List<Employee> employees = new List<Employee>
        // {
        //     new Employee
        //     {
        //         Id=0,
        //         FullName="Test employee1",
        //         Department=departments[0],
        //         Position=positions[0],
        //         RecruitedDate=DateTime.Parse("2020-09-01"),
        //     },
        //     new Employee
        //     {
        //         Id=1,
        //         FullName="Test employee2",
        //         Department=departments[0],
        //         Position=positions[1],
        //         RecruitedDate=DateTime.Parse("2020-09-01"),
        //     },
        //     new Employee
        //     {
        //         Id=2,
        //         FullName="Test employee3",
        //         Department=departments[1],
        //         Position=positions[2],
        //         RecruitedDate=DateTime.Parse("2020-09-01"),
        //     },
        // };

        // public static readonly List<Project> projects = new List<Project>
        // {
        //     new Project
        //     {
        //         Id=0,
        //         Type=0,
        //         Name="Name 1",
        //         AdditionalName="Additional name 1",
        //         BaseSeries="M32788",
        //     },
        //     new Project
        //     {
        //         Id=1,
        //         Type=0,
        //         Name="Name 2",
        //         AdditionalName="Additional name 2",
        //         BaseSeries="V14578",
        //     },
        //     new Project
        //     {
        //         Id=2,
        //         Type=0,
        //         Name="Name 3",
        //         AdditionalName="Additional name 3",
        //         BaseSeries="G29856",
        //     },
        // };

        // public static readonly List<Node> nodes = new List<Node>
        // {
        //     new Node
        //     {
        //         Id=0,
        //         Project=projects[0],
        //         Code="111",
        //         Name="Name 1",
        //         AdditionalName="AdditionalName 1",
        //         ChiefEngineer=employees[0],
        //         ActiveNode="1",
        //         Created=DateTime.Parse("2020-09-01"),
        //     },
        //     new Node
        //     {
        //         Id=1,
        //         Project=projects[0],
        //         Code="222",
        //         Name="Name 2",
        //         AdditionalName="AdditionalName 2",
        //         ChiefEngineer=employees[1],
        //         ActiveNode="1",
        //         Created=DateTime.Parse("2020-09-01"),
        //     },
        //     new Node
        //     {
        //         Id=2,
        //         Project=projects[1],
        //         Code="333",
        //         Name="Name 3",
        //         AdditionalName="AdditionalName 3",
        //         ChiefEngineer=employees[2],
        //         ActiveNode="1",
        //         Created=DateTime.Parse("2020-09-01"),
        //     },
        // };

        // public static readonly List<Subnode> subnodes = new List<Subnode>
        // {
        //     new Subnode
        //     {
        //         Id=0,
        //         Node=nodes[0],
        //         Code="Test subnode1",
        //         Name="Name 1",
        //         AdditionalName="AdditionalName 1",
        //         Created=DateTime.Parse("2020-09-01"),
        //     },
        //     new Subnode
        //     {
        //         Id=1,
        //         Node=nodes[1],
        //         Code="Test subnode2",
        //         Name="Name 2",
        //         AdditionalName="AdditionalName 2",
        //         Created=DateTime.Parse("2020-09-01"),
        //     },
        //     new Subnode
        //     {
        //         Id=2,
        //         Node=nodes[1],
        //         Code="Test subnode3",
        //         Name="Name 3",
        //         AdditionalName="AdditionalName 3",
        //         Created=DateTime.Parse("2020-09-01"),
        //     },
        // };

        // public static readonly List<Mark> marks = new List<Mark>
        // {
        //     new Mark
        //     {
        //         Id=0,
        //         Subnode=subnodes[0],
        //         Code="Test mark1",
        //         AdditionalCode="C1",
        //         Name="Name 1",
        //         Department=departments[0],
        //         MainBuilder=employees[0],
        //     },
        //     new Mark
        //     {
        //         Id=1,
        //         Subnode=subnodes[0],
        //         Code="Test mark2",
        //         AdditionalCode="C2",
        //         Name="Name 2",
        //         Department=departments[1],
        //         MainBuilder=employees[1],
        //     },
        //     new Mark
        //     {
        //         Id=2,
        //         Subnode=subnodes[1],
        //         Code="Test mark3",
        //         AdditionalCode="C3",
        //         Name="Name 3",
        //         Department=departments[2],
        //         MainBuilder=employees[2],
        //     },
        // };

        // public static readonly List<User> users = new List<User>
        // {
        //     new User
        //     {
        //         Id=0,
        //         Login="1",
        //         Password=BCrypt.Net.BCrypt.HashPassword("1"),
        //         Employee=employees[0],
        //     },
        //     new User
        //     {
        //         Id=1,
        //         Login="2",
        //         Password=BCrypt.Net.BCrypt.HashPassword("2"),
        //         Employee=employees[1],
        //     },
        //     new User
        //     {
        //         Id=2,
        //         Login="3",
        //         Password=BCrypt.Net.BCrypt.HashPassword("3"),
        //         Employee=employees[2],
        //     },
        // };
    }
}