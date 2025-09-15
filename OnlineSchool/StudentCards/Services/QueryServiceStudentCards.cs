using OnlineSchool.StudentCards.Models;
using OnlineSchool.StudentCards.Repository.interfaces;
using OnlineSchool.StudentCards.Services.interfaces;
using OnlineSchool.System.Constants;
using OnlineSchool.System.Exceptions;

namespace OnlineSchool.StudentCards.Services
{
    public class QueryServiceStudentCards: IQueryServiceStudentCard
    {
        private IRepositoryStudentCard _repository;

        public QueryServiceStudentCards(IRepositoryStudentCard repository)
        {
            _repository = repository;
        }

        public async Task<List<StudentCard>> GetAll()
        {
            var student = await _repository.GetAllAsync();

            if (student.Count() == 0)
            {
                throw new ItemsDoNotExist(Constants.ItemsDoNotExist);
            }

            return student;
        }

        public async Task<StudentCard> GetByNameAsync(string name)
        {
            var student = await _repository.GetByNameAsync(name);

            if (student == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }

            return student;
        }

        public async Task<StudentCard> GetById(int id)
        {
            var student = await _repository.GetByIdAsync(id);

            if (student == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }

            return student;
        }

    }
}
