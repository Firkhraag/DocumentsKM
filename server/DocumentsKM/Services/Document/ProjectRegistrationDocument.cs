using DocumentsKM.Data;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using System;
using DocumentsKM.Helpers;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using DocumentsKM.Models;
using System.Linq;
using DocumentFormat.OpenXml;

namespace DocumentsKM.Services
{
    public class ProjectRegistrationDocumentService : IProjectRegistrationDocumentService
    {
        private readonly IMarkRepo _markRepo;
        private readonly IAttachedDocRepo _attachedDocRepo;
        private readonly IConstructionRepo _constructionRepo;
        private readonly IStandardConstructionRepo _standardConstructionRepo;
        private readonly IConstructionElementRepo _constructionElementRepo;
        private readonly IDocService _docService;
        private readonly IAdditionalWorkService _additionalWorkService;

        public ProjectRegistrationDocumentService(
            IMarkRepo markRepo,
            IAttachedDocRepo attachedDocRepo,
            IConstructionRepo constructionRepo,
            IStandardConstructionRepo standardConstructionRepo,
            IConstructionElementRepo constructionElementRepo,
            IDocService docService,
            IAdditionalWorkService additionalWorkService)
        {
            _markRepo = markRepo;
            _attachedDocRepo = attachedDocRepo;
            _constructionRepo = constructionRepo;
            _standardConstructionRepo = standardConstructionRepo;
            _constructionElementRepo = constructionElementRepo;
            _docService = docService;
            _additionalWorkService = additionalWorkService;
        }

