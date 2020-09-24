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
    public class DepartmentsController : ControllerBase
    {
        private readonly ILogger<DepartmentsController> _logger;
        private readonly IDepartmentRepo _repository;
        private readonly IMapper _mapper;

        public DepartmentsController(
            ILogger<DepartmentsController> logger,
            IDepartmentRepo repo,
            IMapper mapper
        )
        {
            _logger = logger;
            _repository = repo;
            _mapper = mapper;
        }

        [HttpGet, Route("departments")]
        public ActionResult<IEnumerable<DepartmentCodeReadDto>> GetActiveDepartments()
        {
            var departments = _repository.GetAllActiveDepartments();
            // TBD: Should catch Internal server error!
            // Ok even if array is empty
            return Ok(_mapper.Map<IEnumerable<DepartmentCodeReadDto>>(departments));
        }
    }
}
