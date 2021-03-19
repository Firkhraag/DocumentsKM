using DocumentsKM.Models;
using DocumentsKM.Data;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using System;
using DocumentsKM.Helpers;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Options;

namespace DocumentsKM.Services
{
    public class EstimationPagesDocumentService : IEstimationPagesDocumentService
    {
        private readonly IMarkRepo _markRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly AppSettings _appSettings;

        public EstimationPagesDocumentService(
            IMarkRepo markRepo,
            IEmployeeRepo employeeRepo,
            IOptions<AppSettings> appSettings)
        {
            _markRepo = markRepo;
            _employeeRepo = employeeRepo;
            _appSettings = appSettings.Value;
        }

        public void PopulateDocument(int markId, MemoryStream memory)
        {
            var mark = _markRepo.GetById(markId);
            if (mark == null)
                throw new ArgumentNullException(nameof(mark));
            var subnode = mark.Subnode;
            var node = subnode.Node;
            var project = node.Project;

            var departmentHead = _employeeRepo.GetByDepartmentIdAndPosition(
                mark.Department.Id, _appSettings.DepartmentHeadPosId);
            if (departmentHead == null)
                departmentHead = _employeeRepo.GetByDepartmentIdAndPosition(
                mark.Department.Id, _appSettings.ActingDepartmentHeadPosId);
            if (departmentHead == null)
                departmentHead = _employeeRepo.GetByDepartmentIdAndPosition(
                mark.Department.Id, _appSettings.DeputyDepartmentHeadPosId);
            if (departmentHead == null)
                departmentHead = _employeeRepo.GetByDepartmentIdAndPosition(
                mark.Department.Id, _appSettings.ActingDeputyDepartmentHeadPosId);
            if (departmentHead == null)
                throw new ConflictException();

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
