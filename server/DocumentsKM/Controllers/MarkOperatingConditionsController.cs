using System;
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
    public class MarkOperatingConditionsController : ControllerBase
    {
        private readonly IMarkOperatingConditionsService _service;
        private readonly IEnvAggressivenessService _envAggressivenessService;
        private readonly IOperatingAreaService _operatingAreaService;
        private readonly IGasGroupService _gasGroupService;
        private readonly IConstructionMaterialService _constructionMaterialService;
        private readonly IPaintworkTypeService _paintworkTypeService;
        private readonly IHighTensileBoltsTypeService _highTensileBoltsTypeService;
        private readonly IMapper _mapper;

        public MarkOperatingConditionsController(
            IMarkOperatingConditionsService markOperatingConditionsService,
            IEnvAggressivenessService envAggressivenessService,
            IOperatingAreaService operatingAreaService,
            IGasGroupService gasGroupService,
            IConstructionMaterialService constructionMaterialService,
            IPaintworkTypeService paintworkTypeService,
            IHighTensileBoltsTypeService highTensileBoltsTypeService,
            IMapper mapper)
        {
            _service = markOperatingConditionsService;
            _envAggressivenessService = envAggressivenessService;
            _operatingAreaService = operatingAreaService;
            _gasGroupService = gasGroupService;
            _constructionMaterialService = constructionMaterialService;
            _paintworkTypeService = paintworkTypeService;
            _highTensileBoltsTypeService = highTensileBoltsTypeService;
            _mapper = mapper;
        }

        [HttpGet, Route("operating-conditions/data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<OperatingConditionsDataResponse> GetData()
        {
            var envAggressiveness = _envAggressivenessService.GetAll();
            var operatingAreas = _operatingAreaService.GetAll();
            var gasGroups = _gasGroupService.GetAll();
            var constructionMaterials = _constructionMaterialService.GetAll();
            var paintworkTypes = _paintworkTypeService.GetAll();
            var highTensileBoltsTypes = _highTensileBoltsTypeService.GetAll();
            return Ok(new OperatingConditionsDataResponse(
                envAggressiveness, operatingAreas, gasGroups,
                constructionMaterials, paintworkTypes, highTensileBoltsTypes
            ));
        }

        [HttpGet, Route("marks/{markId}/mark-operating-conditions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<MarkOperatingConditionsResponse> GetByMarkId(int markId)
        {
            var markOperatingConditions = _service.GetByMarkId(markId);
            if (markOperatingConditions != null)
                return Ok(_mapper.Map<MarkOperatingConditionsResponse>(markOperatingConditions));
            return NotFound();
        }

        [HttpPost, Route("marks/{markId}/mark-operating-conditions")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult Create(
            int markId,
            [FromBody] MarkOperatingConditionsCreateRequest markOperatingConditionsRequest)
        {
            var markOperatingConditionsModel = _mapper.Map<MarkOperatingConditions>(
                markOperatingConditionsRequest);
            try
            {
                _service.Create(
                    markOperatingConditionsModel,
                    markId,
                    markOperatingConditionsRequest.EnvAggressivenessId,
                    markOperatingConditionsRequest.OperatingAreaId,
                    markOperatingConditionsRequest.GasGroupId,
                    markOperatingConditionsRequest.ConstructionMaterialId,
                    markOperatingConditionsRequest.PaintworkTypeId,
                    markOperatingConditionsRequest.HighTensileBoltsTypeId);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (ConflictException)
            {
                return Conflict();
            }
            return Created(
                $"marks/{markId}/mark-operating-conditions", null);
        }

        [HttpPatch, Route("marks/{markId}/mark-operating-conditions")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(
            int markId,
            [FromBody] MarkOperatingConditionsUpdateRequest markOperatingConditionsRequest)
        {
            if (!markOperatingConditionsRequest.Validate())
                return BadRequest();
            try
            {
                _service.Update(markId, markOperatingConditionsRequest);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
