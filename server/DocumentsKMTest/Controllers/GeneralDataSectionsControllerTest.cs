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
    public class GeneralDataSectionsController : ControllerBase
    {
        private readonly IGeneralDataSectionService _service;
        private readonly IMapper _mapper;

        public GeneralDataSectionsController(
            IGeneralDataSectionService generalDataSectionService,
            IMapper mapper)
        {
            _service = generalDataSectionService;
            _mapper = mapper;
        }

        [HttpGet, Route("general-data-sections")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<GeneralDataSection>> GetAll()
        {
            var sections = _service.GetAll();
            return Ok(sections);
        }
    }
}
