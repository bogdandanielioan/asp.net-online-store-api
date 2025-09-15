using OnlineSchool.StudentCards.Models;

namespace OnlineSchool.StudentCards.Services.interfaces
{
    public interface IQueryServiceStudentCard
    {
        Task<List<StudentCard>> GetAll();

        Task<StudentCard> GetById(string id);

        Task<StudentCard> GetByNameAsync(string name);
    }
}
