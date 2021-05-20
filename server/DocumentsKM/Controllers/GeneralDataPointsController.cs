using System.Collections.Generic;
using AutoMapper;
using DocumentsKM.Dtos;
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
    public class GeneralDataPointsController : ControllerBase
    {
        private readonly IGeneralDataPointService _service;
        private readonly IMapper _mapper;

        public GeneralDataPointsController(
            IGeneralDataPointService generalDataPointService,
            IMapper mapper)
        {
            _service = generalDataPointService;
            _mapper = mapper;
        }

        [HttpGet,
            Route("general-data-sections/{sectionId}/general-data-points")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<GeneralDataPointResponse>> GetAllBySectionId(int sectionId)
        {
            var points = _service.GetAllBySectionId(sectionId);
            return Ok(_mapper.Map<IEnumerable<GeneralDataPointResponse>>(points));
        }

        [HttpGet,
            Route("general-data-section-names/{sectionName}/general-data-points")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<GeneralDataPointResponse>> GetAllBySectionName(string sectionName)
        {
            var points = _service.GetAllBySectionName(sectionName);
            return Ok(_mapper.Map<IEnumerable<GeneralDataPointResponse>>(points));
        }
    }
}
