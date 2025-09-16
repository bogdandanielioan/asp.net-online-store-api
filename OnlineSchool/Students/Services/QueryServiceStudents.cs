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
            var students = await _repository.GetAllAsync();

            if (!students.Any())
            {
                throw new ItemsDoNotExist(Constants.ItemsDoNotExist);
            }

            return students;
        }

        public async Task<DtoStudentView> GetByNameAsync(string name)
        {
            var student = await _repository.GetByNameAsync(name);

            return student ?? throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
        }

        public async Task<DtoStudentView> GetById(string id)
        {
            var student = await _repository.GetByIdAsync(id);

            return student ?? throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
        }

        public async Task<StudentCard> CardById(string id)
        {
            var student = await _repository.CardByIdAsync(id);

            return student ?? throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
        }

    }

}
