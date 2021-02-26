using DocumentsKM.Data;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using System.Collections.Generic;
using System;
using DocumentsKM.Helpers;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public class EstimateTaskDocumentService : IEstimateTaskDocumentService
    {
        private readonly int _departmentHeadPosId = 7;
        private readonly int _sheetDocTypeId = 1;
        private readonly int _paintingGeneralDataSectionId = 13;
        
        private readonly IMarkRepo _markRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IDocRepo _docRepo;
        private readonly IConstructionRepo _constructionRepo;
        private readonly IConstructionElementRepo _constructionElementRepo;
        private readonly IStandardConstructionRepo _standardConstructionRepo;
        private readonly IMarkOperatingConditionsRepo _markOperatingConditionsRepo;
        private readonly IEstimateTaskRepo _estimateTaskRepo;
        private readonly IMarkGeneralDataPointRepo _markGeneralDataPointRepo;

        private class GroupedSteel
        {
            public string Name { set; get; }
            public float Length { set; get; }
            public float Weight { set; get; }
            public float Area { set; get; }
        }
        private class ListText
        {
            public string Text { set; get; }
            public int LevelNum { set; get; }
            public bool IsBold { set; get; }
            public bool WithSuperscript { set; get; }
        }

        public EstimateTaskDocumentService(
            IMarkRepo markRepo,
            IEmployeeRepo employeeRepo,
            IDocRepo docRepo,
            IConstructionRepo constructionRepo,
            IConstructionElementRepo constructionElementRepo,
            IStandardConstructionRepo standardConstructionRepo,
            IMarkOperatingConditionsRepo markOperatingConditionsRepo,
            IEstimateTaskRepo estimateTaskRepo,
            IMarkGeneralDataPointRepo markGeneralDataPointRepo)
        {
            _markRepo = markRepo;
            _employeeRepo = employeeRepo;
            _docRepo = docRepo;
            _constructionRepo = constructionRepo;
            _constructionElementRepo = constructionElementRepo;
            _standardConstructionRepo = standardConstructionRepo;
            _markOperatingConditionsRepo = markOperatingConditionsRepo;
            _estimateTaskRepo = estimateTaskRepo;
            _markGeneralDataPointRepo = markGeneralDataPointRepo;
        }

        public void PopulateDocument(int markId, MemoryStream memory)
        {
            var mark = _markRepo.GetById(markId);
            if (mark == null)
                throw new ArgumentNullException(nameof(mark));
            var subnode = mark.Subnode;
            var node = subnode.Node;
            var project = node.Project;

            var departmentHeadArr = _employeeRepo.GetAllByDepartmentIdAndPosition(
                mark.Department.Id,
                _departmentHeadPosId);
            if (departmentHeadArr.Count() != 1)
                throw new ConflictException();
            var departmentHead = departmentHeadArr.ToList()[0];

            var sheets = _docRepo.GetAllByMarkIdAndDocType(markId, _sheetDocTypeId);
            // Вкл в состав спецификации
            var constructions = _constructionRepo.GetAllByMarkId(markId);
            var standardConstructions = _standardConstructionRepo.GetAllByMarkId(markId);
            var opCond = _markOperatingConditionsRepo.GetByMarkId(markId);
            if (opCond == null)
                throw new ArgumentNullException(nameof(opCond));
            var estTask = _estimateTaskRepo.GetByMarkId(markId);
            if (estTask == null)
                throw new ArgumentNullException(nameof(estTask));
            var markApprovals = new List<MarkApproval> {};
            if (estTask.ApprovalEmployee != null)
            {
                markApprovals.Add(new MarkApproval
                {
                    MarkId = markId,
                    Employee = estTask.ApprovalEmployee,
                });
            }

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memory, true))
            {
                var markName = MarkHelper.MakeMarkName(
                    project.BaseSeries, node.Code, subnode.Code, mark.Code);
                (var complexName, var objectName) = MarkHelper.MakeComplexAndObjectName(
                    project.Name, node.Name, subnode.Name, mark.Name);

                AppendText(wordDoc, estTask.TaskText);

                var arr = new List<ListText>
                {
                    new ListText
                    {
                        Text = "Изготовление и монтаж конструкций",
                        LevelNum = 0,
                        IsBold = true,
                        WithSuperscript = false,
                    },
                    new ListText
                    {
                        Text = "Дополнительно учитывать:",
                        LevelNum = 4,
                        IsBold = false,
                        WithSuperscript = false,
                    },
                    new ListText
                    {
                        Text = $"коэффициент надежности по ответственности: {opCond.SafetyCoeff}",
                        LevelNum = 2,
                        IsBold = false,
                        WithSuperscript = false,
                    },
                    new ListText
                    {
                        Text = $"среда: {opCond.EnvAggressiveness.Name}",
                        LevelNum = 2,
                        IsBold = false,
                        WithSuperscript = false,
                    },
                    new ListText
                    {
                        Text = $"расчетная температура эксплуатации: {(opCond.Temperature < 0 ? ("минус " + -opCond.Temperature) : opCond.Temperature)}",
                        LevelNum = 2,
                        IsBold = false,
                        WithSuperscript = false,
                    },
                    
                };
                AppendList(wordDoc, arr);
                
                arr = new List<ListText> {};
                double overallInitialWeightSum = 0.0;
                double overallAreaSum = 0.0;
                foreach (var construction in constructions)
                {
                    arr.Add(new ListText
                    {
                        Text = construction.Name,
                        LevelNum = 1,
                        IsBold = false,
                        WithSuperscript = false,
                    });
                    if (construction.Valuation != null)
                        arr.Add(new ListText
                        {
                            Text = $"Расценка: {construction.Valuation}",
                            LevelNum = 5,
                            IsBold = false,
                            WithSuperscript = false,
                        });
                    if (construction.StandardAlbumCode != null)
                        arr.Add(new ListText
                        {
                            Text = $"Шифр типового альбома конструкций: {construction.StandardAlbumCode}",
                            LevelNum = 5,
                            IsBold = false,
                            WithSuperscript = false,
                        });
                    if (construction.NumOfStandardConstructions != 0)
                        arr.Add(new ListText
                        {
                            Text = $"Число типовых конструкций: {construction.NumOfStandardConstructions}",
                            LevelNum = 5,
                            IsBold = false,
                            WithSuperscript = false,
                        });
                    if (construction.HasEdgeBlunting)
                    arr.Add(new ListText
                        {
                            Text = "Притупление кромок",
                            LevelNum = 5,
                            IsBold = false,
                            WithSuperscript = false,
                        });
                    if (construction.HasDynamicLoad)
                        arr.Add(new ListText
                        {
                            Text = "Динамическая нагрузка",
                            LevelNum = 5,
                            IsBold = false,
                            WithSuperscript = false,
                        });
                    if (construction.HasFlangedConnections)
                        arr.Add(new ListText
                        {
                            Text = "Фланцевые соединения",
                            LevelNum = 5,
                            IsBold = false,
                            WithSuperscript = false,
                        });
                    if (construction.WeldingControl.Id > 1)
                        arr.Add(new ListText
                        {
                            Text = $"Контроль плотности сварных швов {construction.WeldingControl.Name}",
                            LevelNum = 5,
                            IsBold = false,
                            WithSuperscript = false,
                        });
                    var constructionElements = _constructionElementRepo.GetAllByConstructionId(construction.Id);
                    if (constructionElements.Count() > 0)
                    {
                        var groupedSteel = constructionElements.Where(
                            v => v.Steel != null).GroupBy(v => v.Steel).Select(
                                v => new GroupedSteel
                                {
                                    Name = v.First().Steel.Name,
                                    Length = v.Sum(v => v.Length),
                                    Weight = v.Sum(v => v.Profile.Weight),
                                    Area = v.Sum(v => v.Profile.Area),
                                });
                        arr.Add(new ListText
                        {
                            Text = "в том числе по маркам стали:",
                            LevelNum = 5,
                            IsBold = false,
                            WithSuperscript = false,
                        });
                        double initialWeightSum = 0.0;
                        double areaSum = 0.0;
                        foreach (var s in groupedSteel)
                        {
                            var initialWeightValue = s.Weight * s.Length * 0.001;
                            var initialWeightValueRounded = Math.Ceiling(initialWeightValue * 1000) / 1000;
                            arr.Add(new ListText
                            {
                                Text = $"{s.Name} {initialWeightValueRounded} x 1.04 = {Math.Ceiling(initialWeightValueRounded * 1.04 * 1000) / 1000} т",
                                LevelNum = 6,
                                IsBold = false,
                                WithSuperscript = false,
                            });
                            initialWeightSum += initialWeightValue;
                            areaSum += s.Area * s.Length;
                        }
                        overallInitialWeightSum += initialWeightSum;
                        var initialWeightSumRounded = Math.Ceiling(initialWeightSum * 1000) / 1000;
                        arr.Add(new ListText
                        {
                            Text = $"Итого масса металла: {initialWeightSumRounded} x 1.04 = {Math.Ceiling(initialWeightSumRounded * 1.04 * 1000) / 1000} т",
                            LevelNum = 5,
                            IsBold = false,
                            WithSuperscript = false,
                        });
                        overallAreaSum += areaSum;
                        arr.Add(new ListText
                        {
                            Text = $"Площадь поверхности для окраски: {Math.Round(areaSum, 3)} x 100 м^2",
                            LevelNum = 5,
                            IsBold = false,
                            WithSuperscript = true,
                        });
                        arr.Add(new ListText
                        {
                            Text = $"Относительная площадь окраски: {Math.Round(areaSum * 100 / (initialWeightSum * 1.04), 3)} м^2/т",
                            LevelNum = 5,
                            IsBold = false,
                            WithSuperscript = true,
                        });
                    }
                }

                foreach (var standardConstruction in standardConstructions)
                {
                    arr.Add(new ListText
                    {
                        Text = $"{standardConstruction.Name}: {Math.Ceiling(standardConstruction.Weight * 1000) / 1000} т",
                        LevelNum = 1,
                        IsBold = false,
                        WithSuperscript = false,
                    });
                }

                arr.Add(new ListText
                {
                    Text = "Окраска конструкций",
                    LevelNum = 0,
                    IsBold = true,
                    WithSuperscript = false,
                });

                arr.Add(new ListText
                {
                    Text = $"Масса металла для окраски: {Math.Round(overallInitialWeightSum, 3)} x 1.04 = {Math.Round(Math.Round(overallInitialWeightSum, 3) * 1.04, 3)} т",
                    LevelNum = 4,
                    IsBold = false,
                    WithSuperscript = false,
                });
                arr.Add(new ListText
                {
                    Text = $"Площадь поверхности для окраски: {Math.Round(overallAreaSum, 3)} x 100 м^2",
                    LevelNum = 4,
                    IsBold = false,
                    WithSuperscript = true,
                });

                var points = _markGeneralDataPointRepo.GetAllByMarkAndSectionId(
                    markId, _paintingGeneralDataSectionId);
                for (int i = 1; i < points.Count(); i++)
                {
                    var pointText = points.ToList()[i].Text;
                    if (pointText[0] == '#' && pointText[1] == ' ')
                        pointText = pointText.Substring(2) + ".";
                    else if (pointText[0] == '-' && pointText[1] == ' ')
                        pointText = pointText.Substring(2) + ".";
                    arr.Add(new ListText
                    {
                        Text = pointText,
                        LevelNum = 3,
                        IsBold = false,
                        WithSuperscript = false,
                    });
                }

                if (estTask.AdditionalText != null && estTask.AdditionalText != "")
                {
                    arr.Add(new ListText
                    {
                        Text = "Дополнительно",
                        LevelNum = 0,
                        IsBold = true,
                        WithSuperscript = false,
                    });
                    var split = estTask.AdditionalText.Split("\n");
                    foreach (var splitText in split)
                    {
                        arr.Add(new ListText
                        {
                            Text = splitText,
                            LevelNum = 3,
                            IsBold = false,
                            WithSuperscript = false,
                        });
                    }
                }
                AppendList(wordDoc, arr);

                Word.AppendToBigFooterTable(
                    wordDoc,
                    markName,
                    complexName,
                    objectName,
                    sheets.Count(),
                    mark,
                    markApprovals.ToList(),
                    departmentHead);
                Word.AppendToSmallFooterTable(wordDoc, markName);
            }
        }

        private void AppendText(WordprocessingDocument wordDoc, string text)
        {
            Body body = wordDoc.MainDocumentPart.Document.Body;
            var p = body.GetFirstChild<Paragraph>();
            var split = text.Split("\n");
            foreach (var splitText in split)
            {
                var run = Word.GetTextElement(splitText, 26);
                run.AppendChild(new Break());
                p.AppendChild(run);
            }
        }

        private void AppendList(
            WordprocessingDocument wordDoc, List<ListText> arr)
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
                    Val = NumberFormatValues.UpperRoman
                },
                new LevelText()
                {
                    Val = "%1."
                },
                new StartNumberingValue()
                {
                    Val = 1,
                },
                new RunProperties()
                {
                    Italic = new Italic()
                    {
                        Val = OnOffValue.FromBoolean(true)
                    },
                    FontSize = new FontSize()
                    {
                        Val = 26.ToString(),
                    },
                    Bold = new Bold() {},
                })
            {
                LevelIndex = 0
            };
            var abstractLevel1 = new Level(
                new NumberingFormat()
                {
                    Val = NumberFormatValues.Decimal
                },
                new LevelText()
                {
                    Val = "%2."
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
            var abstractLevel2 = new Level(
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
            var abstractLevel3 = new Level(
                new LevelSuffix()
                {
                    Val = LevelSuffixValues.Space
                })
            {
                LevelIndex = 3
            };
            var abstractLevel4 = new Level(
                new LevelSuffix()
                {
                    Val = LevelSuffixValues.Space
                })
            {
                LevelIndex = 4
            };
            var abstractLevel5 = new Level(
                new LevelSuffix()
                {
                    Val = LevelSuffixValues.Space
                })
            {
                LevelIndex = 5
            };
            var abstractLevel6 = new Level(
                new LevelSuffix()
                {
                    Val = LevelSuffixValues.Space
                })
            {
                LevelIndex = 6
            };

            var abstractNum = new AbstractNum(
                abstractLevel, abstractLevel1, abstractLevel2, abstractLevel3,
                abstractLevel4, abstractLevel5, abstractLevel6)
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
            for (var i = 0; i < arr.Count(); i++)
            {
                var spacingBetweenLines = new SpacingBetweenLines() { After = "120", Line = "240" };
                var indentation = new Indentation() { Left = "360", Right = "360", FirstLine = "1160" };

                NumberingProperties numberingProperties;
                if (arr[i].LevelNum == 0)
                {
                    numberingProperties = new NumberingProperties(
                        new NumberingLevelReference() { Val = 0 }, new NumberingId() { Val = numberId });
                    indentation = new Indentation() { Left = "360", Right = "360", FirstLine = "720" };
                }
                else if (arr[i].LevelNum == 1)
                {
                    numberingProperties = new NumberingProperties(
                        new NumberingLevelReference() { Val = 1 }, new NumberingId() { Val = numberId });
                }
                else if (arr[i].LevelNum == 2)
                {
                    numberingProperties = new NumberingProperties(
                        new NumberingLevelReference() { Val = 2 }, new NumberingId() { Val = numberId });
                }
                else if (arr[i].LevelNum == 3)
                {
                    numberingProperties = new NumberingProperties(
                        new NumberingLevelReference() { Val = 3 }, new NumberingId() { Val = numberId });
                    indentation = new Indentation() { Left = "360", Right = "360", FirstLine = "640" };
                }
                else if (arr[i].LevelNum == 4)
                {
                    numberingProperties = new NumberingProperties(
                        new NumberingLevelReference() { Val = 4 }, new NumberingId() { Val = numberId });
                    indentation = new Indentation() { Left = "360", Right = "360", FirstLine = "1080" };
                }
                else if (arr[i].LevelNum == 5)
                {
                    numberingProperties = new NumberingProperties(
                        new NumberingLevelReference() { Val = 5 }, new NumberingId() { Val = numberId });
                    indentation = new Indentation() { Left = "360", Right = "360", FirstLine = "1800" };
                }
                else
                {
                    numberingProperties = new NumberingProperties(
                        new NumberingLevelReference() { Val = 6 }, new NumberingId() { Val = numberId });
                    indentation = new Indentation() { Left = "360", Right = "360", FirstLine = "2400" };
                }

                var paragraphProperties = new ParagraphProperties(
                    numberingProperties, spacingBetweenLines, indentation);
                var newPara = new Paragraph(paragraphProperties);

                if (arr[i].WithSuperscript)
                {
                    var split = arr[i].Text.Split('^');
                    if (split.Count() > 1)
                    {
                        for (int k = 0; k < split.Count(); k++)
                        {
                            if (k > 0)
                                newPara.AppendChild(
                                    Word.GetTextElement(split[k][0].ToString(), 26, false, true));
                            if (k == 0)
                                newPara.AppendChild(Word.GetTextElement(split[k], 26));
                            else
                                if (split[k].Length > 1)
                                    newPara.AppendChild(
                                        Word.GetTextElement(split[k].Substring(1), 26));
                        }
                    }
                    else
                        newPara.AppendChild(
                            Word.GetTextElement(arr[i].Text, 26, false, false, arr[i].IsBold));
                }
                else
                    newPara.AppendChild(
                        Word.GetTextElement(arr[i].Text, 26, false, false, arr[i].IsBold));

                body.AppendChild(newPara);
            }
        }
    }
}
