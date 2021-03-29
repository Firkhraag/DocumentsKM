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
                Id = 1,
                Name = "D1",
                IsActive = true,
            },
            new Department
            {
                Id = 2,
                IsActive = true,
            },
            new Department
            {
                Id = 3,
                IsActive = false,
            },
        };

        public static readonly List<Position> positions = new List<Position>
        {
            new Position
            {
                Id = 1,
                Name = "P1",
            },
            new Position
            {
                Id = 2,
                Name = "P2",
            },
            new Position
            {
                Id = 3,
                Name = "P3",
            },
            new Position
            {
                Id = 4,
                Name = "P4",
            },
            new Position
            {
                Id = 7,
                Name = "P7",
            },
            new Position
            {
                Id = 9,
                Name = "P9",
            },
            new Position
            {
                Id = 10,
                Name = "P10",
            },
        };

        public static readonly List<Employee> employees = new List<Employee>
        {
            new Employee
            {
                Id = 1,
                Fullname = "E1",
                Name = "E1",
                Department = departments[0],
                Position = positions[0],
                IsActive = true,
            },
            new Employee
            {
                Id = 2,
                Fullname = "E2",
                Name = "E2",
                Department = departments[1],
                Position = positions[1],
                IsActive = true,
            },
            new Employee
            {
                Id = 3,
                Fullname = "E3",
                Name = "E3",
                Department = departments[0],
                Position = positions[2],
                IsActive = true,
            },
            new Employee
            {
                Id = 4,
                Fullname = "E4",
                Name = "E4",
                Department = departments[1],
                Position = positions[1],
                IsActive = true,
            },
            new Employee
            {
                Id = 5,
                Fullname = "E5",
                Name = "E5",
                Department = departments[0],
                Position = positions[4],
                IsActive = true,
            },
            new Employee
            {
                Id = 6,
                Fullname = "E6",
                Name = "E6",
                Department = departments[1],
                Position = positions[4],
                IsActive = true,
            },
            new Employee
            {
                Id = 7,
                Fullname = "E7",
                Name = "E7",
                Department = departments[0],
                Position = positions[5],
                IsActive = true,
            },
            new Employee
            {
                Id = 8,
                Fullname = "E8",
                Name = "E8",
                Department = departments[1],
                Position = positions[6],
                IsActive = true,
            },
            new Employee
            {
                Id = 9,
                Fullname = "E9",
                Name = "E9",
                Department = departments[0],
                Position = positions[0],
                IsActive = false,
            },
        };

        public static readonly List<Primer> primer = new List<Primer>
        {
            new Primer
            {
                Id = 1,
                GroupNum = 1,
                Name = "P1",
                CanBePrimed = true,
                Priority = 1,
            },
            new Primer
            {
                Id = 2,
                GroupNum = 1,
                Name = "P2",
                CanBePrimed = true,
                Priority = 2,
            },
            new Primer
            {
                Id = 3,
                GroupNum = 2,
                Name = "P3",
                CanBePrimed = false,
                Priority = 3,
            },
        };

        public static readonly List<Project> projects = new List<Project>
        {
            new Project
            {
                Id = 1,
                Name = "P1",
                BaseSeries = "M32787",
            },
            new Project
            {
                Id = 2,
                Name = "2",
                BaseSeries = "M32788",
            },
        };

        public static readonly List<Node> nodes = new List<Node>
        {
            new Node
            {
                Id = 1,
                Project = projects[0],
                Code = "11",
                Name = "Name 1",
                ChiefEngineer = "CE1",
            },
            new Node
            {
                Id = 2,
                Project = projects[1],
                Code = "22",
                Name = "Name 2",
                ChiefEngineer = "CE2",
            },
        };

        public static readonly List<Subnode> subnodes = new List<Subnode>
        {
            new Subnode
            {
                Id = 1,
                Node = nodes[0],
                Code = "Code1",
                Name = "Name 1",
            },
            new Subnode
            {
                Id = 2,
                Node = nodes[1],
                Code = "Code2",
                Name = "Name 2",
            },
        };

        public static readonly List<Mark> marks = new List<Mark>
        {
            new Mark
            {
                Id = 1,
                Subnode = subnodes[0],
                Code = "KM1",
                Name = "Name 1",
                Department = departments[0],
                NormContr = employees[0],
            },
            new Mark
            {
                Id = 2,
                Subnode = subnodes[0],
                Code = "KM2",
                Name = "Name 2",
                Department = departments[0],
                NormContr = employees[1],
            },
            new Mark
            {
                Id = 3,
                Subnode = subnodes[1],
                Code = "KM3",
                Name = "Name 3",
                Department = departments[1],
                NormContr = employees[2],
            },
            new Mark
            {
                Id = 4,
                Subnode = subnodes[1],
                Code = "KM4",
                Name = "Name 4",
                Department = departments[1],
                NormContr = employees[2],
            },
        };

        public static readonly List<MarkApproval> markApprovals = new List<MarkApproval>
        {
            new MarkApproval
            {
                Mark = marks[0],
                Employee = employees[0],
            },
            new MarkApproval
            {
                Mark = marks[0],
                Employee = employees[1],
            },
            new MarkApproval
            {
                Mark = marks[1],
                Employee = employees[0],
            },
            new MarkApproval
            {
                Mark = marks[1],
                Employee = employees[1],
            },
            new MarkApproval
            {
                Mark = marks[2],
                Employee = employees[0],
            },
            new MarkApproval
            {
                Mark = marks[2],
                Employee = employees[1],
            },
        };

        public static readonly List<Specification> specifications = new List<Specification>
        {
            new Specification
            {
                Id = 1,
                Mark = marks[0],
                Num = 0,
                IsCurrent = true,
            },
            new Specification
            {
                Id = 2,
                Mark = marks[1],
                Num = 0,
                IsCurrent = false,
            },
            new Specification
            {
                Id = 3,
                Mark = marks[1],
                Num = 1,
                IsCurrent = true,
            },
            new Specification
            {
                Id = 4,
                Mark = marks[2],
                Num = 0,
                IsCurrent = true,
            },
        };

        public static readonly List<ConstructionType> constructionTypes = new List<ConstructionType>
        {
            new ConstructionType
            {
                Id = 1,
                Name = "T1",
            },
            new ConstructionType
            {
                Id = 2,
                Name = "T2",
            },
            new ConstructionType
            {
                Id = 3,
                Name = "T3",
            },
        };

        public static readonly List<ConstructionSubtype> constructionSubtypes = new List<ConstructionSubtype>
        {
            new ConstructionSubtype
            {
                Id = 1,
                Type = constructionTypes[0],
                Name = "S1",
                Valuation = "V1",
            },
            new ConstructionSubtype
            {
                Id = 2,
                Type = constructionTypes[1],
                Name = "S2",
                Valuation = "V2",
            },
            new ConstructionSubtype
            {
                Id = 3,
                Type = constructionTypes[2],
                Name = "S3",
                Valuation = "V3",
            },
        };

        public static readonly List<DocType> docTypes = new List<DocType>
        {
            new DocType
            {
                Id = 1,
                Code = "C1",
                Name = "DT1",
            },
            new DocType
            {
                Id = 2,
                Code = "C2",
                Name = "DT2",
            },
            new DocType
            {
                Id = 3,
                Code = "C3",
                Name = "DT3",
            },
        };

        public static readonly List<Doc> docs = new List<Doc>
        {
            new Doc
            {
                Id = 1,
                Mark = marks[0],
                Type = docTypes[0],
                Name = "Name 1",
                Creator = employees[0],
                NumOfPages = 1,
                Form = 1.0f,
            },
            new Doc
            {
                Id = 2,
                Mark = marks[1],
                Type = docTypes[0],
                Name = "Name 2",
                Creator = employees[0],
                NumOfPages = 1,
                Form = 1.0f,
            },
            new Doc
            {
                Id = 3,
                Mark = marks[2],
                Type = docTypes[0],
                Name = "Name 3",
                Creator = employees[0],
                NumOfPages = 1,
                Form = 1.0f,
            },
            new Doc
            {
                Id = 4,
                Mark = marks[0],
                Type = docTypes[1],
                Name = "Name 4",
                Creator = employees[1],
                NumOfPages = 1,
                Form = 1.0f,
            },
            new Doc
            {
                Id = 5,
                Mark = marks[1],
                Type = docTypes[1],
                Name = "Name 5",
                Creator = employees[1],
                NumOfPages = 1,
                Form = 1.0f,
            },
            new Doc
            {
                Id = 6,
                Mark = marks[2],
                Type = docTypes[1],
                Name = "Name 6",
                Creator = employees[1],
                NumOfPages = 1,
                Form = 1.0f,
            },
        };

        public static readonly List<User> users = new List<User>
        {
            new User
            {
                Id = 1,
                Login = "1",
                Password = "1",
                Employee = employees[0],
            },
            new User
            {
                Id = 2,
                Login = "2",
                Password = "2",
                Employee = employees[1],
            },
            new User
            {
                Id = 3,
                Login = "3",
                Password = "3",
                Employee = employees[2],
            },
        };

        public static readonly List<DefaultValues> defaultValues = new List<DefaultValues>
        {
            new DefaultValues
            {
                User = users[0],
                Department = departments[0],
                Creator = employees[0],
                Inspector = employees[0],
                NormContr = employees[0],
            },
            new DefaultValues
            {
                User = users[1],
                Department = departments[1],
                Creator = employees[1],
                Inspector = employees[1],
                NormContr = employees[1],
            },
            new DefaultValues
            {
                User = users[2],
                Department = departments[1],
                Creator = employees[1],
                Inspector = employees[1],
                NormContr = employees[1],
            },
        };

        public static readonly List<ConstructionMaterial> constructionMaterials = new List<ConstructionMaterial>
        {
            new ConstructionMaterial
            {
                Id = 1,
                Name = "CM1",
            },
            new ConstructionMaterial
            {
                Id = 2,
                Name = "CM2",
            },
            new ConstructionMaterial
            {
                Id = 3,
                Name = "CM3",
            },
        };

        public static readonly List<EnvAggressiveness> envAggressiveness = new List<EnvAggressiveness>
        {
            new EnvAggressiveness
            {
                Id = 1,
                Name = "EA1",
            },
            new EnvAggressiveness
            {
                Id = 2,
                Name = "EA2",
            },
            new EnvAggressiveness
            {
                Id = 3,
                Name = "EA3",
            },
        };

        public static readonly List<GasGroup> gasGroups = new List<GasGroup>
        {
            new GasGroup
            {
                Id = 1,
                Name = "GG1",
            },
            new GasGroup
            {
                Id = 2,
                Name = "GG2",
            },
            new GasGroup
            {
                Id = 3,
                Name = "GG3",
            },
        };

        public static readonly List<HighTensileBoltsType> highTensileBoltsTypes = new List<HighTensileBoltsType>
        {
            new HighTensileBoltsType
            {
                Id = 1,
                Name = "HTBT1",
            },
            new HighTensileBoltsType
            {
                Id = 2,
                Name = "HTBT2",
            },
            new HighTensileBoltsType
            {
                Id = 3,
                Name = "HTBT3",
            },
        };

        public static readonly List<OperatingArea> operatingAreas = new List<OperatingArea>
        {
            new OperatingArea
            {
                Id = 1,
                Name = "OA1",
            },
            new OperatingArea
            {
                Id = 2,
                Name = "OA2",
            },
            new OperatingArea
            {
                Id = 3,
                Name = "OA3",
            },
        };

        public static readonly List<PaintworkType> paintworkTypes = new List<PaintworkType>
        {
            new PaintworkType
            {
                Id = 1,
                Name = "PT1",
            },
            new PaintworkType
            {
                Id = 2,
                Name = "PT2",
            },
            new PaintworkType
            {
                Id = 3,
                Name = "PT3",
            },
        };

        public static readonly List<SheetName> sheetNames = new List<SheetName>
        {
            new SheetName
            {
                Id = 1,
                Name = "SN1",
            },
            new SheetName
            {
                Id = 2,
                Name = "SN2",
            },
            new SheetName
            {
                Id = 3,
                Name = "SN3",
            },
        };

        public static readonly List<WeldingControl> weldingControl = new List<WeldingControl>
        {
            new WeldingControl
            {
                Id = 1,
                Name = "WC1",
            },
            new WeldingControl
            {
                Id = 2,
                Name = "WC2",
            },
            new WeldingControl
            {
                Id = 3,
                Name = "WC3",
            },
        };

        // Mark ids: 1-3
        public static readonly List<AttachedDoc> attachedDocs = new List<AttachedDoc>
        {
            new AttachedDoc
            {
                Id = 1,
                Mark = marks[0],
                Designation = "D1",
                Name = "AD1",
            },
            new AttachedDoc
            {
                Id = 2,
                Mark = marks[1],
                Designation = "D2",
                Name = "AD2",
            },
            new AttachedDoc
            {
                Id = 3,
                Mark = marks[2],
                Designation = "D3",
                Name = "AD3",
            },
            // For Conflict Exception
            new AttachedDoc
            {
                Id = 4,
                Mark = marks[0],
                Designation = "D4",
                Name = "AD4",
            },
        };

        public static readonly List<LinkedDocType> linkedDocTypes = new List<LinkedDocType>
        {
            new LinkedDocType
            {
                Id = 1,
                Name = "LDT1",
            },
            new LinkedDocType
            {
                Id = 2,
                Name = "LDT2",
            },
            new LinkedDocType
            {
                Id = 3,
                Name = "LDT3",
            },
        };

        public static readonly List<LinkedDoc> linkedDocs = new List<LinkedDoc>
        {
            new LinkedDoc
            {
                Id = 1,
                Type = linkedDocTypes[0],
                Code = "C1",
                Name = "LD1",
                Designation = "D1",
            },
            new LinkedDoc
            {
                Id = 2,
                Type = linkedDocTypes[0],
                Code = "C2",
                Name = "LD2",
                Designation = "D2",
            },
            new LinkedDoc
            {
                Id = 3,
                Type = linkedDocTypes[1],
                Code = "C3",
                Name = "LD3",
                Designation = "D3",
            },
            new LinkedDoc
            {
                Id = 4,
                Type = linkedDocTypes[1],
                Code = "C4",
                Name = "LD4",
                Designation = "D4",
            },
            new LinkedDoc
            {
                Id = 5,
                Type = linkedDocTypes[2],
                Code = "C5",
                Name = "LD5",
                Designation = "D5",
            },
        };

        public static readonly List<Construction> constructions = new List<Construction>
        {
            new Construction
            {
                Id = 1,
                Specification = specifications[0],
                Name = "N1",
                Type = constructionTypes[0],
                Subtype = constructionSubtypes[0],
                Valuation = "1701",
                NumOfStandardConstructions = 1,
                StandardAlbumCode = "C1",
                HasEdgeBlunting = true,
                HasDynamicLoad = false,
                HasFlangedConnections = true,
                WeldingControl = weldingControl[0],
                PaintworkCoeff = 1,
            },
            new Construction
            {
                Id = 2,
                Specification = specifications[0],
                Name = "N2",
                Type = constructionTypes[1],
                Valuation = "1702",
                NumOfStandardConstructions = 1,
                StandardAlbumCode = "C1",
                HasEdgeBlunting = true,
                HasDynamicLoad = false,
                HasFlangedConnections = true,
                WeldingControl = weldingControl[1],
                PaintworkCoeff = 1,
            },
            new Construction
            {
                Id = 3,
                Specification = specifications[1],
                Name = "N3",
                Type = constructionTypes[0],
                Valuation = "1703",
                NumOfStandardConstructions = 0,
                HasEdgeBlunting = true,
                HasDynamicLoad = false,
                HasFlangedConnections = true,
                WeldingControl = weldingControl[1],
                PaintworkCoeff = 1,
            },
            new Construction
            {
                Id = 4,
                Specification = specifications[1],
                Name = "N4",
                Type = constructionTypes[2],
                Valuation = "1704",
                NumOfStandardConstructions = 0,
                HasEdgeBlunting = true,
                HasDynamicLoad = false,
                HasFlangedConnections = true,
                WeldingControl = weldingControl[2],
                PaintworkCoeff = 2,
            },
            new Construction
            {
                Id = 5,
                Specification = specifications[2],
                Name = "N5",
                Type = constructionTypes[0],
                Valuation = "1705",
                NumOfStandardConstructions = 0,
                HasEdgeBlunting = true,
                HasDynamicLoad = false,
                HasFlangedConnections = true,
                WeldingControl = weldingControl[0],
                PaintworkCoeff = 2,
            },
        };

        public static readonly List<BoltDiameter> boltDiameters = new List<BoltDiameter>
        {
            new BoltDiameter
            {
                Id = 1,
                Diameter = 1,
                NutWeight = 1,
                WasherSteel = "WS1",
                WasherWeight = 1,
                WasherThickness = 1,
                BoltTechSpec = "BTS1",
                StrengthClass = "S1",
                NutTechSpec = "NTS1",
                WasherTechSpec = "WTS1",
            },
            new BoltDiameter
            {
                Id = 2,
                Diameter = 2,
                NutWeight = 2,
                WasherSteel = "WS2",
                WasherWeight = 2,
                WasherThickness = 2,
                BoltTechSpec = "BTS2",
                StrengthClass = "S2",
                NutTechSpec = "NTS2",
                WasherTechSpec = "WTS2",
            },
            new BoltDiameter
            {
                Id = 3,
                Diameter = 3,
                NutWeight = 3,
                WasherSteel = "WS3",
                WasherWeight = 3,
                WasherThickness = 3,
                BoltTechSpec = "BTS3",
                StrengthClass = "S3",
                NutTechSpec = "NTS3",
                WasherTechSpec = "WTS3",
            },
        };

        public static readonly List<BoltLength> boltLengths = new List<BoltLength>
        {
            new BoltLength
            {
                Id = 1,
                Diameter = boltDiameters[0],
                Length = 1,
                ScrewLength = 1,
                Weight = 1,
            },
            new BoltLength
            {
                Id = 2,
                Diameter = boltDiameters[1],
                Length = 2,
                ScrewLength = 2,
                Weight = 2,
            },
            new BoltLength
            {
                Id = 3,
                Diameter = boltDiameters[2],
                Length = 3,
                ScrewLength = 3,
                Weight = 3,
            },
        };

        public static readonly List<ConstructionBolt> constructionBolts = new List<ConstructionBolt>
        {
            new ConstructionBolt
            {
                Id = 1,
                Construction = constructions[0],
                Diameter = boltDiameters[0],
                Packet = 1,
                Num = 1,
                NutNum = 1,
                WasherNum = 1,
            },
            new ConstructionBolt
            {
                Id = 2,
                Construction = constructions[0],
                Diameter = boltDiameters[1],
                Packet = 2,
                Num = 2,
                NutNum = 2,
                WasherNum = 2,
            },
            new ConstructionBolt
            {
                Id = 3,
                Construction = constructions[1],
                Diameter = boltDiameters[0],
                Packet = 3,
                Num = 3,
                NutNum = 3,
                WasherNum = 3,
            },
            new ConstructionBolt
            {
                Id = 4,
                Construction = constructions[1],
                Diameter = boltDiameters[2],
                Packet = 4,
                Num = 4,
                NutNum = 4,
                WasherNum = 4,
            },
            new ConstructionBolt
            {
                Id = 5,
                Construction = constructions[2],
                Diameter = boltDiameters[1],
                Packet = 5,
                Num = 5,
                NutNum = 5,
                WasherNum = 5,
            },
        };

        public static readonly List<ProfileClass> profileClasses = new List<ProfileClass>
        {
            new ProfileClass
            {
                Id = 1,
                Name = "N1",
                Note = "N1",
            },
            new ProfileClass
            {
                Id = 2,
                Name = "N2",
            },
            new ProfileClass
            {
                Id = 3,
                Name = "N3",
            },
        };
        public static readonly List<ProfileType> profileTypes = new List<ProfileType>
        {
            new ProfileType
            {
                Id = 1,
                Name = "N1",
            },
            new ProfileType
            {
                Id = 2,
                Name = "N2",
            },
            new ProfileType
            {
                Id = 3,
                Name = "N3",
            },
        };
        public static readonly List<Steel> steel = new List<Steel>
        {
            new Steel
            {
                Id = 1,
                Name = "N1",
                Standard = "S1",
                Strength = 1,
            },
            new Steel
            {
                Id = 2,
                Name = "N2",
                Standard = "S2",
                Strength = 2,
            },
            new Steel
            {
                Id = 3,
                Name = "N3",
                Standard = "S3",
            },
        };
        public static readonly List<Profile> profiles = new List<Profile>
        {
            new Profile
            {
                Id = 1,
                Class = profileClasses[0],
                Name = "P1",
                Symbol = "S1",
                Weight = 1,
                Area = 1,
                Type = profileTypes[0],

            },
            new Profile
            {
                Id = 2,
                Class = profileClasses[1],
                Name = "P2",
                Symbol = "S2",
                Weight = 2,
                Area = 2,
                Type = profileTypes[1],
            },
            new Profile
            {
                Id = 3,
                Class = profileClasses[2],
                Name = "P3",
                Symbol = "S3",
                Weight = 3,
                Area = 3,
                Type = profileTypes[2],
            },
        };
        public static readonly List<ConstructionElement> constructionElements = new List<ConstructionElement>
        {
            new ConstructionElement
            {
                Id = 1,
                Construction = constructions[0],
                Profile = profiles[0],
                Steel = steel[0],
                Length = 1.0f,
            },
            new ConstructionElement
            {
                Id = 2,
                Construction = constructions[0],
                Profile = profiles[1],
                Steel = steel[1],
                Length = 1.0f,
            },
            new ConstructionElement
            {
                Id = 3,
                Construction = constructions[0],
                Profile = profiles[2],
                Steel = steel[2],
                Length = 1.0f,
            },
            new ConstructionElement
            {
                Id = 4,
                Construction = constructions[1],
                Profile = profiles[0],
                Steel = steel[0],
                Length = 1.0f,
            },
            new ConstructionElement
            {
                Id = 5,
                Construction = constructions[1],
                Profile = profiles[1],
                Steel = steel[1],
                Length = 1.0f,
            },
            new ConstructionElement
            {
                Id = 6,
                Construction = constructions[1],
                Profile = profiles[2],
                Steel = steel[2],
                Length = 1.0f,
            },
            new ConstructionElement
            {
                Id = 7,
                Construction = constructions[2],
                Profile = profiles[0],
                Steel = steel[0],
                Length = 1.0f,
            },
            new ConstructionElement
            {
                Id = 8,
                Construction = constructions[2],
                Profile = profiles[1],
                Steel = steel[1],
                Length = 1.0f,
            },
            new ConstructionElement
            {
                Id = 9,
                Construction = constructions[2],
                Profile = profiles[2],
                Steel = steel[2],
                Length = 1.0f,
            },
        };

        public static readonly List<StandardConstruction> standardConstructions = new List<StandardConstruction>
        {
            new StandardConstruction
            {
                Id = 1,
                Specification = specifications[0],
                Name = "N1",
                Num = 1,
                Sheet = "S1",
                Weight = 1.0f,
            },
            new StandardConstruction
            {
                Id = 2,
                Specification = specifications[1],
                Name = "N2",
                Num = 2,
                Sheet = "S2",
                Weight = 2.0f,
            },
            new StandardConstruction
            {
                Id = 3,
                Specification = specifications[2],
                Name = "N3",
                Num = 3,
                Sheet = "S3",
                Weight = 3.0f,
            },
            new StandardConstruction
            {
                Id = 4,
                Specification = specifications[0],
                Name = "N4",
                Num = 4,
                Sheet = "S4",
                Weight = 4.0f,
            },
        };

        public static readonly List<MarkOperatingConditions> markOperatingConditions = new List<MarkOperatingConditions>
        {
            new MarkOperatingConditions
            {
                Mark = marks[0],
                SafetyCoeff = 1.0f,
                EnvAggressiveness = envAggressiveness[0],
                Temperature = -34,
                OperatingArea = operatingAreas[0],
                GasGroup = gasGroups[0],
                ConstructionMaterial = constructionMaterials[0],
                PaintworkType = paintworkTypes[0],
                HighTensileBoltsType = highTensileBoltsTypes[0],
            },
            new MarkOperatingConditions
            {
                Mark = marks[1],
                SafetyCoeff = 1.0f,
                EnvAggressiveness = envAggressiveness[1],
                Temperature = -34,
                OperatingArea = operatingAreas[1],
                GasGroup = gasGroups[0],
                ConstructionMaterial = constructionMaterials[0],
                PaintworkType = paintworkTypes[1],
                HighTensileBoltsType = highTensileBoltsTypes[0],
            },
        };

        public static readonly List<EstimateTask> estimateTask = new List<EstimateTask>
        {
            new EstimateTask
            {
                Mark = marks[0],
                TaskText = "T1",
            },
            new EstimateTask
            {
                Mark = marks[1],
                TaskText = "T2",
            },
            new EstimateTask
            {
                Mark = marks[2],
                TaskText = "T3",
            },
        };

        public static readonly List<AdditionalWork> additionalWork = new List<AdditionalWork>
        {
            new AdditionalWork
            {
                Id = 1,
                Mark = marks[0],
                Employee = employees[0],
                Valuation = 1,
                MetalOrder = 1,
            },
            new AdditionalWork
            {
                Id = 2,
                Mark = marks[0],
                Employee = employees[1],
                Valuation = 1,
                MetalOrder = 1,
            },
            new AdditionalWork
            {
                Id = 3,
                Mark = marks[1],
                Employee = employees[0],
                Valuation = 2,
                MetalOrder = 2,
            },
            new AdditionalWork
            {
                Id = 4,
                Mark = marks[1],
                Employee = employees[1],
                Valuation = 2,
                MetalOrder = 2,
            },
            new AdditionalWork
            {
                Id = 5,
                Mark = marks[2],
                Employee = employees[2],
                Valuation = 3,
                MetalOrder = 3,
            },
            new AdditionalWork
            {
                Id = 6,
                Mark = marks[2],
                Employee = employees[1],
                Valuation = 3,
                MetalOrder = 3,
            },
        };

        // LinkedDoc ids: 1-3
        public static readonly List<MarkLinkedDoc> markLinkedDocs = new List<MarkLinkedDoc>
        {
            new MarkLinkedDoc
            {
                Id = 1,
                Mark = marks[0],
                LinkedDoc = linkedDocs[0],
            },
            new MarkLinkedDoc
            {
                Id = 2,
                Mark = marks[0],
                LinkedDoc = linkedDocs[1],
            },
            new MarkLinkedDoc
            {
                Id = 3,
                Mark = marks[1],
                LinkedDoc = linkedDocs[0],
            },
            new MarkLinkedDoc
            {
                Id = 4,
                Mark = marks[1],
                LinkedDoc = linkedDocs[1],
            },
            new MarkLinkedDoc
            {
                Id = 5,
                Mark = marks[2],
                LinkedDoc = linkedDocs[0],
            },
            new MarkLinkedDoc
            {
                Id = 6,
                Mark = marks[2],
                LinkedDoc = linkedDocs[2],
            },

        };

        public static readonly List<GeneralDataSection> generalDataSections = new List<GeneralDataSection>
        {
            new GeneralDataSection
            {
                Id = 1,
                Name = "S1",
                User = users[0],
            },
            new GeneralDataSection
            {
                Id = 2,
                Name = "S2",
                User = users[0],
            },
            new GeneralDataSection
            {
                Id = 3,
                Name = "S3",
                User = users[0],
            },
            new GeneralDataSection
            {
                Id = 4,
                Name = "S4",
                User = users[1],
            },
            new GeneralDataSection
            {
                Id = 5,
                Name = "S5",
                User = users[1],
            },
            new GeneralDataSection
            {
                Id = 6,
                Name = "S6",
                User = users[2],
            },
        };

        public static readonly List<GeneralDataPoint> generalDataPoints = new List<GeneralDataPoint>
        {
            new GeneralDataPoint
            {
                Id = 1,
                Section = generalDataSections[0],
                Text = "gdp1",
                OrderNum = 1,
            },
            new GeneralDataPoint
            {
                Id = 2,
                Section = generalDataSections[1],
                Text = "gdp2",
                OrderNum = 2,
            },
            new GeneralDataPoint
            {
                Id = 3,
                Section = generalDataSections[0],
                Text = "gdp3",
                OrderNum = 1,
            },
            new GeneralDataPoint
            {
                Id = 4,
                Section = generalDataSections[1],
                Text = "gdp4",
                OrderNum = 2,
            },
            new GeneralDataPoint
            {
                Id = 5,
                Section = generalDataSections[0],
                Text = "gdp5",
                OrderNum = 1,
            },
            new GeneralDataPoint
            {
                Id = 6,
                Section = generalDataSections[1],
                Text = "gdp6",
                OrderNum = 2,
            },
            new GeneralDataPoint
            {
                Id = 7,
                Section = generalDataSections[0],
                Text = "mgdp7",
                OrderNum = 2,
            },
        };

        public static readonly List<MarkGeneralDataSection> markGeneralDataSections = new List<MarkGeneralDataSection>
        {
            new MarkGeneralDataSection
            {
                Id = 1,
                Name = "S1",
                Mark = marks[0],
            },
            new MarkGeneralDataSection
            {
                Id = 2,
                Name = "S2",
                Mark = marks[1],
            },
            new MarkGeneralDataSection
            {
                Id = 3,
                Name = "S3",
                Mark = marks[2],
            },
        };

        public static readonly List<MarkGeneralDataPoint> markGeneralDataPoints = new List<MarkGeneralDataPoint>
        {
            new MarkGeneralDataPoint
            {
                Id = 1,
                Section = markGeneralDataSections[0],
                Text = "mgdp1",
                OrderNum = 1,
            },
            new MarkGeneralDataPoint
            {
                Id = 2,
                Section = markGeneralDataSections[1],
                Text = "mgdp2",
                OrderNum = 2,
            },
            new MarkGeneralDataPoint
            {
                Id = 3,
                Section = markGeneralDataSections[0],
                Text = "mgdp3",
                OrderNum = 1,
            },
            new MarkGeneralDataPoint
            {
                Id = 4,
                Section = markGeneralDataSections[1],
                Text = "mgdp4",
                OrderNum = 2,
            },
            new MarkGeneralDataPoint
            {
                Id = 5,
                Section = markGeneralDataSections[0],
                Text = "mgdp5",
                OrderNum = 1,
            },
            new MarkGeneralDataPoint
            {
                Id = 6,
                Section = markGeneralDataSections[1],
                Text = "mgdp6",
                OrderNum = 2,
            },
            new MarkGeneralDataPoint
            {
                Id = 7,
                Section = markGeneralDataSections[0],
                Text = "mgdp7",
                OrderNum = 2,
            },
        };
    }
}
