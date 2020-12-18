using System.Threading.Tasks;
using DocumentsKM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsKM.Controllers
{
    [Route("api")]
    [Authorize]
    [ApiController]
    public class GeneralDataDocController : ControllerBase
    {
        private readonly IGeneralDataDocService _service;

        public GeneralDataDocController(
            IGeneralDataDocService generalDataDocService)
        {
            _service = generalDataDocService;
        }

        [HttpGet, Route("marks/{markId}/general-data")]
        public async Task<IActionResult> GetGeneralDataDocument(int markId)
        {
            var file = await _service.GetDocByMarkId(markId);
            return File(file, "application/x-tex", "Общие данные.tex");
        }
    }
}
