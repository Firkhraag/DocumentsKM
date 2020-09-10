using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IBrandRepo
    {
        IEnumerable<Brand> GetAllBrands();
        Brand GetBrandById(int id);
    }
}