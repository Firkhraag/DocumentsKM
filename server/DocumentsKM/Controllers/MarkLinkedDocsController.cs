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

        [HttpGet, Route("marks/{markId}/mark-linked-docs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MarkLinkedDocResponse>> GetAllByMarkId(int markId)
        {
            var markLinkedDocs = _service.GetAllByMarkId(markId);
            return Ok(_mapper.Map<IEnumerable<MarkLinkedDocResponse>>(markLinkedDocs));
        }

        [HttpPost, Route("marks/{markId}/mark-linked-docs")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult Create(
            int markId, MarkLinkedDocCreateRequest markLinkedDocRequest)
        {
            try
            {
                var markLinkedDocModel = _mapper.Map<MarkLinkedDoc>(markLinkedDocRequest);
                _service.Create(markLinkedDocModel, markId, markLinkedDocRequest.LinkedDocId);
                return Created($"mark-linked-docs/{markLinkedDocModel.Id}", null);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (ConflictException)
            {
                return Conflict();
            }
        }

        [HttpPatch, Route("mark-linked-docs/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult Update(
            int id, [FromBody] MarkLinkedDocUpdateRequest markLinkedDocRequest)
        {
            if (!markLinkedDocRequest.Validate())
                return BadRequest();
            try
            {
                _service.Update(id, markLinkedDocRequest);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (ConflictException)
            {
                return Conflict();
            }
            return NoContent();
        }

        [HttpDelete, Route("mark-linked-docs/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            try
            {
                _service.Delete(id);
                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }
    }
}
