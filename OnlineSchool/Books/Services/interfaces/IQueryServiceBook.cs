using OnlineSchool.Books.Models;

namespace OnlineSchool.Books.Services.interfaces
{
    public interface IQueryServiceBook
    {

        Task<List<Book>> GetAllAsync();

        Task<Book> GetByIdAsync(string id);

        Task<Book> GetByNameAsync(string name);


    }
}
