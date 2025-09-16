
using OnlineSchool.Courses.Models;
using OnlineSchool.Enrolments.Dto;
using OnlineSchool.StudentCards.Models;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Models;
using System;

namespace OnlineSchool.Students.Repository.interfaces
{
    public interface IRepositoryStudent
    {

        Task<List<DtoStudentView>> GetAllAsync();

        Task<DtoStudentView?> GetByNameAsync(string destination);

        Task<Student?> GetById(string id);

        Task<DtoStudentView?> GetByIdAsync(string id);

        Task<StudentCard?> CardByIdAsync(string id);

        Task<Student> Create(CreateRequestStudent request);

        Task<Student?> Update(string id, UpdateRequestStudent request);

        Task<Student?> DeleteById(string id);

        Task<Student?> CreateBookForStudent(string idStudent, BookCreateDTO createRequestBook);

        Task<Student?> UpdateBookForStudent(string idStudent, string idBook, BookUpdateDTO bookUpdateDTO);

        Task<Student?> DeleteBookForStudent(string idStudent, string idBook);

        Task<Student?> EnrollmentCourse(string idStudent, Course course);

        Task<Student?> UnEnrollmentCourse(string idStudent, Course course);

    }
}
