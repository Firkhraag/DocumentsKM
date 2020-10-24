using System.Collections.Generic;
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

        [HttpGet, Route("marks/{markId}/sheets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<SheetResponse>> GetAllByMarkId(int markId)
        {
            var sheets = _service.GetAllByMarkId(markId);
            return Ok(_mapper.Map<IEnumerable<SheetResponse>>(sheets));
        }
    }
}
