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
    public class standardConstructionsController : ControllerBase
    {
        private readonly IStandardConstructionService _service;
        private readonly IMapper _mapper;

        public standardConstructionsController(
            IStandardConstructionService standardConstructionService,
            IMapper mapper)
        {
            _service = standardConstructionService;
            _mapper = mapper;
        }

        [HttpGet, Route("specifications/{specificationId}/standard-constructions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<StandardConstructionResponse>> GetAllBySpecificationId(
            int specificationId)
        {
            var docs = _service.GetAllBySpecificationId(specificationId);
            return Ok(_mapper.Map<IEnumerable<StandardConstructionResponse>>(docs));
        }

        [HttpPost, Route("specifications/{specificationId}/standard-constructions")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<StandardConstructionResponse> Create(
            int specificationId,
            [FromBody] StandardConstructionCreateRequest standardConstructionRequest)
        {
            var standardConstructionModel = _mapper.Map<StandardConstruction>(
                standardConstructionRequest);
            try
            {
                _service.Create(
                    standardConstructionModel,
                    specificationId);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (ConflictException)
            {
                return Conflict();
            }
            return Created($"standard-constructions/{standardConstructionModel.Id}",
                _mapper.Map<StandardConstructionResponse>(standardConstructionModel));
        }

        [HttpPatch, Route("standard-constructions/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult Update(
            int id, [FromBody] StandardConstructionUpdateRequest standardConstructionRequest)
        {
            try
            {
                _service.Update(id, standardConstructionRequest);
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

        [HttpDelete, Route("standard-constructions/{id}")]
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
