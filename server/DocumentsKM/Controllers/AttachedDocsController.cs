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
    public class AttachedDocsController : ControllerBase
    {
        private readonly IAttachedDocService _service;
        private readonly IMapper _mapper;

        public AttachedDocsController(
            IAttachedDocService docService,
            IMapper mapper)
        {
            _service = docService;
            _mapper = mapper;
        }

        [HttpGet, Route("marks/{markId}/attached-docs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<AttachedDocResponse>> GetAllByMarkId(
            int markId)
        {
            var docs = _service.GetAllByMarkId(markId);
            return Ok(_mapper.Map<IEnumerable<AttachedDocResponse>>(docs));
        }

        [HttpPost, Route("marks/{markId}/attached-docs")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult Create(
            int markId, [FromBody] AttachedDocCreateRequest attachedDocRequest)
        {
            var attachedDocModel = _mapper.Map<AttachedDoc>(attachedDocRequest);
            try
            {
                _service.Create(
                    attachedDocModel,
                    markId);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (ConflictException)
            {
                return Conflict();
            }
            return Created($"docs/{attachedDocModel.Id}", null);
        }

        [HttpPatch, Route("attached-docs/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult Update(
            int id, [FromBody] AttachedDocUpdateRequest attachedDocRequest)
        {
            try
            {
                _service.Update(id, attachedDocRequest);
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

        [HttpDelete, Route("attached-docs/{id}")]
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
