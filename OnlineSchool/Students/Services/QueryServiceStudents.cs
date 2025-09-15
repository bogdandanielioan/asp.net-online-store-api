using OnlineSchool.StudentCards.Models;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Models;
using OnlineSchool.Students.Repository.interfaces;
using OnlineSchool.Students.Services.interfaces;
using OnlineSchool.System.Constants;
using OnlineSchool.System.Exceptions;
using System;

namespace OnlineSchool.Students.Services
{
    public class QueryServiceStudents : IQueryServiceStudent
    {

        private IRepositoryStudent _repository;

        public QueryServiceStudents(IRepositoryStudent repository)
        {
            _repository = repository;
        }

        public async Task<List<DtoStudentView>> GetAll()
        {
            var student = await _repository.GetAllAsync();

            if (student.Count() == 0)
            {
                throw new ItemsDoNotExist(Constants.ItemsDoNotExist);
            }

            return student;
        }

        public async Task<DtoStudentView> GetByNameAsync(string name)
        {
            var student = await _repository.GetByNameAsync(name);

            if (student == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }
            return student;
        }

        public async Task<DtoStudentView> GetById(int id)
        {
            var student = await _repository.GetByIdAsync(id);

            if (student == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }

            return student;
        }

        public async Task<StudentCard> CardById(int id)
        {
            var student = await _repository.CardByIdAsync(id);

            if (student == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }

            return student;
        }

    }

}
