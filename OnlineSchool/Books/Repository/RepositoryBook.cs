using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Books.Models;
using OnlineSchool.Books.Repository.interfaces;
using OnlineSchool.Data;

namespace OnlineSchool.Books.Repository
{
    public class RepositoryBook : IRepositoryBook
    {

        private AppDbContext _context;
        private IMapper _mapper;

        public RepositoryBook(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _context.Books.AsNoTracking().ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(string id)
        {
            return await _context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book?> GetByNameAsync(string name)
        {
            return await _context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Name == name);
        }

    }
}
