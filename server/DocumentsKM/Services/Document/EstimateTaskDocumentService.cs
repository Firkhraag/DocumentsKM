using DocumentsKM.Data;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using System.Collections.Generic;
using System;
using DocumentsKM.Helpers;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;

namespace DocumentsKM.Services
{
    public class EstimateTaskDocumentService : IEstimateTaskDocumentService
    {
        private readonly int _departmentHeadPosId = 7;
        private readonly int _sheetDocTypeId = 1;
        
        private readonly IMarkRepo _markRepo;
        private readonly IMarkApprovalRepo _markApprovalRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IDocRepo _docRepo;
        private readonly IConstructionRepo _constructionRepo;
        private readonly IMarkOperatingConditionsRepo _markOperatingConditionsRepo;
        private readonly IEstimateTaskRepo _estimateTaskRepo;

        public EstimateTaskDocumentService(
            IMarkRepo markRepo,
            IMarkApprovalRepo markApprovalRepo,
            IEmployeeRepo employeeRepo,
            IDocRepo docRepo,
            IConstructionRepo constructionRepo,
            IMarkOperatingConditionsRepo markOperatingConditionsRepo,
            IEstimateTaskRepo estimateTaskRepo)
        {
            _markRepo = markRepo;
            _markApprovalRepo = markApprovalRepo;
            _employeeRepo = employeeRepo;
            _docRepo = docRepo;
            _constructionRepo = constructionRepo;
            _markOperatingConditionsRepo = markOperatingConditionsRepo;
            _estimateTaskRepo = estimateTaskRepo;
        }

        public void PopulateDocument(int markId, MemoryStream memory)
        {
            var mark = _markRepo.GetById(markId);
            if (mark == null)
                throw new ArgumentNullException(nameof(mark));
            var markApprovals = _markApprovalRepo.GetAllByMarkId(markId);
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
            var opCond = _markOperatingConditionsRepo.GetByMarkId(markId);
            if (opCond == null)
                throw new ArgumentNullException(nameof(opCond));
            var estTask = _estimateTaskRepo.GetByMarkId(markId);
            if (estTask == null)
                throw new ArgumentNullException(nameof(estTask));

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memory, true))
            {
                var markName = MarkHelper.MakeMarkName(
                    project.BaseSeries, node.Code, subnode.Code, mark.Code);
                (var complexName, var objectName) = MarkHelper.MakeComplexAndObjectName(
                    project.Name, node.Name, subnode.Name, mark.Name);

                var arr = new List<Word.ListText>
                {
                    new Word.ListText
                    {
                        Text = "Изготовление и монтаж конструкций",
                        LevelNum = 0,
                        IsBold = true,
                    },
                    new Word.ListText
                    {
                        Text = "Дополнительно учитывать:",
                        LevelNum = 4,
                        IsBold = false,
                    },
                    new Word.ListText
                    {
                        Text = $"коэффициент надежности по ответственности: {opCond.SafetyCoeff}",
                        LevelNum = 2,
                        IsBold = false,
                    },
                    new Word.ListText
                    {
                        Text = $"среда: {opCond.EnvAggressiveness.Name}",
                        LevelNum = 2,
                        IsBold = false,
                    },
                    new Word.ListText
                    {
                        Text = $"расчетная температура эксплуатации: {(opCond.Temperature < 0 ? ("минус " + -opCond.Temperature) : opCond.Temperature)}",
                        LevelNum = 2,
                        IsBold = false,
                    },
                    
                };
                AppendList(wordDoc, arr);
                
                arr = new List<Word.ListText> {};
                foreach (var construction in constructions)
                {
                    arr.Add(new Word.ListText
                    {
                        Text = construction.Name,
                        LevelNum = 1,
                        IsBold = false,
                    });
                    if (construction.Valuation != null)
                        arr.Add(new Word.ListText
                        {
                            Text = $"Расценка: {construction.Valuation}",
                            LevelNum = 5,
                            IsBold = false,
                        });
                    if (construction.StandardAlbumCode != null)
                        arr.Add(new Word.ListText
                        {
                            Text = $"Шифр типового альбома конструкций: {construction.StandardAlbumCode}",
                            LevelNum = 5,
                            IsBold = false,
                        });
                    if (construction.NumOfStandardConstructions != 0)
                        arr.Add(new Word.ListText
                        {
                            Text = $"Число типовых конструкций: {construction.NumOfStandardConstructions}",
                            LevelNum = 5,
                            IsBold = false,
                        });
                    if (construction.HasEdgeBlunting)
                    arr.Add(new Word.ListText
                        {
                            Text = "Притупление кромок",
                            LevelNum = 5,
                            IsBold = false,
                        });
                    if (construction.HasDynamicLoad)
                        arr.Add(new Word.ListText
                        {
                            Text = "Динамическая нагрузка",
                            LevelNum = 5,
                            IsBold = false,
                        });
                    if (construction.HasFlangedConnections)
                        arr.Add(new Word.ListText
                        {
                            Text = "Фланцевые соединения",
                            LevelNum = 5,
                            IsBold = false,
                        });
                    if (construction.WeldingControl.Id > 1)
                    {
                        arr.Add(new Word.ListText
                        {
                            Text = $"Контроль плотности сварных швов {construction.WeldingControl.Name}",
                            LevelNum = 5,
                            IsBold = false,
                        });
                    }
                }

                arr.Add(new Word.ListText
                {
                    Text = "Окраска конструкций",
                    LevelNum = 0,
                    IsBold = true,
                });

                arr.Add(new Word.ListText
                {
                    Text = "Масса металла для окраски",
                    LevelNum = 4,
                    IsBold = false,
                });

                if (estTask.AdditionalText != null && estTask.AdditionalText != "")
                {
                    arr.Add(new Word.ListText
                    {
                        Text = "Дополнительно",
                        LevelNum = 0,
                        IsBold = true,
                    });
                    arr.Add(new Word.ListText
                    {
                        Text = estTask.AdditionalText,
                        LevelNum = 3,
                        IsBold = false,
                    });
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
            }
        }

        private void AppendList(
            WordprocessingDocument wordDoc, List<Word.ListText> arr)
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

            var abstractNum = new AbstractNum(
                abstractLevel, abstractLevel1, abstractLevel2, abstractLevel3, abstractLevel4, abstractLevel5)
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
                // var indentation = new Indentation() { Left = "360", Right = "360", FirstLine = "720" };
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
                else
                {
                    numberingProperties = new NumberingProperties(
                        new NumberingLevelReference() { Val = 5 }, new NumberingId() { Val = numberId });
                    indentation = new Indentation() { Left = "360", Right = "360", FirstLine = "1800" };
                }

                var paragraphProperties = new ParagraphProperties(
                    numberingProperties, spacingBetweenLines, indentation);
                var newPara = new Paragraph(paragraphProperties);

                newPara.AppendChild(Word.GetTextElement(arr[i].Text, 26, false, false, arr[i].IsBold));
                body.AppendChild(newPara);
            }
        }
    }
}
