
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

        Task<Student> Update(string id, UpdateRequestStudent request);

        Task<Student> Delete(string id);

        Task<Student> CreateBookForStudent(string idStudent, BookCreateDTO createRequestBook);

        Task<Student> UpdateBookForStudent(string idStudent, string idBook, BookUpdateDTO bookUpdateDTO);

        Task<Student> DeleteBookForStudent(string idStudent, string idBook);

        Task<Student> EnrollmentCourse(string idStudent, Course course);

        Task<Student> UnEnrollmentCourse(string idStudent, Course course);

    }
}
