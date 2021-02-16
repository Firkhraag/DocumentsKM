using DocumentsKM.Models;
using DocumentsKM.Data;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System;
using System.Text;

namespace DocumentsKM.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly int _departmentHeadPosId = 7;
        private readonly int _sheetDocTypeId = 1;
        
        private readonly IMarkRepo _markRepo;
        private readonly IMarkApprovalRepo _markApprovalRepo;
        private readonly IEmployeeRepo _employeeRepo;

        private readonly IDocRepo _docRepo;
        private readonly IMarkGeneralDataPointRepo _markGeneralDataPointRepo;
        private readonly IMarkLinkedDocRepo _markLinkedDocRepo;
        private readonly IAttachedDocRepo _attachedDocRepo;

        private readonly IBoltDiameterRepo _boltDiameterRepo;
        private readonly IBoltLengthRepo _boltLengthRepo;
        private readonly IConstructionBoltRepo _constructionBoltRepo;

        public DocumentService(
            IMarkRepo markRepo,
            IMarkApprovalRepo markApprovalRepo,
            IEmployeeRepo employeeRepo,

            IDocRepo docRepo,
            IMarkGeneralDataPointRepo markGeneralDataPointRepo,
            IMarkLinkedDocRepo markLinkedDocRepo,
            IAttachedDocRepo attachedDocRepo,
            
            IBoltDiameterRepo boltDiameterRepo,
            IBoltLengthRepo boltLengthRepo,
            IConstructionBoltRepo constructionBoltRepo)
        {
            _markRepo = markRepo;
            _markApprovalRepo = markApprovalRepo;
            _employeeRepo = employeeRepo;

            _docRepo = docRepo;
            _markGeneralDataPointRepo = markGeneralDataPointRepo;
            _markLinkedDocRepo = markLinkedDocRepo;
            _attachedDocRepo = attachedDocRepo;

            _boltDiameterRepo = boltDiameterRepo;
            _boltLengthRepo = boltLengthRepo;
            _constructionBoltRepo = constructionBoltRepo;
        }

        public MemoryStream GetGeneralDataDocument(int markId)
        {
            var mark = _markRepo.GetById(markId);
            if (mark == null)
                throw new ArgumentNullException(nameof(mark));
            var markApprovals = _markApprovalRepo.GetAllByMarkId(markId);
            var subnode = mark.Subnode;
            var node = subnode.Node;
            var project = node.Project;

            var markGeneralDataPoints = _markGeneralDataPointRepo.GetAllByMarkId(
                markId).OrderByDescending(
                    v => v.Section.OrderNum).ThenByDescending(v => v.OrderNum);
            var sheets = _docRepo.GetAllByMarkIdAndDocType(markId, _sheetDocTypeId);

            var path = "D:\\Dev\\Gipromez\\word\\template.docx";
            var memory = GetStreamFromTemplate(path);
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memory, true))
            {
                var markName = MakeMarkName(
                    project.BaseSeries, node.Code, subnode.Code, mark.Code);
                (var complexName, var objectName) = MakeComplexAndObjectName(
                    project.Name, node.Name, subnode.Name, mark.Name);
                GeneralDataDocument.AppendList(wordDoc, markGeneralDataPoints);

                GeneralDataDocument.AppendToSheetTable(wordDoc, sheets.ToList());
                GeneralDataDocument.AppendToLinkedAndAttachedDocsTable(
                    wordDoc,
                    _markLinkedDocRepo.GetAllByMarkId(markId).ToList(),
                    _attachedDocRepo.GetAllByMarkId(markId).ToList());
                AppendToBigFooterTable(
                    wordDoc,
                    markName,
                    complexName,
                    objectName,
                    sheets.Count(),
                    mark,
                    markApprovals.ToList());
                AppendToSmallFooterTable(wordDoc, markName);
            }
            memory.Seek(0, SeekOrigin.Begin);
            return memory;
        }

        public MemoryStream GetBoltDocument(int markId)
        {
            var mark = _markRepo.GetById(markId);
            if (mark == null)
                throw new ArgumentNullException(nameof(mark));
            var subnode = mark.Subnode;
            var node = subnode.Node;
            var project = node.Project;

            var constructionBolts = _constructionBoltRepo.GetAllByMarkId(markId);
            var boltLengths = new List<BoltLength> {};
            foreach (var bolt in constructionBolts)
            {
                var arr = _boltLengthRepo.GetAllByDiameterId(bolt.Diameter.Id);
                var bl = arr.Where(
                    v => v.Length >= bolt.Packet + v.ScrewLength).Aggregate(
                        (i1, i2) => i1.Length < i2.Length ? i1 : i2);
                boltLengths.Add(bl);
            }

            var path = "D:\\Dev\\Gipromez\\word\\template_bolt.docx";
            var memory = GetStreamFromTemplate(path);
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memory, true))
            {
                var markName = MakeMarkName(
                    project.BaseSeries, node.Code, subnode.Code, mark.Code);
                BoltDocument.AppendToTable(wordDoc, constructionBolts.ToList(), boltLengths);
                AppendToSmallFooterTable(wordDoc, markName);
            }
            memory.Seek(0, SeekOrigin.Begin);
            return memory;
        }

        private void AppendToBigFooterTable(
            WordprocessingDocument document,
            string markFullCodeName,
            string complexName,
            string objectName,
            int sheetsCount,
            Mark mark,
            List<MarkApproval> markApprovals)
        {
            const int firstPartColumnIndexToFill = 6;
            const int secondPartColumnIndexToFill = 4;

            MainDocumentPart mainPart = document.MainDocumentPart;
            var commonFooter = mainPart.FooterParts.LastOrDefault();
            var t = commonFooter.RootElement.Descendants<Table>().FirstOrDefault();
            var trArr = t.Descendants<TableRow>().ToList();

            var trCells = trArr[0].Descendants<TableCell>().ToList();
            var tc = trCells[firstPartColumnIndexToFill];
            var p = tc.GetFirstChild<Paragraph>();
            p.Append(GetWordTextElement(markFullCodeName, 22));

            trCells = trArr[2].Descendants<TableCell>().ToList();
            tc = trCells[firstPartColumnIndexToFill];
            p = tc.GetFirstChild<Paragraph>();
            p.Append(GetWordTextElement(complexName, 22));

            trCells = trArr[5].Descendants<TableCell>().ToList();

            // tc = trCells[1];
            // p = tc.GetFirstChild<Paragraph>();
            // p.Append(GetWordTextElement("E1", 22));

            tc = trCells[secondPartColumnIndexToFill];
            p = tc.GetFirstChild<Paragraph>();
            p.Append(GetWordTextElement(objectName, 20));

            trCells = trArr[6].Descendants<TableCell>().ToList();

            if (mark.ChiefSpecialist != null)
            {
                tc = trCells[1];
                p = tc.GetFirstChild<Paragraph>();
                p.Append(GetWordTextElement(mark.ChiefSpecialist.Name, 22));
            }

            tc = trCells.LastOrDefault();
            p = tc.GetFirstChild<Paragraph>();
            p.Append(GetWordTextElement(sheetsCount.ToString(), 22));

            trCells = trArr[7].Descendants<TableCell>().ToList();
            tc = trCells[1];
            p = tc.GetFirstChild<Paragraph>();

            var departmentHeadArr = _employeeRepo.GetAllByDepartmentIdAndPosition(
                mark.Department.Id,
                _departmentHeadPosId);
            if (departmentHeadArr.Count() != 1)
                throw new ConflictException();

            p.Append(GetWordTextElement(
                departmentHeadArr.ToList()[0].Name, 22));

            trCells = trArr[8].Descendants<TableCell>().ToList();
            tc = trCells[1];
            p = tc.GetFirstChild<Paragraph>();
            p.Append(GetWordTextElement(mark.Subnode.Node.ChiefEngineer.Name, 22));

            // trCells = trArr[9].Descendants<TableCell>().ToList();
            // tc = trCells[1];
            // p = tc.GetFirstChild<Paragraph>();
            // p.Append(GetWordTextElement("E5", 22));

            // trCells = trArr[10].Descendants<TableCell>().ToList();
            // tc = trCells[1];
            // p = tc.GetFirstChild<Paragraph>();
            // p.Append(GetWordTextElement("E6", 22));

            for (int i = 0; i < markApprovals.Count(); i++)
            {
                if (i < 3)
                {
                    trCells = trArr[12 + i].Descendants<TableCell>().ToList();
                    tc = trCells[0];
                    p = tc.GetFirstChild<Paragraph>();
                    p.Append(GetWordTextElement(markApprovals[i].Employee.Department.Name, 22));
                    tc = trCells[1];
                }
                else if (i == 3)
                {
                    trCells = trArr[8 + i].Descendants<TableCell>().ToList();
                    tc = trCells[1];
                    p = tc.GetFirstChild<Paragraph>();
                    p.Append(GetWordTextElement(markApprovals[i].Employee.Department.Name, 22));
                    tc = trCells[2];
                }
                else
                {
                    trCells = trArr[8 + i].Descendants<TableCell>().ToList();
                    tc = trCells[4];
                    p = tc.GetFirstChild<Paragraph>();
                    p.Append(GetWordTextElement(markApprovals[i].Employee.Department.Name, 22));
                    tc = trCells[5];
                }
                p = tc.GetFirstChild<Paragraph>();
                p.Append(GetWordTextElement(markApprovals[i].Employee.Name, 22));
            }
        }

        private void AppendToSmallFooterTable(WordprocessingDocument document, string markName)
        {
            var columnIndexToFill = 6;
            MainDocumentPart mainPart = document.MainDocumentPart;
            var commonFooter = mainPart.FooterParts.FirstOrDefault();
            var t = commonFooter.RootElement.Descendants<Table>().FirstOrDefault();

            var firstTr = t.Descendants<TableRow>().FirstOrDefault();
            var firstTrCells = firstTr.Descendants<TableCell>().ToList();
            var tc = firstTrCells[columnIndexToFill];
            var p = tc.GetFirstChild<Paragraph>();
            p.Append(GetWordTextElement(markName, 26));
        }

        private Run GetWordTextElement(
            string text,
            int fSize,
            bool isUnderlined = false,
            bool isSuperscript = false)
        {
            Run run = new Run();
            RunProperties runProperties = run.AppendChild(new RunProperties());
            Italic italic = new Italic();
            italic.Val = OnOffValue.FromBoolean(true);
            FontSize fontSize = new FontSize() { Val = fSize.ToString() };
            runProperties.AppendChild(italic);
            runProperties.AppendChild(fontSize);
            RunFonts font = new RunFonts()
            {
                Ascii = "GOST type B",
                HighAnsi = "GOST type B",
                ComplexScript = "GOST type B"
            };
            runProperties.Append(font);
            if (isUnderlined)
            {
                Underline underline = new Underline()
                {
                    Val = UnderlineValues.Single,
                };
                runProperties.Append(underline);
            }
            if (isSuperscript)
            {
                VerticalTextAlignment vertAlign = new VerticalTextAlignment()
                {
                    Val = VerticalPositionValues.Superscript,
                };
                runProperties.Append(vertAlign);
            }
            run.AppendChild(new Text()
            {
                Text = text,
                Space = SpaceProcessingModeValues.Preserve,
            });
            return run;
        }

        private MemoryStream GetStreamFromTemplate(string inputPath)
        {
            MemoryStream documentStream;
            using (Stream stream = File.OpenRead(inputPath))
            {
                documentStream = new MemoryStream((int)stream.Length);
                stream.CopyTo(documentStream);
                documentStream.Position = 0L;
            }
            using (WordprocessingDocument template = WordprocessingDocument.Open(documentStream, true))
            {
                template.ChangeDocumentType(DocumentFormat.OpenXml.WordprocessingDocumentType.Document);
                MainDocumentPart mainPart = template.MainDocumentPart;
                mainPart.DocumentSettingsPart.AddExternalRelationship(
                    "http://schemas.openxmlformats.org/officeDocument/2006/relationships/attachedTemplate",
                new Uri(inputPath, UriKind.Absolute));

                mainPart.Document.Save();
            }
            return documentStream;
        }

        private string MakeMarkName(
            string projectBaseSeries,
            string nodeCode,
            string subnodeCode,
            string markCode)
        {
            var markName = new StringBuilder(projectBaseSeries, 255);
            var overhaul = "";
            if (nodeCode != "-" && nodeCode != "")
            {
                var nodeCodeSplitted = nodeCode.Split('-');
                var nodeValue = nodeCodeSplitted[0];
                if (nodeCodeSplitted.Count() == 2)
                {
                    overhaul = nodeCodeSplitted[1];
                }

                markName.Append($".{nodeValue}");
            }
            if (subnodeCode != "-" && subnodeCode != "")
            {
                markName.Append($".{subnodeCode}");
                if (overhaul != "")
                {
                    markName.Append($"-{overhaul}");
                }
            }
            if (markCode != "-" && markCode != "")
            {
                markName.Append($"-{markCode}");
            }
            return markName.ToString();
        }

        private (string, string) MakeComplexAndObjectName(
            string projectName,
            string nodeName,
            string subnodeName,
            string markName)
        {
            var complexName = projectName;
            var objectName = nodeName + ". " + subnodeName + ". " + markName;

            return (complexName, objectName);
        }
    }
}
