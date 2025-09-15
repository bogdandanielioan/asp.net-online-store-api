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

        private static DtoStudentView ToDto(Student s)
        {
            return new DtoStudentView
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                Age = s.Age,
                UpdateData = s.UpdateDate,
                StudentBooks = s.StudentBooks ?? new List<Book>(),
                MyCardNumber = s.CardNumber,
                MyCourses = new List<OnlineSchool.Courses.Dto.DtoCourseViewForStudents>(),
                Role = s.Role
            };
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

        public override async Task<ActionResult<DtoStudentView>> GetByName([FromQuery] string name)
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

        public override async Task<ActionResult<DtoStudentView>> GetById([FromQuery] string id)
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

        public override async Task<ActionResult<StudentCard>> GetStudentCard([FromQuery] string id)
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

        public override async Task<ActionResult<DtoStudentView>> CreateStudent(CreateRequestStudent request)
        {
            try
            {
                var student = await _commandService.Create(request);
                return Ok(ToDto(student));
            }
            catch (InvalidAge ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public override async Task<ActionResult<DtoStudentView>> UpdateStudent([FromQuery] string id, UpdateRequestStudent request)
        {
            try
            {
                var student = await _commandService.Update(id, request);
                return Ok(ToDto(student));
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

        public override async Task<ActionResult<DtoStudentView>> DeleteStudent([FromQuery] string id)
        {
            try
            {
                var student = await _commandService.Delete(id);
                return Ok(ToDto(student));
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        public override async Task<ActionResult<DtoStudentView>> CreateBookForStudent([FromQuery] string idStudent, BookCreateDTO request)
        {
            try
            {
                var student = await _commandService.CreateBookForStudent(idStudent, request);

                return Ok(ToDto(student));

            }catch(ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
            catch(InvalidName ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public override async Task<ActionResult<DtoStudentView>> UpdateBookForStudent([FromQuery] string idStudent, [FromQuery] string idBook, BookUpdateDTO request)
        {
            try
            {
                var student = await _commandService.UpdateBookForStudent(idStudent,idBook, request);

                return Ok(ToDto(student));

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

        public override async Task<ActionResult<DtoStudentView>> DeleteBookForStudent([FromQuery] string idStudent, [FromQuery] string idBook)
        {
            try
            {
                var student = await _commandService.DeleteBookForStudent(idStudent, idBook);

                return Ok(ToDto(student));

            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        public override async Task<ActionResult<DtoStudentView>> EnrollmentCourse([FromQuery] string idStudent, [FromQuery] string name)
        {
            try
            {
                Course course = await _queryServiceCourse.GetByName(name);
                if (course == null)
                {
                    return NotFound(Constants.NotFoundcourse);
                }

                var student = await _commandService.EnrollmentCourse(idStudent, course);
                return Ok(ToDto(student));
            }
            catch (NotFoundCourse ex)
            {
                return NotFound(ex.Message);
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

        public override async Task<ActionResult<DtoStudentView>> UnEnrollmentCourse([FromQuery] string idStudent, [FromQuery] string name)
        {
            try
            {

                Course course = await _queryServiceCourse.GetByName(name);
                if (course == null)
                {
                    return NotFound(Constants.NotFoundcourse);
                }

                var student = await _commandService.UnEnrollmentCourse(idStudent, course);

                return Ok(ToDto(student));

            }
            catch(NotFoundCourse ex)
            {
                return NotFound(ex.Message);
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
    }
}
