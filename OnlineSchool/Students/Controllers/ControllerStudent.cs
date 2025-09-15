using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Books.Models;
using OnlineSchool.Books.Services.interfaces;
using OnlineSchool.Courses.Models;
using OnlineSchool.Courses.Services.interfaces;
using OnlineSchool.Enrolments.Dto;
using OnlineSchool.StudentCards.Models;
using OnlineSchool.Students.Controllers.interfaces;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Models;
using OnlineSchool.Students.Services.interfaces;
using OnlineSchool.System.Constants;
using OnlineSchool.System.Exceptions;
using System;
using System.Configuration;
using System.Xml.Linq;

namespace OnlineSchool.Students.Controllers
{
    public class ControllerStudent : ControllerAPIStudent
    {
        private IQueryServiceStudent _queryService;
        private ICommandServiceStudent _commandService;
        private IQueryServiceCourse _queryServiceCourse;

        public ControllerStudent(IQueryServiceStudent queryService, ICommandServiceStudent commandService,IQueryServiceCourse queryServiceCourse) 
        {
            _queryService = queryService;
            _commandService = commandService;
            _queryServiceCourse = queryServiceCourse;
        }

        public override async Task<ActionResult<List<DtoStudentView>>> GetStudents()
        {
            try
            {
                var students = await _queryService.GetAll();

                return Ok(students);

            }
            catch (ItemsDoNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        public override async Task<ActionResult<Student>> GetByName([FromQuery] string name)
        {

            try
            {
                var student = await _queryService.GetByNameAsync(name);
                return Ok(student);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }

        }

        public override async Task<ActionResult<DtoStudentView>> GetById([FromQuery] int id)
        {

            try
            {
                var student = await _queryService.GetById(id);
                return Ok(student);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }

        }

        public override async Task<ActionResult<StudentCard>> GetStudentCard([FromQuery] int id)
        {

            try
            {
                var student = await _queryService.CardById(id);
                return Ok(student);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }

        }

        public override async Task<ActionResult<Student>> CreateStudent(CreateRequestStudent request)
        {
            try
            {
                var student = await _commandService.Create(request);
                return Ok(student);
            }
            catch (InvalidAge ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public override async Task<ActionResult<Student>> UpdateStudent([FromQuery] int id, UpdateRequestStudent request)
        {
            try
            {
                var student = await _commandService.Update(id, request);
                return Ok(student);
            }
            catch (InvalidAge ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        public override async Task<ActionResult<Student>> DeleteStudent([FromQuery] int id)
        {
            try
            {
                var student = await _commandService.Delete(id);
                return Ok(student);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        public override async Task<ActionResult<Student>> CreateBookForStudent([FromQuery] int idStudent, BookCreateDTO request)
        {
            try
            {
                var student = await _commandService.CreateBookForStudent(idStudent, request);

                return Ok(student);

            }catch(ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
            catch(InvalidName ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public override async Task<ActionResult<Student>> UpdateBookForStudent([FromQuery] int idStudent, [FromQuery] int idBook, BookUpdateDTO request)
        {
            try
            {
                var student = await _commandService.UpdateBookForStudent(idStudent,idBook, request);

                return Ok(student);

            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidName ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public override async Task<ActionResult<Student>> DeleteBookForStudent([FromQuery] int idStudent, [FromQuery] int idBook)
        {
            try
            {
                var student = await _commandService.DeleteBookForStudent(idStudent, idBook);

                return Ok(student);

            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        public override async Task<ActionResult<Student>> EnrollmentCourse([FromQuery] int idStudent, [FromQuery] string name)
        {

            try
            {
                Course course = await _queryServiceCourse.GetByName(name);
                if (course == null)
                {
                    throw new NotFoundCourse(Constants.NotFoundcourse);
                }
                else
                {
                    var student = await _commandService.EnrollmentCourse(idStudent, course);

                    return Ok(student);
                }

            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidCourse ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public override async Task<ActionResult<Student>> UnEnrollmentCourse([FromQuery] int idStudent, [FromQuery] string nameCourse)
        {
            try
            {

                Course course = await _queryServiceCourse.GetByName(nameCourse);

                var student = await _commandService.UnEnrollmentCourse(idStudent, course);

                return Ok(student);

            }
            catch(NotFoundCourse ex)
            {
                return NotFound(ex.Message);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
            
        }
    }
}
