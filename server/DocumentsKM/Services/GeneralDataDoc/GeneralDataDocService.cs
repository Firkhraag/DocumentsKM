using DocumentsKM.Models;
using DocumentsKM.Data;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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


            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(
                "D:\\Dev\\Gipromez\\word\\test.docx", true))
            {
                AppendList(wordDoc, markGeneralDataPoints);
                // AppendList2(wordDoc);

                // AppendToTable(wordDoc, "Лист");
                // AppendToTable(wordDoc, "Обозначение");
                // AppendToFirstFooterTable(wordDoc);
                // AppendToMainFooterTable(wordDoc);
            }

            var memory = new MemoryStream();
            var docStr = TexTemplate.docTop;

            Byte[] b = new UTF8Encoding(true).GetBytes(docStr);
            await memory.WriteAsync(b, 0, b.Length);
            memory.Position = 0;
            return memory;
        }

        private void AppendList2(WordprocessingDocument wordDoc)
        {
            NumberingDefinitionsPart numberingPart = wordDoc.MainDocumentPart.NumberingDefinitionsPart;
            var abstractNumId = numberingPart.Numbering.Elements<AbstractNum>().Count() + 1;
            AbstractNum abstractNum1 = new AbstractNum() { AbstractNumberId = abstractNumId };
            abstractNum1.SetAttribute(new OpenXmlAttribute("w15", "restartNumberingAfterBreak", "http://schemas.microsoft.com/office/word/2012/wordml", "0"));

            Paragraph paragraph15 = new Paragraph() { RsidParagraphMarkRevision = "005B5111", RsidParagraphAddition = "0012034F", RsidParagraphProperties = "005B5111", RsidRunAdditionDefault = "004A6864", ParagraphId = "555A4D6E", TextId = "49553577" };

            ParagraphProperties paragraphProperties11 = new ParagraphProperties();
            ParagraphStyleId paragraphStyleId11 = new ParagraphStyleId() { Val = "ListParagraph" };
            Level level2 = new Level() { LevelIndex = 6, TemplateCode = "04090019", Tentative = true };
            StartNumberingValue startNumberingValue2 = new StartNumberingValue() { Val = 1 };
            NumberingFormat numberingFormat2 = new NumberingFormat() { Val = NumberFormatValues.LowerLetter };

            LevelJustification levelJustification2 = new LevelJustification() { Val = LevelJustificationValues.Left };

            PreviousParagraphProperties previousParagraphProperties2 = new PreviousParagraphProperties();

            NumberingProperties numberingProperties1 = new NumberingProperties();
            NumberingLevelReference numberingLevelReference1 = new NumberingLevelReference() { Val = 0 };
            NumberingId numberingId1 = new NumberingId() { Val = 4 };
            LevelText levelText2 = new LevelText() { Val = "%7." };
            level2.Append(startNumberingValue2);
            level2.Append(numberingFormat2);
            level2.Append(levelText2);
            level2.Append(levelJustification2);
            level2.Append(previousParagraphProperties2);

            abstractNum1.Append(level2);

            numberingProperties1.Append(numberingLevelReference1);
            numberingProperties1.Append(numberingId1);

            var indentation = new Indentation() { Left = "360", Right="360", FirstLine = "210" };

            paragraphProperties11.Append(paragraphStyleId11);
            paragraphProperties11.Append(numberingProperties1);
            paragraphProperties11.Append(indentation);

            paragraph15.Append(paragraphProperties11);
            paragraph15.Append(GetWordTextElement("test test", 12));
            wordDoc.MainDocumentPart.Document.Body.Prepend(paragraph15);
        }

        private void AppendList(
            WordprocessingDocument wordDoc, IEnumerable<MarkGeneralDataPoint> markGeneralDataPoints)
        {
            var runList = new List<Run>();
            foreach (string item in markGeneralDataPoints.Select(v => v.Text).ToList())
            {
                runList.Add(GetWordTextElement(item, 12));
            }

            NumberingDefinitionsPart numberingPart = wordDoc.MainDocumentPart.NumberingDefinitionsPart;
            if (numberingPart == null)
            {
                numberingPart = wordDoc.MainDocumentPart.AddNewPart<NumberingDefinitionsPart>(
                    "NumberingDefinitionsPart001");
                Numbering element = new Numbering();
                element.Save(numberingPart);
            }

            var abstractNumberId = numberingPart.Numbering.Elements<AbstractNum>().Count() + 1;
            var abstractLevel = new Level(
                new NumberingFormat() {Val = NumberFormatValues.NumberInDash}, new LevelText() {Val = "-"}) {LevelIndex = 0};
            var abstractNum1 = new AbstractNum(abstractLevel) {AbstractNumberId = abstractNumberId};

            if (abstractNumberId == 1)
            {
                numberingPart.Numbering.Append(abstractNum1);
            }
            else
            {
                AbstractNum lastAbstractNum = numberingPart.Numbering.Elements<AbstractNum>().Last();
                numberingPart.Numbering.InsertAfter(abstractNum1, lastAbstractNum);
            }

            var numberId = numberingPart.Numbering.Elements<NumberingInstance>().Count() + 1;
            NumberingInstance numberingInstance1 = new NumberingInstance() {NumberID = numberId};
            AbstractNumId abstractNumId1 = new AbstractNumId() {Val = abstractNumberId};
            numberingInstance1.Append(abstractNumId1);

            if (numberId == 1)
            {
                numberingPart.Numbering.Append(numberingInstance1);
            }
            else
            {
                var lastNumberingInstance = numberingPart.Numbering.Elements<NumberingInstance>().Last();
                numberingPart.Numbering.InsertAfter(numberingInstance1, lastNumberingInstance);
            }

            Body body = wordDoc.MainDocumentPart.Document.Body;

            foreach (Run runItem in runList)
            {
                var numberingProperties = new NumberingProperties(
                    new NumberingLevelReference() {Val = 0}, new NumberingId() {Val = numberId});
                var spacingBetweenLines1 = new SpacingBetweenLines() { After = "120", Line="300" };
                var indentation = new Indentation() { Left = "360", Right="360", FirstLine = "720" };

                ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
                RunFonts runFonts1 = new RunFonts() { Ascii = "Symbol", HighAnsi = "Symbol" };
                paragraphMarkRunProperties1.Append(runFonts1);

                var paragraphProperties = new ParagraphProperties(
                    numberingProperties, spacingBetweenLines1, indentation, paragraphMarkRunProperties1);

                var newPara = new Paragraph(paragraphProperties);

                newPara.AppendChild(runItem);

                body.PrependChild(newPara);
            }
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
    }
}
