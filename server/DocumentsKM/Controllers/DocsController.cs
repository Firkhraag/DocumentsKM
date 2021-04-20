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
    public class DocsController : ControllerBase
    {
        private readonly IDocService _service;
        private readonly IMapper _mapper;

        public DocsController(
            IDocService docService,
            IMapper mapper)
        {
            _service = docService;
            _mapper = mapper;
        }

        [HttpGet, Route("marks/{markId}/docs/sheets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<SheetResponse>> GetAllSheetsByMarkId(
            int markId)
        {
            var docs = _service.GetAllSheetsByMarkId(markId);
            return Ok(_mapper.Map<IEnumerable<SheetResponse>>(docs));
        }

        [HttpGet, Route("marks/{markId}/docs/attached")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<DocResponse>> GetAllAttachedByMarkId(
            int markId)
        {
            var docs = _service.GetAllAttachedByMarkId(markId);
            return Ok(_mapper.Map<IEnumerable<DocResponse>>(docs));
        }

        [HttpPost, Route("marks/{markId}/docs")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Create(
            int markId, [FromBody] DocCreateRequest docRequest)
        {
            var docModel = _mapper.Map<Doc>(docRequest);
            try
            {
                _service.Create(
                    docModel,
                    markId,
                    docRequest.TypeId,
                    docRequest.CreatorId,
                    docRequest.InspectorId,
                    docRequest.NormContrId);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            return Created(
                $"docs/{docModel.Id}", new { Id = docModel.Id });
        }

        [HttpPatch, Route("docs/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(int id, [FromBody] DocUpdateRequest docRequest)
        {
            if (!docRequest.Validate())
                return BadRequest();
            try
            {
                _service.Update(id, docRequest);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete, Route("docs/{id}")]
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
