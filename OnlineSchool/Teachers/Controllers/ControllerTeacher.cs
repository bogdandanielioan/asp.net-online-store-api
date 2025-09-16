using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineSchool.System.Exceptions;
using OnlineSchool.Teachers.Controllers.interfaces;
using OnlineSchool.Teachers.Dto;
using OnlineSchool.Teachers.Features.Commands;
using OnlineSchool.Teachers.Features.Queries;
using OnlineSchool.Teachers.Models;
using System.Linq;

namespace OnlineSchool.Teachers.Controllers;

public class ControllerTeacher : ControllerAPITeacher
{
    private readonly IMediator _mediator;

    public ControllerTeacher(IMediator mediator)
    {
        _mediator = mediator;
    }

    private static DtoTeacherView MapToDto(Teacher t)
    {
        return new DtoTeacherView
        {
            Id = t.Id,
            Name = t.Name,
            Email = t.Email,
            Subject = t.Subject,
            UpdateDate = t.UpdateDate,
            Role = t.Role
        };
    }

    public override async Task<ActionResult<List<DtoTeacherView>>> GetTeachers()
    {
        try
        {
            var items = await _mediator.Send(new GetTeachersQuery());
            var dtos = items.Select(MapToDto).ToList();
            return Ok(dtos);
        }
        catch (ItemsDoNotExist ex)
        {
            return NotFound(ex.Message);
        }
    }

    public override async Task<ActionResult<DtoTeacherView>> GetById([FromQuery] string id)
    {
        try
        {
            var item = await _mediator.Send(new GetTeacherByIdQuery(id));
            return Ok(MapToDto(item));
        }
        catch (ItemDoesNotExist ex)
        {
            return NotFound(ex.Message);
        }
    }

    public override async Task<ActionResult<DtoTeacherView>> GetByEmail([FromQuery] string email)
    {
        try
        {
            var item = await _mediator.Send(new GetTeacherByEmailQuery(email));
            return Ok(MapToDto(item));
        }
        catch (ItemDoesNotExist ex)
        {
            return NotFound(ex.Message);
        }
    }

    public override async Task<ActionResult<DtoTeacherView>> Create([FromBody] TeacherCreateRequest request)
    {
        try
        {
            var created = await _mediator.Send(new CreateTeacherCommand(request.Name, request.Email, request.Subject, request.Password));
            return Ok(MapToDto(created));
        }
        catch (InvalidName ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public override async Task<ActionResult<DtoTeacherView>> Update([FromQuery] string id, [FromBody] TeacherUpdateRequest request)
    {
        try
        {
            var updated = await _mediator.Send(new UpdateTeacherCommand(id, request.Name, request.Email, request.Subject));
            return Ok(MapToDto(updated));
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

    public override async Task<ActionResult<DtoTeacherView>> Delete([FromQuery] string id)
    {
        try
        {
            var deleted = await _mediator.Send(new DeleteTeacherCommand(id));
            return Ok(MapToDto(deleted));
        }
        catch (ItemDoesNotExist ex)
        {
            return NotFound(ex.Message);
        }
    }
}
