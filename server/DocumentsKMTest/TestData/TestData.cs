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
            new Position
            {
                Id=4,
                Name="P4",
            },
            new Position
            {
                Id=7,
                Name="P7",
            },
            new Position
            {
                Id=9,
                Name="P9",
            },
            new Position
            {
                Id=10,
                Name="P10",
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
                Department=departments[1],
                Position=positions[1],
            },
            new Employee
            {
                Id=3,
                Name="E3",
                Department=departments[0],
                Position=positions[2],
            },
            new Employee
            {
                Id=4,
                Name="E4",
                Department=departments[1],
                Position=positions[1],
            },
            new Employee
            {
                Id=5,
                Name="E5",
                Department=departments[0],
                Position=positions[4],
            },
            new Employee
            {
                Id=6,
                Name="E6",
                Department=departments[1],
                Position=positions[4],
            },
            new Employee
            {
                Id=7,
                Name="E7",
                Department=departments[0],
                Position=positions[5],
            },
            new Employee
            {
                Id=8,
                Name="E8",
                Department=departments[1],
                Position=positions[6],
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

        public static readonly List<MarkApproval> markApprovals = new List<MarkApproval>
        {
            new MarkApproval
            {
                Mark=marks[0],
                Employee=employees[0],
            },
            new MarkApproval
            {
                Mark=marks[0],
                Employee=employees[1],
            },
            new MarkApproval
            {
                Mark=marks[1],
                Employee=employees[0],
            },
            new MarkApproval
            {
                Mark=marks[1],
                Employee=employees[1],
            },
            new MarkApproval
            {
                Mark=marks[2],
                Employee=employees[0],
            },
            new MarkApproval
            {
                Mark=marks[2],
                Employee=employees[1],
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

        public static readonly List<ConstructionType> constructionTypes = new List<ConstructionType>
        {
            new ConstructionType
            {
                Id=1,
                Name="T1",
            },
            new ConstructionType
            {
                Id=2,
                Name="T2",
            },
            new ConstructionType
            {
                Id=3,
                Name="T3",
            },
        };

        public static readonly List<ConstructionSubtype> constructionSubtypes = new List<ConstructionSubtype>
        {
            new ConstructionSubtype
            {
                Id=1,
                Type=constructionTypes[0],
                Name="S1",
                Valuation="V1",
            },
            new ConstructionSubtype
            {
                Id=2,
                Type=constructionTypes[1],
                Name="S2",
                Valuation="V2",
            },
            new ConstructionSubtype
            {
                Id=3,
                Type=constructionTypes[2],
                Name="S3",
                Valuation="V3",
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
            new Doc
            {
                Id=4,
                Mark=marks[0],
                Type=docTypes[1],
                Name="Name 4",
                Creator=employees[1],
                NumOfPages=1,
                Form=1.0f,
            },
            new Doc
            {
                Id=5,
                Mark=marks[1],
                Type=docTypes[1],
                Name="Name 5",
                Creator=employees[1],
                NumOfPages=1,
                Form=1.0f,
            },
            new Doc
            {
                Id=6,
                Mark=marks[2],
                Type=docTypes[1],
                Name="Name 6",
                Creator=employees[1],
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

        public static readonly List<EnvAggressiveness> envAggressiveness = new List<EnvAggressiveness>
        {
            new EnvAggressiveness
            {
                Id=1,
                Name="EA1",
            },
            new EnvAggressiveness
            {
                Id=2,
                Name="EA2",
            },
            new EnvAggressiveness
            {
                Id=3,
                Name="EA3",
            },
        };

        public static readonly List<GasGroup> gasGroups = new List<GasGroup>
        {
            new GasGroup
            {
                Id=1,
                Name="GG1",
            },
            new GasGroup
            {
                Id=2,
                Name="GG2",
            },
            new GasGroup
            {
                Id=3,
                Name="GG3",
            },
        };

        public static readonly List<HighTensileBoltsType> highTensileBoltsTypes = new List<HighTensileBoltsType>
        {
            new HighTensileBoltsType
            {
                Id=1,
                Name="HTBT1",
            },
            new HighTensileBoltsType
            {
                Id=2,
                Name="HTBT2",
            },
            new HighTensileBoltsType
            {
                Id=3,
                Name="HTBT3",
            },
        };

        public static readonly List<OperatingArea> operatingAreas = new List<OperatingArea>
        {
            new OperatingArea
            {
                Id=1,
                Name="OA1",
            },
            new OperatingArea
            {
                Id=2,
                Name="OA2",
            },
            new OperatingArea
            {
                Id=3,
                Name="OA3",
            },
        };

        public static readonly List<PaintworkType> paintworkTypes = new List<PaintworkType>
        {
            new PaintworkType
            {
                Id=1,
                Name="PT1",
            },
            new PaintworkType
            {
                Id=2,
                Name="PT2",
            },
            new PaintworkType
            {
                Id=3,
                Name="PT3",
            },
        };

        public static readonly List<SheetName> sheetNames = new List<SheetName>
        {
            new SheetName
            {
                Id=1,
                Name="SN1",
            },
            new SheetName
            {
                Id=2,
                Name="SN2",
            },
            new SheetName
            {
                Id=3,
                Name="SN3",
            },
        };

        public static readonly List<WeldingControl> weldingControl = new List<WeldingControl>
        {
            new WeldingControl
            {
                Id=1,
                Name="WC1",
            },
            new WeldingControl
            {
                Id=2,
                Name="WC2",
            },
            new WeldingControl
            {
                Id=3,
                Name="WC3",
            },
        };

        // Mark ids: 1-3
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

        public static readonly List<LinkedDocType> linkedDocTypes = new List<LinkedDocType>
        {
            new LinkedDocType
            {
                Id=1,
                Name="LDT1",
            },
            new LinkedDocType
            {
                Id=2,
                Name="LDT2",
            },
            new LinkedDocType
            {
                Id=3,
                Name="LDT3",
            },
        };

        public static readonly List<LinkedDoc> linkedDocs = new List<LinkedDoc>
        {
            new LinkedDoc
            {
                Id=1,
                Type=linkedDocTypes[0],
                Code="C1",
                Name="LD1",
                Designation="D1",
            },
            new LinkedDoc
            {
                Id=2,
                Type=linkedDocTypes[0],
                Code="C2",
                Name="LD2",
                Designation="D2",
            },
            new LinkedDoc
            {
                Id=3,
                Type=linkedDocTypes[1],
                Code="C3",
                Name="LD3",
                Designation="D3",
            },
            new LinkedDoc
            {
                Id=4,
                Type=linkedDocTypes[1],
                Code="C4",
                Name="LD4",
                Designation="D4",
            },
            new LinkedDoc
            {
                Id=5,
                Type=linkedDocTypes[2],
                Code="C5",
                Name="LD5",
                Designation="D5",
            },
        };

        // LinkedDoc ids: 1-3
        public static readonly List<MarkLinkedDoc> markLinkedDocs = new List<MarkLinkedDoc>
        {
            new MarkLinkedDoc
            {
                Id=1,
                Mark=marks[0],
                LinkedDoc=linkedDocs[0],
            },
            new MarkLinkedDoc
            {
                Id=2,
                Mark=marks[0],
                LinkedDoc=linkedDocs[1],
            },
            new MarkLinkedDoc
            {
                Id=3,
                Mark=marks[1],
                LinkedDoc=linkedDocs[0],
            },
            new MarkLinkedDoc
            {
                Id=4,
                Mark=marks[1],
                LinkedDoc=linkedDocs[1],
            },
            new MarkLinkedDoc
            {
                Id=5,
                Mark=marks[2],
                LinkedDoc=linkedDocs[0],
            },
            new MarkLinkedDoc
            {
                Id=6,
                Mark=marks[2],
                LinkedDoc=linkedDocs[2],
            },

        };

        public static readonly List<GeneralDataSection> generalDataSections = new List<GeneralDataSection>
        {
            new GeneralDataSection
            {
                Id=1,
                Name="S1",
            },
            new GeneralDataSection
            {
                Id=2,
                Name="S2",
            },
            new GeneralDataSection
            {
                Id=3,
                Name="S3",
            },
        };

        public static readonly List<MarkOperatingConditions> markOperatingConditions = new List<MarkOperatingConditions>
        {
            new MarkOperatingConditions
            {
                Mark=marks[0],
                SafetyCoeff=1.0f,
                EnvAggressiveness=envAggressiveness[0],
                Temperature=-34,
                OperatingArea=operatingAreas[0],
                GasGroup=gasGroups[0],
                ConstructionMaterial=constructionMaterials[0],
                PaintworkType=paintworkTypes[0],
                HighTensileBoltsType=highTensileBoltsTypes[0],
            },
            new MarkOperatingConditions
            {
                Mark=marks[1],
                SafetyCoeff=1.0f,
                EnvAggressiveness=envAggressiveness[1],
                Temperature=-34,
                OperatingArea=operatingAreas[1],
                GasGroup=gasGroups[0],
                ConstructionMaterial=constructionMaterials[0],
                PaintworkType=paintworkTypes[1],
                HighTensileBoltsType=highTensileBoltsTypes[0],
            },
        };
    }
}