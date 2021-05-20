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
    public class MarkGeneralDataSectionsController : ControllerBase
    {
        private readonly IMarkGeneralDataSectionService _service;
        private readonly IMapper _mapper;

        public MarkGeneralDataSectionsController(
            IMarkGeneralDataSectionService markGeneralDataSectionService,
            IMapper mapper)
        {
            _service = markGeneralDataSectionService;
            _mapper = mapper;
        }

        [HttpGet, Route("marks/{markId}/mark-general-data-sections")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MarkGeneralDataSectionResponse>> GetAllByMarkId(
            int markId)
        {
            var sections = _service.GetAllByMarkId(markId);
            return Ok(_mapper.Map<IEnumerable<MarkGeneralDataSectionResponse>>(sections));
        }

        [HttpPost, Route("marks/{markId}/mark-general-data-sections")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<MarkGeneralDataSection> Create(int markId,
            [FromBody] MarkGeneralDataSectionCreateRequest markGeneralDataSectionRequest)
        {
            var markGeneralDataSectionModel = _mapper.Map<MarkGeneralDataSection>(
                markGeneralDataSectionRequest);
            try
            {
                _service.Create(
                    markGeneralDataSectionModel,
                    markId);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (ConflictException)
            {
                return Conflict();
            }
            return Created($"mark-general-data-sections/{markGeneralDataSectionModel.Id}",
                _mapper.Map<MarkGeneralDataSectionResponse>(markGeneralDataSectionModel));
        }

        [HttpPatch, Route("marks/{markId}/mark-general-data-sections")]
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
            Route("marks/{markId}/mark-general-data-sections/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult Update(int markId, int id,
            [FromBody] MarkGeneralDataSectionUpdateRequest markGeneralDataSectionRequest)
        {
            if (!markGeneralDataSectionRequest.Validate())
                return BadRequest();
            try
            {
                _service.Update(id, markId, markGeneralDataSectionRequest);
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
            Route("marks/{markId}/mark-general-data-sections/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int markId, int id)
        {
            try
            {
                _service.Delete(id, markId);
                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        // [HttpPost, Route("mark-general-data-sections/copy")]
        // [Consumes(MediaTypeNames.Application.Json)]
        // [ProducesResponseType(StatusCodes.Status201Created)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesResponseType(StatusCodes.Status409Conflict)]
        // public ActionResult<GeneralDataSection> Copy(int userId,
        //     [FromBody] MarkGeneralDataSectionCopyRequest markGeneralDataSectionRequest)
        // {
        //     try
        //     {
        //         _service.Copy(markGeneralDataSectionRequest);
        //     }
        //     catch (ArgumentNullException)
        //     {
        //         return NotFound();
        //     }
        //     catch (ConflictException)
        //     {
        //         return Conflict();
        //     }
        //     return Ok();
        // }
    }
}
