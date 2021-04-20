using System;
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
    public class GeneralDataDocController : ControllerBase
    {
        private readonly IDocumentService _service;
        private readonly IMarkService _markService;

        public GeneralDataDocController(
            IDocumentService documentService,
            IMarkService markService)
        {
            _service = documentService;
            _markService = markService;
        }

        [HttpGet, Route("marks/{markId}/general-data-document")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetGeneralDataDocument(int markId)
        {
            try
            {
                var file = _service.GetGeneralDataDocument(markId);
                return File(
                    file,
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "Общие данные.docx");
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [HttpGet, Route("users/{userId}/marks/{markId}/spec-document")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetSpecificationDocument(int markId, int userId)
        {
            try
            {
                var file = _service.GetSpecificationDocument(markId, userId);
                return File(
                    file,
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "Спецификация металла.docx");
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [HttpGet, Route("users/{userId}/marks/{markId}/construction-document")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetConstructionDocument(int markId, int userId)
        {
            try
            {
                var file = _service.GetConstructionDocument(markId, userId);
                return File(
                    file,
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "Ведомость металлоконструкций.docx");
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [HttpGet, Route("marks/{markId}/bolt-document")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetBoltDocument(int markId)
        {
            try
            {
                var file = _service.GetBoltDocument(markId);
                return File(
                    file,
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "Ведомость болтов.docx");
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [HttpGet, Route("marks/{markId}/estimate-task-document")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEstimateTaskDocument(int markId)
        {
            try
            {
                var file = _service.GetEstimateTaskDocument(markId);
                return File(
                    file,
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "Задание на смету.docx");
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [HttpPost, Route("marks/{markId}/project-reg-document")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetProjectRegistrationDocument(
            int markId, [FromBody] MarkIssueDateRequest issueDateRequest)
        {
            try
            {
                var mark = _markService.GetById(markId);
                if (issueDateRequest.IssueDate != null)
                    _markService.UpdateIssueDate(
                        mark, issueDateRequest.IssueDate.GetValueOrDefault());
                else
                    if (mark.IssueDate == null)
                        _markService.UpdateIssueDate(mark);
                var file = _service.GetProjectRegistrationDocument(markId);
                return File(
                    file,
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "Лист регистрации проекта.docx");
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [HttpGet, Route("marks/{markId}/estimation-document-title")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEstimationDocumentTitle(int markId)
        {
            try
            {
                var file = _service.GetEstimationDocumentTitle(markId);
                return File(
                    file,
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "Комплект для расчета, титульник.docx");
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [HttpGet, Route("marks/{markId}/estimation-document-pages/{numOfPages}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEstimationDocumentPages(int markId, int numOfPages)
        {
            try
            {
                var file = _service.GetEstimationDocumentPages(markId, numOfPages);
                return File(
                    file,
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "Комплект для расчета.docx");
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }
    }
}
