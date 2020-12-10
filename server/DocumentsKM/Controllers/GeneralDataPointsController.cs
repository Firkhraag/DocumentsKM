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
    public class GeneralDataPointsController : ControllerBase
    {
        private readonly IGeneralDataPointService _service;
        private readonly IMapper _mapper;

        public GeneralDataPointsController(
            IGeneralDataPointService generalDataPointService,
            IMapper mapper)
        {
            _service = generalDataPointService;
            _mapper = mapper;
        }

        [HttpGet, Route("users/{userId}/general-data-sections/{sectionId}/general-data-points")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<GeneralDataPointResponse>> GetAllByUserAndSectionId(
            int userId, int sectionId)
        {
            var points = _service.GetAllByUserAndSectionId(userId, sectionId);
            return Ok(_mapper.Map<IEnumerable<GeneralDataPointResponse>>(points));
        }

        [HttpPost, Route("users/{userId}/general-data-sections/{sectionId}/general-data-points")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<GeneralDataPoint> Create(int userId, int sectionId,
            [FromBody] GeneralDataPointCreateRequest generalDataPointRequest)
        {
            var generalDataPointModel = _mapper.Map<GeneralDataPoint>(generalDataPointRequest);
            try
            {
                _service.Create(
                    generalDataPointModel,
                    userId,
                    sectionId);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (ConflictException)
            {
                return Conflict();
            }
            return Created($"general-data-points/{generalDataPointModel.Id}",
                _mapper.Map<GeneralDataPointResponse>(generalDataPointModel));
        }

        [HttpPatch, Route("general-data-points/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult Update(int id, [FromBody] GeneralDataPointUpdateRequest generalDataPointRequest)
        {
            try
            {
                _service.Update(id, generalDataPointRequest);
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

        [HttpDelete, Route("general-data-points/{id}")]
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
