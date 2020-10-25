using System;
using System.Collections.Generic;
using System.Net.Mime;
using AutoMapper;
using DocumentsKM.Dtos;
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
    public class SpecificationsController : ControllerBase
    {
        private readonly ISpecificationService _service;
        private readonly IMapper _mapper;

        public SpecificationsController(
            ISpecificationService specificationService,
            IMapper mapper)
        {
            _service = specificationService;
            _mapper = mapper;
        }

        [HttpGet, Route("marks/{markId}/specifications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<SpecificationResponse>> GetAllByMarkId(int markId)
        {
            var specifications = _service.GetAllByMarkId(markId);
            return Ok(_mapper.Map<IEnumerable<SpecificationResponse>>(specifications));
        }

        [HttpPost, Route("marks/{markId}/specifications")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<SpecificationResponse>> Create(int markId)
        {
            try
            {
                var specification = _service.Create(markId);
                return Created($"specifications/{specification.Id}", _mapper.Map<SpecificationResponse>(specification));
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [HttpPatch, Route("marks/{markId}/specifications/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(int markId, int id, [FromBody] SpecificationUpdateRequest specificationRequest)
        {
            // DEBUG
            // Log.Information(JsonSerializer.Serialize(markRequest));
            try
            {
                _service.Update(markId, id, specificationRequest);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete, Route("marks/{markId}/specifications/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<IEnumerable<SpecificationResponse>> Delete(int markId, int id)
        {
            try
            {
                _service.Delete(markId, id);
                return NoContent();
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
    }
}
