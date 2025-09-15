
using OnlineSchool.Books.Models;
using OnlineSchool.Books.Repository.interfaces;
using OnlineSchool.Courses.Models;
using OnlineSchool.Enrolments.Dto;
using OnlineSchool.Enrolments.Models;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Models;
using OnlineSchool.Students.Repository.interfaces;
using OnlineSchool.Students.Services.interfaces;
using OnlineSchool.System.Constants;
using OnlineSchool.System.Exceptions;
using System;

namespace OnlineSchool.Students.Services
{
    public class CommandServiceStudents : ICommandServiceStudent
    {
        private IRepositoryStudent _repository;

        public CommandServiceStudents(IRepositoryStudent repository)
        {
            _repository = repository;
        }

        public async Task<Student> Create(CreateRequestStudent request)
        {

            if (request.Age <= 0)
            {
                throw new InvalidAge(Constants.InvalidAge);
            }
            var student = await _repository.Create(request);

            return student;
        }

        public async Task<Student> Update(int id, UpdateRequestStudent request)
        {

            var student = await _repository.GetById(id);
            if (student == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }


            if (student.Age <= 0)
            {
                throw new InvalidAge(Constants.InvalidAge);
            }
            student = await _repository.Update(id, request);
            return student;
        }

        public async Task<Student> Delete(int id)
        {

            var student = await _repository.GetById(id);
            if (student == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }
            await _repository.DeleteById(id);
            return student;
        }

        public async Task<Student> CreateBookForStudent(int idStudent,BookCreateDTO createRequestBook)
        {
            var student = await _repository.GetById(idStudent);
            if (student == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }

            if (createRequestBook.Name.Equals(""))
            {
                throw new InvalidName(Constants.InvalidName);
            }

            student = await _repository.CreateBookForStudent(idStudent,createRequestBook);

            return student;
        }

        public async Task<Student> UpdateBookForStudent(int idStudent, int idBook, BookUpdateDTO bookUpdateDTO)
        {
            var student = await _repository.GetById(idStudent);
            if (student == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }

            var books = student.StudentBooks;
            var book = (Book)null;
            for(int i = 0; i < books.Count; i++)
            {
                if (books[i].Id == idBook)
                {
                    book = books[i];
                    break;
                }
            }

            if (book == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }

            if (book.Name == null)
            {
                throw new InvalidName(Constants.InvalidName);
            }

            student = await _repository.UpdateBookForStudent(idStudent,idBook, bookUpdateDTO);
            return student;
        }

        public async Task<Student> DeleteBookForStudent(int idStudent, int idBook)
        {
            var student = await _repository.GetById(idStudent);
            if (student == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }

            var books = student.StudentBooks;
            var book = (Book)null;
            for (int i = 0; i < books.Count; i++)
            {
                if (books[i].Id == idBook)
                {
                    book = books[i];
                    break;
                }
            }

            if (book == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }

             await _repository.DeleteBookForStudent(idStudent, idBook);
            return student;
        }

        public async Task<Student> EnrollmentCourse(int idStudent, Course course)
        {
            var student = await _repository.GetById(idStudent);
            if (student == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }

            student = await _repository.EnrollmentCourse(idStudent, course);

            if (student == null)
            {
                throw new InvalidCourse(Constants.InvalidCourse);
            }


            return student;
        }

        public async Task<Student> UnEnrollmentCourse(int idStudent, Course course)
        {
            var student = await _repository.GetById(idStudent);
            if (student == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }

            student = await _repository.UnEnrollmentCourse(idStudent, course);

            if (student == null)
            {
                throw new ItemsDoNotExist(Constants.ItemDoesNotExist);
            }


            return student;
        }
    }
}
