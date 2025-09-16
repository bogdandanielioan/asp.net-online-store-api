using MediatR;
using OnlineSchool.Enrolments.Models;
using OnlineSchool.Enrolments.Services.interfaces;

namespace OnlineSchool.Enrolments.Features.Queries;

public record GetEnrolmentsQuery() : IRequest<List<Enrolment>>;

public class GetEnrolmentsQueryHandler : IRequestHandler<GetEnrolmentsQuery, List<Enrolment>>
{
    private readonly IQueryServiceEnrolment _service;

    public GetEnrolmentsQueryHandler(IQueryServiceEnrolment service)
    {
        _service = service;
    }

    public Task<List<Enrolment>> Handle(GetEnrolmentsQuery request, CancellationToken cancellationToken)
    {
        return _service.GetAll();
    }
}

public record GetEnrolmentByIdQuery(string Id) : IRequest<Enrolment>;

public class GetEnrolmentByIdQueryHandler : IRequestHandler<GetEnrolmentByIdQuery, Enrolment>
{
    private readonly IQueryServiceEnrolment _service;

    public GetEnrolmentByIdQueryHandler(IQueryServiceEnrolment service)
    {
        _service = service;
    }

    public Task<Enrolment> Handle(GetEnrolmentByIdQuery request, CancellationToken cancellationToken)
    {
        return _service.GetById(request.Id);
    }
}
