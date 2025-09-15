using OnlineSchool.StudentCards.Models;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Models;
using System;

namespace OnlineSchool.Students.Services.interfaces
{
    public interface IQueryServiceStudent
    {

        Task<List<DtoStudentView>> GetAll();

        Task<DtoStudentView> GetById(string id);

        Task<DtoStudentView> GetByNameAsync(string name);

        Task<StudentCard> CardById(string id);
    }
}
