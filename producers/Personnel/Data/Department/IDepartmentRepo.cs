using Personnel.Models;

namespace Personnel.Data
{
    public interface IDepartmentRepo
    {
        Department GetById(int id);
    }
}
