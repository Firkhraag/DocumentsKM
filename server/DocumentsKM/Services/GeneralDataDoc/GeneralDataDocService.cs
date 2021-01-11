using DocumentsKM.Models;
using DocumentsKM.Data;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public class GeneralDataDocService : IGeneralDataDocService
    {
        private IMarkGeneralDataPointRepo _markGeneralDataPointRepo;

        public GeneralDataDocService(IMarkGeneralDataPointRepo markGeneralDataPointRepo)
        {
            _markGeneralDataPointRepo = markGeneralDataPointRepo;
        }

        public async Task<MemoryStream> GetDocByMarkId(int markId)
        {
            var markGeneralDataPoints = _markGeneralDataPointRepo.GetAllByMarkId(
                markId).OrderByDescending(
                    v => v.Section.OrderNum).ThenByDescending(v => v.OrderNum);

            var memory = new MemoryStream();
            var path = "D:\\Dev\\Gipromez\\word\\test.docx";

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(path, true))
            {
                AppendList(wordDoc, markGeneralDataPoints);

                AppendToTable(wordDoc, "Лист");
                AppendToTable(wordDoc, "Обозначение");
                AppendToFirstFooterTable(wordDoc);
                AppendToMainFooterTable(wordDoc);
            }

            var b = System.IO.File.ReadAllBytes(path);
            await memory.WriteAsync(b, 0, b.Length);
            memory.Position = 0;
            return memory;
        }

        private void AppendToFirstFooterTable(WordprocessingDocument document)
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
            p.Append(GetWordTextElement("111", 11));

            trCells = trArr[2].Descendants<TableCell>().ToList();
            tc = trCells[firstPartColumnIndexToFill];
            p = tc.GetFirstChild<Paragraph>();
            p.Append(GetWordTextElement("222", 11));

            trCells = trArr[5].Descendants<TableCell>().ToList();

            tc = trCells[1];
            p = tc.GetFirstChild<Paragraph>();
            p.Append(GetWordTextElement("E1", 11));

            tc = trCells[secondPartColumnIndexToFill];
            p = tc.GetFirstChild<Paragraph>();
            p.Append(GetWordTextElement("333", 11));
            
            trCells = trArr[6].Descendants<TableCell>().ToList();

            tc = trCells[1];
            p = tc.GetFirstChild<Paragraph>();
            p.Append(GetWordTextElement("E2", 11));

            tc = trCells.LastOrDefault();
            p = tc.GetFirstChild<Paragraph>();
            p.Append(GetWordTextElement("44", 11));

            trCells = trArr[7].Descendants<TableCell>().ToList();
            tc = trCells[1];
            p = tc.GetFirstChild<Paragraph>();
            p.Append(GetWordTextElement("E3", 11));

            trCells = trArr[8].Descendants<TableCell>().ToList();
            tc = trCells[1];
            p = tc.GetFirstChild<Paragraph>();
            p.Append(GetWordTextElement("E4", 11));

            trCells = trArr[9].Descendants<TableCell>().ToList();
            tc = trCells[1];
            p = tc.GetFirstChild<Paragraph>();
            p.Append(GetWordTextElement("E5", 11));

            trCells = trArr[10].Descendants<TableCell>().ToList();
            tc = trCells[1];
            p = tc.GetFirstChild<Paragraph>();
            p.Append(GetWordTextElement("E6", 11));

            for (int i = 0; i < 3; i++)
            {
                trCells = trArr[12 + i].Descendants<TableCell>().ToList();
                tc = trCells[0];
                p = tc.GetFirstChild<Paragraph>();
                p.Append(GetWordTextElement("AP1", 11));
                tc = trCells[1];
                p = tc.GetFirstChild<Paragraph>();
                p.Append(GetWordTextElement("AE1", 11));
            }
        }

        private void AppendToMainFooterTable(WordprocessingDocument document)
        {
            var columnIndexToFill = 6;
            MainDocumentPart mainPart = document.MainDocumentPart;
            var commonFooter = mainPart.FooterParts.FirstOrDefault();
            var t = commonFooter.RootElement.Descendants<Table>().FirstOrDefault();

            var firstTr = t.Descendants<TableRow>().FirstOrDefault();
            var firstTrCells = firstTr.Descendants<TableCell>().ToList();
            var tc = firstTrCells[columnIndexToFill];
            var p = tc.GetFirstChild<Paragraph>();
            p.Append(GetWordTextElement("111", 12));
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
                p.Append(GetWordTextElement("111", 12));
            }

            for (var i = 0; i < 3; i++)
            {
                var newTr = clonedFirstTr.CloneNode(true);
                firstTrCells = newTr.Descendants<TableCell>().ToList();
                foreach (var tc in firstTrCells)
                {
                    var p = tc.GetFirstChild<Paragraph>();
                    p.Append(GetWordTextElement("222", 12));
                }
                t.Append(newTr);
            }
        }

        private Run GetWordTextElement(string text, int fontSizePt)
        {
            Run run = new Run();
            RunProperties runProperties = run.AppendChild(new RunProperties());
            Italic italic = new Italic();
            italic.Val = OnOffValue.FromBoolean(true);
            FontSize fontSize = new FontSize() { Val = (fontSizePt * 2).ToString() };
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
                })
            {
                LevelIndex = 2
            };

            var abstractNum = new AbstractNum(
                abstractLevel, abstractLevel2, abstractLevel3) {AbstractNumberId = abstractNumberId};
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
                }
                var paragraphProperties = new ParagraphProperties(
                    numberingProperties, spacingBetweenLines, indentation);

                var newPara = new Paragraph(paragraphProperties);
                newPara.AppendChild(GetWordTextElement(pointText, 12));
                body.PrependChild(newPara);                
            }
        }
    }
}
