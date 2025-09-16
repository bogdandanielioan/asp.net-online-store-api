using MediatR;
using OnlineSchool.Books.Models;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Models;
using OnlineSchool.Students.Services.interfaces;

namespace OnlineSchool.Students.Features.Commands;

public record CreateStudentCommand(CreateRequestStudent Request) : IRequest<Student>;

public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Student>
{
    private readonly ICommandServiceStudent _service;

    public CreateStudentCommandHandler(ICommandServiceStudent service)
    {
        _service = service;
    }

    public Task<Student> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        return _service.Create(request.Request);
    }
}

public record UpdateStudentCommand(string Id, UpdateRequestStudent Request) : IRequest<Student>;

public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, Student>
{
    private readonly ICommandServiceStudent _service;

    public UpdateStudentCommandHandler(ICommandServiceStudent service)
    {
        _service = service;
    }

    public Task<Student> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        return _service.Update(request.Id, request.Request);
    }
}

public record DeleteStudentCommand(string Id) : IRequest<Student>;

public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, Student>
{
    private readonly ICommandServiceStudent _service;

    public DeleteStudentCommandHandler(ICommandServiceStudent service)
    {
        _service = service;
    }

    public Task<Student> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        return _service.Delete(request.Id);
    }
}

public record CreateStudentBookCommand(string StudentId, BookCreateDTO Request) : IRequest<Student>;

public class CreateStudentBookCommandHandler : IRequestHandler<CreateStudentBookCommand, Student>
{
    private readonly ICommandServiceStudent _service;

    public CreateStudentBookCommandHandler(ICommandServiceStudent service)
    {
        _service = service;
    }

    public Task<Student> Handle(CreateStudentBookCommand request, CancellationToken cancellationToken)
    {
        return _service.CreateBookForStudent(request.StudentId, request.Request);
    }
}

public record UpdateStudentBookCommand(string StudentId, string BookId, BookUpdateDTO Request) : IRequest<Student>;

public class UpdateStudentBookCommandHandler : IRequestHandler<UpdateStudentBookCommand, Student>
{
    private readonly ICommandServiceStudent _service;

    public UpdateStudentBookCommandHandler(ICommandServiceStudent service)
    {
        _service = service;
    }

    public Task<Student> Handle(UpdateStudentBookCommand request, CancellationToken cancellationToken)
    {
        return _service.UpdateBookForStudent(request.StudentId, request.BookId, request.Request);
    }
}

public record DeleteStudentBookCommand(string StudentId, string BookId) : IRequest<Student>;

public class DeleteStudentBookCommandHandler : IRequestHandler<DeleteStudentBookCommand, Student>
{
    private readonly ICommandServiceStudent _service;

    public DeleteStudentBookCommandHandler(ICommandServiceStudent service)
    {
        _service = service;
    }

    public Task<Student> Handle(DeleteStudentBookCommand request, CancellationToken cancellationToken)
    {
        return _service.DeleteBookForStudent(request.StudentId, request.BookId);
    }
}
