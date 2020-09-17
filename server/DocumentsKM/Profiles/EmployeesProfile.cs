using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Model;

namespace DocumentsKM.Profiles
{
    public class EmployeesProfile : Profile
    {
        public EmployeesProfile()
        {
            // Souce -> Target
            CreateMap<Employee, EmployeeNameReadDto>();
        }
    }
}
