
using OnlineSchool.Courses.Models;
using OnlineSchool.Enrolments.Dto;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Models;
using System;

namespace OnlineSchool.Students.Services.interfaces
{
    public interface ICommandServiceStudent
    {
        Task<Student> Create(CreateRequestStudent request);

        Task<Student> Update(int id, UpdateRequestStudent request);

        Task<Student> Delete(int id);

        Task<Student> CreateBookForStudent(int idStudent, BookCreateDTO createRequestBook);

        Task<Student> UpdateBookForStudent(int idStudent, int idBook, BookUpdateDTO bookUpdateDTO);

        Task<Student> DeleteBookForStudent(int idStudent, int idBook);

        Task<Student> EnrollmentCourse(int idStudent, Course course);

        Task<Student> UnEnrollmentCourse(int idStudent, Course course);

    }
}
