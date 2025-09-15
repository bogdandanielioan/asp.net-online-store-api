

using OnlineSchool.Courses.Dto;
using OnlineSchool.Courses.Models;

namespace OnlineSchool.Courses.Repository.interfaces
{
    public interface IRepositoryCourse
    {
        Task<List<DtoCourseView>> GetAllAsync();

        Task<DtoCourseView> GetByNameAsync(string destination);

        Task<Course> GetByName(string destination);

        Task<DtoCourseView> GetByIdAsync(int id);

        Task<Course> GetById(int id);

        Task<Course> Create(CreateRequestCourse request);

        Task<Course> Update(int id, UpdateRequestCourse request);

        Task<Course> DeleteById(int id);

    }
}
