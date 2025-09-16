using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Books.Controllers.interfaces;
using OnlineSchool.Books.Features.Queries;
using OnlineSchool.Books.Models;
using OnlineSchool.System.Exceptions;

namespace OnlineSchool.Books.Controllers;

public class ControllerBook : ControllerAPIBook
{
    private readonly IMediator _mediator;

    public ControllerBook(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<ActionResult<List<Book>>> GetBooks()
    {
        try
        {
            var books = await _mediator.Send(new GetBooksQuery());
            return Ok(books);
        }
        catch (ItemsDoNotExist ex)
        {
            return NotFound(ex.Message);
        }
    }

    public override async Task<ActionResult<Book>> GetById([FromQuery] string id)
    {
        try
        {
            var book = await _mediator.Send(new GetBookByIdQuery(id));
            return Ok(book);
        }
        catch (ItemDoesNotExist ex)
        {
            return NotFound(ex.Message);
        }
    }

    public override async Task<ActionResult<Book>> GetByName([FromQuery] string name)
    {
        try
        {
            var book = await _mediator.Send(new GetBookByNameQuery(name));
            return Ok(book);
        }
        catch (ItemDoesNotExist ex)
        {
            return NotFound(ex.Message);
        }
    }
}
