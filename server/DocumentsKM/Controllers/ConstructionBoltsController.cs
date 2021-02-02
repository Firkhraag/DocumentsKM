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
    public class ConstructionBoltsController : ControllerBase
    {
        private readonly IConstructionBoltService _service;
        private readonly IMapper _mapper;

        public ConstructionBoltsController(
            IConstructionBoltService constructionBoltService,
            IMapper mapper
        )
        {
            _service = constructionBoltService;
            _mapper = mapper;
        }

        [HttpGet, Route("constructions/{constructionId}/bolts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ConstructionBoltResponse>> GetAllByConstructionId(
            int constructionId)
        {
            var constructionBolts = _service.GetAllByConstructionId(constructionId);
            return Ok(
                _mapper.Map<IEnumerable<ConstructionBoltResponse>>(constructionBolts));
        }

        [HttpPost, Route("constructions/{constructionId}/bolts")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<ConstructionBolt> Add(
            int constructionId, ConstructionBoltCreateRequest constructionBoltRequest)
        {
            try
            {
                var constructionBoltModel = _mapper.Map<ConstructionBolt>(
                    constructionBoltRequest);
                _service.Create(
                    constructionBoltModel,
                    constructionId,
                    constructionBoltRequest.DiameterId);
                return Created($"construction-bolts/", constructionBoltModel);
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

        [HttpPatch, Route("construction-bolts/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult Update(
            int id, [FromBody] ConstructionBoltUpdateRequest constructionBoltRequest)
        {
            try
            {
                _service.Update(id, constructionBoltRequest);
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

        [HttpDelete, Route("construction-bolts/{id}")]
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
