using System.Collections.Generic;
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
    public class ProfileClassesController : ControllerBase
    {
        private readonly IProfileClassService _service;

        public ProfileClassesController(IProfileClassService profileClassService)
        {
            _service = profileClassService;
        }

        [HttpGet, Route("profile-classes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ProfileClass>> GetAll()
        {
            var profileClasses = _service.GetAll();
            return Ok(profileClasses);
        }
    }
}
