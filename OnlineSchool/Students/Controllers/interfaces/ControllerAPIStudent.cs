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
    [Route("api/v1/[controller]/")]
    public abstract class ControllerAPIStudent : ControllerBase
    {

        [HttpGet("all")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<Student>))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<List<DtoStudentView>>> GetStudents();

        [HttpGet("studentCard")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<Student>))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<StudentCard>> GetStudentCard([FromQuery]int id);

        [HttpGet("findById")]
        [ProducesResponseType(statusCode: 200, type: typeof(Student))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<DtoStudentView>> GetById([FromQuery] int id);

        [HttpGet("findByName")]
        [ProducesResponseType(statusCode: 200, type: typeof(Student))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<Student>> GetByName([FromQuery] string name);

        [HttpPost("createStudent")]
        [ProducesResponseType(statusCode: 201, type: typeof(Student))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<Student>> CreateStudent(CreateRequestStudent request);

        [HttpPut("updateStudent")]
        [ProducesResponseType(statusCode: 200, type: typeof(Student))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<Student>> UpdateStudent([FromQuery] int id, UpdateRequestStudent request);

        [HttpDelete("deleteStudent")]
        [ProducesResponseType(statusCode: 200, type: typeof(Student))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<Student>> DeleteStudent([FromQuery] int id);


        [HttpPost("createBookForStudent")]
        [ProducesResponseType(statusCode: 201, type: typeof(Student))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<Student>> CreateBookForStudent([FromQuery]int idStudent,BookCreateDTO request);

        [HttpPut("updateBookForStudent")]
        [ProducesResponseType(statusCode: 200, type: typeof(Student))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<Student>> UpdateBookForStudent([FromQuery] int idStudent, [FromQuery] int idBook, BookUpdateDTO request);

        [HttpDelete("deleteBookForStudent")]
        [ProducesResponseType(statusCode: 200, type: typeof(Student))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<Student>> DeleteBookForStudent([FromQuery] int idStudent, [FromQuery] int idBook);

        [HttpPost("enrollmentCourse")]
        [ProducesResponseType(statusCode: 201, type: typeof(Student))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<Student>> EnrollmentCourse([FromQuery] int idStudent,[FromQuery]string name);

        [HttpDelete("unenrollmentCourse")]
        [ProducesResponseType(statusCode: 200, type: typeof(Student))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<Student>> UnEnrollmentCourse([FromQuery] int idStudent, [FromQuery] string name);


    }
}
