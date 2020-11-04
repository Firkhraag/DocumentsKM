using System;
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
    public class MarkLinkedDocsController : ControllerBase
    {
        private readonly IMarkLinkedDocService _service;
        private readonly IMapper _mapper;

        public MarkLinkedDocsController(
            IMarkLinkedDocService markLinkedDocService,
            IMapper mapper
        )
        {
            _service = markLinkedDocService;
            _mapper = mapper;
        }

        [HttpGet, Route("marks/{markId}/linked-docs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<LinkedDoc>> GetAllByMarkId(int markId)
        {
            var linkedDocs = _service.GetAllLinkedDocsByMarkId(markId);
            // return Ok(_mapper.Map<IEnumerable<EmployeeDepartmentResponse>>(employees));
            return Ok(linkedDocs);
        }

        [HttpPatch, Route("marks/{markId}/approvals")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(int markId, [FromBody] List<int> employeeIds)
        {
            try
            {
                _service.Update(markId, employeeIds);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
