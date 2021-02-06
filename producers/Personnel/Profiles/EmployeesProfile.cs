using AutoMapper;
using Personnel.Dtos;
using Personnel.Models;

namespace Personnel.Profiles
{
    public class EmployeesProfile : Profile
    {
        public EmployeesProfile()
        {
            CreateMap<EmployeeRequest, Employee>();
        }
    }
}
