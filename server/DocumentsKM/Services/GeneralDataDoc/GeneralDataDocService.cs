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
            // CreateTable("D:\\Dev\\Gipromez\\word\\test.docx");

            // Dictionary<string, string> keyValues = new Dictionary<string, string>();
            // keyValues.Add("Ведомость", "test");
            // SearchAndReplace("D:\\Dev\\Gipromez\\word\\1.docx", keyValues);

            // var memory = new MemoryStream();
            // var docStr = XmlTemplate.doc;

            // Byte[] b = new UTF8Encoding(true).GetBytes(docStr);
            // await memory.WriteAsync(b, 0, b.Length);
            // memory.Position = 0;
            // return memory;

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
                docStr += $"\\item {point.Text}\n";
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
            foreach (Table t in bod.Descendants<Table>().Where(tbl => tbl.InnerText.Contains("myTable")))
            {
                t.Append(new TableRow(new TableCell(new Paragraph(new Run(new Text("test"))))));
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
    }
}
