using OnlineSchool.StudentCards.Models;

namespace OnlineSchool.StudentCards.Repository.interfaces
{
    public interface IRepositoryStudentCard
    {
        Task<List<StudentCard>> GetAllAsync();

        Task<StudentCard?> GetByIdAsync(string id);

        Task<StudentCard?> GetByNameAsync(string name);

    }
}
