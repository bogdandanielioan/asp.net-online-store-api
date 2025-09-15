using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Courses.Controllers.interfaces;
using OnlineSchool.Courses.Dto;
using OnlineSchool.Courses.Models;
using OnlineSchool.Courses.Services.interfaces;
using OnlineSchool.System.Exceptions;

namespace OnlineSchool.Courses.Controllers
{
    public class ControllerCourse : ControllerAPICourse
    {
        private IQueryServiceCourse _queryService;
        private ICommandServiceCourse _commandService;

        public ControllerCourse(IQueryServiceCourse queryService, ICommandServiceCourse commandService)
        {
            _queryService = queryService;
            _commandService = commandService;
        }

        public override async Task<ActionResult<List<DtoCourseView>>> GetCourses()
        {
            try
            {
                var courses = await _queryService.GetAll();

                return Ok(courses);

            }
            catch (ItemsDoNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        public override async Task<ActionResult<DtoCourseView>> GetByName([FromQuery] string name)
        {

            try
            {
                var course = await _queryService.GetByNameAsync(name);
                return Ok(course);
            }
            catch (NotFoundCourse ex)
            {
                return NotFound(ex.Message);
            }

        }

        public override async Task<ActionResult<DtoCourseView>> GetById([FromQuery] int id)
        {

            try
            {
                var course = await _queryService.GetById(id);
                return Ok(course);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }

        }

        public override async Task<ActionResult<Course>> CreateCourse(CreateRequestCourse request)
        {
            try
            {
                var course = await _commandService.Create(request);
                return Ok(course);
            }
            catch (InvalidName ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public override async Task<ActionResult<Course>> UpdateCourse([FromQuery] int id, UpdateRequestCourse request)
        {
            try
            {
                var course = await _commandService.Update(id, request);
                return Ok(course);
            }
            catch (InvalidName ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        public override async Task<ActionResult<Course>> DeleteCourse([FromQuery] int id)
        {
            try
            {
                var course = await _commandService.Delete(id);
                return Ok(course);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
