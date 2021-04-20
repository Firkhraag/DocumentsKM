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

        public void PopulateDocument(int markId, int numOfPages, MemoryStream memory)
        {
            var mark = _markRepo.GetById(markId);
            if (mark == null)
                throw new ArgumentNullException(nameof(mark));

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
                departmentHead = new Employee{
                    Name = "",
                };

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memory, true))
            {
                AppendToMainFooterTable(
                    wordDoc,
                    mark.Designation,
                    mark.ComplexName,
                    mark.ObjectName,
                    departmentHead);
                Word.AppendToSmallFooterTable(wordDoc, mark.Designation);

                for (int i = 1; i < numOfPages; i++)
                {
                    Body body = wordDoc.MainDocumentPart.Document.Body;
                    var p = new Paragraph(new Run(new Break
                    {
                        Type = BreakValues.Page
                    }));
                    body.AppendChild(p);
                }
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
