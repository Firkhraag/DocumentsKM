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
    public class SubnodesController : ControllerBase
    {
        private readonly ISubnodeService _service;
        private readonly IMapper _mapper;

        public SubnodesController(
            ISubnodeService subnodeService,
            IMapper mapper
        )
        {
            _service = subnodeService;
            _mapper = mapper;
        }

        [HttpGet, Route("nodes/{nodeId}/subnodes")]
        public ActionResult<IEnumerable<SubnodeBaseResponse>> GetAllByNodeId(int nodeId)
        {
            var subnodes = _service.GetAllByNodeId(nodeId);
            return Ok(_mapper.Map<IEnumerable<SubnodeBaseResponse>>(subnodes));
        }

        [HttpGet, Route("subnodes/{id}/parents")]
        public ActionResult<SubnodeParentResponse> GetSubnodeParentResponseById(int id)
        {
            var subnode = _service.GetById(id);
            if (subnode != null) {
                return Ok(_mapper.Map<SubnodeParentResponse>(subnode));
            }
            return NotFound();
        }
    }
}
