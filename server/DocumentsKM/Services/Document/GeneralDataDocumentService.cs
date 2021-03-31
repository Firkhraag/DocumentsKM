using DocumentsKM.Models;
using DocumentsKM.Data;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System;
using DocumentsKM.Helpers;
using DocumentFormat.OpenXml;
using Microsoft.Extensions.Options;

namespace DocumentsKM.Services
{
    public class GeneralDataDocumentService : IGeneralDataDocumentService
    {
        private readonly string _conditionsSectionName = "Характеристики конструкций";
        
        private readonly IMarkRepo _markRepo;
        private readonly IMarkApprovalRepo _markApprovalRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IDocRepo _docRepo;
        private readonly IMarkGeneralDataPointRepo _markGeneralDataPointRepo;
        private readonly IMarkLinkedDocRepo _markLinkedDocRepo;
        private readonly IAttachedDocRepo _attachedDocRepo;
        private readonly IMarkOperatingConditionsRepo _markOperatingConditionsRepo;
        private readonly AppSettings _appSettings;

        public GeneralDataDocumentService(
            IMarkRepo markRepo,
            IMarkApprovalRepo markApprovalRepo,
            IEmployeeRepo employeeRepo,
            IDocRepo docRepo,
            IMarkGeneralDataPointRepo markGeneralDataPointRepo,
            IMarkLinkedDocRepo markLinkedDocRepo,
            IAttachedDocRepo attachedDocRepo,
            IMarkOperatingConditionsRepo markOperatingConditionsRepo,
            IOptions<AppSettings> appSettings)
        {
            _markRepo = markRepo;
            _markApprovalRepo = markApprovalRepo;
            _employeeRepo = employeeRepo;
            _docRepo = docRepo;
            _markGeneralDataPointRepo = markGeneralDataPointRepo;
            _markLinkedDocRepo = markLinkedDocRepo;
            _attachedDocRepo = attachedDocRepo;
            _markOperatingConditionsRepo = markOperatingConditionsRepo;
            _appSettings = appSettings.Value;
        }

