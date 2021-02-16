using DocumentsKM.Models;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System;

namespace DocumentsKM.Services
{
    public static class BoltDocument
    {
        public static void AppendToTable(
            WordprocessingDocument document,
            List<ConstructionBolt> bolts,
            List<BoltLength> boltLengths)
        {
            if (bolts.Count() > 0)
            {
                Body body = document.MainDocumentPart.Document.Body;
                var t = body.Descendants<Table>().FirstOrDefault(
                    tbl => tbl.InnerText.Contains("Наименование"));

                var firstTr = t.Descendants<TableRow>().ToList()[1];
                var clonedFirstTr = firstTr.CloneNode(true);
                var trCells = firstTr.Descendants<TableCell>().ToList();

                InsertThreeRows(trCells, clonedFirstTr, t, 1, bolts[0], boltLengths[0]);

                for (int i = 1; i < bolts.Count(); i++)
                {
                    InsertThreeRows(trCells, clonedFirstTr, t, i + 1, bolts[i], boltLengths[i]);
                }

                var newTr = clonedFirstTr.CloneNode(true);
                trCells = newTr.Descendants<TableCell>().ToList();

                trCells[1].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement("Всего", 24));

                double sum = 0.0;
                for (var i = 0; i < bolts.Count(); i++)
                {
                    sum += Math.Ceiling(bolts[i].Num * boltLengths[i].Weight * 10) / 10;
                }
                trCells[7].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(sum.ToString(), 24));

                sum = 0.0;
                for (var i = 0; i < bolts.Count(); i++)
                {
                    sum += Math.Ceiling(bolts[i].NutNum * bolts[i].Diameter.NutWeight * 10) / 10;
                }
                trCells[8].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(sum.ToString(), 24));

                sum = 0.0;
                for (var i = 0; i < bolts.Count(); i++)
                {
                    sum += Math.Ceiling(bolts[i].WasherNum * bolts[i].Diameter.WasherWeight * 10) / 10;
                }
                trCells[9].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(sum.ToString(), 24));

                t.Append(newTr);
            }
        }

        private static void InsertThreeRows(
            List<TableCell> trCells,
            OpenXmlElement clonedFirstTr,
            Table t,
            int num,
            ConstructionBolt bolt,
            BoltLength boltLength)
        {
            // Болт
            OpenXmlElement newTr = null;
            if (num != 1)
            {
                newTr = clonedFirstTr.CloneNode(true);
                trCells = newTr.Descendants<TableCell>().ToList();
            }

            trCells[0].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement(num.ToString(), 24));
            trCells[1].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement($"Высокопрочные болты по {bolt.Diameter.BoltTechSpec}", 24));
            trCells[2].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement(bolt.Diameter.Diameter.ToString(), 24));
            trCells[3].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement(bolt.Diameter.StrengthClass, 24));
            trCells[4].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement(bolt.Packet.ToString(), 24));
            trCells[5].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement(boltLength.Length.ToString(), 24));
            trCells[6].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement(bolt.Num.ToString(), 24));
            trCells[7].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement(
                    (Math.Ceiling(bolt.Num * boltLength.Weight * 10) / 10).ToString(), 24));

            foreach (var cell in trCells)
            {
                var c1 = cell.GetFirstChild<TableCellProperties>();
                var c2 = c1.GetFirstChild<TableCellBorders>();
                var c3 = c2.GetFirstChild<BottomBorder>();
                c3.Size = 4;
            }

            if (num != 1)
                t.Append(newTr);

            // Гайка
            newTr = clonedFirstTr.CloneNode(true);
            trCells = newTr.Descendants<TableCell>().ToList();

            trCells[1].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement($"Гайки по {bolt.Diameter.NutTechSpec}", 24));
            trCells[3].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement("?", 24));
            trCells[6].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement(bolt.NutNum.ToString(), 24));
            trCells[8].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement(
                    (Math.Ceiling(bolt.NutNum * bolt.Diameter.NutWeight * 10) / 10).ToString(), 24));

            foreach (var cell in trCells)
            {
                var c1 = cell.GetFirstChild<TableCellProperties>();
                var c2 = c1.GetFirstChild<TableCellBorders>();
                var c3 = c2.GetFirstChild<TopBorder>();
                var c4 = c2.GetFirstChild<BottomBorder>();
                c3.Size = 4;
                c4.Size = 4;
            }

            t.Append(newTr);

            // Шайба
            newTr = clonedFirstTr.CloneNode(true);
            trCells = newTr.Descendants<TableCell>().ToList();

            trCells[1].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement($"Шайбы по {bolt.Diameter.WasherTechSpec}", 24));
            trCells[3].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement(bolt.Diameter.WasherSteel, 24));
            trCells[6].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement(bolt.WasherNum.ToString(), 24));
            trCells[9].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement(
                    (Math.Ceiling(bolt.WasherNum * bolt.Diameter.WasherWeight * 10) / 10).ToString(), 24));

            foreach (var cell in trCells)
            {
                var c1 = cell.GetFirstChild<TableCellProperties>();
                var c2 = c1.GetFirstChild<TableCellBorders>();
                var c3 = c2.GetFirstChild<TopBorder>();
                c3.Size = 4;
            }

            t.Append(newTr);
        }
    }
}
