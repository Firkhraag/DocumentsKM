using AutoMapper;
using Personnel.Dtos;
using Personnel.Models;

namespace Personnel.Profiles
{
    public class DepartmentsProfile : Profile
    {
        public DepartmentsProfile()
        {
            CreateMap<DepartmentRequest, Department>();
        }
    }
}
