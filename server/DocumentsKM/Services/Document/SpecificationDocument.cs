using DocumentsKM.Models;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System;

namespace DocumentsKM.Services
{
    public static class SpecificationDocument
    {
        public static void AppendToTable(
            WordprocessingDocument document)
        {
            // if (specifications.Count() > 0)
            if (1 > 0)
            {
                Body body = document.MainDocumentPart.Document.Body;
                var t = body.Descendants<Table>().FirstOrDefault();

                var firstTr = t.Descendants<TableRow>().ToList()[0];
                var clonedFirstTr = firstTr.CloneNode(true);
                var trCells = firstTr.Descendants<TableCell>().ToList();

                trCells[0].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement("1", 24));
                trCells[1].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement("1", 24));
                trCells[2].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement("1", 24));
                trCells[3].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement("1", 24));
                trCells[4].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement("1", 24));
                trCells[5].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement("1", 24));
            }
        }
    }
}
