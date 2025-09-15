using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Books.Models;
using OnlineSchool.Enrolments.Dto;
using OnlineSchool.StudentCards.Models;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Models;
using System;

namespace OnlineSchool.Students.Controllers.interfaces
{
    [ApiController]
    [Route("api/v1/students/")]
    [Route("api/v1/ControllerStudent/")]
    public abstract class ControllerAPIStudent : ControllerBase
    {

        [HttpGet("all")]
        [Authorize(Policy = "read:student")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<Student>))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<List<DtoStudentView>>> GetStudents();

        [HttpGet("studentCard")]
        [Authorize(Policy = "read:student")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<Student>))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<StudentCard>> GetStudentCard([FromQuery]string id);

        [HttpGet("findById")]
        [Authorize(Policy = "read:student")]
        [ProducesResponseType(statusCode: 200, type: typeof(Student))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<DtoStudentView>> GetById([FromQuery] string id);

        [HttpGet("findByName")]
        [Authorize(Policy = "read:student")]
        [ProducesResponseType(statusCode: 200, type: typeof(DtoStudentView))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<DtoStudentView>> GetByName([FromQuery] string name);

        [HttpPost("createStudent")]
        [AllowAnonymous]
        [ProducesResponseType(statusCode: 201, type: typeof(DtoStudentView))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<DtoStudentView>> CreateStudent(CreateRequestStudent request);

        [HttpPut("updateStudent")]
        [Authorize(Policy = "write:student")]
        [ProducesResponseType(statusCode: 200, type: typeof(DtoStudentView))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<DtoStudentView>> UpdateStudent([FromQuery] string id, UpdateRequestStudent request);

        [HttpDelete("deleteStudent")]
        [Authorize(Policy = "write:student")]
        [ProducesResponseType(statusCode: 200, type: typeof(DtoStudentView))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<DtoStudentView>> DeleteStudent([FromQuery] string id);


        [HttpPost("createBookForStudent")]
        [Authorize(Policy = "write:student")]
        [ProducesResponseType(statusCode: 201, type: typeof(DtoStudentView))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<DtoStudentView>> CreateBookForStudent([FromQuery]string idStudent,BookCreateDTO request);

        [HttpPut("updateBookForStudent")]
        [Authorize(Policy = "write:student")]
        [ProducesResponseType(statusCode: 200, type: typeof(DtoStudentView))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<DtoStudentView>> UpdateBookForStudent([FromQuery] string idStudent, [FromQuery] string idBook, BookUpdateDTO request);

        [HttpDelete("deleteBookForStudent")]
        [Authorize(Policy = "write:student")]
        [ProducesResponseType(statusCode: 200, type: typeof(DtoStudentView))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<DtoStudentView>> DeleteBookForStudent([FromQuery] string idStudent, [FromQuery] string idBook);

        [HttpPost("enrollmentCourse")]
        [Authorize(Policy = "write:student")]
        [ProducesResponseType(statusCode: 201, type: typeof(DtoStudentView))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<DtoStudentView>> EnrollmentCourse([FromQuery] string idStudent,[FromQuery]string name);

        [HttpDelete("unenrollmentCourse")]
        [Authorize(Policy = "write:student")]
        [ProducesResponseType(statusCode: 200, type: typeof(DtoStudentView))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<DtoStudentView>> UnEnrollmentCourse([FromQuery] string idStudent, [FromQuery] string name);


    }
}
