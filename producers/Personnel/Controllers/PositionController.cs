// using System;
// using System.Collections.Generic;
// using System.Text.Json;
// using AutoMapper;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Personnel.Dtos;
// using Personnel.Models;
// using Personnel.Services;

// namespace DocumentsKM.Controllers
// {
//     [Route("api")]
//     [ApiController]
//     [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//     public class PositionsController : ControllerBase
//     {
//         private readonly IPositionService _service;
//         private readonly IPublisherService _publisherService;
//         private readonly IMapper _mapper;

//         public PositionsController(
//             IPositionService positionService,
//             IPublisherService publisherService,
//             IMapper mapper)
//         {
//             _service = positionService;
//             _publisherService = publisherService;
//             _mapper = mapper;
//         }

//         [HttpGet, Route("positions")]
//         [ProducesResponseType(StatusCodes.Status200OK)]
//         public ActionResult<IEnumerable<Position>> GetAlld()
//         {
//             var positions = _service.GetAll();
//             return Ok(positions);
//         }

//         [HttpPost, Route("positions")]
//         public ActionResult<Position> Create([FromBody] PositionRequest positionRequest)
//         {
//             var position = _mapper.Map<Position>(positionRequest);
//             try
//             {
//                 _service.Create(position);
//             }
//             catch (ArgumentNullException)
//             {
//                 return NotFound();
//             }

//             try
//             {
//                 IDictionary<string, object> d = new Dictionary<string, object>()
//                 {
//                     { "entity", "position" },
//                 };
//                 _publisherService.Publish(
//                     JsonSerializer.Serialize(new PositionRabbitResponse(position)),
//                     "personnel.position.add", d);
//             }
//             catch
//             {
//                 // Log failure
//             }
            
//             return Created(
//                 $"positions/{position.Id}", position);
//         }

//         [HttpPatch, Route("positions/{id}")]
//         public ActionResult Update(int id, [FromBody] PositionRequest positionRequest)
//         {
//             try
//             {
//                 _service.Update(id, positionRequest);
//             }
//             catch (ArgumentNullException)
//             {
//                 return NotFound();
//             }
//             return NoContent();
//         }

//         [HttpDelete, Route("positions/{id}")]
//         public ActionResult Delete(int id)
//         {
//             try
//             {
//                 _service.Delete(id);

//                 try
//                 {
//                     IDictionary<string, object> d = new Dictionary<string, object>()
//                     {
//                         { "entity", "position" },
//                     };
//                     _publisherService.Publish(
//                         JsonSerializer.Serialize(new { Id = id }),
//                         "personnel.position.delete", d);
//                 }
//                 catch
//                 {
//                     // Log failure
//                 }

//                 return NoContent();
//             }
//             catch (ArgumentNullException)
//             {
//                 return NotFound();
//             }
//         }
//     }
// }
