using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Data;
using OnlineSchool.StudentCards.Models;
using OnlineSchool.StudentCards.Repository.interfaces;

namespace OnlineSchool.StudentsCard.Repository
{
    public class RepositoryStudentCard : IRepositoryStudentCard
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RepositoryStudentCard(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<StudentCard>> GetAllAsync()
        {
            return await _context.Studentscard.AsNoTracking().ToListAsync();
        }

        public async Task<StudentCard?> GetByIdAsync(string id)
        {
            return await _context.Studentscard.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<StudentCard?> GetByNameAsync(string name)
        {
            return await _context.Studentscard.AsNoTracking().FirstOrDefaultAsync(c => c.Namecard == name);
        }


    }
}
