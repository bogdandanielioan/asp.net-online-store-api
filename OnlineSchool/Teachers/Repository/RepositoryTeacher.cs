using Microsoft.EntityFrameworkCore;
using OnlineSchool.Data;
using OnlineSchool.Teachers.Models;
using OnlineSchool.Teachers.Repository.interfaces;

namespace OnlineSchool.Teachers.Repository
{
    public class RepositoryTeacher : IRepositoryTeacher
    {
        private readonly AppDbContext _context;

        public RepositoryTeacher(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Teacher>> GetAllAsync()
        {
            return await _context.Teachers.AsNoTracking().ToListAsync();
        }

        public async Task<Teacher?> GetByIdAsync(string id)
        {
            return await _context.Teachers.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Teacher?> GetByEmailAsync(string email)
        {
            return await _context.Teachers.AsNoTracking().FirstOrDefaultAsync(t => t.Email == email);
        }

        public async Task<Teacher> CreateAsync(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
            return teacher;
        }

        public async Task<Teacher?> UpdateAsync(string id, Teacher teacher)
        {
            var existing = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (existing == null) return null;

            existing.Name = teacher.Name;
            existing.Email = teacher.Email;
            existing.Subject = teacher.Subject;
            existing.UpdateDate = teacher.UpdateDate;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<Teacher?> DeleteAsync(string id)
        {
            var existing = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (existing == null) return null;
            _context.Teachers.Remove(existing);
            await _context.SaveChangesAsync();
            return existing;
        }
    }
}