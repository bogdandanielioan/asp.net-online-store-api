using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Books.Models;
using OnlineSchool.StudentCards.Models;
using OnlineSchool.Students.Controllers.interfaces;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Features.Commands;
using OnlineSchool.Students.Features.Queries;
using OnlineSchool.Students.Models;
using OnlineSchool.System.Exceptions;

namespace OnlineSchool.Students.Controllers;

public class ControllerStudent : ControllerAPIStudent
{
    private readonly IMediator _mediator;

    public ControllerStudent(IMediator mediator)
    {
        _mediator = mediator;
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
            var students = await _mediator.Send(new GetStudentsQuery());
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
            var student = await _mediator.Send(new GetStudentByNameQuery(name));
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
            var student = await _mediator.Send(new GetStudentByIdQuery(id));
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
            var card = await _mediator.Send(new GetStudentCardQuery(id));
            return Ok(card);
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
            var student = await _mediator.Send(new CreateStudentCommand(request));
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
            var student = await _mediator.Send(new UpdateStudentCommand(id, request));
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
            var student = await _mediator.Send(new DeleteStudentCommand(id));
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
            var student = await _mediator.Send(new CreateStudentBookCommand(idStudent, request));
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

    public override async Task<ActionResult<DtoStudentView>> UpdateBookForStudent([FromQuery] string idStudent, [FromQuery] string idBook, BookUpdateDTO request)
    {
        try
        {
            var student = await _mediator.Send(new UpdateStudentBookCommand(idStudent, idBook, request));
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
            var student = await _mediator.Send(new DeleteStudentBookCommand(idStudent, idBook));
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
            var student = await _mediator.Send(new EnrollStudentInCourseCommand(idStudent, name));
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
            var student = await _mediator.Send(new UnenrollStudentFromCourseCommand(idStudent, name));
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
}
