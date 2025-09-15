using Moq;
using OnlineSchool.Courses.Models;
using OnlineSchool.Courses.Repository.interfaces;
using OnlineSchool.Courses.Services.interfaces;
using OnlineSchool.Courses.Services;
using OnlineSchool.System.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teste.Courses.Helpers;
using OnlineSchool.System.Constants;

namespace Teste.Courses.UnitTests
{
    public  class TestCourseQueryService
    {
        private readonly Mock<IRepositoryCourse> _mock;
        private readonly IQueryServiceCourse _courseQueryService;

        public TestCourseQueryService()
        {
            _mock = new Mock<IRepositoryCourse>();
            _courseQueryService = new QueryServiceCourse(_mock.Object);

        }

        [Fact]
        public async Task GetAllCourses_ThrowItemsDoNoeExistException()
        {
            _mock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Course>());

            var exception = await Assert.ThrowsAsync<ItemsDoNotExist>(() => _courseQueryService.GetAll());

            Assert.Equal(Constants.ItemsDoNotExist, exception.Message);
        }

        [Fact]
        public async Task GetAllCourses_ReturnAllCourses()
        {
            var courses = TestCourseFactory.CreateCourses(5);
            _mock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(courses);


            var result = await _courseQueryService.GetAll();

            Assert.NotNull(result);
            Assert.Contains(courses[1], result);

        }

        [Fact]
        public async Task GetById_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.GetByIdAsync(50)).ReturnsAsync((Course)null);

            var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _courseQueryService.GetById(50));

            Assert.Equal(Constants.ItemDoesNotExist, exception.Message);
        }

        [Fact]
        public async Task GetById_ReturnCourse()
        {
            var course = TestCourseFactory.CreateCourse(12);

            _mock.Setup(repo => repo.GetByIdAsync(12)).ReturnsAsync(course);

            var result = await _courseQueryService.GetById(12);

            Assert.NotNull(result);
            Assert.Equal(course, result);

        }

        [Fact]
        public async Task GetByName_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.GetByNameAsync("test")).ReturnsAsync((Course)null);
            var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _courseQueryService.GetByNameAsync("test"));

            Assert.Equal(Constants.ItemDoesNotExist, exception.Message);
        }

        [Fact]
        public async Task GetByName_ReturnCourse()
        {
            var course = TestCourseFactory.CreateCourse(5);
            _mock.Setup(repo => repo.GetByNameAsync("test5")).ReturnsAsync(course);
            var result = await _courseQueryService.GetByNameAsync("test5");

            Assert.NotNull(result);
            Assert.Equal(course, result);

        }

    }
}
