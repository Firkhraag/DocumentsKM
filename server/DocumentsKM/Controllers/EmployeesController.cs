using System.Collections.Generic;
using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsKM.Controllers
{
    [Route("api")]
    [Authorize]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EmployeeBaseResponse>> GetMarkApprovalEmployeesByDepartmentNumber(int departmentNumber)
        {
            var employees = _service.GetMarkApprovalEmployeesByDepartmentNumber(departmentNumber);
            return Ok(_mapper.Map<IEnumerable<EmployeeBaseResponse>>(employees));
        }

        [HttpGet, Route("departments/{departmentNumber}/mark-main-employees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<MarkMainEmployeesResponse> GetMarkMainEmployeesByDepartmentNumber(int departmentNumber)
        {
            (var departmentHead, var chiefSpecialists, var groupLeaders, var mainBuilders) = _service
                .GetMarkMainEmployeesByDepartmentNumber(departmentNumber);
            return Ok(new MarkMainEmployeesResponse(
                _mapper.Map<EmployeeBaseResponse>(departmentHead),
                _mapper.Map<IEnumerable<EmployeeBaseResponse>>(chiefSpecialists),
                _mapper.Map<IEnumerable<EmployeeBaseResponse>>(groupLeaders),
                _mapper.Map<IEnumerable<EmployeeBaseResponse>>(mainBuilders)));
        }
    }
}
