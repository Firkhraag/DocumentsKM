using System.Collections.Generic;
using AutoMapper;
using DocumentsKM.Data;
using DocumentsKM.Dtos;
using DocumentsKM.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DocumentsKM.Controllers
{
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ILogger<ProjectsController> _logger;
        private readonly IProjectRepo _repository;
        private readonly IMapper _mapper;

        public ProjectsController(
            ILogger<ProjectsController> logger,
            IProjectRepo repo,
            IMapper mapper
        )
        {
            _logger = logger;
            _repository = repo;
            _mapper = mapper;
        }

        [Route("api/projects")]
        [HttpGet]
        public ActionResult<IEnumerable<ProjectSeriesReadDto>> GetAllProjects()
        {
            var projects = _repository.GetAllProjects();
            if (projects != null) {
                return Ok(_mapper.Map<IEnumerable<ProjectSeriesReadDto>>(projects));
            }
            return NotFound();
        }
    }
}
