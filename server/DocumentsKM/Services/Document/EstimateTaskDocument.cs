using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public static class EstimateTaskDocument
    {
        // public static void AppendListItem(
        public static void AppendList(
            WordprocessingDocument wordDoc, List<string> textArr)
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
                    Val = "%1."
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
                LevelIndex = 1
            };
            var abstractLevel3 = new Level(
                new LevelSuffix()
                {
                    Val = LevelSuffixValues.Space
                })
            {
                LevelIndex = 2
            };

            var abstractNum = new AbstractNum(
                abstractLevel, abstractLevel2, abstractLevel3)
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
            for (var i = 0; i < textArr.Count(); i++)
            {
                var spacingBetweenLines = new SpacingBetweenLines() { After = "120", Line = "240" };
                var indentation = new Indentation() { Left = "360", Right = "360", FirstLine = "720" };

                NumberingProperties numberingProperties;
                var pointText = textArr[i];
                if (textArr[i][0] == '#' && textArr[i][1] == ' ')
                {
                    numberingProperties = new NumberingProperties(
                        new NumberingLevelReference() { Val = 0 }, new NumberingId() { Val = numberId });
                    pointText = pointText.Substring(2);
                }
                else if (textArr[i][0] == '-' && textArr[i][1] == ' ')
                {
                    numberingProperties = new NumberingProperties(
                        new NumberingLevelReference() { Val = 1 }, new NumberingId() { Val = numberId });
                    pointText = pointText.Substring(2);
                }
                else
                {
                    numberingProperties = new NumberingProperties(
                        new NumberingLevelReference() { Val = 2 }, new NumberingId() { Val = numberId });
                    indentation = new Indentation() { Left = "360", Right = "360", FirstLine = "640" };
                }

                var paragraphProperties = new ParagraphProperties(
                    numberingProperties, spacingBetweenLines, indentation);
                var newPara = new Paragraph(paragraphProperties);

                newPara.AppendChild(Word.GetTextElement(pointText, 26));
                body.AppendChild(newPara);
            }
        }
    }
}
