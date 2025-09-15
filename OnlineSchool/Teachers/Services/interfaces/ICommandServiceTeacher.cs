using OnlineSchool.Teachers.Models;

namespace OnlineSchool.Teachers.Services.interfaces
{
    public interface ICommandServiceTeacher
    {
        Task<Teacher> CreateAsync(Teacher teacher);
        Task<Teacher> UpdateAsync(string id, Teacher teacher);
        Task<Teacher> DeleteAsync(string id);
    }
}