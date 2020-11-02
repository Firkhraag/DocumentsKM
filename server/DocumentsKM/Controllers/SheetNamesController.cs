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

// Только листы основного комплекта?
namespace DocumentsKM.Controllers
{
    [Route("api")]
    [Authorize]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class SheetNamesController : ControllerBase
    {
        private readonly ISheetNameService _service;

        public SheetNamesController(ISheetNameService sheetNameService)
        {
            _service = sheetNameService;
        }

        [HttpGet, Route("sheet-names")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<SheetName>> GetAll()
        {
            var sheetNames = _service.GetAll();
            return Ok(sheetNames);
        }
    }
}
