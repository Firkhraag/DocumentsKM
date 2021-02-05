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
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileService _service;

        public ProfilesController(IProfileService profileService)
        {
            _service = profileService;
        }

        [HttpGet, Route("profile-classes/{profileClassId}/profiles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Profile>> GetAllByProfileClass(int profileClassId)
        {
            var profiles = _service.GetAllByProfileClass(profileClassId);
            return Ok(profiles);
        }
    }
}
