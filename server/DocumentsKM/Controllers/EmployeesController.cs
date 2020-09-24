using System.Collections.Generic;
using AutoMapper;
using DocumentsKM.Data;
using DocumentsKM.Dtos;
using DocumentsKM.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DocumentsKM.Controllers
{
    [Route("api")]
    [Authorize]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger;
        private readonly IEmployeeRepo _repository;
        private readonly IMapper _mapper;

        public EmployeesController(
            ILogger<EmployeesController> logger,
            IEmployeeRepo repo,
            IMapper mapper
        )
        {
            _logger = logger;
            _repository = repo;
            _mapper = mapper;
        }

        [HttpGet, Route("departments/{departmentId}/employee/approval")]
        public ActionResult<IEnumerable<EmployeeNameReadDto>> GetAllApprovalSpecialists(ulong departmentId)
        {
            uint minPosCode = 1170;
            uint maxPosCode = 1251;
            var specialists = _repository.GetAllApprovalSpecialists(departmentId, minPosCode, maxPosCode);
            // TBD: Should catch Internal server error!
            // Ok even if array is empty
            return Ok(_mapper.Map<IEnumerable<EmployeeNameReadDto>>(specialists));
        }
    }
}
