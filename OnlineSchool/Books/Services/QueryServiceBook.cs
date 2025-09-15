using OnlineSchool.Books.Models;
using OnlineSchool.Books.Repository.interfaces;
using OnlineSchool.Books.Services.interfaces;
using OnlineSchool.System.Constants;
using OnlineSchool.System.Exceptions;

namespace OnlineSchool.Books.Services
{
    public class QueryServiceBook : IQueryServiceBook
    {

        private IRepositoryBook _repo;

        public QueryServiceBook(IRepositoryBook repo)
        {
            _repo = repo;
        }

        public async Task<List<Book>> GetAllAsync()
        {
           var books = await _repo.GetAllAsync();

            if(books.Count == 0)
            {
                throw new ItemsDoNotExist(Constants.ItemsDoNotExist);
            }

            return books;
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            var book = await _repo.GetByIdAsync(id);

            if(book == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }

            return book;
        }

        public async Task<Book> GetByNameAsync(string name)
        {
            var book = await _repo.GetByNameAsync(name);

            if (book == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }

            return book;
        }
    }
}
