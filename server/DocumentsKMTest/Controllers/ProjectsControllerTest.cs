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
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _service;
        private readonly IMapper _mapper;

        public ProjectsController(
            IProjectService projectService,
            IMapper mapper)
        {
            _service = projectService;
            _mapper = mapper;
        }

        [HttpGet, Route("projects")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ProjectBaseResponse>> GetAll()
        {
            var projects = _service.GetAll();
            return Ok(_mapper.Map<IEnumerable<ProjectBaseResponse>>(projects));
        }
    }
}
