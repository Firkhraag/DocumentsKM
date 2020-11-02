using System.Collections.Generic;
using AutoMapper;
using DocumentsKM.Dtos;
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
    public class NodesController : ControllerBase
    {
        private readonly INodeService _service;
        private readonly IMapper _mapper;

        public NodesController(
            INodeService nodeService,
            IMapper mapper)
        {
            _service = nodeService;
            _mapper = mapper;
        }

        [HttpGet, Route("projects/{projectId}/nodes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<NodeBaseResponse>> GetAllByProjectId(int projectId)
        {
            var nodes = _service.GetAllByProjectId(projectId);
            return Ok(_mapper.Map<IEnumerable<NodeBaseResponse>>(nodes));
        }
    }
}
