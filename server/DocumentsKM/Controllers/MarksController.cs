using System.Collections.Generic;
using System.Net.Mime;
using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Models;
using DocumentsKM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
            IMapper mapper
        )
        {
            _service = markService;
            _mapper = mapper;
        }

        [HttpGet, Route("subnodes/{subnodeId}/marks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MarkBaseResponse>> GetAllBySubnodeId(int subnodeId)
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
            if (mark != null) {
                return Ok(_mapper.Map<MarkResponse>(mark));
            }
            return NotFound();
        }

        [HttpGet, Route("marks/{id}/parents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<MarkParentResponse> GetMarkParentResponseById(int id)
        {
            var mark = _service.GetById(id);
            if (mark != null) {
                return Ok(_mapper.Map<MarkParentResponse>(mark));
            }
            return NotFound();
        }

        [HttpPost, Route("marks")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<MarkResponse> Create([FromBody] MarkRequest markRequest)
        {
            var markModel = _mapper.Map<Mark>(markRequest);
            _service.Create(markModel);
            var markResponse = _mapper.Map<MarkResponse>(markModel);
            return CreatedAtRoute(nameof(GetById), new {Id = markResponse.Id}, markResponse);
        }

        [HttpPatch, Route("marks/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(int id, [FromBody] JsonPatchDocument<MarkRequest> patchDoc)
        {
            var markModel = _service.GetById(id);
            if (markModel == null) {
                return NotFound();
            }
            var markToPatch = _mapper.Map<MarkRequest>(markModel);
            patchDoc.ApplyTo(markToPatch, ModelState);
            if (!TryValidateModel(markToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(markToPatch, markModel);
            _service.Update(markModel);
            
            return NoContent();
        }
    }
}
