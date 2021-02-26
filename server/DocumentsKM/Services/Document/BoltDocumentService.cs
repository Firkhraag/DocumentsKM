using DocumentsKM.Models;
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
    public class BoltDocumentService : IBoltDocumentService
    {
        private readonly IMarkRepo _markRepo;
        private readonly IBoltLengthRepo _boltLengthRepo;
        private readonly IConstructionBoltRepo _constructionBoltRepo;

        public BoltDocumentService(
            IMarkRepo markRepo,
            IBoltLengthRepo boltLengthRepo,
            IConstructionBoltRepo constructionBoltRepo)
        {
            _markRepo = markRepo;
            _boltLengthRepo = boltLengthRepo;
            _constructionBoltRepo = constructionBoltRepo;
        }

        public void PopulateDocument(int markId, MemoryStream memory)
        {
            var mark = _markRepo.GetById(markId);
            if (mark == null)
                throw new ArgumentNullException(nameof(mark));
            var subnode = mark.Subnode;
            var node = subnode.Node;
            var project = node.Project;

            var constructionBolts = _constructionBoltRepo.GetAllByMarkId(markId);
            var boltLengths = new List<BoltLength> {};
            foreach (var bolt in constructionBolts)
            {
                var arr = _boltLengthRepo.GetAllByDiameterId(bolt.Diameter.Id);
                var bl = arr.Where(
                    v => v.Length >= bolt.Packet + v.ScrewLength).Aggregate(
                        (i1, i2) => i1.Length < i2.Length ? i1 : i2);
                boltLengths.Add(bl);
            }
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memory, true))
            {
                var markName = MarkHelper.MakeMarkName(
                    project.BaseSeries, node.Code, subnode.Code, mark.Code);
                AppendToTable(wordDoc, constructionBolts.ToList(), boltLengths);
                Word.AppendToSmallFooterTable(wordDoc, markName);
                Word.AppendToMainSmallFooterTable(wordDoc, markName);
            }
        }

        private void AppendToTable(
            WordprocessingDocument document,
            List<ConstructionBolt> bolts,
            List<BoltLength> boltLengths)
        {
            if (bolts.Count() > 0)
            {
                Body body = document.MainDocumentPart.Document.Body;
                var t = body.Descendants<Table>().FirstOrDefault();
                var firstTr = t.Descendants<TableRow>().ToList()[0];
                var clonedFirstTr = firstTr.CloneNode(true);
                var trCells = firstTr.Descendants<TableCell>().ToList();

                for (int i = 0; i < bolts.Count(); i++)
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
                    Word.GetTextElement(Math.Round(sum, 1).ToStringWithComma(), 24));

                sum = 0.0;
                for (var i = 0; i < bolts.Count(); i++)
                {
                    sum += Math.Ceiling(bolts[i].NutNum * bolts[i].Diameter.NutWeight * 10) / 10;
                }
                trCells[8].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(Math.Round(sum, 1).ToStringWithComma(), 24));

                sum = 0.0;
                for (var i = 0; i < bolts.Count(); i++)
                {
                    sum += Math.Ceiling(bolts[i].WasherNum * bolts[i].Diameter.WasherWeight * 10) / 10;
                }
                trCells[9].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(Math.Round(sum, 1).ToStringWithComma(), 24));
                t.Append(newTr);
            }
        }

        private void InsertThreeRows(
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
                Word.GetTextElement(bolt.Diameter.StrengthClass.ToStringWithComma(), 24));
            trCells[4].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement(bolt.Packet.ToString(), 24));
            trCells[5].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement(boltLength.Length.ToString(), 24));
            trCells[6].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement(bolt.Num.ToString(), 24));
            trCells[7].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement(
                    (Math.Ceiling(bolt.Num * boltLength.Weight * 10) / 10).ToStringWithComma(), 24));
            Word.MakeBordersThin(trCells, true, false);
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
                    (Math.Ceiling(bolt.NutNum * bolt.Diameter.NutWeight * 10) / 10).ToStringWithComma(), 24));
            Word.MakeBordersThin(trCells);
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
                    (Math.Ceiling(bolt.WasherNum * bolt.Diameter.WasherWeight * 10) / 10).ToStringWithComma(), 24));
            Word.MakeBordersThin(trCells, false);
            t.Append(newTr);
        }
    }
}
