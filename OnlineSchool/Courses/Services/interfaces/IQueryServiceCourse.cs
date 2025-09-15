

using OnlineSchool.Courses.Dto;
using OnlineSchool.Courses.Models;

namespace OnlineSchool.Courses.Services.interfaces
{
    public interface IQueryServiceCourse
    {

        Task<List<DtoCourseView>> GetAll();

        Task<DtoCourseView> GetById(int id);

        Task<DtoCourseView> GetByNameAsync(string name);
        Task<Course> GetByName(string name);

    }
}
