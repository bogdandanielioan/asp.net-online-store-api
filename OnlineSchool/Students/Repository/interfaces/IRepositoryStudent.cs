
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

        Task<DtoStudentView> GetByNameAsync(string destination);

        Task<Student> GetById(int id);

        Task<DtoStudentView> GetByIdAsync(int id);

        Task<StudentCard> CardByIdAsync(int id);

        Task<Student> Create(CreateRequestStudent request);

        Task<Student> Update(int id, UpdateRequestStudent request);

        Task<Student> DeleteById(int id);

        Task<Student> CreateBookForStudent(int idStudent, BookCreateDTO createRequestBook);

        Task<Student> UpdateBookForStudent(int idStudent, int idBook, BookUpdateDTO bookUpdateDTO);

        Task<Student> DeleteBookForStudent(int idStudent, int idBook);

        Task<Student> EnrollmentCourse(int idStudent, Course course);

        Task<Student> UnEnrollmentCourse(int idStudent, Course course);

    }
}
