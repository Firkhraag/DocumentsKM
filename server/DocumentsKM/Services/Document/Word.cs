using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public static class Word
    {
        public static Run GetTextElement(
            string text,
            int fSize,
            bool isUnderlined = false,
            bool isSuperscript = false,
            bool isBold = false,
            bool isItalic = true)
        {
            Run run = new Run();
            RunProperties runProperties = run.AppendChild(new RunProperties());
            FontSize fontSize = new FontSize() { Val = fSize.ToString() };
            if (isItalic)
                runProperties.AppendChild(new Italic()
                {
                    Val = OnOffValue.FromBoolean(true),
                });
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
            if (isBold)
            {
                Bold bold = new Bold() {};
                runProperties.Append(bold);
            }
            run.AppendChild(new Text()
            {
                Text = text,
                Space = SpaceProcessingModeValues.Preserve,
            });
            return run;
        }

        public static void AppendToBigFooterTable(
            WordprocessingDocument document,
            string markFullCodeName,
            string complexName,
            string objectName,
            int sheetsCount,
            Mark mark,
            List<MarkApproval> markApprovals,
            Employee departmentHead,
            string organizationShortName)
        {
            const int firstPartColumnIndexToFill = 6;
            const int secondPartColumnIndexToFill = 4;

            MainDocumentPart mainPart = document.MainDocumentPart;
            var commonFooter = mainPart.FooterParts.FirstOrDefault();
            var t = commonFooter.RootElement.Descendants<Table>().FirstOrDefault();
            var trArr = t.Descendants<TableRow>().ToList();

            var trCells = trArr[0].Descendants<TableCell>().ToList();
            var tc = trCells[firstPartColumnIndexToFill];
            var p = tc.GetFirstChild<Paragraph>();
            p.Append(GetTextElement(markFullCodeName, 28));

            trCells = trArr[2].Descendants<TableCell>().ToList();
            tc = trCells[firstPartColumnIndexToFill];
            p = tc.GetFirstChild<Paragraph>();
            p.Append(GetTextElement(complexName, 24));


            trCells = trArr[5].Descendants<TableCell>().ToList();

            // tc = trCells[1];
            // p = tc.GetFirstChild<Paragraph>();
            // p.Append(GetTextElement("E1", 22));

            tc = trCells[secondPartColumnIndexToFill];
            p = tc.GetFirstChild<Paragraph>();
            p.Append(GetTextElement(objectName, 20));

            trCells = trArr[8].Descendants<TableCell>().ToList();
            tc = trCells[firstPartColumnIndexToFill - 1];
            p = tc.GetFirstChild<Paragraph>();
            p.Append(GetTextElement(organizationShortName, 24));

            trCells = trArr[6].Descendants<TableCell>().ToList();

            if (mark.ChiefSpecialist != null)
            {
                tc = trCells[1];
                p = tc.GetFirstChild<Paragraph>();
                p.Append(GetTextElement(mark.ChiefSpecialist.Name, 22));
            }

            if (sheetsCount != -1)
            {
                tc = trCells.LastOrDefault();
                p = tc.GetFirstChild<Paragraph>();
                p.Append(GetTextElement(sheetsCount.ToString(), 22));
            }

            trCells = trArr[7].Descendants<TableCell>().ToList();
            tc = trCells[1];
            p = tc.GetFirstChild<Paragraph>();

            p.Append(GetTextElement(departmentHead.Name, 22));

            if (mark.ChiefEngineerName != null)
            {
                trCells = trArr[8].Descendants<TableCell>().ToList();
                tc = trCells[1];
                p = tc.GetFirstChild<Paragraph>();
                var split =  mark.ChiefEngineerName.Split(" ");
                p.Append(GetTextElement(split.LastOrDefault(), 22));
            }

            // trCells = trArr[9].Descendants<TableCell>().ToList();
            // tc = trCells[1];
            // p = tc.GetFirstChild<Paragraph>();
            // p.Append(GetTextElement("E5", 22));

            // trCells = trArr[10].Descendants<TableCell>().ToList();
            // tc = trCells[1];
            // p = tc.GetFirstChild<Paragraph>();
            // p.Append(GetTextElement("E6", 22));

            for (int i = 0; i < markApprovals.Count(); i++)
            {
                if (i < 3)
                {
                    trCells = trArr[12 + i].Descendants<TableCell>().ToList();
                    tc = trCells[0];
                    p = tc.GetFirstChild<Paragraph>();
                    p.Append(GetTextElement(markApprovals[i].Employee.Department.Name, 22));
                    tc = trCells[1];
                }
                else if (i == 3)
                {
                    trCells = trArr[8 + i].Descendants<TableCell>().ToList();
                    tc = trCells[1];
                    p = tc.GetFirstChild<Paragraph>();
                    p.Append(GetTextElement(markApprovals[i].Employee.Department.Name, 22));
                    tc = trCells[2];
                }
                else
                {
                    trCells = trArr[8 + i].Descendants<TableCell>().ToList();
                    tc = trCells[4];
                    p = tc.GetFirstChild<Paragraph>();
                    p.Append(GetTextElement(markApprovals[i].Employee.Department.Name, 22));
                    tc = trCells[5];
                }
                p = tc.GetFirstChild<Paragraph>();
                p.Append(GetTextElement(markApprovals[i].Employee.Name, 22));
            }
        }

        public static void AppendToMediumFooterTable(
            WordprocessingDocument document,
            string markFullCodeName,
            string complexName,
            string objectName,
            Mark mark,
            Employee departmentHead,
            string organizationShortName)
        {
            const int firstPartColumnIndexToFill = 6;
            const int secondPartColumnIndexToFill = 4;

            MainDocumentPart mainPart = document.MainDocumentPart;
            // var commonFooter = mainPart.FooterParts.LastOrDefault();
            var commonFooter = mainPart.FooterParts.FirstOrDefault();
            var t = commonFooter.RootElement.Descendants<Table>().FirstOrDefault();
            var trArr = t.Descendants<TableRow>().ToList();

            var trCells = trArr[0].Descendants<TableCell>().ToList();
            var tc = trCells[firstPartColumnIndexToFill];
            var p = tc.GetFirstChild<Paragraph>();
            p.Append(GetTextElement(markFullCodeName, 28));

            trCells = trArr[2].Descendants<TableCell>().ToList();
            tc = trCells[firstPartColumnIndexToFill];
            p = tc.GetFirstChild<Paragraph>();
            p.Append(GetTextElement(complexName, 24));

            trCells = trArr[5].Descendants<TableCell>().ToList();

            // tc = trCells[1];
            // p = tc.GetFirstChild<Paragraph>();
            // p.Append(GetTextElement("E1", 22));

            tc = trCells[secondPartColumnIndexToFill];
            p = tc.GetFirstChild<Paragraph>();
            p.Append(GetTextElement(objectName, 20));

            trCells = trArr[8].Descendants<TableCell>().ToList();
            tc = trCells[firstPartColumnIndexToFill - 1];
            p = tc.GetFirstChild<Paragraph>();
            p.Append(GetTextElement(organizationShortName, 24));

            // trCells = trArr[6].Descendants<TableCell>().ToList();

            // if (mark.ChiefSpecialist != null)
            // {
            //     tc = trCells[1];
            //     p = tc.GetFirstChild<Paragraph>();
            //     p.Append(GetTextElement(mark.ChiefSpecialist.Name, 22));
            // }

            // trCells = trArr[7].Descendants<TableCell>().ToList();
            // tc = trCells[1];
            // p = tc.GetFirstChild<Paragraph>();
            // p.Append(GetTextElement("E4", 22));

            // trCells = trArr[8].Descendants<TableCell>().ToList();
            // tc = trCells[1];
            // p = tc.GetFirstChild<Paragraph>();
            // p.Append(GetTextElement("E5", 22));

            // trCells = trArr[9].Descendants<TableCell>().ToList();
            // tc = trCells[1];
            // p = tc.GetFirstChild<Paragraph>();
            // p.Append(GetTextElement("E6", 22));

            trCells = trArr[10].Descendants<TableCell>().ToList();
            tc = trCells[1];
            p = tc.GetFirstChild<Paragraph>();
            p.Append(GetTextElement(departmentHead.Name, 22));
        }

        public static void AppendToSmallFooterTable(WordprocessingDocument document, string markName)
        {
            var columnIndexToFill = 6;
            MainDocumentPart mainPart = document.MainDocumentPart;
            // var commonFooter = mainPart.FooterParts.FirstOrDefault();
            var commonFooter = mainPart.FooterParts.LastOrDefault();
            var t = commonFooter.RootElement.Descendants<Table>().FirstOrDefault();

            var firstTr = t.Descendants<TableRow>().FirstOrDefault();
            var firstTrCells = firstTr.Descendants<TableCell>().ToList();
            var tc = firstTrCells[columnIndexToFill];
            var p = tc.GetFirstChild<Paragraph>();
            p.Append(GetTextElement(markName, 28));
        }

        public static void AppendToMainSmallFooterTable(WordprocessingDocument document, string markName)
        {
            var columnIndexToFill = 6;
            MainDocumentPart mainPart = document.MainDocumentPart;
            // var commonFooter = mainPart.FooterParts.LastOrDefault();
            var commonFooter = mainPart.FooterParts.FirstOrDefault();
            var t = commonFooter.RootElement.Descendants<Table>().FirstOrDefault();

            var firstTr = t.Descendants<TableRow>().FirstOrDefault();
            var firstTrCells = firstTr.Descendants<TableCell>().ToList();
            var tc = firstTrCells[columnIndexToFill];
            var p = tc.GetFirstChild<Paragraph>();
            p.Append(Word.GetTextElement(markName, 28));
        }

        public static void MakeBordersThin(
            List<TableCell> trCells,
            bool isBottom = true,
            bool isTop = true)
        {
            foreach (var cell in trCells)
            {
                var tcp = cell.GetFirstChild<TableCellProperties>();
                var tcb = tcp.GetFirstChild<TableCellBorders>();
                if (isBottom)
                {
                    var bb = tcb.GetFirstChild<BottomBorder>();
                    bb.Size = 4;
                }
                if (isTop)
                {
                    var tb = tcb.GetFirstChild<TopBorder>();
                    tb.Size = 4;
                }
            }
        }

        public static void ReplaceText(WordprocessingDocument wordDoc, string textToReplace, string replacedText)
        {
            Body body = wordDoc.MainDocumentPart.Document.Body;
            var paras = body.Elements<Paragraph>();
            foreach (var para in paras)
            {
                foreach (var run in para.Elements<Run>())
                {
                    foreach (var text in run.Elements<Text>())
                    {
                        if (text.Text.Contains(textToReplace))
                        {
                            text.Text = text.Text.Replace(textToReplace, replacedText);
                        }
                    }
                }
            }
        }
    }
}
