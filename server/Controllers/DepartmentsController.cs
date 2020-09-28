using System.Collections.Generic;
using AutoMapper;
using DocumentsKM.Data;
using DocumentsKM.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsKM.Controllers
{
    [Route("api")]
    [Authorize]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentRepo _repository;
        private readonly IMapper _mapper;

        public DepartmentsController(
            IDepartmentRepo repo,
            IMapper mapper)
        {
            _repository = repo;
            _mapper = mapper;
        }

        [HttpGet, Route("departments")]
        public ActionResult<IEnumerable<DepartmentCodeResponse>> GetAllActive()
        {
            var departments = _repository.GetAllActive();
            return Ok(_mapper.Map<IEnumerable<DepartmentCodeResponse>>(departments));
        }
    }
}
