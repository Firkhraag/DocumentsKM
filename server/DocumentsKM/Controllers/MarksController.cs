using System.Collections.Generic;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsKM.Controllers
{
    [Route("api/marks")]
    [ApiController]
    public class MarksController : ControllerBase
    {
        private readonly IMarkRepo _repository;

        public MarksController(IMarkRepo repo)
        {
            _repository = repo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Mark>> GetAllMarks()
        {
            var marks = _repository.GetAllMarks();
            return Ok(marks);
        }

        [HttpGet("{id}")]
        public ActionResult<Mark> GetMarkById(int id)
        {
            var mark = _repository.GetMarkById(id);
            return Ok(mark);
        }
    }
}