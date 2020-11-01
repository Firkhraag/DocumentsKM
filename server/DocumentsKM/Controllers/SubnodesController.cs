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
        public ActionResult<IEnumerable<SubnodeBaseResponse>> GetAllByNodeId(int nodeId)
        {
            var subnodes = _service.GetAllByNodeId(nodeId);
            return Ok(_mapper.Map<IEnumerable<SubnodeBaseResponse>>(subnodes));
        }

        [HttpGet, Route("subnodes/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Subnode> GetById(int id)
        {
            var subnode = _service.GetById(id);
            if (subnode != null)
                return Ok(subnode);
            return NotFound();
        }

        [HttpGet, Route("subnodes/{id}/parents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<SubnodeParentResponse> GetSubnodeParentResponseById(int id)
        {
            var subnode = _service.GetById(id);
            if (subnode != null)
                return Ok(_mapper.Map<SubnodeParentResponse>(subnode));
            return NotFound();
        }
    }
}
