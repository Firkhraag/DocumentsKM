namespace DocumentsKM.Services
{
    public interface IConstructionCopyService
    {
        // Скопировать вид конструкций в вид спецификации
        void Copy(
            int constructionId,
            int specificationId);
    }
}
