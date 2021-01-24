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
    public class ConstructionsController : ControllerBase
    {
        private readonly IConstructionService _service;
        private readonly IMapper _mapper;

        public ConstructionsController(
            IConstructionService constructionService,
            IMapper mapper)
        {
            _service = constructionService;
            _mapper = mapper;
        }

        [HttpGet, Route("specifications/{specificationId}/constructions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ConstructionResponse>> GetAllBySpecificationId(
            int specificationId)
        {
            var docs = _service.GetAllBySpecificationId(specificationId);
            return Ok(_mapper.Map<IEnumerable<ConstructionResponse>>(docs));
        }

        [HttpPost, Route("specifications/{specificationId}/constructions")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<ConstructionResponse> Create(
            int specificationId,
            [FromBody] ConstructionCreateRequest constructionRequest)
        {
            var constructionModel = _mapper.Map<Construction>(constructionRequest);
            try
            {
                _service.Create(
                    constructionModel,
                    specificationId,
                    constructionRequest.TypeId,
                    constructionRequest.SubtypeId,
                    constructionRequest.WeldingControlId);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (ConflictException)
            {
                return Conflict();
            }
            return Created($"constructions/{constructionModel.Id}",
                _mapper.Map<ConstructionResponse>(constructionModel));
        }

        [HttpPatch, Route("constructions/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult Update(
            int id, [FromBody] ConstructionUpdateRequest constructionRequest)
        {
            try
            {
                _service.Update(id, constructionRequest);
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

        [HttpDelete, Route("constructions/{id}")]
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
