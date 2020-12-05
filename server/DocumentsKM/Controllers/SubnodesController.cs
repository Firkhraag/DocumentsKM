using System.Collections.Generic;
using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;
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
    public class SubnodesController : ControllerBase
    {
        private readonly ISubnodeService _service;
        private readonly IMapper _mapper;

        public SubnodesController(
            ISubnodeService subnodeService,
            IMapper mapper)
        {
            _service = subnodeService;
            _mapper = mapper;
        }

        [HttpGet, Route("nodes/{nodeId}/subnodes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<SubnodeResponse>> GetAllByNodeId(int nodeId)
        {
            var subnodes = _service.GetAllByNodeId(nodeId);
            return Ok(_mapper.Map<IEnumerable<SubnodeResponse>>(subnodes));
        }
    }
}
