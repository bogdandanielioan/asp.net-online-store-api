using OnlineSchool.Courses.Services.interfaces;
using OnlineSchool.Courses.Repository.interfaces;
using OnlineSchool.System.Constants;
using OnlineSchool.System.Exceptions;
using OnlineSchool.Courses.Models;
using OnlineSchool.Courses.Dto;
using OnlineSchool.Teachers.Services.interfaces;

namespace OnlineSchool.Courses.Services
{
    public class CommandServiceCourse : ICommandServiceCourse
    {
        private readonly IRepositoryCourse _repository;
        private readonly IQueryServiceTeacher _teacherQueryService;

        public CommandServiceCourse(IRepositoryCourse repository, IQueryServiceTeacher teacherQueryService)
        {
            _repository = repository;
            _teacherQueryService = teacherQueryService;
        }

        public async Task<Course> Create(CreateRequestCourse request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new InvalidName(Constants.InvalidName);
            }
            if (string.IsNullOrWhiteSpace(request.TeacherId))
            {
                throw new InvalidName(Constants.InvalidName);
            }

            // Ensure the teacher exists
            try
            {
                await _teacherQueryService.GetByIdAsync(request.TeacherId);
            }
            catch (ItemDoesNotExist)
            {
                throw; // propagate 404 handling to controller layer
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
