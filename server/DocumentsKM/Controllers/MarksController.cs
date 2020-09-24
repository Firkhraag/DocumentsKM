using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DocumentsKM.Data;
using DocumentsKM.Dtos;
using DocumentsKM.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DocumentsKM.Controllers
{
    [Route("api")]
    [Authorize]
    [ApiController]
    public class MarksController : ControllerBase
    {
        private readonly ILogger<MarksController> _logger;
        private readonly IMarkRepo _repository;
        private readonly IMapper _mapper;

        public MarksController(
            ILogger<MarksController> logger,
            IMarkRepo repo,
            IMapper mapper
        )
        {
            _logger = logger;
            _repository = repo;
            _mapper = mapper;
        }

        [HttpGet, Route("subnodes/{subnodeId}/marks")]
        public ActionResult<IEnumerable<SubnodeCodeReadDto>> GetAllSubnodeMarks(ulong subnodeId)
        {
            var marks = _repository.GetAllSubnodeMarks(subnodeId);
            // TBD: Should catch Internal server error!
            // Ok even if array is empty
            return Ok(_mapper.Map<IEnumerable<MarkCodeReadDto>>(marks));
        }

        [HttpGet, Route("marks/recent")]
        public ActionResult<IEnumerable<MarkWithSubnodeReadDto>> GetUserRecentMarks()
        {
            var marks = _repository.GetUserRecentMarks();
            // TBD: Should catch Internal server error!
            // Ok even if array is empty
            return Ok(_mapper.Map<IEnumerable<MarkWithSubnodeReadDto>>(marks));
        }

        [Route("marks")]
        [HttpGet("{id}", Name="GetMarkById")]
        public ActionResult<MarkReadDto> GetMarkById(ulong id)
        {
            var mark = _repository.GetMarkById(id);
            if (mark != null) {
                return Ok(_mapper.Map<MarkReadDto>(mark));
            }
            return NotFound();
        }

        [HttpGet, Route("marks")]
        public ActionResult<IEnumerable<MarkReadDto>> GetAllMarks()
        {
            var marks = _repository.GetAllMarks();
            // TBD: Should catch Internal server error!
            // Ok even if array is empty
            return Ok(_mapper.Map<IEnumerable<MarkReadDto>>(marks));
        }

        [HttpPost, Route("marks")]
        public ActionResult<MarkReadDto> CreateMark(MarkCreateDto markCreateDto)
        {
            var markModel = _mapper.Map<Mark>(markCreateDto);
            _repository.CreateMark(markModel);
            _repository.SaveChanges();

            var markReadDto = _mapper.Map<MarkReadDto>(markModel);

            // Returns 201 and adds the header Location: https://localhost:5001/api/marks/{id}
            return CreatedAtRoute(nameof(GetMarkById), new {Id = markReadDto.Id}, markReadDto);
        }

        [Route("marks")]
        [HttpPatch("{id}")]
        public ActionResult UpdateMark(ulong id, JsonPatchDocument<MarkUpdateDto> patchDoc)
        {
            var markModel = _repository.GetMarkById(id);
            if (markModel == null) {
                return NotFound();
            }
            var markToPatch = _mapper.Map<MarkUpdateDto>(markModel);
            patchDoc.ApplyTo(markToPatch, ModelState);
            if (!TryValidateModel(markToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(markToPatch, markModel);
            _repository.UpdateMark(markModel);
            _repository.SaveChanges();
            
            return NoContent();
        }
    }
}
