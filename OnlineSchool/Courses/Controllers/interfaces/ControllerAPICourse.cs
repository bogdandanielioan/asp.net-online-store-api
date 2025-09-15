using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Courses.Dto;
using OnlineSchool.Courses.Models;

namespace OnlineSchool.Courses.Controllers.interfaces
{

    [ApiController]
    [Route("api/v1/[controller]/")]
    public abstract class ControllerAPICourse: ControllerBase
    {

        [HttpGet("all")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<Course>))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<List<DtoCourseView>>> GetCourses();

        [HttpGet("findById")]
        [ProducesResponseType(statusCode: 200, type: typeof(Course))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<DtoCourseView>> GetById([FromQuery] int id);

        [HttpGet("findByName")]
        [ProducesResponseType(statusCode: 200, type: typeof(Course))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<DtoCourseView>> GetByName([FromQuery] string name);

        [HttpPost("createCourse")]
        [ProducesResponseType(statusCode: 201, type: typeof(Course))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<Course>> CreateCourse(CreateRequestCourse request);

        [HttpPut("updateCourse")]
        [ProducesResponseType(statusCode: 200, type: typeof(Course))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<Course>> UpdateCourse([FromQuery] int id, UpdateRequestCourse request);

        [HttpDelete("deleteCourse")]
        [ProducesResponseType(statusCode: 200, type: typeof(Course))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<Course>> DeleteCourse([FromQuery] int id);


    }
}
