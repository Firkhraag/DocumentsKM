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

        [HttpGet, Route("departments/{departmentId}/approval-employees")]
        public ActionResult<IEnumerable<EmployeeNameResponse>> GetAllApprovalByDepartmentId(int departmentNumber)
        {
            var employees = _service.GetAllApprovalByDepartmentNumber(departmentNumber);
            return Ok(_mapper.Map<IEnumerable<EmployeeNameResponse>>(employees));
        }
    }
}
