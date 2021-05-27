using DocumentsKM.Data;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using System;
using DocumentsKM.Helpers;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentFormat.OpenXml;
using Microsoft.Extensions.Options;

namespace DocumentsKM.Services
{
    public class ConstructionDocumentService : IConstructionDocumentService
    {
        private readonly IMarkRepo _markRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IDocRepo _docRepo;
        private readonly IConstructionRepo _constructionRepo;
        private readonly IStandardConstructionRepo _standardConstructionRepo;
        private readonly IConstructionElementRepo _constructionElementRepo;
        private readonly IOrganizationNameRepo _organizationNameRepo;
        private readonly AppSettings _appSettings;

        public ConstructionDocumentService(
            IMarkRepo markRepo,
            IEmployeeRepo employeeRepo,
            IDocRepo docRepo,
            IConstructionRepo constructionRepo,
            IStandardConstructionRepo standardConstructionRepo,
            IConstructionElementRepo constructionElementRepo,
            IOrganizationNameRepo organizationNameRepo,
            IOptions<AppSettings> appSettings)
        {
            _markRepo = markRepo;
            _employeeRepo = employeeRepo;
            _docRepo = docRepo;
            _constructionRepo = constructionRepo;
            _standardConstructionRepo = standardConstructionRepo;
            _constructionElementRepo = constructionElementRepo;
            _organizationNameRepo = organizationNameRepo;
            _appSettings = appSettings.Value;
        }

        public void PopulateDocument(int markId, MemoryStream memory)
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

            var constructions = _constructionRepo.GetAllIncludedByMarkId(markId);
            var standardConstructions = _standardConstructionRepo.GetAllByMarkId(markId);
            var organizationShortName = _organizationNameRepo.Get().ShortName;

