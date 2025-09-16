using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineSchool.StudentCards.Controllers.interfaces;
using OnlineSchool.StudentCards.Features.Queries;
using OnlineSchool.StudentCards.Models;
using OnlineSchool.System.Exceptions;

namespace OnlineSchool.StudentCards.Controllers;

public class ControllerStudentCards : ControllerAPIStudentCards
{
    private readonly IMediator _mediator;

    public ControllerStudentCards(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<ActionResult<List<StudentCard>>> GetStudentCards()
    {
        try
        {
            var students = await _mediator.Send(new GetStudentCardsQuery());
            return Ok(students);
        }
        catch (ItemsDoNotExist ex)
        {
            return NotFound(ex.Message);
        }
    }

    public override async Task<ActionResult<StudentCard>> GetByName([FromQuery] string name)
    {
        try
        {
            var student = await _mediator.Send(new GetStudentCardByNameQuery(name));
            return Ok(student);
        }
        catch (ItemDoesNotExist ex)
        {
            return NotFound(ex.Message);
        }
    }

    public override async Task<ActionResult<StudentCard>> GetById([FromQuery] string id)
    {
        try
        {
            var student = await _mediator.Send(new GetStudentCardByIdQuery(id));
            return Ok(student);
        }
        catch (ItemDoesNotExist ex)
        {
            return NotFound(ex.Message);
        }
    }
}
