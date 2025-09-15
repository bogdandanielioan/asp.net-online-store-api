using OnlineSchool.Teachers.Models;

namespace OnlineSchool.Teachers.Services.interfaces
{
    public interface IQueryServiceTeacher
    {
        Task<List<Teacher>> GetAllAsync();
        Task<Teacher> GetByIdAsync(string id);
        Task<Teacher> GetByEmailAsync(string email);
    }
}