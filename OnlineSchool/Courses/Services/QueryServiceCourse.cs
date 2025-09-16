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

            return course ?? throw new NotFoundCourse(Constants.NotFoundcourse);
        }

        public async Task<Course> GetByName(string name)
        {
            var course = await _repository.GetByName(name);

            return course ?? throw new NotFoundCourse(Constants.NotFoundcourse);
        }

        public async Task<DtoCourseView> GetById(string id)
        {
            var course = await _repository.GetByIdAsync(id);

            return course ?? throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
        }


    }
}
