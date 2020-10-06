using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class EmployeesProfile : Profile
    {
        public EmployeesProfile()
        {
            // Souce -> Target
            CreateMap<Employee, EmployeeBaseResponse>();
        }
    }
}
