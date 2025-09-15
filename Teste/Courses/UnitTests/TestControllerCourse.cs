using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineSchool.Courses.Controllers.interfaces;
using OnlineSchool.Courses.Controllers;
using OnlineSchool.Courses.Dto;
using OnlineSchool.Courses.Models;
using OnlineSchool.Courses.Services.interfaces;
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
    public class TestControllerCourse
    {


        private readonly Mock<ICommandServiceCourse> _mockCommandService;
        private readonly Mock<IQueryServiceCourse> _mockQueryService;
        private readonly ControllerAPICourse courseApiController;

        public TestControllerCourse()
        {
            _mockCommandService = new Mock<ICommandServiceCourse>();
            _mockQueryService = new Mock<IQueryServiceCourse>();

            courseApiController = new ControllerCourse(_mockQueryService.Object, _mockCommandService.Object);
        }

        [Fact]
        public async Task GetAll_ItemsDoNotExist()
        {
            _mockQueryService.Setup(repo => repo.GetAll()).ThrowsAsync(new ItemsDoNotExist(Constants.ItemsDoNotExist));

            var restult = await courseApiController.GetCourses();

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(restult.Result);

            Assert.Equal(Constants.ItemsDoNotExist, notFoundResult.Value);
            Assert.Equal(404, notFoundResult.StatusCode);

        }

        [Fact]
        public async Task GetAll_ValidData()
        {
            var courses = TestCourseFactory.CreateCourses(5);
            _mockQueryService.Setup(repo => repo.GetAll()).ReturnsAsync(courses);

            var result = await courseApiController.GetCourses();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var allCourses = Assert.IsType<List<Course>>(okResult.Value);

            Assert.Equal(5, allCourses.Count);
            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async Task Create_InvalidName()
        {

            var createRequest = new CreateRequestCourse
            {
                Department = "test",
                Name = ""
            };

            _mockCommandService.Setup(repo => repo.Create(It.IsAny<CreateRequestCourse>())).
                ThrowsAsync(new InvalidName(Constants.InvalidName));

            var result = await courseApiController.CreateCourse(createRequest);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);

            Assert.Equal(400, badRequest.StatusCode);
            Assert.Equal(Constants.InvalidName, badRequest.Value);

        }

        [Fact]
        public async Task Create_ValidData()
        {
            var createRequest = new CreateRequestCourse
            {
                Department = "test",
                Name = "test"
            };
            var course = TestCourseFactory.CreateCourse(1);
            course.Department = createRequest.Department;
            course.Name = createRequest.Name;

            _mockCommandService.Setup(repo => repo.Create(It.IsAny<CreateRequestCourse>())).ReturnsAsync(course);

            var result = await courseApiController.CreateCourse(createRequest);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(okResult.StatusCode, 200);
            Assert.Equal(okResult.Value, course);

        }

        [Fact]
        public async Task Update_ItemDoesNotExist()
        {
            var update = new UpdateRequestCourse
            {
                Name = "test"
            };

            _mockCommandService.Setup(repo => repo.Update(1, update)).ThrowsAsync(new ItemDoesNotExist(Constants.ItemDoesNotExist));

            var result = await courseApiController.UpdateCourse(1, update);

            var ntFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(ntFound.StatusCode, 404);
            Assert.Equal(Constants.ItemDoesNotExist, ntFound.Value);

        }
        [Fact]
        public async Task Update_ValidData()
        {
            var update = new UpdateRequestCourse
            {
                Name = "test"
            };


            var course = TestCourseFactory.CreateCourse(1);

            _mockCommandService.Setup(repo => repo.Update(It.IsAny<int>(), It.IsAny<UpdateRequestCourse>())).ReturnsAsync(course);

            var result = await courseApiController.UpdateCourse(1, update);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(okResult.StatusCode, 200);
            Assert.Equal(okResult.Value, course);

        }

        [Fact]
        public async Task Delete_ItemDoesNotExist()
        {
            _mockCommandService.Setup(repo => repo.Delete(1)).ThrowsAsync(new ItemDoesNotExist(Constants.ItemDoesNotExist));

            var result = await courseApiController.DeleteCourse(1);

            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);

            Assert.Equal(notFound.StatusCode, 404);
            Assert.Equal(notFound.Value, Constants.ItemDoesNotExist);

        }

        [Fact]
        public async Task Delete_ValidData()
        {

            var course = TestCourseFactory.CreateCourse(1);

            _mockCommandService.Setup(repo => repo.Delete(It.IsAny<int>())).ReturnsAsync(course);

            var result = await courseApiController.DeleteCourse(1);

            var okresult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(200, okresult.StatusCode);
            Assert.Equal(okresult.Value, course);

        }

    }
}
