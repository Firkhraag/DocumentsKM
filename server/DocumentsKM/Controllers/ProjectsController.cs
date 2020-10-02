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
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _service;
        private readonly IMapper _mapper;

        public ProjectsController(
            IProjectService projectService,
            IMapper mapper
        )
        {
            _service = projectService;
            _mapper = mapper;
        }

        [HttpGet, Route("projects")]
        public ActionResult<IEnumerable<ProjectSeriesReadDto>> GetAll()
        {
            var projects = _service.GetAll();
            return Ok(_mapper.Map<IEnumerable<ProjectSeriesReadDto>>(projects));
        }
    }
}
