using DocumentsKM.Data;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using System;
using DocumentsKM.Helpers;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using DocumentsKM.Models;
using System.Linq;

namespace DocumentsKM.Services
{
    public class ProjectRegistrationDocumentService : IProjectRegistrationDocumentService
    {
        private readonly IMarkRepo _markRepo;

        public ProjectRegistrationDocumentService(
            IMarkRepo markRepo,
            IEmployeeRepo employeeRepo,
            IConstructionRepo constructionRepo,
            IConstructionElementRepo constructionElementRepo,
            IStandardConstructionRepo standardConstructionRepo,
            IMarkOperatingConditionsRepo markOperatingConditionsRepo,
            IEstimateTaskRepo estimateTaskRepo,
            IMarkGeneralDataPointRepo markGeneralDataPointRepo)
        {
            _markRepo = markRepo;
        }

        public void PopulateDocument(int markId, MemoryStream memory)
        {
            var mark = _markRepo.GetById(markId);
            if (mark == null)
                throw new ArgumentNullException(nameof(mark));
            var subnode = mark.Subnode;
            var node = subnode.Node;
            var project = node.Project;

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memory, true))
            {
                var markName = MarkHelper.MakeMarkName(
                    project.BaseSeries, node.Code, subnode.Code, mark.Code);
                (var complexName, var objectName) = MarkHelper.MakeComplexAndObjectName(
                    project.Name, node.Name, subnode.Name, mark.Name);

                ReplaceText(wordDoc, "A", markName);
                ReplaceText(wordDoc, "B", complexName);
                ReplaceText(wordDoc, "C", objectName);
                ReplaceText(wordDoc, "D", "111");
                ReplaceText(wordDoc, "E", "111");
                ReplaceText(wordDoc, "F", "111");

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

        private void AppendToLinkedAndAttachedDocsTable(
            WordprocessingDocument document,
            List<AttachedDoc> attachedDocs)
        {
            Body body = document.MainDocumentPart.Document.Body;
            var t = body.Descendants<Table>().FirstOrDefault(
                tbl => tbl.InnerText.Contains("Обозначение"));

            var firstTr = t.Descendants<TableRow>().ToList()[1];
            var clonedFirstTr = firstTr.CloneNode(true);

            if (attachedDocs.Count() > 0)
            {
                var newTr = clonedFirstTr.CloneNode(true);
                var trCells = newTr.Descendants<TableCell>().ToList();
                var p = trCells[1].GetFirstChild<Paragraph>();
                p.ParagraphProperties.Append(new Justification
                {
                    Val = JustificationValues.Center,
                });

                p.Append(Word.GetTextElement("Прилагаемые документы", 26, true));
                t.Append(newTr);

                for (int i = 0; i < attachedDocs.Count(); i++)
                {
                    newTr = clonedFirstTr.CloneNode(true);
                    trCells = newTr.Descendants<TableCell>().ToList();

                    trCells[0].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement(attachedDocs[i].Designation, 26));
                    trCells[1].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement(attachedDocs[i].Name, 26));
                    trCells[2].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement(attachedDocs[i].Note, 26));

                    t.Append(newTr);
                }
            }
        }
    }
}
