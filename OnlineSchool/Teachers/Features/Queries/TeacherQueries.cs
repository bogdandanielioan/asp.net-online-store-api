using MediatR;
using OnlineSchool.Teachers.Models;
using OnlineSchool.Teachers.Services.interfaces;

namespace OnlineSchool.Teachers.Features.Queries;

public record GetTeachersQuery() : IRequest<List<Teacher>>;

public class GetTeachersQueryHandler : IRequestHandler<GetTeachersQuery, List<Teacher>>
{
    private readonly IQueryServiceTeacher _service;

    public GetTeachersQueryHandler(IQueryServiceTeacher service)
    {
        _service = service;
    }

    public Task<List<Teacher>> Handle(GetTeachersQuery request, CancellationToken cancellationToken)
    {
        return _service.GetAllAsync();
    }
}

public record GetTeacherByIdQuery(string Id) : IRequest<Teacher>;

public class GetTeacherByIdQueryHandler : IRequestHandler<GetTeacherByIdQuery, Teacher>
{
    private readonly IQueryServiceTeacher _service;

    public GetTeacherByIdQueryHandler(IQueryServiceTeacher service)
    {
        _service = service;
    }

    public Task<Teacher> Handle(GetTeacherByIdQuery request, CancellationToken cancellationToken)
    {
        return _service.GetByIdAsync(request.Id);
    }
}

public record GetTeacherByEmailQuery(string Email) : IRequest<Teacher>;

public class GetTeacherByEmailQueryHandler : IRequestHandler<GetTeacherByEmailQuery, Teacher>
{
    private readonly IQueryServiceTeacher _service;

    public GetTeacherByEmailQueryHandler(IQueryServiceTeacher service)
    {
        _service = service;
    }

    public Task<Teacher> Handle(GetTeacherByEmailQuery request, CancellationToken cancellationToken)
    {
        return _service.GetByEmailAsync(request.Email);
    }
}
