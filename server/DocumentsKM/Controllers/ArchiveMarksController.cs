using System;
using System.Collections.Generic;
using System.Net.Mime;
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
    public class ArchiveMarksController : ControllerBase
    {
        private readonly IArchiveMarkService _service;

        public ArchiveMarksController(
            IArchiveMarkService archiveMarkService)
        {
            _service = archiveMarkService;
        }

        [HttpGet, Route("subnodes/{subnodeId}/archive-marks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MarkResponse>> GetAllBySubnodeId(
            int subnodeId)
        {
            var archiveMarks = _service.GetAllBySubnodeId(subnodeId);
            return Ok(archiveMarks);
        }
    }
}
