using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineSchool.StudentCards.Models;

namespace OnlineSchool.StudentCards.Controllers.interfaces
{

    [ApiController]
    [Route("api/v1/studentcards/")]
    [Route("api/v1/ControllerStudentCards/")]
    public abstract class ControllerAPIStudentCards : ControllerBase
    {

        [HttpGet("all")]
        [Authorize(Policy = "Read")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<StudentCard>))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<List<StudentCard>>> GetStudentCards();

        [HttpGet("findById")]
        [Authorize(Policy = "Read")]
        [ProducesResponseType(statusCode: 200, type: typeof(StudentCard))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<StudentCard>> GetById([FromQuery] string id);

        [HttpGet("findByName")]
        [Authorize(Policy = "Read")]
        [ProducesResponseType(statusCode: 200, type: typeof(StudentCard))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<StudentCard>> GetByName([FromQuery] string name);

    }
}
