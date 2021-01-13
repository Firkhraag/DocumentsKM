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
    public class GeneralDataDocService : IGeneralDataDocService
    {
        private IMarkGeneralDataPointRepo _markGeneralDataPointRepo;
        private IMarkRepo _markRepo;
        private IMarkApprovalRepo _markApprovalRepo;
        private IEmployeeRepo _employeeRepo;
        private int departmentHeadPosId = 7;

        public GeneralDataDocService(
            IMarkGeneralDataPointRepo markGeneralDataPointRepo,
            IMarkRepo markRepo,
            IMarkApprovalRepo markApprovalRepo,
            IEmployeeRepo employeeRepo)
        {
            _markGeneralDataPointRepo = markGeneralDataPointRepo;
            _markRepo = markRepo;
            _markApprovalRepo = markApprovalRepo;
            _employeeRepo = employeeRepo;
        }

        public MemoryStream GetDocByMarkId(int markId)
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

            var path = "D:\\Dev\\Gipromez\\word\\template.docx";

            var g = Guid.NewGuid();
            string guidString = Convert.ToBase64String(g.ToByteArray());
            guidString = guidString.Replace("=","");
            guidString = guidString.Replace("+","");
            var outputPath = $"D:\\Dev\\Gipromez\\word\\{guidString}.docx";

            var memory = GetStreamFromTemplate(path);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memory, true))
            {
                var markName = MakeMarkName(
                    project.BaseSeries, node.Code, subnode.Code, mark.Code);
                (var complexName, var objectName) = MakeComplexAndObjectName(
                    project.Name, node.Name, subnode.Name, mark.Name);
                AppendList(wordDoc, markGeneralDataPoints);

                AppendToTable(wordDoc, "Лист");
                AppendToTable(wordDoc, "Обозначение");
                AppendToFirstFooterTable(
                    wordDoc,
                    markName,
                    complexName,
                    objectName,
                    mark,
                    markApprovals.ToList());
                AppendToMainFooterTable(wordDoc, markName);
            }
            memory.Seek(0, SeekOrigin.Begin);
            return memory;
        }

        private void AppendToFirstFooterTable(
            WordprocessingDocument document,
            string markFullCodeName,
            string complexName,
            string objectName,
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
            p.Append(GetWordTextElement("lists", 22));

            trCells = trArr[7].Descendants<TableCell>().ToList();
            tc = trCells[1];
            p = tc.GetFirstChild<Paragraph>();

            var departmentHeadArr = _employeeRepo.GetAllByDepartmentIdAndPosition(
                mark.Department.Id,
                departmentHeadPosId);
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

        private void AppendToMainFooterTable(WordprocessingDocument document, string markName)
        {
            var columnIndexToFill = 6;
            MainDocumentPart mainPart = document.MainDocumentPart;
            var commonFooter = mainPart.FooterParts.FirstOrDefault();
            var t = commonFooter.RootElement.Descendants<Table>().FirstOrDefault();

            var firstTr = t.Descendants<TableRow>().FirstOrDefault();
            var firstTrCells = firstTr.Descendants<TableCell>().ToList();
            var tc = firstTrCells[columnIndexToFill];
            var p = tc.GetFirstChild<Paragraph>();
            p.Append(GetWordTextElement(markName, 24));
        }

        private void AppendToTable(WordprocessingDocument document, string uniqueColumnName)
        {
            Body body = document.MainDocumentPart.Document.Body;
            var t = body.Descendants<Table>().FirstOrDefault(tbl => tbl.InnerText.Contains(uniqueColumnName));

            var firstTr = t.Descendants<TableRow>().ToList()[1];
            var clonedFirstTr = firstTr.CloneNode(true);

            var firstTrCells = firstTr.Descendants<TableCell>().ToList();
            foreach (var tc in firstTrCells)
            {
                var p = tc.GetFirstChild<Paragraph>();
                p.Append(GetWordTextElement("111", 24));
            }

            for (var i = 0; i < 3; i++)
            {
                var newTr = clonedFirstTr.CloneNode(true);
                firstTrCells = newTr.Descendants<TableCell>().ToList();
                foreach (var tc in firstTrCells)
                {
                    var p = tc.GetFirstChild<Paragraph>();
                    p.Append(GetWordTextElement("222", 24));
                }
                t.Append(newTr);
            }
        }

        private void AppendList(
            WordprocessingDocument wordDoc, IEnumerable<MarkGeneralDataPoint> markGeneralDataPoints)
        {
            NumberingDefinitionsPart numberingPart = wordDoc.MainDocumentPart.NumberingDefinitionsPart;
            if (numberingPart == null)
            {
                numberingPart = wordDoc.MainDocumentPart.AddNewPart<NumberingDefinitionsPart>(
                    "NumberingDefinitionsPart1");
                Numbering element = new Numbering();
                element.Save(numberingPart);
            }

            var abstractNumberId = numberingPart.Numbering.Elements<AbstractNum>().Count() + 1;
            var abstractLevel = new Level(
                new NumberingFormat()
                {
                    Val = NumberFormatValues.Decimal
                },
                new LevelText()
                {
                    Val = "%1"
                },
                new StartNumberingValue()
                {
                    Val = 1,
                },
                new RunProperties()
                {
                    RunFonts = new RunFonts()
                    {
                        Ascii = "Calibri",
                        HighAnsi = "Calibri",
                        ComplexScript  = "Calibri"
                    },
                    Italic = new Italic()
                    {
                        Val = OnOffValue.FromBoolean(true)
                    },
                    FontSize = new FontSize()
                    {
                        Val = 24.ToString(),
                    }
                })
            {
                LevelIndex = 0
            };
            var abstractLevel2 = new Level(
                new NumberingFormat()
                {
                    Val = NumberFormatValues.Decimal
                },
                new LevelText()
                {
                    Val = "%1.%2"
                },
                new StartNumberingValue()
                {
                    Val = 1,
                },
                new RunProperties()
                {
                    RunFonts = new RunFonts()
                    {
                        Ascii = "Calibri",
                        HighAnsi = "Calibri",
                        ComplexScript  = "Calibri"
                    },
                    Italic = new Italic()
                    {
                        Val = OnOffValue.FromBoolean(true)
                    },
                    FontSize = new FontSize()
                    {
                        Val = 24.ToString(),
                    }
                })
            {
                LevelIndex = 1
            };
            var abstractLevel3 = new Level(
                new NumberingFormat()
                {
                    Val = NumberFormatValues.Bullet
                },
                new LevelText()
                {
                    Val = "–"
                },
                new RunProperties()
                {
                    RunFonts = new RunFonts()
                    {
                        Ascii = "Calibri",
                        HighAnsi = "Calibri",
                        ComplexScript  = "Calibri"
                    },
                })
            {
                LevelIndex = 2
            };
            var abstractLevel4 = new Level(
                new LevelSuffix()
                {
                    Val = LevelSuffixValues.Space
                })
            {
                LevelIndex = 3
            };

            var abstractNum = new AbstractNum(
                abstractLevel, abstractLevel2, abstractLevel3, abstractLevel4)
            {
                AbstractNumberId = abstractNumberId
            };
            if (abstractNumberId == 1)
            {
                numberingPart.Numbering.Append(abstractNum);
            }
            else
            {
                AbstractNum lastAbstractNum = numberingPart.Numbering.Elements<AbstractNum>().Last();
                numberingPart.Numbering.InsertAfter(abstractNum, lastAbstractNum);
            }

            var numberId = numberingPart.Numbering.Elements<NumberingInstance>().Count() + 1;
            NumberingInstance numberingInstance = new NumberingInstance() {NumberID = numberId};
            AbstractNumId abstractNumId = new AbstractNumId() {Val = abstractNumberId};
            numberingInstance.Append(abstractNumId);

            if (numberId == 1)
            {
                numberingPart.Numbering.Append(numberingInstance);
            }
            else
            {
                var lastNumberingInstance = numberingPart.Numbering.Elements<NumberingInstance>().Last();
                numberingPart.Numbering.InsertAfter(numberingInstance, lastNumberingInstance);
            }

            Body body = wordDoc.MainDocumentPart.Document.Body;
            var markGeneralDataPointsList = markGeneralDataPoints.ToList();
            for (var i = 0; i < markGeneralDataPoints.Count(); i++)
            {
                var item = markGeneralDataPointsList[i];
                var spacingBetweenLines = new SpacingBetweenLines() { After = "120", Line="300" };
                var indentation = new Indentation() { Left = "360", Right="360", FirstLine = "720" };

                NumberingProperties numberingProperties;
                var pointText = item.Text;
                if (item.OrderNum == 1)
                {
                    numberingProperties = new NumberingProperties(
                        new NumberingLevelReference() {Val = 0}, new NumberingId() {Val = numberId});
                }
                else if (item.Text[0] == '#' && item.Text[1] == ' ')
                {
                    numberingProperties = new NumberingProperties(
                        new NumberingLevelReference() {Val = 1}, new NumberingId() {Val = numberId});

                    pointText = pointText.Substring(2) + ".";
                }
                else if (item.Text[0] == '-' && item.Text[1] == ' ')
                {
                    numberingProperties = new NumberingProperties(
                        new NumberingLevelReference() {Val = 2}, new NumberingId() {Val = numberId});

                    if (i == 0)
                    {
                        pointText = pointText.Substring(2) + ".";
                    }
                    else if (markGeneralDataPointsList[i - 1].OrderNum == 1)
                    {
                        pointText = pointText.Substring(2) + ".";
                    }
                    else if (markGeneralDataPointsList[i - 1].Text[0] == '#' &&
                        markGeneralDataPointsList[i - 1].Text[1] == ' ')
                    {
                        pointText = pointText.Substring(2) + ".";
                    }
                    else
                    {
                        pointText = pointText.Substring(2) + ";";
                    }
                }
                else
                {
                    numberingProperties = new NumberingProperties(
                        new NumberingLevelReference() {Val = 3}, new NumberingId() {Val = numberId});
                    pointText = pointText + ".";
                    indentation = new Indentation() { Left = "360", Right="360", FirstLine = "640" };
                }
                var paragraphProperties = new ParagraphProperties(
                    numberingProperties, spacingBetweenLines, indentation);

                var newPara = new Paragraph(paragraphProperties);
                newPara.AppendChild(GetWordTextElement(pointText, 24));
                body.PrependChild(newPara);                
            }
        }

        private Run GetWordTextElement(string text, int fSize)
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
                Ascii = "Calibri",
                HighAnsi = "Calibri",
                ComplexScript  = "Calibri"
            };
            runProperties.Append(font);
            run.AppendChild(new Text(text));
            return run;
        }

        private string MakeMarkName(
            string projectBaseSeries,
            string nodeCode,
            string subnodeCode,
            string markCode)
        {
            var markName = new StringBuilder(projectBaseSeries, 255);
            var overhaul = "";
            if (nodeCode != "-" && nodeCode != "") {
                var nodeCodeSplitted = nodeCode.Split('-');
                var nodeValue = nodeCodeSplitted[0];
                if (nodeCodeSplitted.Count() == 2) {
                    overhaul = nodeCodeSplitted[1];
                }

                markName.Append($".{nodeValue}");
            }
            if (subnodeCode != "-" && subnodeCode != "") {
                markName.Append($".{subnodeCode}");
                if (overhaul != "") {
                    markName.Append($"-{overhaul}");
                }
            }
            if (markCode != "-" && markCode != "") {
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

        private MemoryStream createDocument(Table customTable)
        {
            var stream = new MemoryStream();

            // Create a Wordprocessing document. 
            using (WordprocessingDocument package = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
            {
                // Add a new main document part. 
                package.AddMainDocumentPart();

                // Create the Document DOM. 
                package.MainDocumentPart.Document =
                new Document(
                    new Body(
                    new Paragraph(
                        new Run(
                        new Table(customTable)))));

                // Save changes to the main document part. 
                package.MainDocumentPart.Document.Save();
            }
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
