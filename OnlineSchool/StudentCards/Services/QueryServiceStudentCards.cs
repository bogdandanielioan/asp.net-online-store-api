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
            var cards = await _repository.GetAllAsync();

            if (!cards.Any())
            {
                throw new ItemsDoNotExist(Constants.ItemsDoNotExist);
            }

            return cards;
        }

        public async Task<StudentCard> GetByNameAsync(string name)
        {
            var student = await _repository.GetByNameAsync(name);

            return student ?? throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
        }

        public async Task<StudentCard> GetById(string id)
        {
            var student = await _repository.GetByIdAsync(id);

            return student ?? throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
        }

    }
}
