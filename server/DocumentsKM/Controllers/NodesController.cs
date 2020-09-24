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

        [HttpGet, Route("projects/{projectId}/nodes")]
        public ActionResult<IEnumerable<NodeCodeReadDto>> GetAllProjectNodes(ulong projectId)
        {
            var nodes = _repository.GetAllProjectNodes(projectId);
            // TBD: Should catch Internal server error!
            // Ok even if array is empty
            return Ok(_mapper.Map<IEnumerable<NodeCodeReadDto>>(nodes));
        }
    }
}
