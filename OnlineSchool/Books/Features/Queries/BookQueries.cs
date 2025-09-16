using MediatR;
using OnlineSchool.Books.Models;
using OnlineSchool.Books.Services.interfaces;

namespace OnlineSchool.Books.Features.Queries;

public record GetBooksQuery() : IRequest<List<Book>>;

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, List<Book>>
{
    private readonly IQueryServiceBook _service;

    public GetBooksQueryHandler(IQueryServiceBook service)
    {
        _service = service;
    }

    public Task<List<Book>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        return _service.GetAllAsync();
    }
}

public record GetBookByIdQuery(string Id) : IRequest<Book>;

public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, Book>
{
    private readonly IQueryServiceBook _service;

    public GetBookByIdQueryHandler(IQueryServiceBook service)
    {
        _service = service;
    }

    public Task<Book> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        return _service.GetByIdAsync(request.Id);
    }
}

public record GetBookByNameQuery(string Name) : IRequest<Book>;

public class GetBookByNameQueryHandler : IRequestHandler<GetBookByNameQuery, Book>
{
    private readonly IQueryServiceBook _service;

    public GetBookByNameQueryHandler(IQueryServiceBook service)
    {
        _service = service;
    }

    public Task<Book> Handle(GetBookByNameQuery request, CancellationToken cancellationToken)
    {
        return _service.GetByNameAsync(request.Name);
    }
}
