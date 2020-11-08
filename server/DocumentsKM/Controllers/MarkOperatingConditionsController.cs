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
    public class MarkOperatingConditionsController : ControllerBase
    {
        private readonly IMarkOperatingConditionsService _service;
        private readonly IMapper _mapper;

        public MarkOperatingConditionsController(
            IMarkOperatingConditionsService markOperatingConditionsService,
            IMapper mapper)
        {
            _service = markOperatingConditionsService;
            _mapper = mapper;
        }

        [HttpGet, Route("marks/{markId}/mark-operating-conditions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<MarkOperatingConditionsResponse> GetByMarkId(int markId)
        {
            var markOperatingConditions = _service.GetByMarkId(markId);
            if (markOperatingConditions != null)
                return Ok(_mapper.Map<MarkOperatingConditionsResponse>(markOperatingConditions));
            return NotFound();
        }

        [HttpPost, Route("marks/{markId}/mark-operating-conditions")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<MarkOperatingConditions> Create(int markId, [FromBody] MarkOperatingConditionsCreateRequest markOperatingConditionsRequest)
        {
            var markOperatingConditionsModel = _mapper.Map<MarkOperatingConditions>(markOperatingConditionsRequest);
            try
            {
                _service.Create(
                    markOperatingConditionsModel,
                    markId,
                    markOperatingConditionsRequest.EnvAggressivenessId,
                    markOperatingConditionsRequest.OperatingAreaId,
                    markOperatingConditionsRequest.GasGroupId,
                    markOperatingConditionsRequest.ConstructionMaterialId,
                    markOperatingConditionsRequest.PaintworkTypeId,
                    markOperatingConditionsRequest.HighTensileBoltsTypeId);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (ConflictException)
            {
                return Conflict();
            }
            
            var markOperatingConditionsResponse = _mapper.Map<MarkOperatingConditionsResponse>(markOperatingConditionsModel);
            return CreatedAtAction(nameof(GetByMarkId), new {Id = markId}, markOperatingConditionsResponse);
        }

        [HttpPatch, Route("marks/{markId}/mark-operating-conditions")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(int markId, [FromBody] MarkOperatingConditionsUpdateRequest markOperatingConditionsRequest)
        {
            try
            {
                _service.Update(markId, markOperatingConditionsRequest);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
