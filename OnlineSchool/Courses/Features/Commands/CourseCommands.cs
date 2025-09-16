using MediatR;
using OnlineSchool.Courses.Dto;
using OnlineSchool.Courses.Models;
using OnlineSchool.Courses.Services.interfaces;

namespace OnlineSchool.Courses.Features.Commands;

public record CreateCourseCommand(CreateRequestCourse Request) : IRequest<Course>;

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Course>
{
    private readonly ICommandServiceCourse _service;

    public CreateCourseCommandHandler(ICommandServiceCourse service)
    {
        _service = service;
    }

    public Task<Course> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        return _service.Create(request.Request);
    }
}

public record UpdateCourseCommand(string Id, UpdateRequestCourse Request) : IRequest<Course>;

public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, Course>
{
    private readonly ICommandServiceCourse _service;

    public UpdateCourseCommandHandler(ICommandServiceCourse service)
    {
        _service = service;
    }

    public Task<Course> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        return _service.Update(request.Id, request.Request);
    }
}

public record DeleteCourseCommand(string Id) : IRequest<Course>;

public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, Course>
{
    private readonly ICommandServiceCourse _service;

    public DeleteCourseCommandHandler(ICommandServiceCourse service)
    {
        _service = service;
    }

    public Task<Course> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        return _service.Delete(request.Id);
    }
}
