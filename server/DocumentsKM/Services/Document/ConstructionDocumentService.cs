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

namespace DocumentsKM.Services
{
    public class ConstructionDocumentService : IConstructionDocumentService
    {
        private readonly int _departmentHeadPosId = 7;
        
        private readonly IMarkRepo _markRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IConstructionRepo _constructionRepo;
        private readonly IStandardConstructionRepo _standardConstructionRepo;
        private readonly IConstructionElementRepo _constructionElementRepo;

        public ConstructionDocumentService(
            IMarkRepo markRepo,
            IEmployeeRepo employeeRepo,
            IConstructionRepo constructionRepo,
            IStandardConstructionRepo standardConstructionRepo,
            IConstructionElementRepo constructionElementRepo)
        {
            _markRepo = markRepo;
            _employeeRepo = employeeRepo;
            _constructionRepo = constructionRepo;
            _standardConstructionRepo = standardConstructionRepo;
            _constructionElementRepo = constructionElementRepo;
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

            var constructions = _constructionRepo.GetAllByMarkId(markId);
            var standardConstructions = _standardConstructionRepo.GetAllByMarkId(markId);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memory, true))
            {
                var markName = MarkHelper.MakeMarkName(
                    project.BaseSeries, node.Code, subnode.Code, mark.Code);
                (var complexName, var objectName) = MarkHelper.MakeComplexAndObjectName(
                    project.Name, node.Name, subnode.Name, mark.Name);

                AppendConstructionToTable(wordDoc,
                    constructions.ToList(),
                    standardConstructions.ToList());
                Word.AppendToMediumFooterTable(
                    wordDoc,
                    markName,
                    complexName,
                    objectName,
                    mark,
                    departmentHead);
                Word.AppendToSmallFooterTable(wordDoc, markName);
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

                    p = trCells[11].GetFirstChild<Paragraph>();
                    p.Append(Word.GetTextElement(Math.Round(localSum * multiplier, 3).ToStringWithComma(), 24));
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
    }
}
