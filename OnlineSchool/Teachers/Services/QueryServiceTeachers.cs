using OnlineSchool.System.Constants;
using OnlineSchool.System.Exceptions;
using OnlineSchool.Teachers.Models;
using OnlineSchool.Teachers.Repository.interfaces;
using OnlineSchool.Teachers.Services.interfaces;

namespace OnlineSchool.Teachers.Services
{
    public class QueryServiceTeachers : IQueryServiceTeacher
    {
        private readonly IRepositoryTeacher _repository;
        public QueryServiceTeachers(IRepositoryTeacher repository)
        {
            _repository = repository;
        }

        public async Task<List<Teacher>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            if (items == null || !items.Any())
            {
                throw new ItemsDoNotExist(Constants.ItemsDoNotExist);
            }
            return items;
        }

        public async Task<Teacher> GetByEmailAsync(string email)
        {
            var item = await _repository.GetByEmailAsync(email);
            if (item == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }
            return item;
        }

        public async Task<Teacher> GetByIdAsync(string id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }
            return item;
        }
    }
}