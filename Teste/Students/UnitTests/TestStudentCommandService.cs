using Moq;
using OnlineSchool.Books.Models;
using OnlineSchool.Enrolments.Dto;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Models;
using OnlineSchool.Students.Repository.interfaces;
using OnlineSchool.Students.Services;
using OnlineSchool.Students.Services.interfaces;
using OnlineSchool.System.Constants;
using OnlineSchool.System.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Students.Helpers;
using Xunit.Sdk;

namespace Teste.Students.UnitTests
{
    public class TestStudentCommandService
    {

        private readonly Mock<IRepositoryStudent> _mock;
        private readonly ICommandServiceStudent _commandService;

        public TestStudentCommandService()
        {
            _mock = new Mock<IRepositoryStudent>();
            _commandService = new CommandServiceStudents(_mock.Object);
        }

        [Fact]
        public async Task CreateStudent_InvalidAge()
        {
            var createRequest = new CreateRequestStudent
            {
                Age = 0,
                Email = "test@gmail.com",
                Name = "test"
            };

            _mock.Setup(repo => repo.Create(createRequest)).ReturnsAsync((Student)null);
            Exception exception = await Assert.ThrowsAsync<InvalidAge>(() => _commandService.Create(createRequest));

            Assert.Equal(Constants.InvalidAge, exception.Message);
        }

        [Fact]
        public async Task CreateStudent_ReturnStudent()
        {
            var createRequest = new CreateRequestStudent
            {
                Age = 18,
                Email = "test@gmail.com",
                Name = "test"
            };

            var student = TestStudentFactory.CreateStudent(50);
            student.Age = createRequest.Age;

            _mock.Setup(repo => repo.Create(It.IsAny<CreateRequestStudent>())).ReturnsAsync(student);

            var result = await _commandService.Create(createRequest);

            Assert.NotNull(result);
            Assert.Equal(result.Age, createRequest.Age);
        }

        [Fact]
        public async Task Update_ItemDoesNotExist()
        {
            var updateRequest = new UpdateRequestStudent
            {
                Age = 0
            };

            _mock.Setup(repo => repo.GetByIdAsync(50)).ReturnsAsync((Student)null);

            var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _commandService.Update(50, updateRequest));

            Assert.Equal(Constants.ItemDoesNotExist, exception.Message);
        }

        [Fact]
        public async Task Update_InvalidAge()
        {
            var updateRequest = new UpdateRequestStudent
            {
                Age = 0
            };

            var student = TestStudentFactory.CreateStudent(1);
            student.Age = updateRequest.Age.Value;
            _mock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(student);

            var exception = await Assert.ThrowsAsync<InvalidAge>(() => _commandService.Update(1, updateRequest));

            Assert.Equal(Constants.InvalidAge, exception.Message);
        }

        [Fact]
        public async Task Update_ValidData_ReturnStudent()
        {
            var updateRequest = new UpdateRequestStudent
            {
                Age = 18
            };

            var student = TestStudentFactory.CreateStudent(1);
            student.Age = updateRequest.Age.Value;

            _mock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(student);
            _mock.Setup(repo => repo.Update(It.IsAny<int>(), It.IsAny<UpdateRequestStudent>())).ReturnsAsync(student);

            var result = await _commandService.Update(1, updateRequest);

            Assert.NotNull(result);
            Assert.Equal(student, result);

        }

        [Fact]
        public async Task Delete_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.DeleteById(It.IsAny<int>())).ReturnsAsync((Student)null);

            var exception = await Assert.ThrowsAnyAsync<ItemDoesNotExist>(() => _commandService.Delete(1));

            Assert.Equal(exception.Message, Constants.ItemDoesNotExist);

        }

        [Fact]
        public async Task Delete_ValidData()
        {
            var student = TestStudentFactory.CreateStudent(1);

            _mock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(student);

            var restul = await _commandService.Delete(1);

            Assert.NotNull(restul);
            Assert.Equal(student, restul);
        }

        [Fact]
        public async Task CreateBookForStudent_ItemDoesNotExist()
        {
            var createRequest = new BookCreateDTO
            {
                Created_at = DateTime.Now,
                Name = "test"
            };

            _mock.Setup(repo => repo.CreateBookForStudent(2,createRequest)).ReturnsAsync((Student)null);
            Exception exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _commandService.CreateBookForStudent(2,createRequest));

            Assert.Equal(Constants.ItemDoesNotExist, exception.Message);
        }

        [Fact]
        public async Task CreateBookForStudent_ReturnStudent()
        {
            var createRequest = new BookCreateDTO
            {
                Created_at = DateTime.Now,
                Name = "test"
            };

            var student = TestStudentFactory.CreateStudent(1);

            Book book = new Book();
            book.IdStudent = student.Id;
            book.Created = createRequest.Created_at;
            book.Name = createRequest.Name;
            if(student.StudentBooks == null)
            {
                student.StudentBooks = new List<Book>();
            }
            student.StudentBooks.Add(book);

            _mock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(student);
            _mock.Setup(repo => repo.CreateBookForStudent(It.IsAny<int>(), It.IsAny<BookCreateDTO>())).ReturnsAsync(student);

            var result = await _commandService.CreateBookForStudent(1,createRequest);

            Assert.NotNull(result);
            Assert.Equal(result.StudentBooks[0].Name, createRequest.Name);
        }


        [Fact]
        public async Task DeleteBookForStudent_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.DeleteBookForStudent(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((Student)null);

            var exception = await Assert.ThrowsAnyAsync<ItemDoesNotExist>(() => _commandService.DeleteBookForStudent(1,2));

            Assert.Equal(exception.Message, Constants.ItemDoesNotExist);

        }

        [Fact]
        public async Task DeleteBookForStudent_ValidData()
        {
            var student = TestStudentFactory.CreateStudent(1);

            Book book = new Book();
            book.Id = 2;
            book.IdStudent = 2;
            book.Created = DateTime.Now;
            book.Name = "Test";
            _mock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(student);
            _mock.Setup(repo => repo.Create(It.IsAny<CreateRequestStudent>())).ReturnsAsync(student);

            student.StudentBooks = new List<Book>();

            student.StudentBooks.Add(book);

            var restul = await _commandService.DeleteBookForStudent(1,2);

            Assert.NotNull(restul);
            Assert.Equal(student, restul);
        }

        [Fact]
        public async Task EnrollmentCourse_ItemDoesNotExist()
        {
            var createRequest = new CreateRequestEnrolment
            {
               IdCourse = 1,
               Created = DateTime.Now
            };

            _mock.Setup(repo => repo.EnrollmentCourse(2, createRequest)).ReturnsAsync((Student)null);
            Exception exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _commandService.EnrollmentCourse(2, createRequest));

            Assert.Equal(Constants.ItemDoesNotExist, exception.Message);
        }

        [Fact]
        public async Task EnrollmentCourse_ReturnStudent()
        {

            var createRequest = new CreateRequestEnrolment
            {
                IdCourse = 1,
                Created = DateTime.Now
            };

            var student = TestStudentFactory.CreateStudent(1);

            _mock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(student);
            _mock.Setup(repo => repo.EnrollmentCourse(It.IsAny<int>(), It.IsAny<CreateRequestEnrolment>())).ReturnsAsync(student);

            var result = await _commandService.EnrollmentCourse(1, createRequest);

            Assert.NotNull(result);
            Assert.Equal(result, student);
        }


    }
}
