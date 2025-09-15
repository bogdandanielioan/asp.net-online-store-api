using OnlineSchool.Teachers.Models;

namespace OnlineSchool.Teachers.Repository.interfaces
{
    public interface IRepositoryTeacher
    {
        Task<List<Teacher>> GetAllAsync();
        Task<Teacher?> GetByIdAsync(string id);
        Task<Teacher?> GetByEmailAsync(string email);
        Task<Teacher> CreateAsync(Teacher teacher);
        Task<Teacher?> UpdateAsync(string id, Teacher teacher);
        Task<Teacher?> DeleteAsync(string id);
    }
}