using MediatR;
using OnlineSchool.Teachers.Models;
using OnlineSchool.Teachers.Services.interfaces;

namespace OnlineSchool.Teachers.Features.Commands;

public record CreateTeacherCommand(string Name, string Email, string? Subject, string? Password) : IRequest<Teacher>;

public class CreateTeacherCommandHandler : IRequestHandler<CreateTeacherCommand, Teacher>
{
    private readonly ICommandServiceTeacher _service;

    public CreateTeacherCommandHandler(ICommandServiceTeacher service)
    {
        _service = service;
    }

    public Task<Teacher> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
    {
        var teacher = new Teacher
        {
            Name = request.Name,
            Email = request.Email,
            Subject = request.Subject,
            Password = request.Password
        };

        return _service.CreateAsync(teacher);
    }
}

public record UpdateTeacherCommand(string Id, string? Name, string? Email, string? Subject) : IRequest<Teacher>;

public class UpdateTeacherCommandHandler : IRequestHandler<UpdateTeacherCommand, Teacher>
{
    private readonly ICommandServiceTeacher _service;

    public UpdateTeacherCommandHandler(ICommandServiceTeacher service)
    {
        _service = service;
    }

    public Task<Teacher> Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
    {
        var teacher = new Teacher
        {
            Name = request.Name,
            Email = request.Email,
            Subject = request.Subject
        };

        return _service.UpdateAsync(request.Id, teacher);
    }
}

public record DeleteTeacherCommand(string Id) : IRequest<Teacher>;

public class DeleteTeacherCommandHandler : IRequestHandler<DeleteTeacherCommand, Teacher>
{
    private readonly ICommandServiceTeacher _service;

    public DeleteTeacherCommandHandler(ICommandServiceTeacher service)
    {
        _service = service;
    }

    public Task<Teacher> Handle(DeleteTeacherCommand request, CancellationToken cancellationToken)
    {
        return _service.DeleteAsync(request.Id);
    }
}
