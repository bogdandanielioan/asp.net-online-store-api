using OnlineSchool.StudentCards.Models;

namespace OnlineSchool.StudentCards.Repository.interfaces
{
    public interface IRepositoryStudentCard
    {
        Task<List<StudentCard>> GetAllAsync();

        Task<StudentCard> GetByIdAsync(int id);

        Task<StudentCard> GetByNameAsync(string name);

    }
}
