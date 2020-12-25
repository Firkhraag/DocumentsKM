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

        [HttpGet, Route("marks/{markId}/docs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<AddWorkDocResponse>> GetAllByMarkId(int markId)
        {
            // var docs = _service.GetAllByMarkId(markId);
            (var docsGroupedByCreator, var docsGroupedByNormContr) = _service.GetAddWorkByMarkId(markId);
            return Ok(new { DocsGroupedByCreator = _mapper.Map<IEnumerable<AddWorkDocResponse>>(
                docsGroupedByCreator), docsGroupedByNormContr = _mapper.Map<IEnumerable<AddWorkDocResponse>>(
                    docsGroupedByNormContr) });
        }

        // Endpoint для получения листов основного комплекта
        [HttpGet, Route("marks/{markId}/docs/sheets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<SheetResponse>> GetAllSheetsByMarkId(int markId)
        {
            var docs = _service.GetAllSheetsByMarkId(markId);
            return Ok(_mapper.Map<IEnumerable<SheetResponse>>(docs));
        }

        // Endpoint для получения разрабатываемых прилагаемых документов
        [HttpGet, Route("marks/{markId}/docs/attached")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<DocResponse>> GetAllAttachedByMarkId(int markId)
        {
            var docs = _service.GetAllAttachedByMarkId(markId);
            return Ok(_mapper.Map<IEnumerable<DocResponse>>(docs));
        }

        [HttpPost, Route("marks/{markId}/docs")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DocResponse> Create(int markId, [FromBody] DocCreateRequest docRequest)
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
            
            return Created($"docs/{docModel.Id}", _mapper.Map<DocResponse>(docModel));
        }

        [HttpPatch, Route("docs/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(int id, [FromBody] DocUpdateRequest docRequest)
        {
            // DEBUG
            // Log.Information(JsonSerializer.Serialize(markRequest));
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
