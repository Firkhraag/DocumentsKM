using System.Collections.Generic;
using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsKM.Controllers
{
    // AMQP
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

        [HttpGet, Route("departments/{departmentId}/employees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EmployeeBaseResponse>> GetAllByDepartmentId(int departmentId)
        {
            var employees = _service.GetAllByDepartmentId(departmentId);
            return Ok(_mapper.Map<IEnumerable<EmployeeBaseResponse>>(employees));
        }

        [HttpGet, Route("departments/{departmentId}/mark-approval-employees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EmployeeBaseResponse>> GetMarkApprovalEmployeesByDepartmentId(
            int departmentId)
        {
            var employees = _service.GetMarkApprovalEmployeesByDepartmentId(departmentId);
            return Ok(_mapper.Map<IEnumerable<EmployeeBaseResponse>>(employees));
        }

        [HttpGet, Route("departments/{departmentId}/mark-main-employees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<MarkMainEmployeesResponse> GetMarkMainEmployeesByDepartmentId(
            int departmentId)
        {
            (var departmentHead, var chiefSpecialists, var groupLeaders, var normContrs) = _service
                .GetMarkMainEmployeesByDepartmentId(departmentId);
            return Ok(new MarkMainEmployeesResponse(
                _mapper.Map<EmployeeBaseResponse>(departmentHead),
                _mapper.Map<IEnumerable<EmployeeBaseResponse>>(chiefSpecialists),
                _mapper.Map<IEnumerable<EmployeeBaseResponse>>(groupLeaders),
                _mapper.Map<IEnumerable<EmployeeBaseResponse>>(normContrs)));
        }
    }
}
