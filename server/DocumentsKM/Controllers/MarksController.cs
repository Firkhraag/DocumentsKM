using System.Collections.Generic;
using System.Net;
using AutoMapper;
using DocumentsKM.Dtos;
using DocumentsKM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DocumentsKM.Controllers
{
    [Route("api")]
    [Authorize]
    [ApiController]
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
        public ActionResult<IEnumerable<MarkCodeResponse>> GetAllBySubnodeId(int subnodeId)
        {
            var marks = _service.GetAllBySubnodeId(subnodeId);
            return Ok(_mapper.Map<IEnumerable<MarkCodeResponse>>(marks));
        }

        // [HttpGet, Route("marks/{id}")]
        // [ProducesResponseType((int)HttpStatusCode.OK)]
        // [ProducesResponseType((int)HttpStatusCode.NotFound)]
        // public ActionResult<MarkReadDto> GetById(int id)
        // {
        //     var mark = _service.GetById(id);
        //     if (mark != null) {
        //         return Ok(_mapper.Map<MarkReadDto>(mark));
        //     }
        //     return NotFound();
        // }

        // [HttpPost, Route("marks")]
        // public ActionResult<MarkReadDto> CreateMark(MarkCreateDto markCreateDto)
        // {
        //     var markModel = _mapper.Map<Mark>(markCreateDto);
        //     _service.CreateMark(markModel);
        //     _service.SaveChanges();

        //     var markReadDto = _mapper.Map<MarkReadDto>(markModel);

        //     // Returns 201 and adds the header Location: https://localhost:5001/api/marks/{id}
        //     return CreatedAtRoute(nameof(GetMarkById), new {Id = markReadDto.Id}, markReadDto);
        // }

        // [Route("marks")]
        // [HttpPatch("{id}")]
        // public ActionResult UpdateMark(ulong id, JsonPatchDocument<MarkUpdateDto> patchDoc)
        // {
        //     var markModel = _service.GetMarkById(id);
        //     if (markModel == null) {
        //         return NotFound();
        //     }
        //     var markToPatch = _mapper.Map<MarkUpdateDto>(markModel);
        //     patchDoc.ApplyTo(markToPatch, ModelState);
        //     if (!TryValidateModel(markToPatch))
        //     {
        //         return ValidationProblem(ModelState);
        //     }
        //     _mapper.Map(markToPatch, markModel);
        //     _service.UpdateMark(markModel);
        //     _service.SaveChanges();
            
        //     return NoContent();
        // }
    }
}
