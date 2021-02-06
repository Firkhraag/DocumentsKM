using System;
using System.Collections.Generic;
using System.Text.Json;
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
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _service;
        private readonly IPublisherService _publisherService;
        private readonly IMapper _mapper;

        public DepartmentsController(
            IDepartmentService departmentService,
            IPublisherService publisherService,
            IMapper mapper)
        {
            _service = departmentService;
            _publisherService = publisherService;
            _mapper = mapper;
        }

        [HttpGet, Route("departments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Department>> GetAlld()
        {
            var departments = _service.GetAll();
            return Ok(departments);
        }

        [HttpPost, Route("departments")]
        public ActionResult<Department> Create([FromBody] DepartmentRequest departmentRequest)
        {
            var department = _mapper.Map<Department>(departmentRequest);
            try
            {
                _service.Create(department);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

            try
            {
                IDictionary<string, object> d = new Dictionary<string, object>()
                {
                    { "entity", "department" },
                    { "method", "add" },
                };
                _publisherService.Publish(
                    JsonSerializer.Serialize(new DepartmentRabbitResponse(department)),
                    "personnel.department.add", d);
            }
            catch
            {
                // Log failure
            }
            
            return Created(
                $"departments/{department.Id}", department);
        }

        [HttpPatch, Route("departments/{id}")]
        public ActionResult Update(int id, [FromBody] DepartmentRequest departmentRequest)
        {
            try
            {
                _service.Update(id, departmentRequest);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

            // try
            // {
            //     IDictionary<string, object> d = new Dictionary<string, object>()
            //     {
            //         { "entity", "department" },
            //     };
            //     _publisherService.Publish(
            //         JsonSerializer.Serialize(new DepartmentRabbitResponse(department)),
            //         "personnel.department.update", d);
            // }
            // catch
            // {
            //     // Log failure
            // }

            return NoContent();
        }

        [HttpDelete, Route("departments/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _service.Delete(id);

                try
                {
                    IDictionary<string, object> d = new Dictionary<string, object>()
                    {
                        { "entity", "department" },
                        { "method", "delete" },
                    };
                    _publisherService.Publish(
                        JsonSerializer.Serialize(new { Id = id }),
                        "personnel.department.delete", d);
                }
                catch
                {
                    // Log failure
                }

                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }
    }
}
