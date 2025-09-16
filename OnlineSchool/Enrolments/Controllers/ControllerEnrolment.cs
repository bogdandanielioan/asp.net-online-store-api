using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Enrolments.Controllers.interfaces;
using OnlineSchool.Enrolments.Features.Queries;
using OnlineSchool.Enrolments.Models;
using OnlineSchool.System.Exceptions;

namespace OnlineSchool.Enrolments.Controllers;

public class ControllerEnrolment : ControllerAPIEnrolment
{
    private readonly IMediator _mediator;

    public ControllerEnrolment(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<ActionResult<List<Enrolment>>> GetEnrolments()
    {
        try
        {
            var enrolments = await _mediator.Send(new GetEnrolmentsQuery());
            return Ok(enrolments);
        }
        catch (ItemsDoNotExist ex)
        {
            return NotFound(ex.Message);
        }
    }

    public override async Task<ActionResult<Enrolment>> GetById([FromQuery] string id)
    {
        try
        {
            var enrolment = await _mediator.Send(new GetEnrolmentByIdQuery(id));
            return Ok(enrolment);
        }
        catch (ItemDoesNotExist ex)
        {
            return NotFound(ex.Message);
        }
    }
}
