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
    public class NodesController : ControllerBase
    {
        private readonly ILogger<NodesController> _logger;
        private readonly INodeRepo _repository;
        private readonly IMapper _mapper;

        public NodesController(
            ILogger<NodesController> logger,
            INodeRepo repo,
            IMapper mapper
        )
        {
            _logger = logger;
            _repository = repo;
            _mapper = mapper;
        }

        [Route("api/projects/{projectId}/nodes")]
        [HttpGet]
        public ActionResult<IEnumerable<NodeCodeReadDto>> GetAllProjectNodes(ulong projectId)
        {
            var nodes = _repository.GetAllProjectNodes(projectId);
            if (nodes != null) {
                return Ok(_mapper.Map<IEnumerable<NodeCodeReadDto>>(nodes));
            }
            return NotFound();
        }
    }
}