            var docs = _docRepo.GetAllByMarkId(markId);
            var creatorName = "";
            if (docs.Count() > 0)
            {
                creatorName = docs.ToList()[0].Creator.Name;
            }

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memory, true))
            {
                AppendConstructionToTable(wordDoc,
                    constructions.ToList(),
                    standardConstructions.ToList());
                Word.AppendToMediumFooterTable(
                    wordDoc,
                    mark.Designation,
                    mark.ComplexName,
                    mark.ObjectName,
                    mark,
                    departmentHead,
                    organizationShortName,
                    creatorName);
                AppendToSecondFooterTable(wordDoc, mark.Designation);
            }
        }

        private void AppendConstructionToTable(
            WordprocessingDocument document,
            List<Construction> constructions,
            List<StandardConstruction> standardConstructions)
        {
            const double multiplier = 1.04;
            if (constructions.Count() > 0)
            {
                var sums = new List<double> {};
                for (int i = 0; i < 12; i++)
                {
                    sums.Add(0.0);
                }

                Body body = document.MainDocumentPart.Document.Body;
                var t = body.Descendants<Table>().FirstOrDefault();
                var firstTr = t.Descendants<TableRow>().ToList()[0];
                var clonedFirstTr = firstTr.CloneNode(true);
                var trCells = firstTr.Descendants<TableCell>().ToList();
                var secondTr = t.Descendants<TableRow>().ToList()[1];
                var clonedSecondTr = secondTr.CloneNode(true);
                var thirdTr = t.Descendants<TableRow>().ToList()[2];
                var clonedThirdTr = thirdTr.CloneNode(true);
                t.RemoveChild(thirdTr);
                OpenXmlElement newTr = null;
                Paragraph p = null;

                for (int i = 0; i < constructions.Count(); i++)
                {
                    if (i > 0)
                    {
                        newTr = clonedFirstTr.CloneNode(true);
                        trCells = newTr.Descendants<TableCell>().ToList();
                    }
                    p = trCells[0].GetFirstChild<Paragraph>();
                    p.Append(Word.GetTextElement(constructions[i].Name, 24));
                    if (i > 0)
                        t.Append(newTr);

                    var constructionElements = _constructionElementRepo.GetAllByConstructionId(
                        constructions[i].Id);
                    if (i > 0)
                    {
                        newTr = clonedSecondTr.CloneNode(true);
                        trCells = newTr.Descendants<TableCell>().ToList();
                    }
                    else
                        trCells = secondTr.Descendants<TableCell>().ToList();

                    var weight = Math.Ceiling(constructionElements.Where(
                        v => v.Steel.Strength > 1).Sum(v => v.Profile.Weight * v.Length)) / 1000;
                    sums[0] += weight;
                    if (weight > 0)
                    {
                        p = trCells[0].GetFirstChild<Paragraph>();
                        p.Append(Word.GetTextElement(weight.ToStringWithComma(), 24));
                    }

                    var localSum = 0.0;
                    for (int k = 1; k < 11; k++)
                    {
                        var typeId = k;
                        if (k > 6)
                            typeId += 1;
                        if (k == 10)
                            weight = Math.Ceiling(constructionElements.Where(
                                v => v.Profile.Type.Id == typeId || v.Profile.Type.Id == 7).Sum(
                                    v => v.Profile.Weight * v.Length)) / 1000;
                        else
                            weight = Math.Ceiling(constructionElements.Where(
                                v => v.Profile.Type.Id == typeId).Sum(
                                    v => v.Profile.Weight * v.Length)) / 1000;
                        sums[k] += weight;
                        localSum += weight;
                        if (weight > 0)
                        {
                            p = trCells[k].GetFirstChild<Paragraph>();
                            p.Append(Word.GetTextElement(weight.ToStringWithComma(), 24));
                        }
                    }

                    if (localSum > 0.000001)
                    {
                        p = trCells[11].GetFirstChild<Paragraph>();
                        p.Append(Word.GetTextElement(Math.Round(localSum * multiplier, 3).ToStringWithComma(), 24));
                    }
                    
                    if (i > 0)
                        t.Append(newTr);
                }

                var standardConstWeightSum = 0.0;
                for (int i = 0; i < standardConstructions.Count(); i++)
                {
                    newTr = clonedThirdTr.CloneNode(true);
                    trCells = newTr.Descendants<TableCell>().ToList();
                    p = trCells[0].GetFirstChild<Paragraph>();
                    p.Append(Word.GetTextElement(standardConstructions[i].Name, 24));

                    p = trCells[1].GetFirstChild<Paragraph>();
                    p.Append(Word.GetTextElement(
                        (Math.Ceiling(standardConstructions[i].Weight * 1000) / 1000).ToStringWithComma(), 24));
                    t.Append(newTr);

                    standardConstWeightSum += standardConstructions[i].Weight;
                }

                var lastTr = clonedFirstTr.CloneNode(true);
                trCells = lastTr.Descendants<TableCell>().ToList();
                p = trCells[0].GetFirstChild<Paragraph>();
                p.Append(Word.GetTextElement("Итого массы", 24));
                t.Append(lastTr);

                lastTr = clonedSecondTr.CloneNode(true);
                trCells = lastTr.Descendants<TableCell>().ToList();
                for (int k = 0; k < 11; k++)
                {
                    if (sums[k] > 0)
                    {
                        p = trCells[k].GetFirstChild<Paragraph>();
                        p.Append(Word.GetTextElement(Math.Round(sums[k], 3).ToStringWithComma(), 24));
                    }
                }

                p = trCells[11].GetFirstChild<Paragraph>();
                p.Append(Word.GetTextElement(Math.Round(sums.Skip(1).Sum() * multiplier + standardConstWeightSum, 3).ToStringWithComma(), 24));
                t.Append(lastTr);
            }
        }

        private void AppendToSecondFooterTable(WordprocessingDocument document, string markName)
        {
            var columnIndexToFill = 6;
            MainDocumentPart mainPart = document.MainDocumentPart;
            var commonFooter = mainPart.FooterParts.ToList()[1];
            var t = commonFooter.RootElement.Descendants<Table>().FirstOrDefault();

            var firstTr = t.Descendants<TableRow>().FirstOrDefault();
            var firstTrCells = firstTr.Descendants<TableCell>().ToList();
            var tc = firstTrCells[columnIndexToFill];
            var p = tc.GetFirstChild<Paragraph>();
            p.Append(Word.GetTextElement(markName, 28));
        }
    }
}
