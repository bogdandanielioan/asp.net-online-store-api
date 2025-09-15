using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Teachers.Dto;

namespace OnlineSchool.Teachers.Controllers.interfaces
{
    [ApiController]
    [Route("api/v1/teachers/")]
    [Route("api/v1/ControllerTeacher/")]
    public abstract class ControllerAPITeacher : ControllerBase
    {
        [HttpGet("all")]
        [Authorize(Policy = "Read")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<DtoTeacherView>))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<List<DtoTeacherView>>> GetTeachers();

        [HttpGet("findById")]
        [Authorize(Policy = "Read")]
        [ProducesResponseType(statusCode: 200, type: typeof(DtoTeacherView))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<DtoTeacherView>> GetById([FromQuery] string id);

        [HttpGet("findByEmail")]
        [Authorize(Policy = "Read")]
        [ProducesResponseType(statusCode: 200, type: typeof(DtoTeacherView))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<DtoTeacherView>> GetByEmail([FromQuery] string email);

        [HttpPost("create")]
        [AllowAnonymous]
        [ProducesResponseType(statusCode: 201, type: typeof(DtoTeacherView))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<DtoTeacherView>> Create([FromBody] TeacherCreateRequest request);

        [HttpPut("update")]
        [Authorize(Policy = "Write")]
        [ProducesResponseType(statusCode: 200, type: typeof(DtoTeacherView))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<DtoTeacherView>> Update([FromQuery] string id, [FromBody] TeacherUpdateRequest request);

        [HttpDelete("delete")]
        [Authorize(Policy = "Write")]
        [ProducesResponseType(statusCode: 200, type: typeof(DtoTeacherView))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<DtoTeacherView>> Delete([FromQuery] string id);
    }
}
