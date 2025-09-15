using OnlineSchool.Courses.Dto;
using OnlineSchool.Courses.Models;
using OnlineSchool.Courses.Repository.interfaces;
using OnlineSchool.Courses.Services.interfaces;
using OnlineSchool.System.Constants;
using OnlineSchool.System.Exceptions;

namespace OnlineSchool.Courses.Services
{
    public class QueryServiceCourse : IQueryServiceCourse
    {

        private IRepositoryCourse _repository;

        public QueryServiceCourse(IRepositoryCourse repository)
        {
            _repository = repository;
        }

        public async Task<List<DtoCourseView>> GetAll()
        {
            var courses = await _repository.GetAllAsync();

            if (!courses.Any())
            {
                throw new ItemsDoNotExist(Constants.ItemsDoNotExist);
            }

            return courses;
        }

        public async Task<DtoCourseView> GetByNameAsync(string name)
        {
            var course = await _repository.GetByNameAsync(name);

            if (course == null)
            {
                throw new NotFoundCourse(Constants.NotFoundcourse);
            }
            return course;
        }

        public async Task<Course> GetByName(string name)
        {
            var course = await _repository.GetByName(name);

            if (course == null)
            {
                throw new NotFoundCourse(Constants.NotFoundcourse);
            }
            return course;
        }

        public async Task<DtoCourseView> GetById(string id)
        {
            var course = await _repository.GetByIdAsync(id);

            if (course == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }

            return course;
        }


    }
}
