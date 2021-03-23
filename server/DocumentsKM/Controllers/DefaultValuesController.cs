using System;
using System.Collections.Generic;
using System.Net.Mime;
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
    public class DefaultValuesController : ControllerBase
    {
        private readonly IDefaultValuesService _service;

        public DefaultValuesController(
            IDefaultValuesService defaultValuesService)
        {
            _service = defaultValuesService;
        }

        [HttpGet, Route("users/{userId}/default-values")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<DefaultValues>> GetByUserId(int userId)
        {
            var defaultValues = _service.GetByUserId(userId);
            return Ok(defaultValues);
        }

        [HttpPatch, Route("users/{userId}/default-values")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(
            int userId,
            [FromBody] DefaultValuesUpdateRequest defaultValuesRequest)
        {
            if (!defaultValuesRequest.Validate())
                return BadRequest();
            try
            {
                _service.Update(userId, defaultValuesRequest);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
