using System.Collections.Generic;
using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsKM.Controllers
{
    [Route("api")]
    [Authorize]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _service;
        private readonly IMapper _mapper;

        public EmployeesController(
            IEmployeeService employeeService,
            IMapper mapper)
        {
            _service = employeeService;
            _mapper = mapper;
        }

        [HttpGet, Route("departments/{departmentNumber}/mark-approval-employees")]
        public ActionResult<IEnumerable<EmployeeBaseResponse>> GetMarkApprovalEmployeesByDepartmentNumber(int departmentNumber)
        {
            var employees = _service.GetMarkApprovalEmployeesByDepartmentNumber(departmentNumber);
            return Ok(_mapper.Map<IEnumerable<EmployeeBaseResponse>>(employees));
        }

        [HttpGet, Route("departments/{departmentNumber}/mark-main-employees")]
        public ActionResult<MarkMainEmployeesResponse> GetMarkMainEmployeesByDepartmentNumber(int departmentNumber)
        {
            (var chiefSpecialists, var groupLeaders, var mainBuilders) = _service
                .GetMarkMainEmployeesByDepartmentNumber(departmentNumber);
            return Ok(new MarkMainEmployeesResponse(
                _mapper.Map<IEnumerable<EmployeeBaseResponse>>(chiefSpecialists),
                _mapper.Map<IEnumerable<EmployeeBaseResponse>>(groupLeaders),
                _mapper.Map<IEnumerable<EmployeeBaseResponse>>(mainBuilders)));
        }
    }
}
