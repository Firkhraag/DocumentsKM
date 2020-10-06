using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class DepartmentsProfile : Profile
    {
        public DepartmentsProfile()
        {
            // Souce -> Target
            CreateMap<Department, DepartmentBaseResponse>();
            CreateMap<Department, DepartmentResponse>();
        }
    }
}
