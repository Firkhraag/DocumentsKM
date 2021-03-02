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
    public class MarksController : ControllerBase
    {
        private readonly IMarkService _service;
        private readonly IMapper _mapper;

        public MarksController(
            IMarkService markService,
            IMapper mapper)
        {
            _service = markService;
            _mapper = mapper;
        }

        [HttpGet, Route("subnodes/{subnodeId}/marks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MarkBaseResponse>> GetAllBySubnodeId(
            int subnodeId)
        {
            var marks = _service.GetAllBySubnodeId(subnodeId);
            return Ok(_mapper.Map<IEnumerable<MarkBaseResponse>>(marks));
        }

        [HttpGet, Route("marks/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<MarkResponse> GetById(int id)
        {
            var mark = _service.GetById(id);
            if (mark != null)
                return Ok(_mapper.Map<MarkResponse>(mark));
            return NotFound();
        }

        [HttpGet, Route("marks/{id}/parents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<MarkParentResponse> GetMarkParentResponseById(int id)
        {
            var mark = _service.GetById(id);
            if (mark != null)
                return Ok(_mapper.Map<MarkParentResponse>(mark));
            return NotFound();
        }

        [HttpGet, Route("subnodes/{subnodeId}/new-mark-code")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<string> GetNewMarkCode(int subnodeId)
        {
            var code = _service.GetNewMarkCode(subnodeId);
            return Ok(code);
        }

        [HttpGet, Route("marks/{markId}/issue-date")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetMarkIssueDate(int markId)
        {
            var mark = _service.GetById(markId);
            if (mark != null)
            {
                return Ok(new { IssueDate = mark.IssuedDate });
            }
            return NotFound();
        }

        [HttpPost, Route("marks")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<MarkResponse> Create(
            [FromBody] MarkCreateRequest markRequest)
        {
            var markModel = _mapper.Map<Mark>(markRequest);
            try
            {
                _service.Create(
                    markModel,
                    markRequest.SubnodeId,
                    markRequest.DepartmentId,
                    markRequest.MainBuilderId,
                    markRequest.ChiefSpecialistId,
                    markRequest.GroupLeaderId);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (ConflictException)
            {
                return Conflict();
            }
            
            var markResponse = _mapper.Map<MarkResponse>(markModel);
            return CreatedAtAction(
                nameof(GetById), new {Id = markResponse.Id}, markResponse);
        }

        [HttpPatch, Route("marks/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult Update(int id, [FromBody] MarkUpdateRequest markRequest)
        {
            // DEBUG
            // Log.Information(JsonSerializer.Serialize(markRequest));
            if (!markRequest.Validate())
                return BadRequest();
            try
            {
                _service.Update(id, markRequest);
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
    }
}
