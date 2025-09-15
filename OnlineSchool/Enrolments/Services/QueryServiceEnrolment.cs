using OnlineSchool.Enrolments.Models;
using OnlineSchool.Enrolments.Repository.interfaces;
using OnlineSchool.Enrolments.Services.interfaces;
using OnlineSchool.System.Constants;
using OnlineSchool.System.Exceptions;

namespace OnlineSchool.Enrolments.Services
{
    public class QueryServiceEnrolment : IQueryServiceEnrolment
    {

        private IRepositoryEnrolment _repository;

        public QueryServiceEnrolment(IRepositoryEnrolment repository)
        {
            _repository = repository;
        }

        public async Task<List<Enrolment>> GetAll()
        {
            var enrolments = await _repository.GetAllAsync();

            if (!enrolments.Any())
            {
                throw new ItemsDoNotExist(Constants.ItemsDoNotExist);
            }

            return enrolments;
        }

        public async Task<Enrolment> GetById(string id)
        {
            var enrolment = await _repository.GetByIdAsync(id);

            if (enrolment == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }

            return enrolment;
        }


    }
}
