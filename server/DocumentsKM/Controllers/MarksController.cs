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
        public ActionResult<IEnumerable<MarkResponse>> GetAllBySubnodeId(
            int subnodeId)
        {
            var marks = _service.GetAllBySubnodeId(subnodeId);
            return Ok(_mapper.Map<IEnumerable<MarkResponse>>(marks));
        }

        [HttpPost, Route("marks/recent")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MarkResponse>> GetAllRecentByIds(
            [FromBody] IdsRequest idsRequest)
        {
            var marks = _service.GetAllByIds(idsRequest.Ids);
            return Ok(_mapper.Map<IEnumerable<MarkResponse>>(marks));
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
                return Ok(new { IssueDate = mark.IssueDate });
            }
            return NotFound();
        }

        [HttpPost, Route("users/{userId}/subnodes/{subnodeId}/marks")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult Create(
            int userId, int subnodeId, [FromBody] MarkCreateRequest markRequest)
        {
            var markModel = _mapper.Map<Mark>(markRequest);
            try
            {
                _service.Create(
                    markModel,
                    userId,
                    subnodeId,
                    markRequest.DepartmentId,
                    markRequest.ChiefSpecialistId,
                    markRequest.GroupLeaderId,
                    markRequest.NormContrId);
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
        [ProducesResponseType(StatusCodes.Status200OK)]
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
                var designation =_service.Update(id, markRequest);
                return Ok(new { Designation = designation });
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
