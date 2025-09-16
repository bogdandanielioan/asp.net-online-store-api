using MediatR;
using OnlineSchool.StudentCards.Models;
using OnlineSchool.StudentCards.Services.interfaces;

namespace OnlineSchool.StudentCards.Features.Queries;

public record GetStudentCardsQuery() : IRequest<List<StudentCard>>;

public class GetStudentCardsQueryHandler : IRequestHandler<GetStudentCardsQuery, List<StudentCard>>
{
    private readonly IQueryServiceStudentCard _service;

    public GetStudentCardsQueryHandler(IQueryServiceStudentCard service)
    {
        _service = service;
    }

    public Task<List<StudentCard>> Handle(GetStudentCardsQuery request, CancellationToken cancellationToken)
    {
        return _service.GetAll();
    }
}

public record GetStudentCardByNameQuery(string Name) : IRequest<StudentCard>;

public class GetStudentCardByNameQueryHandler : IRequestHandler<GetStudentCardByNameQuery, StudentCard>
{
    private readonly IQueryServiceStudentCard _service;

    public GetStudentCardByNameQueryHandler(IQueryServiceStudentCard service)
    {
        _service = service;
    }

    public Task<StudentCard> Handle(GetStudentCardByNameQuery request, CancellationToken cancellationToken)
    {
        return _service.GetByNameAsync(request.Name);
    }
}

public record GetStudentCardByIdQuery(string Id) : IRequest<StudentCard>;

public class GetStudentCardByIdQueryHandler : IRequestHandler<GetStudentCardByIdQuery, StudentCard>
{
    private readonly IQueryServiceStudentCard _service;

    public GetStudentCardByIdQueryHandler(IQueryServiceStudentCard service)
    {
        _service = service;
    }

    public Task<StudentCard> Handle(GetStudentCardByIdQuery request, CancellationToken cancellationToken)
    {
        return _service.GetById(request.Id);
    }
}
