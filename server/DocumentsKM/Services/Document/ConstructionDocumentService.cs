using DocumentsKM.Data;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using System;
using DocumentsKM.Helpers;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocumentsKM.Services
{
    public class ConstructionDocumentService : IConstructionDocumentService
    {
        private readonly int _departmentHeadPosId = 7;
        private readonly int _sheetDocTypeId = 1;
        
        private readonly IMarkRepo _markRepo;
        private readonly IMarkApprovalRepo _markApprovalRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IDocRepo _docRepo;

        public ConstructionDocumentService(
            IMarkRepo markRepo,
            IMarkApprovalRepo markApprovalRepo,
            IEmployeeRepo employeeRepo,
            IDocRepo docRepo)
        {
            _markRepo = markRepo;
            _markApprovalRepo = markApprovalRepo;
            _employeeRepo = employeeRepo;
            _docRepo = docRepo;
        }

        public void PopulateDocument(int markId, MemoryStream memory)
        {
            var mark = _markRepo.GetById(markId);
            if (mark == null)
                throw new ArgumentNullException(nameof(mark));
            var markApprovals = _markApprovalRepo.GetAllByMarkId(markId);
            var subnode = mark.Subnode;
            var node = subnode.Node;
            var project = node.Project;

            var departmentHeadArr = _employeeRepo.GetAllByDepartmentIdAndPosition(
                mark.Department.Id,
                _departmentHeadPosId);
            if (departmentHeadArr.Count() != 1)
                throw new ConflictException();
            var departmentHead = departmentHeadArr.ToList()[0];

            var sheets = _docRepo.GetAllByMarkIdAndDocType(markId, _sheetDocTypeId);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memory, true))
            {
                var markName = MarkHelper.MakeMarkName(
                    project.BaseSeries, node.Code, subnode.Code, mark.Code);
                (var complexName, var objectName) = MarkHelper.MakeComplexAndObjectName(
                    project.Name, node.Name, subnode.Name, mark.Name);

                AppendToTable(wordDoc);   
                Word.AppendToBigFooterTable(
                    wordDoc,
                    markName,
                    complexName,
                    objectName,
                    sheets.Count(),
                    mark,
                    markApprovals.ToList(),
                    departmentHead);
            }
        }

        private void AppendToTable(
            WordprocessingDocument document)
        {
            // if (constructions.Count() > 0)
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
                trCells[6].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement("1", 24));
                trCells[7].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement("1", 24));
                trCells[8].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement("1", 24));
                trCells[9].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement("1", 24));
                trCells[10].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement("1", 24));
                trCells[11].GetFirstChild<Paragraph>().Append(
                    Word.GetTextElement("1", 24));
            }
        }
    }
}
