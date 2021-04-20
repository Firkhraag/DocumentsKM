using System.IO;

namespace DocumentsKM.Services
{
    public interface IGeneralDocumentService
    {
        // Заполнить документ
        void PopulateDocument(int markId, MemoryStream memory);
    }

    public interface IGeneralDataDocumentService : IGeneralDocumentService {}
    public interface ISpecificationDocumentService : IGeneralDocumentService {}
    public interface IConstructionDocumentService : IGeneralDocumentService {}
    public interface IBoltDocumentService : IGeneralDocumentService {}
    public interface IEstimateTaskDocumentService : IGeneralDocumentService {}
    public interface IProjectRegistrationDocumentService : IGeneralDocumentService {}
    public interface IEstimationTitleDocumentService : IGeneralDocumentService {}
    public interface IEstimationPagesDocumentService
    {
        // Заполнить документ
        void PopulateDocument(int markId, int numOfPages, MemoryStream memory);
    }
}
