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

        [HttpGet,
            Route("general-data-sections/{sectionId}/general-data-points")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<GeneralDataPointResponse>> GetAllBySectionId(
            int sectionId)
        {
            var points = _service.GetAllBySectionId(sectionId);
            return Ok(_mapper.Map<IEnumerable<GeneralDataPointResponse>>(points));
        }

        [HttpGet,
            Route("users/{userId}/general-data-sections/{sectionName}/general-data-points")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<GeneralDataPointResponse>> GetAllByUserIdAndSectionName(
            int userId, string sectionName)
        {
            var points = _service.GetAllByUserIdAndSectionName(userId, sectionName);
            return Ok(_mapper.Map<IEnumerable<GeneralDataPointResponse>>(points));
        }

        [HttpPost,
            Route("general-data-sections/{sectionId}/general-data-points")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<GeneralDataPoint> Create(int sectionId,
            [FromBody] GeneralDataPointCreateRequest generalDataPointRequest)
        {
            var generalDataPointModel = _mapper.Map<GeneralDataPoint>(
                generalDataPointRequest);
            try
            {
                _service.Create(
                    generalDataPointModel,
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

        [HttpPatch,
            Route("general-data-sections/{sectionId}/general-data-points/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult Update(int sectionId, int id,
            [FromBody] GeneralDataPointUpdateRequest generalDataPointRequest)
        {
            if (!generalDataPointRequest.Validate())
                return BadRequest();
            try
            {
                _service.Update(id, sectionId, generalDataPointRequest);
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
            Route("general-data-sections/{sectionId}/general-data-points/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int sectionId, int id)
        {
            try
            {
                _service.Delete(id, sectionId);
                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }
    }
}
