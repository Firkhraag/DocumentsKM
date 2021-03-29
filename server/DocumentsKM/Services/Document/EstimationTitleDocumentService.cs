using DocumentsKM.Data;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using System;
using DocumentsKM.Helpers;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocumentsKM.Services
{
    public class EstimationTitleDocumentService : IEstimationTitleDocumentService
    {
        private readonly IMarkRepo _markRepo;

        public EstimationTitleDocumentService(
            IMarkRepo markRepo)
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

                AddMarkInfo(wordDoc, complexName, objectName, markName);
                AddChiefEngineer(wordDoc, mark.Subnode.Node.ChiefEngineer);
                AddYear(wordDoc);
            }
        }

        private void AddMarkInfo(
            WordprocessingDocument document,
            string complexName,
            string objectName,
            string markName)
        {
            MainDocumentPart mainPart = document.MainDocumentPart;

            var header = mainPart.HeaderParts.FirstOrDefault();
            var t = header.RootElement.Descendants<Table>().FirstOrDefault();
            var firstTr = t.Descendants<TableRow>().FirstOrDefault();
            var firstTrCells = firstTr.Descendants<TableCell>().ToList();
            var p = firstTrCells[0].GetFirstChild<Paragraph>();
            var text = Word.GetTextElement(complexName, 28, false, false, true);
            text.AppendChild(new Break());
            p.Append(text);
            p.Append(Word.GetTextElement(objectName, 28, false, false, true));

            t = header.RootElement.Descendants<Table>().ToList()[1];
            firstTr = t.Descendants<TableRow>().LastOrDefault();
            firstTrCells = firstTr.Descendants<TableCell>().ToList();
            p = firstTrCells[0].GetFirstChild<Paragraph>();
            p.Append(Word.GetTextElement(markName + ".РР", 32, false, false, true));
        }

        private void AddChiefEngineer(WordprocessingDocument document, string name)
        {
            Body body = document.MainDocumentPart.Document.Body;
            var t = body.Descendants<Table>().FirstOrDefault();
            var firstTr = t.Descendants<TableRow>().ToList()[0];
            var trCells = firstTr.Descendants<TableCell>().ToList();
            trCells[1].GetFirstChild<Paragraph>().Append(
                Word.GetTextElement(name, 28));
        }

        private void AddYear(WordprocessingDocument document)
        {
            MainDocumentPart mainPart = document.MainDocumentPart;

            var footer = mainPart.FooterParts.LastOrDefault();
            var t = footer.RootElement.Descendants<Table>().FirstOrDefault();

            var firstTr = t.Descendants<TableRow>().FirstOrDefault();
            var firstTrCells = firstTr.Descendants<TableCell>().ToList();
            var p = firstTrCells[0].GetFirstChild<Paragraph>();
            p.Append(Word.GetTextElement(DateTime.Now.Year.ToString(), 28));
        }
    }
}