        public void PopulateDocument(int markId, MemoryStream memory)
        {
            var mark = _markRepo.GetById(markId);
            if (mark == null)
                throw new ArgumentNullException(nameof(mark));
            var subnode = mark.Subnode;
            var node = subnode.Node;
            var project = node.Project;

            var sheets = _docService.GetAllSheetsByMarkId(markId).ToList();
            var docs = _docService.GetAllAttachedByMarkId(markId).ToList();
            var attachedDocs = _attachedDocRepo.GetAllByMarkId(markId).ToList();
            var additionalWork = _additionalWorkService.GetAllByMarkId(markId).ToList();
            var constructions = _constructionRepo.GetAllByMarkId(markId).ToList();
            var standardConstructions = _standardConstructionRepo.GetAllByMarkId(markId).ToList();

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memory, true))
            {
                var markName = MarkHelper.MakeMarkName(
                    project.BaseSeries, node.Code, subnode.Code, mark.Code);
                (var complexName, var objectName) = MarkHelper.MakeComplexAndObjectName(
                    project.Name, node.Name, subnode.Name, mark.Name);

                ReplaceText(wordDoc, "A", markName);
                ReplaceText(wordDoc, "B", complexName);
                ReplaceText(wordDoc, "C", objectName);
                ReplaceText(wordDoc, "D", mark.Subnode.Node.ChiefEngineer.Name);
                ReplaceText(wordDoc, "E", mark.GroupLeader.Name);
                ReplaceText(
                    wordDoc, "F", mark.IssuedDate.GetValueOrDefault().ToString("dd.MM.yyyy"));
                ReplaceText(wordDoc, "G", FindWeight(constructions, standardConstructions).ToStringWithComma());
                AppendToSheetsTable(wordDoc, sheets);
                AppendToDocsTable(wordDoc, docs);
                AppendToAttachedDocsTable(wordDoc, attachedDocs);
                AppendToAdditionalWorkTable(wordDoc, additionalWork);

                Word.AppendToMainSmallFooterTable(wordDoc, markName);
                Word.AppendToSmallFooterTable(wordDoc, markName);
            }
        }

        private void ReplaceText(WordprocessingDocument wordDoc, string textToReplace, string replacedText)
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

        private void AppendToSheetsTable(
            WordprocessingDocument document,
            List<Doc> sheets)
        {
            Body body = document.MainDocumentPart.Document.Body;
            var t = body.Descendants<Table>().ToList()[0];

            var firstTr = t.Descendants<TableRow>().ToList()[2];
            var clonedFirstTr = firstTr.CloneNode(true);
            var trCells = firstTr.Descendants<TableCell>().ToList();

            OpenXmlElement newTr = null;
            for (int i = 0; i < sheets.Count(); i++)
            {
                if (i != 0)
                {
                    newTr = clonedFirstTr.CloneNode(true);
                    trCells = newTr.Descendants<TableCell>().ToList();
                }

                trCells[0].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement((i + 1).ToString(), 22));
                trCells[1].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(sheets[i].Name, 22));
                trCells[2].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(sheets[i].Note, 22));

                var p = trCells[3].GetFirstChild<Paragraph>();
                if (sheets[i].Creator != null)
                {
                    var run = Word.GetTextElement(sheets[i].Creator.Name, 22);
                    if (sheets[i].NormContr != null)
                        run.AppendChild(new Break());
                    p.Append(run);
                }
                if (sheets[i].NormContr != null)
                    p.Append(Word.GetTextElement(sheets[i].NormContr.Name, 22));

                trCells[4].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(sheets[i].Form.ToStringWithComma(), 22));

                if (i != 0)
                    t.Append(newTr);

                if (i != sheets.Count() - 1)
                    RemoveBorders(trCells);
            }
        }

        private void AppendToDocsTable(
            WordprocessingDocument document,
            List<Doc> docs)
        {
            Body body = document.MainDocumentPart.Document.Body;
            var t = body.Descendants<Table>().ToList()[1];

            var firstTr = t.Descendants<TableRow>().ToList()[2];
            var clonedFirstTr = firstTr.CloneNode(true);
            var trCells = firstTr.Descendants<TableCell>().ToList();

            OpenXmlElement newTr = null;
            for (int i = 0; i < docs.Count(); i++)
            {
                if (i != 0)
                {
                    newTr = clonedFirstTr.CloneNode(true);
                    trCells = newTr.Descendants<TableCell>().ToList();
                }

                trCells[0].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(docs[i].Type.Code, 22));
                trCells[1].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(docs[i].Name, 22));
                trCells[2].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(docs[i].Note, 22));
                
                var p = trCells[3].GetFirstChild<Paragraph>();
                var run = Word.GetTextElement(docs[i].Creator.Name, 22);
                if (docs[i].NormContr != null)
                    run.AppendChild(new Break());
                p.Append(run);
                if (docs[i].NormContr != null)
                    p.Append(Word.GetTextElement(docs[i].NormContr.Name, 22));

                trCells[4].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(docs[i].Form.ToStringWithComma(), 22));

                if (i != 0)
                    t.Append(newTr);

                if (i != docs.Count() - 1)
                    RemoveBorders(trCells);
            }
        }

        private void AppendToAttachedDocsTable(
            WordprocessingDocument document,
            List<AttachedDoc> attachedDocs)
        {
            Body body = document.MainDocumentPart.Document.Body;
            var t = body.Descendants<Table>().ToList()[2];

            var firstTr = t.Descendants<TableRow>().ToList()[2];
            var clonedFirstTr = firstTr.CloneNode(true);
            var trCells = firstTr.Descendants<TableCell>().ToList();

            OpenXmlElement newTr = null;
            for (int i = 0; i < attachedDocs.Count(); i++)
            {
                if (i != 0)
                {
                    newTr = clonedFirstTr.CloneNode(true);
                    trCells = newTr.Descendants<TableCell>().ToList();
                }

                trCells[0].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(attachedDocs[i].Designation, 22));
                trCells[1].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(attachedDocs[i].Name, 22));
                trCells[2].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(attachedDocs[i].Note, 22));

                if (i != 0)
                    t.Append(newTr);

                if (i != attachedDocs.Count() - 1)
                    RemoveBorders(trCells);
            }
        }

        private void AppendToAdditionalWorkTable(
            WordprocessingDocument document,
            List<Dtos.AdditionalWorkResponse> additionalWork)
        {
            const double valuationCoeff = 0.05;
            const double orderCoeff = 0.004;

            Body body = document.MainDocumentPart.Document.Body;
            var t = body.Descendants<Table>().ToList()[3];

            var firstTr = t.Descendants<TableRow>().ToList()[3];
            var clonedFirstTr = firstTr.CloneNode(true);
            var trCells = firstTr.Descendants<TableCell>().ToList();

            OpenXmlElement newTr = null;
            double sum;
            for (int i = 0; i < additionalWork.Count(); i++)
            {
                if (i != 0)
                {
                    newTr = clonedFirstTr.CloneNode(true);
                    trCells = newTr.Descendants<TableCell>().ToList();
                }

                sum = 0.0;
                trCells[0].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(additionalWork[i].Employee.Name, 22));
                if (Math.Abs(additionalWork[i].DrawingsCompleted) > 0.0000001)
                {
                    sum += additionalWork[i].DrawingsCompleted;
                    trCells[1].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement(additionalWork[i].DrawingsCompleted.ToStringWithComma(), 22));
                }
                if (Math.Abs(additionalWork[i].DrawingsCheck) > 0.0000001)
                    trCells[2].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement(additionalWork[i].DrawingsCheck.ToStringWithComma(), 22));
                if (Math.Abs(additionalWork[i].DrawingsCheck) > 0.0000001)
                {
                    var v = Math.Round(additionalWork[i].DrawingsCheck * valuationCoeff, 3);
                    sum += v;
                    trCells[3].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement(v.ToStringWithComma(), 22));
                }
                if (additionalWork[i].Valuation != 0)
                    trCells[4].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement(additionalWork[i].Valuation.ToString(), 22));
                if (additionalWork[i].Valuation != 0)
                {
                    var v = Math.Round(additionalWork[i].Valuation * valuationCoeff, 3);
                    sum += v;
                    trCells[5].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement(v.ToStringWithComma(), 22));
                }
                if (additionalWork[i].MetalOrder != 0)
                    trCells[6].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement(additionalWork[i].MetalOrder.ToString(), 22));
                if (additionalWork[i].MetalOrder != 0)
                {
                    var v = Math.Round(additionalWork[i].MetalOrder * orderCoeff, 3);
                    sum += v;
                    trCells[7].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement(v.ToStringWithComma(), 22));
                }
                trCells[8].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(Math.Round(sum, 3).ToStringWithComma(), 22));

                if (i != 0)
                    t.Append(newTr);

                if (i != additionalWork.Count() - 1)
                    RemoveBorders(trCells);
            }

            newTr = clonedFirstTr.CloneNode(true);
            trCells = newTr.Descendants<TableCell>().ToList();

            sum = 0.0;
            var p = trCells[0].GetFirstChild<Paragraph>();
            p.ParagraphProperties.Append(new Justification
            {
                Val = JustificationValues.Center,
            });
            p.Append(Word.GetTextElement("Итого:", 22));

            var sum2 = additionalWork.Sum(v => v.DrawingsCompleted);
            if (Math.Abs(sum2) > 0.0000001)
            {
                sum += sum2;
                trCells[1].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(sum2.ToStringWithComma(), 22));
            }
            sum2 = additionalWork.Sum(v => v.DrawingsCheck);
            if (Math.Abs(sum2) > 0.0000001)
                trCells[2].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(sum2.ToStringWithComma(), 22));
            if (Math.Abs(sum2) > 0.0000001)
            {
                var v = Math.Round(sum2 * valuationCoeff, 3);
                sum += v;
                trCells[3].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(v.ToStringWithComma(), 22));
            }
            sum2 = additionalWork.Sum(v => v.Valuation);
            if (sum2 != 0)
                trCells[4].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(sum2.ToString(), 22));
            if (sum2 != 0)
            {
                var v = Math.Round(sum2 * valuationCoeff, 3);
                sum += v;
                trCells[5].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(v.ToStringWithComma(), 22));
            }
            sum2 = additionalWork.Sum(v => v.MetalOrder);
            if (sum2 != 0)
                trCells[6].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(sum2.ToString(), 22));
            if (sum2 != 0)
            {
                var v = Math.Round(sum2 * orderCoeff, 3);
                sum += v;
                trCells[7].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement(v.ToStringWithComma(), 22));
            }
            trCells[8].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement(Math.Round(sum, 3).ToStringWithComma(), 22));

            t.Append(newTr);
        }

        private void RemoveBorders(List<TableCell> trCells)
        {
            foreach (var cell in trCells)
            {
                var tcp = cell.GetFirstChild<TableCellProperties>();
                var tcb = tcp.GetFirstChild<TableCellBorders>();
                var bb = tcb.GetFirstChild<BottomBorder>();
                bb.Val = new EnumValue<BorderValues>(BorderValues.Nil);
                var tb = tcb.GetFirstChild<TopBorder>();
                tb.Val = new EnumValue<BorderValues>(BorderValues.Nil);
            }
        }

        private double FindWeight(
            List<Construction> constructions,
            List<StandardConstruction> standardConstructions)
        {
            const double multiplier = 1.04;

            var sum = 0.0;
            for (int i = 0; i < constructions.Count(); i++)
            {
                var constructionElements = _constructionElementRepo.GetAllByConstructionId(
                    constructions[i].Id);
                sum += Math.Ceiling(constructionElements.Sum(
                    v => v.Profile.Weight * v.Length) * multiplier) / 1000;
            }

            for (int i = 0; i < standardConstructions.Count(); i++)
            {
                sum += standardConstructions[i].Weight;
            }

            return Math.Round(sum, 1);
        }
    }
}
