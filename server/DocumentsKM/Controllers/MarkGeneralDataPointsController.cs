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
    public class MarkGeneralDataPointsController : ControllerBase
    {
        private readonly IMarkGeneralDataPointService _service;
        private readonly IMapper _mapper;

        public MarkGeneralDataPointsController(
            IMarkGeneralDataPointService markGeneralDataPointService,
            IMapper mapper)
        {
            _service = markGeneralDataPointService;
            _mapper = mapper;
        }

        [HttpGet, Route("marks/{markId}/general-data-sections/{sectionId}/general-data-points")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MarkGeneralDataPointResponse>> GetAllByMarkAndSectionId(
            int markId, int sectionId)
        {
            var points = _service.GetAllByMarkAndSectionId(markId, sectionId);
            return Ok(_mapper.Map<IEnumerable<MarkGeneralDataPointResponse>>(points));
        }

        [HttpGet, Route("marks/{markId}/general-data-sections")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<GeneralDataSection>> GetSectionsByMarkId(int markId)
        {
            var sections = _service.GetSectionsByMarkId(markId);
            return Ok(_mapper.Map<IEnumerable<GeneralDataSection>>(sections));
        }

        [HttpPost, Route("marks/{markId}/general-data-sections/{sectionId}/general-data-points")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<MarkGeneralDataPoint> Create(int markId, int sectionId,
            [FromBody] MarkGeneralDataPointCreateRequest markGeneralDataPointRequest)
        {
            var markGeneralDataPointModel = _mapper.Map<MarkGeneralDataPoint>(
                markGeneralDataPointRequest);
            try
            {
                _service.Create(
                    markGeneralDataPointModel,
                    markId,
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
            return Created($"mark-general-data-points/{markGeneralDataPointModel.Id}",
                _mapper.Map<MarkGeneralDataPointResponse>(markGeneralDataPointModel));
        }

        [HttpPatch, Route("marks/{markId}/general-data-points")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult UpdateAllBySectionIds(int markId, [FromBody] List<int> sectionIds)
        {
            try
            {
                _service.UpdateAllBySectionIds(markId, sectionIds);
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

        [HttpPatch,
            Route("users/{userId}/marks/{markId}/general-data-sections/{sectionId}/general-data-points")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<MarkGeneralDataPointResponse>> UpdateAllByPointIds(
            int userId,
            int markId,
            int sectionId,
            [FromBody] List<int> pointIds)
        {
            try
            {
                var addedPoints = _service.UpdateAllByPointIds(
                    userId, markId, sectionId, pointIds);
                return Ok(_mapper.Map<IEnumerable<MarkGeneralDataPointResponse>>(
                    addedPoints));
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [HttpPatch,
            Route("marks/{markId}/general-data-sections/{sectionId}/general-data-points/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult Update(int markId, int sectionId, int id,
            [FromBody] MarkGeneralDataPointUpdateRequest markGeneralDataPointRequest)
        {
            try
            {
                _service.Update(id, markId, sectionId, markGeneralDataPointRequest);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete,
            Route("marks/{markId}/general-data-sections/{sectionId}/general-data-points/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int markId, int sectionId, int id)
        {
            try
            {
                _service.Delete(id, markId, sectionId);
                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }
    }
}
