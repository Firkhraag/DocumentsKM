using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class DepartmentsProfile : AutoMapper.Profile
    {
        public DepartmentsProfile()
        {
            CreateMap<Department, DepartmentResponse>();
        }
    }
}
