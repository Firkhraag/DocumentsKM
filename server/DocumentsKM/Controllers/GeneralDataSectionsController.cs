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
    public class GeneralDataSectionsController : ControllerBase
    {
        private readonly IGeneralDataSectionService _service;
        private readonly IMapper _mapper;

        public GeneralDataSectionsController(
            IGeneralDataSectionService GeneralDataSectionService,
            IMapper mapper)
        {
            _service = GeneralDataSectionService;
            _mapper = mapper;
        }

        [HttpGet, Route("users/{userId}/general-data-sections")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<GeneralDataSectionResponse>> GetAllByUserId(
            int userId)
        {
            var sections = _service.GetAllByUserId(userId);
            return Ok(_mapper.Map<IEnumerable<GeneralDataSectionResponse>>(sections));
        }

        [HttpPost, Route("users/{userId}/general-data-sections")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<GeneralDataSection> Create(int userId,
            [FromBody] GeneralDataSectionCreateRequest generalDataSectionRequest)
        {
            var generalDataSectionModel = _mapper.Map<GeneralDataSection>(
                generalDataSectionRequest);
            try
            {
                _service.Create(
                    generalDataSectionModel,
                    userId);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (ConflictException)
            {
                return Conflict();
            }
            return Created($"general-data-sections/{generalDataSectionModel.Id}",
                _mapper.Map<GeneralDataSectionResponse>(generalDataSectionModel));
        }

        [HttpPatch,
            Route("users/{userId}/general-data-sections/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult Update(int userId, int id,
            [FromBody] GeneralDataSectionUpdateRequest GeneralDataSectionRequest)
        {
            if (!GeneralDataSectionRequest.Validate())
                return BadRequest();
            try
            {
                _service.Update(id, userId, GeneralDataSectionRequest);
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

        [HttpDelete,
            Route("users/{userId}/general-data-sections/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int userId, int id)
        {
            try
            {
                _service.Delete(id, userId);
                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [HttpPost, Route("users/{userId}/general-data-sections/copy")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<GeneralDataSection> Copy(int userId,
            [FromBody] GeneralDataSectionCopyRequest generalDataSectionRequest)
        {
            try
            {
                _service.Copy(
                    userId,
                    generalDataSectionRequest);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (ConflictException)
            {
                return Conflict();
            }
            return Ok();
        }
    }
}

