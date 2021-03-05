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
    public class EstimationPagesDocumentService : IEstimationPagesDocumentService
    {
        private readonly int _departmentHeadPosId = 7;

        private readonly IMarkRepo _markRepo;
        private readonly IEmployeeRepo _employeeRepo;

        public EstimationPagesDocumentService(
            IMarkRepo markRepo,
            IEmployeeRepo employeeRepo)
        {
            _markRepo = markRepo;
            _employeeRepo = employeeRepo;
        }

        public void PopulateDocument(int markId, MemoryStream memory)
        {
            var mark = _markRepo.GetById(markId);
            if (mark == null)
                throw new ArgumentNullException(nameof(mark));
            var subnode = mark.Subnode;
            var node = subnode.Node;
            var project = node.Project;

            var departmentHeadArr = _employeeRepo.GetAllByDepartmentIdAndPosition(
                mark.Department.Id,
                _departmentHeadPosId);
            if (departmentHeadArr.Count() != 1)
                throw new ConflictException();
            var departmentHead = departmentHeadArr.ToList()[0];

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memory, true))
            {
                var markName = MarkHelper.MakeMarkName(
                    project.BaseSeries, node.Code, subnode.Code, mark.Code);
                (var complexName, var objectName) = MarkHelper.MakeComplexAndObjectName(
                    project.Name, node.Name, subnode.Name, mark.Name);

                AppendToMainFooterTable(
                    wordDoc, markName, complexName, objectName, departmentHead);
                Word.AppendToSmallFooterTable(wordDoc, markName);
            }
        }

        private void AppendToMainFooterTable(
            WordprocessingDocument document,
            string markFullCodeName,
            string complexName,
            string objectName,
            Employee departmentHead)
        {
            MainDocumentPart mainPart = document.MainDocumentPart;
            var commonFooter = mainPart.FooterParts.LastOrDefault();
            var t = commonFooter.RootElement.Descendants<Table>().FirstOrDefault();
            var trArr = t.Descendants<TableRow>().ToList();

            var trCells = trArr[0].Descendants<TableCell>().ToList();
            var tc = trCells[6];
            var p = tc.GetFirstChild<Paragraph>();
            p.Append(Word.GetTextElement(markFullCodeName + ".РР", 28));

            trCells = trArr[3].Descendants<TableCell>().ToList();
            tc = trCells[4];
            p = tc.GetFirstChild<Paragraph>();
            var text = Word.GetTextElement(complexName, 20);
            text.AppendChild(new Break());
            p.Append(text);
            p.Append(Word.GetTextElement(objectName, 20));

            trCells = trArr[7].Descendants<TableCell>().ToList();
            tc = trCells[1];
            p = tc.GetFirstChild<Paragraph>();
            p.Append(Word.GetTextElement(departmentHead.Name, 22));
        }
    }
}