        public void PopulateDocument(int markId, MemoryStream memory)
        {
            var mark = _markRepo.GetById(markId);
            if (mark == null)
                throw new ArgumentNullException(nameof(mark));
            var markApprovals = _markApprovalRepo.GetAllByMarkId(markId);

            var departmentHead = _employeeRepo.GetByDepartmentIdAndPosition(
                mark.Department.Id, _appSettings.DepartmentHeadPosId);
            if (departmentHead == null)
                departmentHead = _employeeRepo.GetByDepartmentIdAndPosition(
                mark.Department.Id, _appSettings.ActingDepartmentHeadPosId);
            if (departmentHead == null)
                departmentHead = _employeeRepo.GetByDepartmentIdAndPosition(
                mark.Department.Id, _appSettings.DeputyDepartmentHeadPosId);
            if (departmentHead == null)
                departmentHead = _employeeRepo.GetByDepartmentIdAndPosition(
                mark.Department.Id, _appSettings.ActingDeputyDepartmentHeadPosId);
            if (departmentHead == null)
                throw new ConflictException();

            var opCond = _markOperatingConditionsRepo.GetByMarkId(markId);
            if (opCond == null)
                throw new ArgumentNullException(nameof(opCond));
            var markGeneralDataPoints = _markGeneralDataPointRepo.GetAllByMarkId(
                markId).OrderByDescending(
                    v => v.Section.OrderNum).ThenByDescending(v => v.OrderNum);
            var sheets = _docRepo.GetAllByMarkIdAndDocType(markId, _appSettings.SheetDocTypeId);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memory, true))
            {  
                AppendList(wordDoc, markGeneralDataPoints, opCond);
                AppendToSheetTable(wordDoc, sheets.ToList());
                AppendToLinkedAndAttachedDocsTable(
                    wordDoc,
                    _markLinkedDocRepo.GetAllByMarkId(markId).ToList(),
                    _attachedDocRepo.GetAllByMarkId(markId).ToList());
                Word.AppendToBigFooterTable(
                    wordDoc,
                    mark.Designation,
                    mark.ComplexName,
                    mark.ObjectName,
                    sheets.Count(),
                    mark,
                    markApprovals.ToList(),
                    departmentHead);
                Word.AppendToSmallFooterTable(wordDoc, mark.Designation);
            }
        }

        private void AppendToSheetTable(WordprocessingDocument document, List<Doc> docs)
        {
            if (docs.Count() > 0)
            {
                Body body = document.MainDocumentPart.Document.Body;
                var t = body.Descendants<Table>().FirstOrDefault(
                    tbl => tbl.InnerText.Contains("Лист"));
                var firstTr = t.Descendants<TableRow>().ToList()[1];
                var clonedFirstTr = firstTr.CloneNode(true);
                var trCells = firstTr.Descendants<TableCell>().ToList();
                OpenXmlElement newTr = null;

                for (int i = 0; i < docs.Count(); i++)
                {
                    if (i > 0)
                    {
                        newTr = clonedFirstTr.CloneNode(true);
                        trCells = newTr.Descendants<TableCell>().ToList();
                    }
                    trCells[0].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement((i + 1).ToString(), 26));
                    trCells[1].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement(docs[i].Name, 26));
                    trCells[2].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement(docs[i].Note, 26));
                    if (i > 0)
                        t.Append(newTr);
                }
            }
        }

        private void AppendToLinkedAndAttachedDocsTable(
            WordprocessingDocument document,
            List<MarkLinkedDoc> markLinkedDocs,
            List<AttachedDoc> attachedDocs)
        {
            Body body = document.MainDocumentPart.Document.Body;
            var t = body.Descendants<Table>().FirstOrDefault(
                tbl => tbl.InnerText.Contains("Обозначение"));

            var firstTr = t.Descendants<TableRow>().ToList()[1];
            var clonedFirstTr = firstTr.CloneNode(true);

            if (markLinkedDocs.Count() > 0)
            {
                var trCells = firstTr.Descendants<TableCell>().ToList();
                var p = trCells[1].GetFirstChild<Paragraph>();
                p.ParagraphProperties.Append(new Justification
                {
                    Val = JustificationValues.Center,
                });
                p.Append(Word.GetTextElement("Ссылочные документы", 26, true));

                for (int i = 0; i < markLinkedDocs.Count(); i++)
                {
                    var newTr = clonedFirstTr.CloneNode(true);
                    trCells = newTr.Descendants<TableCell>().ToList();

                    trCells[0].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement(markLinkedDocs[i].LinkedDoc.Designation, 26));
                    trCells[1].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement(markLinkedDocs[i].LinkedDoc.Name, 26));
                    trCells[2].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement(markLinkedDocs[i].Note, 26));
                    t.Append(newTr);
                }
            }
            if (attachedDocs.Count() > 0)
            {

                var newTr = clonedFirstTr.CloneNode(true);
                var trCells = newTr.Descendants<TableCell>().ToList();
                if (markLinkedDocs.Count() == 0)
                    trCells = firstTr.Descendants<TableCell>().ToList();
                var p = trCells[1].GetFirstChild<Paragraph>();
                p.ParagraphProperties.Append(new Justification
                {
                    Val = JustificationValues.Center,
                });

                p.Append(Word.GetTextElement("Прилагаемые документы", 26, true));
                if (markLinkedDocs.Count() != 0)
                    t.Append(newTr);

                for (int i = 0; i < attachedDocs.Count(); i++)
                {
                    newTr = clonedFirstTr.CloneNode(true);
                    trCells = newTr.Descendants<TableCell>().ToList();

                    trCells[0].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement(attachedDocs[i].Designation, 26));
                    trCells[1].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement(attachedDocs[i].Name, 26));
                    trCells[2].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement(attachedDocs[i].Note, 26));

                    t.Append(newTr);
                }
            }
        }

        private void AppendList(
            WordprocessingDocument wordDoc,
            IEnumerable<MarkGeneralDataPoint> markGeneralDataPoints,
            MarkOperatingConditions markOperatingConditions)
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
                        Ascii = "GOST type B",
                        HighAnsi = "GOST type B",
                        ComplexScript = "GOST type B"
                    },
                    Italic = new Italic()
                    {
                        Val = OnOffValue.FromBoolean(true)
                    },
                    FontSize = new FontSize()
                    {
                        Val = 26.ToString(),
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
                        Ascii = "GOST type B",
                        HighAnsi = "GOST type B",
                        ComplexScript = "GOST type B"
                    },
                    Italic = new Italic()
                    {
                        Val = OnOffValue.FromBoolean(true)
                    },
                    FontSize = new FontSize()
                    {
                        Val = 26.ToString(),
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
                        ComplexScript = "Calibri"
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
            NumberingInstance numberingInstance = new NumberingInstance() { NumberID = numberId };
            AbstractNumId abstractNumId = new AbstractNumId() { Val = abstractNumberId };
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
                var spacingBetweenLines = new SpacingBetweenLines() { After = "120", Line = "240" };
                var indentation = new Indentation() { Left = "360", Right = "360", FirstLine = "720" };

                NumberingProperties numberingProperties;
                var pointText = item.Text;
                if (item.OrderNum == 1)
                {
                    numberingProperties = new NumberingProperties(
                        new NumberingLevelReference() { Val = 0 }, new NumberingId() { Val = numberId });
                }
                else if (item.Text[0] == '#' && item.Text[1] == ' ')
                {
                    numberingProperties = new NumberingProperties(
                        new NumberingLevelReference() { Val = 1 }, new NumberingId() { Val = numberId });

                    pointText = pointText.Substring(2) + ".";
                }
                else if (item.Text[0] == '-' && item.Text[1] == ' ')
                {
                    numberingProperties = new NumberingProperties(
                        new NumberingLevelReference() { Val = 2 }, new NumberingId() { Val = numberId });

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
                        new NumberingLevelReference() { Val = 3 }, new NumberingId() { Val = numberId });
                    pointText = pointText + ".";
                    indentation = new Indentation() { Left = "360", Right = "360", FirstLine = "640" };
                }

                var paragraphProperties = new ParagraphProperties(
                    numberingProperties, spacingBetweenLines, indentation);
                var newPara = new Paragraph(paragraphProperties);

                if (item.Section.Name == _conditionsSectionName)
                {
                    if (pointText.Contains("коэффициент надежности по ответственности"))
                        pointText = pointText.Replace("{}", markOperatingConditions.SafetyCoeff.ToString());
                    else if (pointText.Contains("степень агрессивного воздействия среды"))
                        pointText = pointText.Replace("{}", markOperatingConditions.EnvAggressiveness.Name);
                    else if (pointText.Contains("расчетная температура эксплуатации"))
                        pointText = pointText.Replace("{}",
                            $"{(markOperatingConditions.Temperature < 0 ? ("минус " + -markOperatingConditions.Temperature) : markOperatingConditions.Temperature)}");
                }

                if (pointText.Contains('^'))
                {
                    var split = pointText.Split('^');
                    if (split.Count() > 1)
                    {
                        for (int k = 0; k < split.Count(); k++)
                        {
                            if (k > 0)
                                newPara.AppendChild(Word.GetTextElement(split[k][0].ToString(), 26, false, true));
                            if (k == 0)
                                newPara.AppendChild(Word.GetTextElement(split[k], 26));
                            else
                                if (split[k].Length > 1)
                                    newPara.AppendChild(Word.GetTextElement(split[k].Substring(1), 26));
                        }
                    }
                    else
                        newPara.AppendChild(Word.GetTextElement(pointText, 26));
                }
                else
                    newPara.AppendChild(Word.GetTextElement(pointText, 26));
                body.PrependChild(newPara);
            }
        }
    }
}
