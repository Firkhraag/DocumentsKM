using DocumentsKM.Data;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using System;
using DocumentsKM.Helpers;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using DocumentsKM.Models;

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
        private readonly IConstructionRepo _constructionRepo;
        private readonly IStandardConstructionRepo _standardConstructionRepo;
        private readonly IConstructionElementRepo _constructionElementRepo;

        public ConstructionDocumentService(
            IMarkRepo markRepo,
            IMarkApprovalRepo markApprovalRepo,
            IEmployeeRepo employeeRepo,
            IDocRepo docRepo,
            IConstructionRepo constructionRepo,
            IStandardConstructionRepo standardConstructionRepo,
            IConstructionElementRepo constructionElementRepo)
        {
            _markRepo = markRepo;
            _markApprovalRepo = markApprovalRepo;
            _employeeRepo = employeeRepo;
            _docRepo = docRepo;
            _constructionRepo = constructionRepo;
            _standardConstructionRepo = standardConstructionRepo;
            _constructionElementRepo = constructionElementRepo;
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
                    sheets.Count(),
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
            if (constructions.Count() > 0)
            {
                var sums = new List<double> {};
                for (int i = 0; i < 12; i++)
                {
                    sums.Add(0.0);
                }

                var constructionElements = _constructionElementRepo.GetAllByConstructionId(
                    constructions[0].Id);
                    
                // var weight = constructionElements.Where(
                //     v => v.Steel.Strength > 1).Sum(v => v.Profile.Weight * v.Length * 0.001);

                Body body = document.MainDocumentPart.Document.Body;
                var t = body.Descendants<Table>().FirstOrDefault();

                // 1
                var firstTr = t.Descendants<TableRow>().ToList()[0];
                var clonedFirstTr = firstTr.CloneNode(true);

                var trCells = firstTr.Descendants<TableCell>().ToList();
                var p = trCells[0].GetFirstChild<Paragraph>();
                // p.ParagraphProperties.Append(new Justification
                //     {
                //         Val = JustificationValues.Left,
                //     });
                p.Append(Word.GetTextElement(constructions[0].Name, 24));

                // 2
                var secondTr = t.Descendants<TableRow>().ToList()[1];
                var clonedSecondTr = secondTr.CloneNode(true);

                trCells = secondTr.Descendants<TableCell>().ToList();

                var weight = Math.Ceiling(constructionElements.Where(
                    v => v.Steel.Strength > 1).Sum(v => v.Profile.Weight * v.Length * 0.001) * 1000) / 1000;
                sums[0] += weight;
                if (weight > 0)
                {
                    p = trCells[0].GetFirstChild<Paragraph>();
                    p.Append(Word.GetTextElement(weight.ToString(), 24));
                }

                for (int i = 1; i < 11; i++)
                {
                    var typeId = i;
                    if (i > 6)
                        typeId += 1;
                    weight = Math.Ceiling(constructionElements.Where(
                        v => v.Profile.Type.Id == typeId).Sum(v => v.Profile.Weight * v.Length * 0.001) * 1000) / 1000;
                    sums[i] += weight;
                    if (weight > 0)
                    {
                        p = trCells[i].GetFirstChild<Paragraph>();
                        p.Append(Word.GetTextElement(weight.ToString(), 24));
                    }
                }

                p = trCells[11].GetFirstChild<Paragraph>();
                p.Append(Word.GetTextElement(Math.Round(sums.Skip(1).Sum() * 1.01 * 1.03, 3).ToString(), 24));

                // 3
                var thirdTr = t.Descendants<TableRow>().ToList()[2];
                var clonedThirdTr = thirdTr.CloneNode(true);
                t.RemoveChild(thirdTr);

                for (int i = 1; i < constructions.Count(); i++)
                {
                    constructionElements = _constructionElementRepo.GetAllByConstructionId(
                        constructions[i].Id);
                        
                    // 1
                    var newTr = clonedFirstTr.CloneNode(true);
                    trCells = newTr.Descendants<TableCell>().ToList();

                    p = trCells[0].GetFirstChild<Paragraph>();
                    // p.ParagraphProperties.Append(new Justification
                    // {
                    //     Val = JustificationValues.Left,
                    // });
                    p.Append(Word.GetTextElement(constructions[i].Name, 24));
                    t.Append(newTr);

                    // 2
                    newTr = clonedSecondTr.CloneNode(true);
                    trCells = newTr.Descendants<TableCell>().ToList();

                    weight = Math.Ceiling(constructionElements.Where(
                        v => v.Steel.Strength > 1).Sum(v => v.Profile.Weight * v.Length * 0.001) * 1000) / 1000;
                    sums[0] += weight;
                    if (weight > 0)
                    {
                        p = trCells[0].GetFirstChild<Paragraph>();
                        p.Append(Word.GetTextElement(weight.ToString(), 24));
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
                                    v => v.Profile.Weight * v.Length * 0.001) * 1000) / 1000;
                        else
                            weight = Math.Ceiling(constructionElements.Where(
                                v => v.Profile.Type.Id == typeId).Sum(
                                    v => v.Profile.Weight * v.Length * 0.001) * 1000) / 1000;
                        sums[k] += weight;
                        localSum += weight;
                        if (weight > 0)
                        {
                            p = trCells[k].GetFirstChild<Paragraph>();
                            p.Append(Word.GetTextElement(weight.ToString(), 24));
                        }
                    }

                    p = trCells[11].GetFirstChild<Paragraph>();
                    p.Append(Word.GetTextElement(Math.Round(localSum * 1.01 * 1.03, 3).ToString(), 24));

                    t.Append(newTr);
                }

                for (int i = 0; i < standardConstructions.Count(); i++)
                {
                    // 1
                    var newTr = clonedThirdTr.CloneNode(true);
                    trCells = newTr.Descendants<TableCell>().ToList();

                    p = trCells[0].GetFirstChild<Paragraph>();
                    // p.ParagraphProperties.Append(new Justification
                    // {
                    //     Val = JustificationValues.Left,
                    // });
                    p.Append(Word.GetTextElement(standardConstructions[i].Name, 24));

                    p = trCells[1].GetFirstChild<Paragraph>();
                    p.Append(Word.GetTextElement((Math.Ceiling(standardConstructions[i].Weight * 1000) / 1000).ToString(), 24));

                    // var tcProp = trCells[0].GetFirstChild<TableCellProperties>();
                    // var gridSpan = tcProp.GetFirstChild<GridSpan>();
                    // gridSpan.Val = 11;

                    // var tcb = tcProp.AppendChild(new TableCellBorders() {});
                    // // var tcb = tcProp.GetFirstChild<TableCellBorders>();
                    // tcb.AppendChild(new BottomBorder
                    // {
                    //     Size = 12,
                    // });
                    // tcb.AppendChild(new RightBorder
                    // {
                    //     Size = 12,
                    // });
                    // // var bb = tcb.GetFirstChild<BottomBorder>();
                    // // var rb = tcb.GetFirstChild<RightBorder>();
                    // // bb.Size = 4;
                    // // rb.Size = 4;
                    t.Append(newTr);
                }

                // 1
                var lastTr = clonedFirstTr.CloneNode(true);
                trCells = lastTr.Descendants<TableCell>().ToList();

                p = trCells[0].GetFirstChild<Paragraph>();
                // p.ParagraphProperties.Append(new Justification
                // {
                //     Val = JustificationValues.Left,
                // });
                p.Append(Word.GetTextElement("Итого массы", 24));
                t.Append(lastTr);

                // 2
                lastTr = clonedSecondTr.CloneNode(true);
                trCells = lastTr.Descendants<TableCell>().ToList();

                for (int k = 0; k < 11; k++)
                {
                    if (sums[k] > 0)
                    {
                        p = trCells[k].GetFirstChild<Paragraph>();
                        p.Append(Word.GetTextElement(Math.Round(sums[k], 3).ToString(), 24));
                    }
                }

                p = trCells[11].GetFirstChild<Paragraph>();
                p.Append(Word.GetTextElement(Math.Round(sums.Skip(1).Sum() * 1.01 * 1.03, 3).ToString(), 24));
                t.Append(lastTr);
            }
        }
    }
}
