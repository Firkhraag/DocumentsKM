using System.IO;
using DocumentFormat.OpenXml.Packaging;
using System;

namespace DocumentsKM.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IGeneralDataDocumentService _generalDataDocumentService;
        private readonly IConstructionDocumentService _constructionDocumentService;
        private readonly IBoltDocumentService _boltDocumentService;
        private readonly IEstimateTaskDocumentService _estimateTaskDocumentService;
        private readonly ISpecificationDocumentService _specificationDocumentService;

        public DocumentService(
            IGeneralDataDocumentService generalDataDocumentService,
            IConstructionDocumentService constructionDocumentService,
            IBoltDocumentService boltDocumentService,
            IEstimateTaskDocumentService estimateTaskDocumentService,
            ISpecificationDocumentService specificationDocumentService)
        {
            _generalDataDocumentService = generalDataDocumentService;
            _constructionDocumentService = constructionDocumentService;
            _boltDocumentService = boltDocumentService;
            _estimateTaskDocumentService = estimateTaskDocumentService;
            _specificationDocumentService = specificationDocumentService;
        }

        public MemoryStream GetGeneralDataDocument(int markId)
        {
            var memory = GetStreamFromTemplate("D:\\Dev\\Gipromez\\word\\template_general_data.docx");
            _generalDataDocumentService.PopulateDocument(markId, memory);
            memory.Seek(0, SeekOrigin.Begin);
            return memory;
        }

        public MemoryStream GetConstructionDocument(int markId)
        {
            var memory = GetStreamFromTemplate("D:\\Dev\\Gipromez\\word\\template_construction.docx");
            _constructionDocumentService.PopulateDocument(markId, memory);
            memory.Seek(0, SeekOrigin.Begin);
            return memory;
        }

        public MemoryStream GetBoltDocument(int markId)
        {
            var memory = GetStreamFromTemplate("D:\\Dev\\Gipromez\\word\\template_bolt.docx");
            _boltDocumentService.PopulateDocument(markId, memory);
            memory.Seek(0, SeekOrigin.Begin);
            return memory;
        }

        public MemoryStream GetEstimateTaskDocument(int markId)
        {
            var memory = GetStreamFromTemplate("D:\\Dev\\Gipromez\\word\\template_estimate_task.docx");
            _estimateTaskDocumentService.PopulateDocument(markId, memory);
            memory.Seek(0, SeekOrigin.Begin);
            return memory;
        }

        public MemoryStream GetSpecificationDocument(int markId)
        {
            var memory = GetStreamFromTemplate("D:\\Dev\\Gipromez\\word\\template_specification.docx");
            _specificationDocumentService.PopulateDocument(markId, memory);
            memory.Seek(0, SeekOrigin.Begin);
            return memory;
        }

        private MemoryStream GetStreamFromTemplate(string inputPath)
        {
            MemoryStream documentStream;
            using (Stream stream = File.OpenRead(inputPath))
            {
                documentStream = new MemoryStream((int)stream.Length);
                stream.CopyTo(documentStream);
                documentStream.Position = 0L;
            }
            using (WordprocessingDocument template = WordprocessingDocument.Open(documentStream, true))
            {
                template.ChangeDocumentType(DocumentFormat.OpenXml.WordprocessingDocumentType.Document);
                MainDocumentPart mainPart = template.MainDocumentPart;
                mainPart.DocumentSettingsPart.AddExternalRelationship(
                    "http://schemas.openxmlformats.org/officeDocument/2006/relationships/attachedTemplate",
                new Uri(inputPath, UriKind.Absolute));

                mainPart.Document.Save();
            }
            return documentStream;
        }
    }
}
