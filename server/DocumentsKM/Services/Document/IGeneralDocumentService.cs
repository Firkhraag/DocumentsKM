using System.IO;

namespace DocumentsKM.Services
{
    public interface IGeneralDocumentService
    {
        // Заполнить документ
        void PopulateDocument(int markId, MemoryStream memory);
    }

    public interface IGeneralDataDocumentService : IGeneralDocumentService {}
    public interface IConstructionDocumentService : IGeneralDocumentService {}
    public interface IBoltDocumentService : IGeneralDocumentService {}
    public interface IEstimateTaskDocumentService : IGeneralDocumentService {}
    public interface ISpecificationDocumentService : IGeneralDocumentService {}
}
