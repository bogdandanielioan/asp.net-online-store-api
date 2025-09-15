using Moq;
using OnlineSchool.Books.Models;
using OnlineSchool.Enrolments.Dto;
using OnlineSchool.Courses.Dto;
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
    public class TestCourseCommandService
    {

        private readonly Mock<IRepositoryCourse> _mock;
        private readonly ICommandServiceCourse _commandService;

        public TestCourseCommandService()
        {
            _mock = new Mock<IRepositoryCourse>();
            _commandService = new CommandServiceCourse(_mock.Object);
        }

        [Fact]
        public async Task CreateCourse_InvalidName()
        {
            var createRequest = new CreateRequestCourse
            {
                Department = "test",
                Name = ""
            };

            _mock.Setup(repo => repo.Create(createRequest)).ReturnsAsync((Course)null);
            Exception exception = await Assert.ThrowsAsync<InvalidName>(() => _commandService.Create(createRequest));

            Assert.Equal(Constants.InvalidName, exception.Message);
        }

        [Fact]
        public async Task CreateCourse_ReturnCourse()
        {
            var createRequest = new CreateRequestCourse
            {
                Department = "test",
                Name = "test"
            };

            var course = TestCourseFactory.CreateCourse(50);
            course.Name = createRequest.Name;

            _mock.Setup(repo => repo.Create(It.IsAny<CreateRequestCourse>())).ReturnsAsync(course);

            var result = await _commandService.Create(createRequest);

            Assert.NotNull(result);
            Assert.Equal(result.Name, createRequest.Name);
        }

        [Fact]
        public async Task Update_ItemDoesNotExist()
        {
            var updateRequest = new UpdateRequestCourse
            {
                Name = "test"
            };

            _mock.Setup(repo => repo.GetByIdAsync(50)).ReturnsAsync((Course)null);

            var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _commandService.Update(50, updateRequest));

            Assert.Equal(Constants.ItemDoesNotExist, exception.Message);
        }

        [Fact]
        public async Task Update_InvalidName()
        {
            var updateRequest = new UpdateRequestCourse
            {
                Name = ""
            };

            var course = TestCourseFactory.CreateCourse(1);
            course.Name = updateRequest.Name;
            _mock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(course);

            var exception = await Assert.ThrowsAsync<InvalidName>(() => _commandService.Update(1, updateRequest));

            Assert.Equal(Constants.InvalidName, exception.Message);
        }

        [Fact]
        public async Task Update_ValidData_ReturnCourse()
        {
            var updateRequest = new UpdateRequestCourse
            {
                Name = "test"
            };

            var course = TestCourseFactory.CreateCourse(1);
            course.Name = updateRequest.Name;

            _mock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(course);
            _mock.Setup(repo => repo.Update(It.IsAny<int>(), It.IsAny<UpdateRequestCourse>())).ReturnsAsync(course);

            var result = await _commandService.Update(1, updateRequest);

            Assert.NotNull(result);
            Assert.Equal(course, result);

        }

        [Fact]
        public async Task Delete_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.DeleteById(It.IsAny<int>())).ReturnsAsync((Course)null);

            var exception = await Assert.ThrowsAnyAsync<ItemDoesNotExist>(() => _commandService.Delete(1));

            Assert.Equal(exception.Message, Constants.ItemDoesNotExist);

        }

        [Fact]
        public async Task Delete_ValidData()
        {
            var course = TestCourseFactory.CreateCourse(1);

            _mock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(course);

            var restul = await _commandService.Delete(1);

            Assert.NotNull(restul);
            Assert.Equal(course, restul);
        }

    }
}
