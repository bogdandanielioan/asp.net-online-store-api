
using OnlineSchool.Books.Models;

namespace OnlineSchool.Books.Repository.interfaces
{
    public interface IRepositoryBook
    {

        Task<List<Book>> GetAllAsync();

        Task<Book> GetByIdAsync(int id);

        Task<Book> GetByNameAsync(string name);

    }
}
