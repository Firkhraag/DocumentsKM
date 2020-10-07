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
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _service;
        private readonly IMapper _mapper;

        public DepartmentsController(
            IDepartmentService service,
            IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet, Route("departments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<DepartmentResponse>> GetAllActive()
        {
            var departments = _service.GetAllActive();
            return Ok(_mapper.Map<IEnumerable<DepartmentResponse>>(departments));
        }

        [HttpGet, Route("approval-departments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<DepartmentBaseResponse>> GetAllApprovalDepartments()
        {
            var departments = _service.GetAllActive();
            return Ok(_mapper.Map<IEnumerable<DepartmentBaseResponse>>(departments));
        }
    }
}
