using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Enrolments.Dto;
using OnlineSchool.Enrolments.Models;

namespace OnlineSchool.Enrolments.Controllers.interfaces
{

    [ApiController]
    [Route("api/v1/[controller]/")]
    public abstract class ControllerAPIEnrolment : ControllerBase
    {

        [HttpGet("all")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<Enrolment>))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<List<Enrolment>>> GetEnrolments();

        [HttpGet("findById")]
        [ProducesResponseType(statusCode: 200, type: typeof(Enrolment))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<Enrolment>> GetById([FromQuery] int id);

    }
}
