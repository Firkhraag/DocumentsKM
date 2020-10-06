using System.Collections.Generic;
using AutoMapper;
using DocumentsKM.Data;
using DocumentsKM.Dtos;
using DocumentsKM.Models;
using DocumentsKM.Services;
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
        private readonly INodeService _service;
        private readonly IMapper _mapper;

        public NodesController(
            INodeService nodeService,
            IMapper mapper
        )
        {
            _service = nodeService;
            _mapper = mapper;
        }

        [HttpGet, Route("projects/{projectId}/nodes")]
        public ActionResult<IEnumerable<NodeBaseResponse>> GetAllByProjectId(int projectId)
        {
            var nodes = _service.GetAllByProjectId(projectId);
            return Ok(_mapper.Map<IEnumerable<NodeBaseResponse>>(nodes));
        }
    }
}
