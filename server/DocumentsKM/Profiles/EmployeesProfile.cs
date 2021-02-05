using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Profiles
{
    public class EmployeesProfile : AutoMapper.Profile
    {
        public EmployeesProfile()
        {
            CreateMap<Employee, EmployeeBaseResponse>();
            CreateMap<Employee, EmployeeDepartmentResponse>();
        }
    }
}
