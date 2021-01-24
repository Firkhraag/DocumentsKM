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
    public class ConstructionSubtypesController : ControllerBase
    {
        private readonly IConstructionSubtypeService _service;
        private readonly IMapper _mapper;

        public ConstructionSubtypesController(
            IConstructionSubtypeService constructionSubtypeService,
            IMapper mapper)
        {
            _service = constructionSubtypeService;
            _mapper = mapper;
        }

        [HttpGet, Route("construction-types/{typeId}/construction-subtypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ConstructionSubtypeResponse>> GetAllByTypeId(
            int typeId)
        {
            var constructionSubtype = _service.GetAllByTypeId(typeId);
            return Ok(_mapper.Map<IEnumerable<ConstructionSubtypeResponse>>(
                constructionSubtype));
        }
    }
}
