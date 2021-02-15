using DocumentsKM.Models;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public static class GeneralDataDocument
    {
        public static void AppendToSheetTable(WordprocessingDocument document, List<Doc> docs)
        {
            if (docs.Count() > 0)
            {
                Body body = document.MainDocumentPart.Document.Body;
                var t = body.Descendants<Table>().FirstOrDefault(
                    tbl => tbl.InnerText.Contains("Лист"));

                var firstTr = t.Descendants<TableRow>().ToList()[1];
                var clonedFirstTr = firstTr.CloneNode(true);

                var firstTrCells = firstTr.Descendants<TableCell>().ToList();
                firstTrCells[0].GetFirstChild<Paragraph>().Append(
                    GetWordTextElement("1", 26));
                firstTrCells[1].GetFirstChild<Paragraph>().Append(
                    GetWordTextElement(docs[0].Name, 26));
                firstTrCells[2].GetFirstChild<Paragraph>().Append(
                    GetWordTextElement(docs[0].Note, 26));

                for (int i = 1; i < docs.Count(); i++)
                {
                    var newTr = clonedFirstTr.CloneNode(true);
                    firstTrCells = newTr.Descendants<TableCell>().ToList();

                    firstTrCells[0].GetFirstChild<Paragraph>().Append(
                        GetWordTextElement((i + 1).ToString(), 26));
                    firstTrCells[1].GetFirstChild<Paragraph>().Append(
                        GetWordTextElement(docs[i].Name, 26));
                    firstTrCells[2].GetFirstChild<Paragraph>().Append(
                        GetWordTextElement(docs[i].Note, 26));

                    t.Append(newTr);
                }
            }
        }

        public static void AppendToLinkedAndAttachedDocsTable(
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
                var firstTrCells = firstTr.Descendants<TableCell>().ToList();
                var p = firstTrCells[1].GetFirstChild<Paragraph>();
                var justification = new Justification
                {
                    Val = JustificationValues.Center,
                };
                p.ParagraphProperties.Append(justification);
                p.Append(GetWordTextElement("Ссылочные документы", 26, true));

                for (int i = 0; i < markLinkedDocs.Count(); i++)
                {
                    var newTr = clonedFirstTr.CloneNode(true);
                    firstTrCells = newTr.Descendants<TableCell>().ToList();

                    firstTrCells[0].GetFirstChild<Paragraph>().Append(
                        GetWordTextElement(markLinkedDocs[i].LinkedDoc.Designation, 26));
                    firstTrCells[1].GetFirstChild<Paragraph>().Append(
                        GetWordTextElement(markLinkedDocs[i].LinkedDoc.Name, 26));
                    firstTrCells[2].GetFirstChild<Paragraph>().Append(
                        GetWordTextElement(markLinkedDocs[i].Note, 26));
                    t.Append(newTr);
                }
            }
            if (attachedDocs.Count() > 0)
            {
                var newTr = clonedFirstTr.CloneNode(true);
                var firstTrCells = newTr.Descendants<TableCell>().ToList();
                var p = firstTrCells[1].GetFirstChild<Paragraph>();
                var justification = new Justification
                {
                    Val = JustificationValues.Center,
                };
                p.ParagraphProperties.Append(justification);

                p.Append(GetWordTextElement("Прилагаемые документы", 26, true));
                t.Append(newTr);

                for (int i = 0; i < attachedDocs.Count(); i++)
                {
                    newTr = clonedFirstTr.CloneNode(true);
                    firstTrCells = newTr.Descendants<TableCell>().ToList();

                    firstTrCells[0].GetFirstChild<Paragraph>().Append(
                        GetWordTextElement(attachedDocs[i].Designation, 26));
                    firstTrCells[1].GetFirstChild<Paragraph>().Append(
                        GetWordTextElement(attachedDocs[i].Name, 26));
                    firstTrCells[2].GetFirstChild<Paragraph>().Append(
                        GetWordTextElement(attachedDocs[i].Note, 26));

                    t.Append(newTr);
                }
            }
        }

        public static void AppendList(
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

                if (pointText.Contains('^'))
                {
                    var split = pointText.Split('^');
                    if (split.Count() > 1)
                    {
                        for (int k = 0; k < split.Count(); k++)
                        {
                            if (k > 0)
                                newPara.AppendChild(GetWordTextElement(split[k][0].ToString(), 26, false, true));
                            if (k == 0)
                                newPara.AppendChild(GetWordTextElement(split[k], 26));
                            else
                                if (split[k].Length > 1)
                                    newPara.AppendChild(GetWordTextElement(split[k].Substring(1), 26));
                        }
                    }
                    else
                        newPara.AppendChild(GetWordTextElement(pointText, 26));
                }
                else
                    newPara.AppendChild(GetWordTextElement(pointText, 26));
                // if (pointText.Contains('^'))
                // {
                //     var split = pointText.Split("^2");
                //     if (split.Count() > 1)
                //     {
                //         for (int k = 0; k < split.Count(); k++)
                //         {
                //             if (k > 0)
                //             {
                //                 newPara.AppendChild(GetWordTextElement("2", 26, false, true));
                //             }
                //             newPara.AppendChild(GetWordTextElement(split[k], 26));
                //         }
                //         // var split2 = s.Split("^3");
                //         // if (s.Count() > 1)
                //         // {
                            
                //         // }
                //     }
                //     else
                //     {
                //         split = pointText.Split("^3");
                //         if (split.Count() > 1)
                //         {
                //             for (int k = 0; k < split.Count(); k++)
                //             {
                //                 if (k > 0)
                //                 {
                //                     newPara.AppendChild(GetWordTextElement("3", 26, false, true));
                //                 }
                //                 newPara.AppendChild(GetWordTextElement(split[k], 26));
                //             }
                //         }
                //         else
                //             newPara.AppendChild(GetWordTextElement(pointText, 26));
                //     }
                // }
                // else
                //     newPara.AppendChild(GetWordTextElement(pointText, 26));
                body.PrependChild(newPara);
            }
        }

        private static Run GetWordTextElement(
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
    }
}
