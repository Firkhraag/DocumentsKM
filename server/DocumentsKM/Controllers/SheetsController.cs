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
    public class SheetsController : ControllerBase
    {
        private readonly ISheetService _service;
        private readonly IMapper _mapper;

        public SheetsController(
            ISheetService sheetService,
            IMapper mapper)
        {
            _service = sheetService;
            _mapper = mapper;
        }

        [HttpGet, Route("marks/{markId}/sheets/basic")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<SheetResponse>> GetAllBasicSheetsByMarkId(int markId)
        {
            // Id листа основного комплекта из справочника
            var basicSheetDocTypeId = 1;
            var sheets = _service.GetAllByMarkIdAndDocTypeId(markId, basicSheetDocTypeId);
            return Ok(_mapper.Map<IEnumerable<SheetResponse>>(sheets));
        }

        [HttpPost, Route("marks/{markId}/sheets")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<SheetResponse> Create(int markId, [FromBody] SheetCreateRequest sheetRequest)
        {
            var sheetModel = _mapper.Map<Sheet>(sheetRequest);
            try
            {
                _service.Create(
                    sheetModel,
                    markId,
                    sheetRequest.DocTypeId,
                    sheetRequest.CreatorId,
                    sheetRequest.InspectorId,
                    sheetRequest.NormContrId);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            
            return Created($"sheets/{sheetModel.Id}", _mapper.Map<SheetResponse>(sheetModel));
        }

        [HttpPatch, Route("sheets/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(int id, [FromBody] SheetUpdateRequest sheetRequest)
        {
            // DEBUG
            // Log.Information(JsonSerializer.Serialize(markRequest));
            try
            {
                _service.Update(id, sheetRequest);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete, Route("sheets/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            try
            {
                _service.Delete(id);
                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }
    }
}
