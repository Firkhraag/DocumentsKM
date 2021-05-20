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
        private readonly ICorrProtGeneralDataPointService _corrProtGeneralDataPointService;
        private readonly IMapper _mapper;

        public MarkGeneralDataPointsController(
            IMarkGeneralDataPointService markGeneralDataPointService,
            ICorrProtGeneralDataPointService corrProtGeneralDataPointService,
            IMapper mapper)
        {
            _service = markGeneralDataPointService;
            _corrProtGeneralDataPointService = corrProtGeneralDataPointService;
            _mapper = mapper;
        }

        [HttpGet, Route("mark-general-data-sections/{sectionId}/mark-general-data-points")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MarkGeneralDataPointResponse>> GetAllBySectionId(
            int sectionId)
        {
            var points = _service.GetAllBySectionId(sectionId);
            return Ok(_mapper.Map<IEnumerable<MarkGeneralDataPointResponse>>(points));
        }

        [HttpGet, Route("marks/{markId}/corr-prot-point")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<GeneralDataSection>> GetCorrProtPointByMarkId(int markId)
        {
            var point = _corrProtGeneralDataPointService.GetWholeString(markId);
            return Ok(new { Result = point });
        }

        [HttpPost, Route("mark-general-data-sections/{sectionId}/mark-general-data-points")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<MarkGeneralDataPoint> Create(int sectionId,
            [FromBody] MarkGeneralDataPointCreateRequest markGeneralDataPointRequest)
        {
            var markGeneralDataPointModel = _mapper.Map<MarkGeneralDataPoint>(
                markGeneralDataPointRequest);
            try
            {
                _service.Create(
                    markGeneralDataPointModel,
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

        [HttpPatch,
            Route("mark-general-data-sections/{sectionId}/mark-general-data-points")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<MarkGeneralDataPointResponse>> UpdateAllByPointIds(
            int sectionId,
            [FromBody] List<int> pointIds)
        {
            try
            {
                var addedPoints = _service.UpdateAllByPointIds(sectionId, pointIds);
                return Ok(_mapper.Map<IEnumerable<MarkGeneralDataPointResponse>>(
                    addedPoints));
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [HttpPatch,
            Route("mark-general-data-sections/{sectionId}/mark-general-data-points/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult Update(int sectionId, int id,
            [FromBody] MarkGeneralDataPointUpdateRequest markGeneralDataPointRequest)
        {
            if (!markGeneralDataPointRequest.Validate())
                return BadRequest();
            try
            {
                _service.Update(id, sectionId, markGeneralDataPointRequest);
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
            Route("mark-general-data-sections/{sectionId}/mark-general-data-points/{id}")]
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
