using MediatR;
using OnlineSchool.StudentCards.Models;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Services.interfaces;

namespace OnlineSchool.Students.Features.Queries;

public record GetStudentsQuery() : IRequest<List<DtoStudentView>>;

public class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, List<DtoStudentView>>
{
    private readonly IQueryServiceStudent _service;

    public GetStudentsQueryHandler(IQueryServiceStudent service)
    {
        _service = service;
    }

    public Task<List<DtoStudentView>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
    {
        return _service.GetAll();
    }
}

public record GetStudentByIdQuery(string Id) : IRequest<DtoStudentView>;

public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, DtoStudentView>
{
    private readonly IQueryServiceStudent _service;

    public GetStudentByIdQueryHandler(IQueryServiceStudent service)
    {
        _service = service;
    }

    public Task<DtoStudentView> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        return _service.GetById(request.Id);
    }
}

public record GetStudentByNameQuery(string Name) : IRequest<DtoStudentView>;

public class GetStudentByNameQueryHandler : IRequestHandler<GetStudentByNameQuery, DtoStudentView>
{
    private readonly IQueryServiceStudent _service;

    public GetStudentByNameQueryHandler(IQueryServiceStudent service)
    {
        _service = service;
    }

    public Task<DtoStudentView> Handle(GetStudentByNameQuery request, CancellationToken cancellationToken)
    {
        return _service.GetByNameAsync(request.Name);
    }
}

public record GetStudentCardQuery(string Id) : IRequest<StudentCard>;

public class GetStudentCardQueryHandler : IRequestHandler<GetStudentCardQuery, StudentCard>
{
    private readonly IQueryServiceStudent _service;

    public GetStudentCardQueryHandler(IQueryServiceStudent service)
    {
        _service = service;
    }

    public Task<StudentCard> Handle(GetStudentCardQuery request, CancellationToken cancellationToken)
    {
        return _service.CardById(request.Id);
    }
}
