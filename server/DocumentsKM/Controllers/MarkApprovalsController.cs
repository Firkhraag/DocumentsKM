using System;
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
    public class MarkApprovalsController : ControllerBase
    {
        private readonly IMarkApprovalService _service;
        private readonly IMapper _mapper;

        public MarkApprovalsController(
            IMarkApprovalService markService,
            IMapper mapper
        )
        {
            _service = markService;
            _mapper = mapper;
        }

        [HttpGet, Route("marks/{markId}/approvals")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EmployeeDepartmentResponse>> GetAllByMarkId(
            int markId)
        {
            var employees = _service.GetAllEmployeesByMarkId(markId);
            return Ok(_mapper.Map<IEnumerable<EmployeeDepartmentResponse>>(employees));
        }

        [HttpPatch, Route("marks/{markId}/approvals")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(int markId, [FromBody] List<int> employeeIds)
        {
            try
            {
                _service.Update(markId, employeeIds);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
