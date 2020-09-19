using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Model;

namespace DocumentsKM.Profiles
{
    public class DepartmentsProfile : Profile
    {
        public DepartmentsProfile()
        {
            // Souce -> Target
            CreateMap<Department, DepartmentCodeReadDto>();
        }
    }
}
