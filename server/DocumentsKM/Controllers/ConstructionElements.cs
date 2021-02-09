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
    public class ConstructionElementsController : ControllerBase
    {
        private readonly IConstructionElementService _service;
        private readonly IMapper _mapper;

        public ConstructionElementsController(
            IConstructionElementService constructionElementService,
            IMapper mapper
        )
        {
            _service = constructionElementService;
            _mapper = mapper;
        }

        [HttpGet, Route("constructions/{constructionId}/elements")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ConstructionElementResponse>> GetAllByConstructionId(
            int constructionId)
        {
            var constructionElements = _service.GetAllByConstructionId(constructionId);
            return Ok(
                _mapper.Map<IEnumerable<ConstructionElementResponse>>(constructionElements));
        }

        [HttpPost, Route("constructions/{constructionId}/elements")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<ConstructionElement> Add(
            int constructionId, ConstructionElementCreateRequest constructionElementRequest)
        {
            try
            {
                var constructionElementModel = _mapper.Map<ConstructionElement>(
                    constructionElementRequest);
                _service.Create(
                    constructionElementModel,
                    constructionId,
                    constructionElementRequest.ProfileClassId,
                    constructionElementRequest.ProfileId,
                    constructionElementRequest.SteelId);
                return Created($"construction-elements/", constructionElementModel);
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

        [HttpPatch, Route("construction-elements/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult Update(
            int id, [FromBody] ConstructionElementUpdateRequest constructionElementRequest)
        {
            try
            {
                _service.Update(id, constructionElementRequest);
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

        [HttpDelete, Route("construction-elements/{id}")]
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
