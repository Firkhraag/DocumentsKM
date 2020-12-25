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
            MyProcess.OpenWithArguments();

            var markGeneralDataPoints = _markGeneralDataPointRepo.GetAllByMarkId(markId);
            var memory = new MemoryStream();
            var docStr = TexTemplate.docTop;

            // var currentSectionId = -1;
            foreach (var point in markGeneralDataPoints)
            {
                // point.Text = point.Text.Replace("a", "b");
                // if (point.Section.Id != currentSectionId)
                // {
                //     currentSectionId = point.Section.Id;
                //     docStr += $"\\item {point.Text}\n";
                // }
                if (point.Text.Length > 1)
                {
                    if ((point.Text[0] == '#' || point.Text[0] == '-') && point.Text[1] == ' ')
                    {
                        point.Text = "\\item" + point.Text.Substring(1);
                    }
                }
                // docStr += $"\\item {point.Text}\n";
                docStr += $"\\item 1\n";
                // if (section.Itemize.Count() != 0)
                // {
                //     docStr += "\\begin{itemize}";
                //     foreach (var item in section.Itemize)
                //         docStr += $"\\item {item}\n";
                //     docStr += "\\end{itemize}";
                // }
                // if (section.Enumerate.Count() != 0)
                // {
                //     docStr += "\\begin{enumerate}";
                //     foreach (var item in section.Enumerate)
                //         docStr += $"\\item {item}\n";
                //     docStr += "\\end{enumerate}";
                // }
            }

            docStr += TexTemplate.docBottom;

            Byte[] b = new UTF8Encoding(true).GetBytes(docStr);
            await memory.WriteAsync(b, 0, b.Length);
            memory.Position = 0;
            return memory;
        }

        // public async Task<MemoryStream> GetDocByMarkId(int markId)
        // {
        //     // CreateTable("D:\\Dev\\Gipromez\\word\\test.docx");

        //     // Dictionary<string, string> keyValues = new Dictionary<string, string>();
        //     // keyValues.Add("Ведомость", "test");
        //     // SearchAndReplace("D:\\Dev\\Gipromez\\word\\1.docx", keyValues);

        //     var markGeneralDataPoints = _markGeneralDataPointRepo.GetAllByMarkId(markId);

        //     using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(
        //         "D:\\Dev\\Gipromez\\word\\test.docx", true))
        //     {
        //         // AppendToTable(wordDoc);

        //         // AddList(wordDoc);

        //         AppendList(wordDoc, markGeneralDataPoints);
        //         // AppendList2(wordDoc);
                
        //     }

        //     var memory = new MemoryStream();
        //     var docStr = TexTemplate.docTop;

        //     Byte[] b = new UTF8Encoding(true).GetBytes(docStr);
        //     await memory.WriteAsync(b, 0, b.Length);
        //     memory.Position = 0;
        //     return memory;
        // }

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

            //
            numberingProperties1.Append(numberingLevelReference1);
            numberingProperties1.Append(numberingId1);

            // ParagraphMarkRunProperties paragraphMarkRunProperties8 = new ParagraphMarkRunProperties();
            // RunFonts runFonts54 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            // // Color color1 = new Color() { Val = "626262" };
            // Color color1 = new Color() { Val = "ff0000" };
            // FontSize fontSize56 = new FontSize() { Val = "20" };
            // FontSizeComplexScript fontSizeComplexScript6 = new FontSizeComplexScript() { Val = "20" };
            // Shading shading1 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF" };

            // paragraphMarkRunProperties8.Append(runFonts54);
            // paragraphMarkRunProperties8.Append(color1);
            // paragraphMarkRunProperties8.Append(fontSize56);
            // paragraphMarkRunProperties8.Append(fontSizeComplexScript6);
            // paragraphMarkRunProperties8.Append(shading1);

            //
            paragraphProperties11.Append(paragraphStyleId11);
            paragraphProperties11.Append(numberingProperties1);
            // paragraphProperties11.Append(paragraphMarkRunProperties8);

            Run run45 = new Run() { RsidRunProperties = "005B5111" };

            RunProperties runProperties41 = new RunProperties();
            RunFonts runFonts55 = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
            Color color2 = new Color() { Val = "626262" };
            FontSize fontSize57 = new FontSize() { Val = "20" };
            FontSizeComplexScript fontSizeComplexScript7 = new FontSizeComplexScript() { Val = "20" };
            Shading shading2 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "FFFFFF" };

            runProperties41.Append(runFonts55);
            runProperties41.Append(color2);
            runProperties41.Append(fontSize57);
            runProperties41.Append(fontSizeComplexScript7);
            runProperties41.Append(shading2);
            Text text21 = new Text();
            text21.Text = "agency or by the Public Records";
            run45.Append(runProperties41);
            run45.Append(text21);

            paragraph15.Append(paragraphProperties11);
            paragraph15.Append(run45);
            wordDoc.MainDocumentPart.Document.Body.Append(paragraph15);
        }

        private void AppendList(
            WordprocessingDocument wordDoc, IEnumerable<MarkGeneralDataPoint> markGeneralDataPoints)
        {
            var runList = new List<Run>();
            foreach (string item in markGeneralDataPoints.Select(v => v.Text).ToList())
            {
                var newRun = new Run();

                RunProperties runProperties = newRun.AppendChild(new RunProperties());
                Italic italic = new Italic();
                italic.Val = OnOffValue.FromBoolean(true);
                runProperties.AppendChild(italic);

                newRun.AppendChild(new Text(item));
                runList.Add(newRun);
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
            // var abstractLevel = new Level(
            //     new NumberingFormat() {Val = NumberFormatValues.NumberInDash}, new LevelText() {Val = "-"}) {LevelIndex = 0};
            var abstractLevel = new Level(
                new NumberingFormat() {Val = NumberFormatValues.Decimal}, new LevelText() {Val = "decimal"}) {LevelIndex = 0};
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
                var indentation = new Indentation() { Left = "360", Right="360", FirstLine = "360" };

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

        // public static void ChangeDoc(string filename)
        // {
        //     try
        //     {
        //         string sourceFile = Server.MapPath(Path.Combine("/", "Templates/mytemplate.docx"));
        //         string destinationFile = Server.MapPath
        //         (Path.Combine("/", "New Documents/FirstDocument.docx"));

        //         // Create a copy of the template file and open the copy 
        //         File.Copy(sourceFile, destinationFile, true);

        //         // create key value pair, key represents words to be replace and 
        //         //values represent values in document in place of keys.
        //         Dictionary<string, string> keyValues = new Dictionary<string, string>();
        //         keyValues.Add(""Replaceable Text"", txtName.Text);                
        //         SearchAndReplace(destinationFile, keyValues);

        //         Process.Start(destinationFile);
        //     }
        //     catch (Exception ex)
        //     {
        //         throw ex;
        //     }
        // }

        public static void SearchAndReplace(string document, Dictionary<string, string> dict)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(document, true))
            {
                // AddList(wordDoc);
                AddHeading(wordDoc, "", 18, "1", "1", "headingText", 1, 1);

                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                foreach (KeyValuePair<string, string> item in dict) 
                {
                    Regex regexText = new Regex(item.Key);
                    docText = regexText.Replace(docText, item.Value);
                }

                using (StreamWriter sw = new StreamWriter(
                        wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }

            }
        }

        // Insert a table into a word processing document.
        public static void CreateTable(string fileName)
        {
            // Use the file name and path passed in as an argument 
            // to open an existing Word 2007 document.

            using (WordprocessingDocument doc 
                = WordprocessingDocument.Open(fileName, true))
            {
                // Create an empty table.
                Table table = new Table();

                // Create a TableProperties object and specify its border information.
                TableProperties tblProp = new TableProperties(
                    new TableBorders(
                        new TopBorder() { Val = 
                            new EnumValue<BorderValues>(BorderValues.Dashed), Size = 24 },
                        new BottomBorder() { Val = 
                            new EnumValue<BorderValues>(BorderValues.Dashed), Size = 24 },
                        new LeftBorder() { Val = 
                            new EnumValue<BorderValues>(BorderValues.Dashed), Size = 24 },
                        new RightBorder() { Val = 
                            new EnumValue<BorderValues>(BorderValues.Dashed), Size = 24 },
                        new InsideHorizontalBorder() { Val = 
                            new EnumValue<BorderValues>(BorderValues.Dashed), Size = 24 },
                        new InsideVerticalBorder() { Val = 
                            new EnumValue<BorderValues>(BorderValues.Dashed), Size = 24 }
                    )
                );

                // Append the TableProperties object to the empty table.
                table.AppendChild<TableProperties>(tblProp);

                // Create a row.
                TableRow tr = new TableRow();

                // Create a cell.
                TableCell tc1 = new TableCell();

                // Specify the width property of the table cell.
                tc1.Append(new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));

                // Specify the table cell content.
                tc1.Append(new Paragraph(new Run(new Text("some text"))));

                // Append the table cell to the table row.
                tr.Append(tc1);

                // Create a second table cell by copying the OuterXml value of the first table cell.
                TableCell tc2 = new TableCell(tc1.OuterXml);

                // Append the table cell to the table row.
                tr.Append(tc2);

                // Append the table row to the table.
                table.Append(tr);

                // Append the table to the document.
                doc.MainDocumentPart.Document.Body.Append(table);
            }
        }



        public static void AppendToTable(WordprocessingDocument document)
        {
            Body bod = document.MainDocumentPart.Document.Body;
            var t = bod.Descendants<Table>().FirstOrDefault(tbl => tbl.InnerText.Contains("Лист"));

            var firstTr = t.Descendants<TableRow>().ToList()[2];
            var firstTrCells = firstTr.Descendants<TableCell>().ToList();
            foreach (var tc in firstTrCells)
            {
                var p = tc.GetFirstChild<Paragraph>();
                p.Append(new Run(new Text("111")));
                // var r = p.GetFirstChild<Run>();
                // var text = r.GetFirstChild<Text>();
                // text.Text = "111";
            }

            for (var i = 0; i < 3; i++)
            {
                var clonedFirstTr = firstTr.CloneNode(true);
                firstTrCells = clonedFirstTr.Descendants<TableCell>().ToList();
                foreach (var tc in firstTrCells)
                {
                    var p = tc.GetFirstChild<Paragraph>();
                    var r = p.GetFirstChild<Run>();
                    r.Append(new Text("222"));
                    // var text = r.GetFirstChild<Text>();
                    // text.Text = "222";
                }
                t.Append(clonedFirstTr);
                // var tr = new TableRow();
                // TableCell tc1 = new TableCell(new Paragraph(new Run(new Text("1"))));
                // TableCell tc2 = new TableCell(new Paragraph(new Run(new Text("2"))));
                // TableCell tc3 = new TableCell(new Paragraph(new Run(new Text("3"))));
                // tr.Append(tc1);
                // tr.Append(tc2);
                // tr.Append(tc3);
                // t.Append(tr);
            }
        }

        public static void AddList(WordprocessingDocument document)
        {
            StyleRunProperties styleRunProperties = new StyleRunProperties();
            DocumentFormat.OpenXml.Wordprocessing.FontSize fontSize1 = new DocumentFormat.OpenXml.Wordprocessing.FontSize();

            styleRunProperties.Append(fontSize1);

            Paragraph p = new Paragraph();
            ParagraphProperties pp = new ParagraphProperties();
            pp.SpacingBetweenLines = new SpacingBetweenLines() { After = "0" };

            ParagraphStyleId paragraphStyleId1 = new ParagraphStyleId() { Val = "ListParagraph" };
            NumberingProperties numberingProperties1 = new NumberingProperties();
            NumberingLevelReference numberingLevelReference1 = new NumberingLevelReference() { Val = 1 };

            NumberingId numberingId1 = new NumberingId() { Val = 1 }; //Val is 1, 2, 3 etc based on your numberingid in your numbering element

            numberingProperties1.Append(numberingLevelReference1); Indentation indentation1 = new Indentation() { FirstLineChars = 0 };
            numberingProperties1.Append(numberingId1);
            pp.Append(paragraphStyleId1);
            pp.Append(numberingProperties1);
            pp.Append(indentation1);
            p.Append(pp);
            Run r = new Run();
            Text t = new Text("testlist") { Space = SpaceProcessingModeValues.Preserve };
            r.Append(t);
            p.Append(r);

            document.MainDocumentPart.Document.Body.PrependChild(p);
        }



        public static void AddHeading(WordprocessingDocument document, string colorVal, int fontSizeVal, string styleId, string styleName, string headingText, int numLvlRef, int numIdVal)
        {
            StyleRunProperties styleRunProperties = new StyleRunProperties();
            Color color = new Color() { Val = colorVal };
            DocumentFormat.OpenXml.Wordprocessing.FontSize fontSize1 = new DocumentFormat.OpenXml.Wordprocessing.FontSize();
            fontSize1.Val = new StringValue(fontSizeVal.ToString());

            styleRunProperties.Append(color);
            styleRunProperties.Append(fontSize1);
            AddStyleToDoc(document.MainDocumentPart.Document.MainDocumentPart, styleId, styleName, styleRunProperties, document);

            Paragraph p = new Paragraph();
            ParagraphProperties pp = new ParagraphProperties();
            pp.ParagraphStyleId = new ParagraphStyleId() { Val = styleId };
            pp.SpacingBetweenLines = new SpacingBetweenLines() { After = "0" };

            ParagraphStyleId paragraphStyleId1 = new ParagraphStyleId() { Val = "ListParagraph" };
            NumberingProperties numberingProperties1 = new NumberingProperties();
            NumberingLevelReference numberingLevelReference1 = new NumberingLevelReference() { Val = numLvlRef };

            NumberingId numberingId1 = new NumberingId() { Val = numIdVal }; //Val is 1, 2, 3 etc based on your numberingid in your numbering element

            numberingProperties1.Append(numberingLevelReference1); Indentation indentation1 = new Indentation() { FirstLineChars = 0 };
            numberingProperties1.Append(numberingId1);
            pp.Append(paragraphStyleId1);
            pp.Append(numberingProperties1);
            pp.Append(indentation1);
            p.Append(pp);
            Run r = new Run();
            Text t = new Text(headingText) { Space = SpaceProcessingModeValues.Preserve };
            r.Append(t);
            p.Append(r);

            document.MainDocumentPart.Document.Body.Append(p);
        }


        public static void AddStyleToDoc(MainDocumentPart mainPart, string styleId, string stylename, StyleRunProperties styleRunProperties, WordprocessingDocument document)
        {
            StyleDefinitionsPart part = mainPart.StyleDefinitionsPart;
            if (part == null)
            {
                part = AddStylesPartToPackage(mainPart);
                AddNewStyle(part, styleId, stylename, styleRunProperties);
            }
            else
            {
                if (IsStyleIdInDocument(mainPart, styleId) != true)
                {
                    string styleIdFromName = GetStyleIdFromStyleName(document, stylename);
                    if (styleIdFromName == null)
                    {
                        AddNewStyle(part, styleId, stylename, styleRunProperties);
                    }
                    else
                        styleId = styleIdFromName;
                }
            }
        }

        public static string GetStyleIdFromStyleName(WordprocessingDocument doc, string styleName)
        {
            StyleDefinitionsPart stylePart = doc.MainDocumentPart.StyleDefinitionsPart;
            string styleId = stylePart.Styles.Descendants<StyleName>()
                .Where(s => s.Val.Value.Equals(styleName) &&
                    (((Style)s.Parent).Type == StyleValues.Paragraph))
                .Select(n => ((Style)n.Parent).StyleId).FirstOrDefault();

            return styleId;
        }

        public static StyleDefinitionsPart AddStylesPartToPackage(MainDocumentPart mainPart)
        {
            StyleDefinitionsPart part;
            part = mainPart.AddNewPart<StyleDefinitionsPart>();
            DocumentFormat.OpenXml.Wordprocessing.Styles root = new DocumentFormat.OpenXml.Wordprocessing.Styles();
            root.Save(part);
            return part;
        }

        public static bool IsStyleIdInDocument(MainDocumentPart mainPart, string styleid)
        {
            DocumentFormat.OpenXml.Wordprocessing.Styles s = mainPart.StyleDefinitionsPart.Styles;

            int n = s.Elements<DocumentFormat.OpenXml.Wordprocessing.Style>().Count();
            if (n == 0)
                return false;

            DocumentFormat.OpenXml.Wordprocessing.Style style = s.Elements<DocumentFormat.OpenXml.Wordprocessing.Style>()
                .Where(st => (st.StyleId == styleid) && (st.Type == StyleValues.Paragraph))
                .FirstOrDefault();
            if (style == null)
                return false;

            return true;
        }

        private static void AddNewStyle(StyleDefinitionsPart styleDefinitionsPart, string styleid, string stylename, StyleRunProperties styleRunProperties)
        {
            DocumentFormat.OpenXml.Wordprocessing.Styles styles = styleDefinitionsPart.Styles;

            DocumentFormat.OpenXml.Wordprocessing.Style style = new DocumentFormat.OpenXml.Wordprocessing.Style()
            {
                Type = StyleValues.Paragraph,
                StyleId = styleid,
                CustomStyle = false
            };
            style.Append(new StyleName() { Val = stylename });
            style.Append(new BasedOn() { Val = "Normal" });
            style.Append(new NextParagraphStyle() { Val = "Normal" });
            style.Append(new UIPriority() { Val = 900 });

            styles.Append(style);
        }















        // public void AddParagraph(string sentence)
        // {
        //     List<Run> runList = ListOfStringToRunList(new List<string> { sentence});
        //     AddParagraph(runList);
        // }
        // public void AddParagraph(List<string> sentences)
        // {
        //     List<Run> runList = ListOfStringToRunList(sentences);
        //     AddParagraph(runList);
        // }

        // public void AddParagraph(List<Run> runList)
        // {
        //     var para = new Paragraph();
        //     foreach (Run runItem in runList)
        //     {
        //         para.AppendChild(runItem);
        //     }

        //     Body body = _wordprocessingDocument.MainDocumentPart.Document.Body;
        //     body.AppendChild(para);
        // }

        // public void AddBulletList(List<string> sentences)
        // {
        //     var runList = ListOfStringToRunList(sentences);

        //     AddBulletList(runList);
        // }


        // public void AddBulletList(List<Run> runList)
        // {
        //     // Introduce bulleted numbering in case it will be needed at some point
        //     NumberingDefinitionsPart numberingPart = _wordprocessingDocument.MainDocumentPart.NumberingDefinitionsPart;
        //     if (numberingPart == null)
        //     {
        //         numberingPart = _wordprocessingDocument.MainDocumentPart.AddNewPart<NumberingDefinitionsPart>("NumberingDefinitionsPart001");
        //         Numbering element = new Numbering();
        //         element.Save(numberingPart);
        //     }

        //     // Insert an AbstractNum into the numbering part numbering list.  The order seems to matter or it will not pass the 
        //     // Open XML SDK Productity Tools validation test.  AbstractNum comes first and then NumberingInstance and we want to
        //     // insert this AFTER the last AbstractNum and BEFORE the first NumberingInstance or we will get a validation error.
        //     var abstractNumberId = numberingPart.Numbering.Elements<AbstractNum>().Count() + 1;
        //     var abstractLevel = new Level(new NumberingFormat() {Val = NumberFormatValues.Bullet}, new LevelText() {Val = "·"}) {LevelIndex = 0};
        //     var abstractNum1 = new AbstractNum(abstractLevel) {AbstractNumberId = abstractNumberId};

        //     if (abstractNumberId == 1)
        //     {
        //         numberingPart.Numbering.Append(abstractNum1);
        //     }
        //     else
        //     {
        //         AbstractNum lastAbstractNum = numberingPart.Numbering.Elements<AbstractNum>().Last();
        //         numberingPart.Numbering.InsertAfter(abstractNum1, lastAbstractNum);
        //     }

        //     // Insert an NumberingInstance into the numbering part numbering list.  The order seems to matter or it will not pass the 
        //     // Open XML SDK Productity Tools validation test.  AbstractNum comes first and then NumberingInstance and we want to
        //     // insert this AFTER the last NumberingInstance and AFTER all the AbstractNum entries or we will get a validation error.
        //     var numberId = numberingPart.Numbering.Elements<NumberingInstance>().Count() + 1;
        //     NumberingInstance numberingInstance1 = new NumberingInstance() {NumberID = numberId};
        //     AbstractNumId abstractNumId1 = new AbstractNumId() {Val = abstractNumberId};
        //     numberingInstance1.Append(abstractNumId1);

        //     if (numberId == 1)
        //     {
        //         numberingPart.Numbering.Append(numberingInstance1);
        //     }
        //     else
        //     {
        //         var lastNumberingInstance = numberingPart.Numbering.Elements<NumberingInstance>().Last();
        //         numberingPart.Numbering.InsertAfter(numberingInstance1, lastNumberingInstance);
        //     }

        //     Body body = _wordprocessingDocument.MainDocumentPart.Document.Body;

        //     foreach (Run runItem in runList)
        //     {
        //         // Create items for paragraph properties
        //         var numberingProperties = new NumberingProperties(new NumberingLevelReference() {Val = 0}, new NumberingId() {Val = numberId});
        //         var spacingBetweenLines1 = new SpacingBetweenLines() { After = "0" };  // Get rid of space between bullets
        //         var indentation = new Indentation() { Left = "720", Hanging = "360" };  // correct indentation 

        //         ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
        //         RunFonts runFonts1 = new RunFonts() { Ascii = "Symbol", HighAnsi = "Symbol" };
        //         paragraphMarkRunProperties1.Append(runFonts1);

        //         // create paragraph properties
        //         var paragraphProperties = new ParagraphProperties(numberingProperties, spacingBetweenLines1, indentation, paragraphMarkRunProperties1);

        //         // Create paragraph 
        //         var newPara = new Paragraph(paragraphProperties);

        //         // Add run to the paragraph
        //         newPara.AppendChild(runItem);

        //         // Add one bullet item to the body
        //         body.AppendChild(newPara);
        //     }
        // }


        // public void Dispose()
        // {
        //     CloseAndDisposeOfDocument();
        //     if (_ms != null)
        //     {
        //         _ms.Dispose();
        //         _ms = null;
        //     }
        // }

        // public MemoryStream SaveToStream()
        // {
        //     _ms.Position = 0;
        //     return _ms;
        // }

        // public void SaveToFile(string fileName)
        // {
        //     if (_wordprocessingDocument != null)
        //     {
        //         CloseAndDisposeOfDocument();
        //     }

        //     if (_ms == null)
        //         throw new ArgumentException("This object has already been disposed of so you cannot save it!");

        //     using (var fs = File.Create(fileName))
        //     {
        //         _ms.WriteTo(fs);
        //     }
        // }

        // private void CloseAndDisposeOfDocument()
        // {
        //     if (_wordprocessingDocument != null)
        //     {
        //         _wordprocessingDocument.Close();
        //         _wordprocessingDocument.Dispose();
        //         _wordprocessingDocument = null;
        //     }
        // }

        private static List<Run> ListOfStringToRunList(List<string> sentences)
        {
            var runList = new List<Run>();
            foreach (string item in sentences)
            {
                var newRun = new Run();
                newRun.AppendChild(new Text(item));
                runList.Add(newRun);
            }

            return runList;
        }
    }
}
