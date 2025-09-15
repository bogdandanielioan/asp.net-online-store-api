using OnlineSchool.Courses.Services.interfaces;
using OnlineSchool.Courses.Repository.interfaces;
using OnlineSchool.System.Constants;
using OnlineSchool.System.Exceptions;
using OnlineSchool.Courses.Models;
using OnlineSchool.Courses.Dto;

namespace OnlineSchool.Courses.Services
{
    public class CommandServiceCourse : ICommandServiceCourse
    {
        private IRepositoryCourse _repository;

        public CommandServiceCourse(IRepositoryCourse repository)
        {
            _repository = repository;
        }

        public async Task<Course> Create(CreateRequestCourse request)
        {

            if (request.Name.Equals(""))
            {
                throw new InvalidName(Constants.InvalidName);
            }
            var course = await _repository.Create(request);

            return course;
        }

        public async Task<Course> Update(string id, UpdateRequestCourse request)
        {
            var course = await _repository.GetById(id);
            if (course == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }

            // Validate new name if provided
            if (request.Name != null && request.Name.Equals(string.Empty))
            {
                throw new InvalidName(Constants.InvalidName);
            }

            course = await _repository.Update(id, request);
            return course;
        }

        public async Task<Course> Delete(string id)
        {

            var course = await _repository.GetById(id);
            if (course == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }
            await _repository.DeleteById(id);
            return course;
        }
    }
}
