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
    public class SpecificationDocumentService : ISpecificationDocumentService
    {
        private readonly int _departmentHeadPosId = 7;
        
        private readonly IMarkRepo _markRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly ISpecificationRepo _specificationRepo;
        private readonly IConstructionRepo _constructionRepo;
        private readonly IConstructionElementRepo _constructionElementRepo;

        private class GroupedElement
        {
            public string Name { set; get; }
        }

        public SpecificationDocumentService(
            IMarkRepo markRepo,
            IEmployeeRepo employeeRepo,
            ISpecificationRepo specificationRepo,
            IConstructionRepo constructionRepo,
            IConstructionElementRepo constructionElementRepo)
        {
            _markRepo = markRepo;
            _employeeRepo = employeeRepo;
            _specificationRepo = specificationRepo;
            _constructionRepo = constructionRepo;
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

            var currentSpec = _specificationRepo.GetCurrentByMarkId(markId);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memory, true))
            {
                var markName = MarkHelper.MakeMarkName(
                    project.BaseSeries, node.Code, subnode.Code, mark.Code);
                (var complexName, var objectName) = MarkHelper.MakeComplexAndObjectName(
                    project.Name, node.Name, subnode.Name, mark.Name);

                AppendToTable(wordDoc, currentSpec.Id);   
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

        private void AppendToTable(
            WordprocessingDocument document,
            int currentSpecId)
        {
            // Вкл в спецификацию
            var constructions = _constructionRepo.GetAllBySpecificationId(
                currentSpecId).ToList();
            if (constructions.Count() > 0)
            {
                Body body = document.MainDocumentPart.Document.Body;
                var t = body.Descendants<Table>().FirstOrDefault();
                var firstTr = t.Descendants<TableRow>().ToList()[0];
                var clonedFirstTr = firstTr.CloneNode(true);
                var trCells = firstTr.Descendants<TableCell>().ToList();
                var secondTr = t.Descendants<TableRow>().ToList()[1];
                var clonedSecondTr = secondTr.CloneNode(true);
                OpenXmlElement newTr = null;

                for (int i = 0; i < constructions.Count(); i++)
                {
                    if (i > 0)
                    {
                        newTr = clonedFirstTr.CloneNode(true);
                        trCells = newTr.Descendants<TableCell>().ToList();
                    }
                    trCells[0].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement(constructions[i].Name, 24));
                    if (i > 0)
                        t.Append(newTr);

                    var groupedConstructionElements = _constructionElementRepo.GetAllByConstructionId(
                        constructions[i].Id).GroupBy(v => v.Profile.Id).Select(
                            v => new GroupedElement
                            {
                                Name = v.First().Profile.Class.Name,
                                // Form = g.Sum(v => v.Form),
                            }).ToList();

                    for (int j = 0; j < groupedConstructionElements.Count(); j++)
                    {
                        if (i == 0 && j == 0)
                            trCells = secondTr.Descendants<TableCell>().ToList();
                        else
                        {
                            newTr = clonedSecondTr.CloneNode(true);
                            trCells = newTr.Descendants<TableCell>().ToList();
                        }
                        trCells[0].GetFirstChild<Paragraph>().Append(
                            Word.GetTextElement(groupedConstructionElements[j].Name, 24));
                        Word.MakeBordersThin(trCells);
                        trCells[0].GetFirstChild<TableCellProperties>().Append(new VerticalMerge
                        {
                            Val = MergedCellValues.Restart,
                        });
                        if (i != 0 || j != 0)
                            t.Append(newTr);

                        newTr = clonedSecondTr.CloneNode(true);
                        trCells = newTr.Descendants<TableCell>().ToList();
                        Word.MakeBordersThin(trCells);
                        trCells[0].GetFirstChild<TableCellProperties>().Append(new VerticalMerge {});
                        t.Append(newTr);

                        newTr = clonedSecondTr.CloneNode(true);
                        trCells = newTr.Descendants<TableCell>().ToList();
                        trCells[0].GetFirstChild<Paragraph>().Append(
                            Word.GetTextElement("Всего профиля:", 24));
                        Word.MakeBordersThin(trCells);
                        t.Append(newTr);
                    }

                    newTr = clonedSecondTr.CloneNode(true);
                    trCells = newTr.Descendants<TableCell>().ToList();
                    trCells[0].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement("Итого масса металла:", 24));
                    Word.MakeBordersThin(trCells);
                    t.Append(newTr);

                    newTr = clonedSecondTr.CloneNode(true);
                    trCells = newTr.Descendants<TableCell>().ToList();
                    var p = trCells[0].GetFirstChild<Paragraph>();
                    p.Append(Word.GetTextElement(
                            "Итого развернутая площадь поверхности конструкций, 100 м", 24));
                    p.Append(Word.GetTextElement("2", 24, false, true));
                    p.Append(Word.GetTextElement(":", 24));
                    Word.MakeBordersThin(trCells);
                    t.Append(newTr);

                    newTr = clonedSecondTr.CloneNode(true);
                    trCells = newTr.Descendants<TableCell>().ToList();
                    trCells[0].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement("В том числе по маркам:", 24));
                    Word.MakeBordersThin(trCells, false);
                    t.Append(newTr);
                }
            }
        }
    }
}
