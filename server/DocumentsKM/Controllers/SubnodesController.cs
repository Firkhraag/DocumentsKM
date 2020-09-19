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
    public class SubnodesController : ControllerBase
    {
        private readonly ILogger<SubnodesController> _logger;
        private readonly ISubnodeRepo _repository;
        private readonly IMapper _mapper;

        public SubnodesController(
            ILogger<SubnodesController> logger,
            ISubnodeRepo repo,
            IMapper mapper
        )
        {
            _logger = logger;
            _repository = repo;
            _mapper = mapper;
        }

        [Route("api/nodes/{nodeId}/subnodes")]
        [HttpGet]
        public ActionResult<IEnumerable<SubnodeCodeReadDto>> GetAllNodeSubnodes(ulong nodeId)
        {
            var subnodes = _repository.GetAllNodeSubnodes(nodeId);
            if (subnodes != null) {
                return Ok(_mapper.Map<IEnumerable<SubnodeCodeReadDto>>(subnodes));
            }
            return NotFound();
        }
    }
}
