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
            public List<GroupedSteel> Steel { set; get; }
        }
        private class GroupedSteel
        {
            public string Name { set; get; }
            public string Standard { set; get; }
            public List<GroupedProfile> Profiles { set; get; }
        }
        private class GroupedProfile
        {
            public string Name { set; get; }
            public string Symbol { set; get; }
            public double Length { set; get; }
            public double Weight { set; get; }
        }

        // private class GroupedElement
        // {
        //     public string Name { set; get; }
        //     public List<Profile> Profiles { set; get; }
        //     public List<Steel> Steel { set; get; }
        // }
        // private class GroupedSteel
        // {
        //     public string Name { set; get; }
        //     public List<Profile> Profiles { set; get; }
        // }
        // private class GroupedProfile
        // {
        //     public string Name { set; get; }
        //     public float Length { set; get; }
        //     // public List<Steel> Steel { set; get; }
        // }

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

                var num = 1;
                for (int i = 0; i < constructions.Count(); i++)
                {
                    var steelMap = new Dictionary<string, double> {};
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
                        constructions[i].Id).GroupBy(v => v.Profile.Type.Id).Select(
                            v => new GroupedElement
                            {
                                Name = v.First().Profile.Class.Name,
                                Steel = v.GroupBy(v2 => v2.Steel.Id).Select(
                                    v2 => new GroupedSteel
                                    {
                                        Name = v2.First().Steel.Name,
                                        Standard = v2.First().Steel.Standard,
                                        Profiles = v2.GroupBy(v3 => v3.Profile.Id).Select(
                                            v3 => new GroupedProfile
                                            {
                                                Name = v3.First().Profile.Name,
                                                Symbol = ((v3.First().Profile.Class.Id == 16) ||
                                                    (v3.First().Profile.Class.Id == 17)) ? "s=" : v3.First().Profile.Symbol,
                                                Length = Math.Round(v3.Sum(v4 => v4.Length), 3),
                                                Weight = Math.Ceiling(v3.Sum(v4 => v4.Profile.Weight * v4.Length)) / 1000,
                                            }).ToList(),
                                    }).ToList(),
                            }).ToList();

                    var elementSum = 0.0;
                    for (int j = 0; j < groupedConstructionElements.Count(); j++)
                    {
                        var steelArr = groupedConstructionElements[j].Steel;
                        var steelSum = 0.0;
                        for (int k = 0; k < steelArr.Count(); k++)
                        {
                            var profileArr = steelArr[k].Profiles;
                            var profileSum = 0.0;
                            for (int z = 0; z < profileArr.Count(); z++)
                            {
                                if (i == 0 && j == 0 && k == 0 && z == 0)
                                    trCells = secondTr.Descendants<TableCell>().ToList();
                                else
                                {
                                    newTr = clonedSecondTr.CloneNode(true);
                                    trCells = newTr.Descendants<TableCell>().ToList();
                                }
                                if (k == 0 && z == 0)
                                {
                                    trCells[0].GetFirstChild<Paragraph>().Append(
                                        Word.GetTextElement(groupedConstructionElements[j].Name, 24));
                                    trCells[0].GetFirstChild<TableCellProperties>().Append(new VerticalMerge
                                    {
                                        Val = MergedCellValues.Restart,
                                    });
                                }
                                else
                                    trCells[0].GetFirstChild<TableCellProperties>().Append(new VerticalMerge {});

                                if (z == 0)
                                {
                                    trCells[1].GetFirstChild<Paragraph>().Append(
                                        Word.GetTextElement(steelArr[k].Name, 24));
                                    var run = Word.GetTextElement(" по", 24);
                                    run.AppendChild(new Break());
                                    trCells[1].GetFirstChild<Paragraph>().Append(run);
                                    var split = steelArr[k].Standard.Split("по ");
                                    trCells[1].GetFirstChild<Paragraph>().Append(
                                        Word.GetTextElement(split[1], 24));

                                    if (profileArr.Count > 1)
                                        trCells[1].GetFirstChild<TableCellProperties>().Append(new VerticalMerge
                                        {
                                            Val = MergedCellValues.Restart,
                                        });
                                }
                                else
                                    trCells[1].GetFirstChild<TableCellProperties>().Append(new VerticalMerge {});

                                trCells[2].GetFirstChild<Paragraph>().Append(
                                    Word.GetTextElement(
                                        profileArr[z].Symbol, 24, false, false, false, profileArr[z].Symbol == "s="));
                                trCells[2].GetFirstChild<Paragraph>().Append(
                                    Word.GetTextElement(profileArr[z].Name, 24));
                                trCells[3].GetFirstChild<Paragraph>().Append(
                                    Word.GetTextElement(num.ToString(), 24));
                                trCells[4].GetFirstChild<Paragraph>().Append(
                                    Word.GetTextElement(profileArr[z].Length.ToStringWithComma(), 24));
                                trCells[5].GetFirstChild<Paragraph>().Append(
                                    Word.GetTextElement(profileArr[z].Weight.ToStringWithComma(), 24));
                                num++;
                                Word.MakeBordersThin(trCells);
                                profileSum += profileArr[z].Weight;
                                if (i != 0 || j != 0 || k != 0 || z != 0)
                                    t.Append(newTr);
                            }

                            if (steelMap.ContainsKey(steelArr[k].Name))
                                steelMap[steelArr[k].Name] += profileSum;
                            else
                                steelMap.Add(steelArr[k].Name, profileSum);

                            newTr = clonedSecondTr.CloneNode(true);
                            trCells = newTr.Descendants<TableCell>().ToList();
                            Word.MakeBordersThin(trCells);
                            trCells[0].GetFirstChild<TableCellProperties>().Append(new VerticalMerge {});
                            trCells[1].GetFirstChild<Paragraph>().Append(
                                Word.GetTextElement("Итого:", 24));
                            trCells[3].GetFirstChild<Paragraph>().Append(
                                Word.GetTextElement(num.ToString(), 24));
                            trCells[5].GetFirstChild<Paragraph>().Append(
                                Word.GetTextElement(Math.Round(profileSum, 3).ToStringWithComma(), 24));
                            num++;
                            t.Append(newTr);

                            steelSum += profileSum;
                        }

                        newTr = clonedSecondTr.CloneNode(true);
                        trCells = newTr.Descendants<TableCell>().ToList();
                        trCells[0].GetFirstChild<Paragraph>().Append(
                            Word.GetTextElement("Всего профиля:", 24));
                        trCells[3].GetFirstChild<Paragraph>().Append(
                            Word.GetTextElement(num.ToString(), 24));
                        trCells[5].GetFirstChild<Paragraph>().Append(
                            Word.GetTextElement(Math.Round(steelSum, 3).ToStringWithComma(), 24));
                        num++;
                        Word.MakeBordersThin(trCells);
                        t.Append(newTr);

                        elementSum += steelSum;
                    }

                    newTr = clonedSecondTr.CloneNode(true);
                    trCells = newTr.Descendants<TableCell>().ToList();
                    trCells[0].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement("Итого масса металла:", 24));
                    trCells[3].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement(num.ToString(), 24));
                    trCells[5].GetFirstChild<Paragraph>().Append(
                            Word.GetTextElement(Math.Round(elementSum, 3).ToStringWithComma(), 24));
                    num++;
                    Word.MakeBordersThin(trCells);
                    t.Append(newTr);

                    newTr = clonedSecondTr.CloneNode(true);
                    trCells = newTr.Descendants<TableCell>().ToList();
                    var p = trCells[0].GetFirstChild<Paragraph>();
                    p.Append(Word.GetTextElement(
                            "Итого развернутая площадь поверхности конструкций, 100 м", 24));
                    p.Append(Word.GetTextElement("2", 24, false, true));
                    p.Append(Word.GetTextElement(":", 24));
                    trCells[3].GetFirstChild<Paragraph>().Append(
                        Word.GetTextElement(num.ToString(), 24));
                    trCells[5].GetFirstChild<Paragraph>().Append(
                            Word.GetTextElement("1111", 24));
                    num++;
                    Word.MakeBordersThin(trCells);
                    t.Append(newTr);

                    var n = 0;
                    foreach (var steel in steelMap)
                    {
                        newTr = clonedSecondTr.CloneNode(true);
                        trCells = newTr.Descendants<TableCell>().ToList();
                        if (n == 0)
                            trCells[0].GetFirstChild<Paragraph>().Append(
                                Word.GetTextElement("В том числе по маркам:", 24));
                        trCells[1].GetFirstChild<Paragraph>().Append(
                            Word.GetTextElement(steel.Key, 24));
                        trCells[3].GetFirstChild<Paragraph>().Append(
                            Word.GetTextElement(num.ToString(), 24));
                        trCells[5].GetFirstChild<Paragraph>().Append(
                                Word.GetTextElement(steel.Value.ToStringWithComma(), 24));
                        num++;
                        if (n == steelMap.Count() - 1)
                            Word.MakeBordersThin(trCells, false);
                        else
                            Word.MakeBordersThin(trCells);
                        t.Append(newTr);
                        n++;
                    }
                }
            }
        }
    }
}
