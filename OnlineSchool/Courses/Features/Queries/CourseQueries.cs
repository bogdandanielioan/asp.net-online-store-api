using MediatR;
using OnlineSchool.Courses.Dto;
using OnlineSchool.Courses.Models;
using OnlineSchool.Courses.Services.interfaces;

namespace OnlineSchool.Courses.Features.Queries;

public record GetCoursesQuery() : IRequest<List<DtoCourseView>>;

public class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, List<DtoCourseView>>
{
    private readonly IQueryServiceCourse _service;

    public GetCoursesQueryHandler(IQueryServiceCourse service)
    {
        _service = service;
    }

    public Task<List<DtoCourseView>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        return _service.GetAll();
    }
}

public record GetCourseByNameQuery(string Name) : IRequest<DtoCourseView>;

public class GetCourseByNameQueryHandler : IRequestHandler<GetCourseByNameQuery, DtoCourseView>
{
    private readonly IQueryServiceCourse _service;

    public GetCourseByNameQueryHandler(IQueryServiceCourse service)
    {
        _service = service;
    }

    public Task<DtoCourseView> Handle(GetCourseByNameQuery request, CancellationToken cancellationToken)
    {
        return _service.GetByNameAsync(request.Name);
    }
}

public record GetCourseByIdQuery(string Id) : IRequest<DtoCourseView>;

public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, DtoCourseView>
{
    private readonly IQueryServiceCourse _service;

    public GetCourseByIdQueryHandler(IQueryServiceCourse service)
    {
        _service = service;
    }

    public Task<DtoCourseView> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        return _service.GetById(request.Id);
    }
}

public record GetCourseEntityByNameQuery(string Name) : IRequest<Course>;

public class GetCourseEntityByNameQueryHandler : IRequestHandler<GetCourseEntityByNameQuery, Course>
{
    private readonly IQueryServiceCourse _service;

    public GetCourseEntityByNameQueryHandler(IQueryServiceCourse service)
    {
        _service = service;
    }

    public Task<Course> Handle(GetCourseEntityByNameQuery request, CancellationToken cancellationToken)
    {
        return _service.GetByName(request.Name);
    }
}
