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
    public class LinkedDocTypesController : ControllerBase
    {
        private readonly ILinkedDocTypeService _service;

        public LinkedDocTypesController(ILinkedDocTypeService linkedDocTypeService)
        {
            _service = linkedDocTypeService;
        }

        [HttpGet, Route("linked-doc-types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<LinkedDocType>> GetAll()
        {
            var linkedDocTypes = _service.GetAll();
            return Ok(linkedDocTypes);
        }
    }
}
