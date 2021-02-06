using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Personnel.Dtos;
using Personnel.Models;
using Personnel.Services;

namespace DocumentsKM.Controllers
{
    [Route("api")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _service;
        // private readonly IPublisherService _publisherService;
        private readonly IMapper _mapper;

        public EmployeesController(
            IEmployeeService employeeService,
            // IPublisherService publisherService,
            IMapper mapper)
        {
            _service = employeeService;
            // _publisherService = publisherService;
            _mapper = mapper;
        }

        [HttpGet, Route("departments/{departmentId}/employees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Employee>> GetAllByDepartmentId(int departmentId)
        {
            var employees = _service.GetAllByDepartmentId(departmentId);
            return Ok(employees);
        }

        [HttpPost, Route("departments/{departmentId}/positions/{positionId}/employees")]
        public ActionResult<Employee> Create(
            int departmentId, int positionId, [FromBody] EmployeeRequest employeeRequest)
        {
            var employee = _mapper.Map<Employee>(employeeRequest);
            try
            {
                _service.Create(
                    employee,
                    departmentId,
                    positionId);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

            // _publisherService.Publish(JsonSerializer.Serialize(employee), "reporty.employee.add", null);
            
            return Created(
                $"employees/{employee.Id}", employee);
        }

        [HttpPatch, Route("employees/{id}")]
        public ActionResult Update(int id, [FromBody] EmployeeRequest employeeRequest)
        {
            try
            {
                _service.Update(id, employeeRequest);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete, Route("employees/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _service.Delete(id);
                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }
    }
}
