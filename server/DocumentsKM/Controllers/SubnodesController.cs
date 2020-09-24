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

        [HttpGet, Route("nodes/{nodeId}/subnodes")]
        public ActionResult<IEnumerable<SubnodeCodeReadDto>> GetAllNodeSubnodes(ulong nodeId)
        {
            var subnodes = _repository.GetAllNodeSubnodes(nodeId);
            // TBD: Should catch Internal server error!
            // Ok even if array is empty
            return Ok(_mapper.Map<IEnumerable<SubnodeCodeReadDto>>(subnodes));
        }
    }
}
