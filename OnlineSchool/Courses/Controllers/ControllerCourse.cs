using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Courses.Controllers.interfaces;
using OnlineSchool.Courses.Dto;
using OnlineSchool.Courses.Features.Commands;
using OnlineSchool.Courses.Features.Queries;
using OnlineSchool.Courses.Models;
using OnlineSchool.System.Exceptions;

namespace OnlineSchool.Courses.Controllers;

public class ControllerCourse : ControllerAPICourse
{
    private readonly IMediator _mediator;

    public ControllerCourse(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<ActionResult<List<DtoCourseView>>> GetCourses()
    {
        try
        {
            var courses = await _mediator.Send(new GetCoursesQuery());
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
            var course = await _mediator.Send(new GetCourseByNameQuery(name));
            return Ok(course);
        }
        catch (NotFoundCourse ex)
        {
            return NotFound(ex.Message);
        }
    }

    public override async Task<ActionResult<DtoCourseView>> GetById([FromQuery] string id)
    {
        try
        {
            var course = await _mediator.Send(new GetCourseByIdQuery(id));
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
            var course = await _mediator.Send(new CreateCourseCommand(request));
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

    public override async Task<ActionResult<Course>> UpdateCourse([FromQuery] string id, UpdateRequestCourse request)
    {
        try
        {
            var course = await _mediator.Send(new UpdateCourseCommand(id, request));
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

    public override async Task<ActionResult<Course>> DeleteCourse([FromQuery] string id)
    {
        try
        {
            var course = await _mediator.Send(new DeleteCourseCommand(id));
            return Ok(course);
        }
        catch (ItemDoesNotExist ex)
        {
            return NotFound(ex.Message);
        }
    }
}
