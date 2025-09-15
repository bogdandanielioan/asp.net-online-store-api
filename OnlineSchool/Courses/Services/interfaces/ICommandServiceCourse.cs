

using OnlineSchool.Courses.Dto;
using OnlineSchool.Courses.Models;

namespace OnlineSchool.Courses.Services.interfaces
{
    public interface ICommandServiceCourse
    {

        Task<Course> Create(CreateRequestCourse request);

        Task<Course> Update(int id, UpdateRequestCourse request);

        Task<Course> Delete(int id);
    }
}
