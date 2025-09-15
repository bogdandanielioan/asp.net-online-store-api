using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Courses.Dto;
using OnlineSchool.Courses.Models;

namespace OnlineSchool.Courses.Controllers.interfaces
{

    [ApiController]
    [Route("api/v1/courses/")]
    [Route("api/v1/ControllerCourse/")]
    public abstract class ControllerAPICourse: ControllerBase
    {

        [HttpGet("all")]
        [Authorize(Policy = "Read")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<Course>))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<List<DtoCourseView>>> GetCourses();

        [HttpGet("findById")]
        [Authorize(Policy = "Read")]
        [ProducesResponseType(statusCode: 200, type: typeof(Course))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<DtoCourseView>> GetById([FromQuery] string id);

        [HttpGet("findByName")]
        [Authorize(Policy = "Read")]
        [ProducesResponseType(statusCode: 200, type: typeof(Course))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<DtoCourseView>> GetByName([FromQuery] string name);

        [HttpPost("createCourse")]
        [Authorize(Policy = "Write")]
        [ProducesResponseType(statusCode: 201, type: typeof(Course))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<Course>> CreateCourse(CreateRequestCourse request);

        [HttpPut("updateCourse")]
        [Authorize(Policy = "Write")]
        [ProducesResponseType(statusCode: 200, type: typeof(Course))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<Course>> UpdateCourse([FromQuery] string id, UpdateRequestCourse request);

        [HttpDelete("deleteCourse")]
        [Authorize(Policy = "Write")]
        [ProducesResponseType(statusCode: 200, type: typeof(Course))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<Course>> DeleteCourse([FromQuery] string id);


    }
}
