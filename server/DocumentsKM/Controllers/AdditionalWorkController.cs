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
    public class AdditionalWorkController : ControllerBase
    {
        private readonly IAdditionalWorkService _service;
        private readonly IMapper _mapper;

        public AdditionalWorkController(
            IAdditionalWorkService docService,
            IMapper mapper)
        {
            _service = docService;
            _mapper = mapper;
        }

        [HttpGet, Route("marks/{markId}/additional-work")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<AdditionalWorkResponse>> GetAllByMarkId(
            int markId)
        {
            return Ok(_service.GetAllByMarkId(markId));
        }

        [HttpPost, Route("marks/{markId}/additional-work")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<AdditionalWorkResponse> Create(
            int markId, [FromBody] AdditionalWorkCreateRequest additionalWorkRequest)
        {
            var additionalWorkModel = _mapper.Map<AdditionalWork>(
                additionalWorkRequest);
            try
            {
                _service.Create(
                    additionalWorkModel,
                    markId,
                    additionalWorkRequest.EmployeeId);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (ConflictException)
            {
                return Conflict();
            }
            return Created($"additional-work/{additionalWorkModel.Id}",
                _mapper.Map<AdditionalWorkResponse>(additionalWorkModel));
        }

        [HttpPatch, Route("additional-work/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult Update(
            int id, [FromBody] AdditionalWorkUpdateRequest additionalWorkRequest)
        {
            try
            {
                _service.Update(id, additionalWorkRequest);
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

        [HttpDelete, Route("additional-work/{id}")]
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
