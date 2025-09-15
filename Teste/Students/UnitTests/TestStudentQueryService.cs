using Moq;
using OnlineSchool.StudentCards.Models;
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

namespace Teste.Students.UnitTests
{
    public class TestStudentQueryService
    {
        private readonly Mock<IRepositoryStudent> _mock;
        private readonly IQueryServiceStudent _studentQueryService;

        public TestStudentQueryService()
        {
            _mock = new Mock<IRepositoryStudent>();
            _studentQueryService = new QueryServiceStudents(_mock.Object);

        }

        [Fact]
        public async Task GetAllStudents_ThrowItemsDoNoeExistException()
        {
            _mock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Student>());

            var exception = await Assert.ThrowsAsync<ItemsDoNotExist>(() => _studentQueryService.GetAll());

            Assert.Equal(Constants.ItemsDoNotExist, exception.Message);
        }

        [Fact]
        public async Task GetAllStudents_ReturnAllStudents()
        {
            var students = TestStudentFactory.CreateStudents(5);
            _mock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(students);


            var result = await _studentQueryService.GetAll();

            Assert.NotNull(result);
            Assert.Contains(students[1], result);

        }

        [Fact]
        public async Task GetById_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.GetByIdAsync(50)).ReturnsAsync((Student)null);

            var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _studentQueryService.GetById(50));

            Assert.Equal(Constants.ItemDoesNotExist, exception.Message);
        }

        [Fact]
        public async Task GetById_ReturnStudent()
        {
            var student = TestStudentFactory.CreateStudent(12);

            _mock.Setup(repo => repo.GetByIdAsync(12)).ReturnsAsync(student);

            var result = await _studentQueryService.GetById(12);

            Assert.NotNull(result);
            Assert.Equal(student, result);

        }

        [Fact]
        public async Task GetByName_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.GetByNameAsync("test")).ReturnsAsync((Student)null);
            var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _studentQueryService.GetByNameAsync("test"));

            Assert.Equal(Constants.ItemDoesNotExist, exception.Message);
        }

        [Fact]
        public async Task GetByName_ReturnStudent()
        {
            var student = TestStudentFactory.CreateStudent(5);
            _mock.Setup(repo => repo.GetByNameAsync("test5")).ReturnsAsync(student);
            var result = await _studentQueryService.GetByNameAsync("test5");

            Assert.NotNull(result);
            Assert.Equal(student, result);

        }

        [Fact]
        public async Task CardById_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.CardByIdAsync(1)).ReturnsAsync((StudentCard)null);
            var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _studentQueryService.CardById(1));

            Assert.Equal(Constants.ItemDoesNotExist, exception.Message);
        }

        [Fact]
        public async Task CardById_ReturnStudent()
        {
            var student = TestStudentFactory.CreateStudentCard(5);
            _mock.Setup(repo => repo.CardByIdAsync(5)).ReturnsAsync(student);
            var result = await _studentQueryService.CardById(5);

            Assert.NotNull(result);
            Assert.Equal(student, result);

        }

    }
}
