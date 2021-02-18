using System;
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
    public class EstimateTaskController : ControllerBase
    {
        private readonly IEstimateTaskService _service;
        private readonly IMapper _mapper;

        public EstimateTaskController(
            IEstimateTaskService estimateTaskService,
            IMapper mapper)
        {
            _service = estimateTaskService;
            _mapper = mapper;
        }

        [HttpGet, Route("marks/{markId}/estimate-task")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<EstimateTaskResponse> GetByMarkId(int markId)
        {
            var estimateTask = _service.GetByMarkId(markId);
            if (estimateTask != null)
                return Ok(_mapper.Map<EstimateTaskResponse>(estimateTask));
            return NotFound();
        }

        [HttpPost, Route("marks/{markId}/estimate-task")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult Create(
            int markId,
            [FromBody] EstimateTaskCreateRequest estimateTaskRequest)
        {
            var estimateTaskModel = _mapper.Map<EstimateTask>(
                estimateTaskRequest);
            try
            {
                _service.Create(
                    estimateTaskModel,
                    markId,
                    estimateTaskRequest.ApprovalEmployeeId);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (ConflictException)
            {
                return Conflict();
            }
            return Created(
                $"marks/{markId}/estimate-task", null);
        }

        [HttpPatch, Route("marks/{markId}/estimate-task")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(
            int markId,
            [FromBody] EstimateTaskUpdateRequest estimateTaskRequest)
        {
            if (!estimateTaskRequest.Validate())
                return BadRequest();
            try
            {
                _service.Update(markId, estimateTaskRequest);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
