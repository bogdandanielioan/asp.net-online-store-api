using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineSchool.Students.Controllers;
using OnlineSchool.Students.Controllers.interfaces;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Models;
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
    public class TestControllerStudent
    {

        private readonly Mock<ICommandServiceStudent> _mockCommandService;
        private readonly Mock<IQueryServiceStudent> _mockQueryService;
        private readonly ControllerAPIStudent studentApiController;

        public TestControllerStudent()
        {
            _mockCommandService = new Mock<ICommandServiceStudent>();
            _mockQueryService = new Mock<IQueryServiceStudent>();

            studentApiController = new ControllerStudent(_mockQueryService.Object, _mockCommandService.Object);
        }

        [Fact]
        public async Task GetAll_ItemsDoNotExist()
        {
            _mockQueryService.Setup(repo => repo.GetAll()).ThrowsAsync(new ItemsDoNotExist(Constants.ItemsDoNotExist));

            var restult = await studentApiController.GetStudents();

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(restult.Result);

            Assert.Equal(Constants.ItemsDoNotExist, notFoundResult.Value);
            Assert.Equal(404, notFoundResult.StatusCode);

        }

        [Fact]
        public async Task GetAll_ValidData()
        {
            var students = TestStudentFactory.CreateStudents(5);
            _mockQueryService.Setup(repo => repo.GetAll()).ReturnsAsync(students);

            var result = await studentApiController.GetStudents();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var allStudents = Assert.IsType<List<Student>>(okResult.Value);

            Assert.Equal(5, allStudents.Count);
            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async Task Create_InvalidAge()
        {

            var createRequest = new CreateRequestStudent
            {
                Age = 0,
                Email = "test@gmail.com",
                Name = "test"
            };

            _mockCommandService.Setup(repo => repo.Create(It.IsAny<CreateRequestStudent>())).
                ThrowsAsync(new InvalidAge(Constants.InvalidAge));

            var result = await studentApiController.CreateStudent(createRequest);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);

            Assert.Equal(400, badRequest.StatusCode);
            Assert.Equal(Constants.InvalidAge, badRequest.Value);

        }

        [Fact]
        public async Task Create_ValidData()
        {
            var createRequest = new CreateRequestStudent
            {
                Age = 18,
                Email = "test@gmail.com",
                Name = "test"
            };
            var student = TestStudentFactory.CreateStudent(1);
            student.Age = createRequest.Age;
            student.Email = createRequest.Email;
            student.Name = createRequest.Name;

            _mockCommandService.Setup(repo => repo.Create(It.IsAny<CreateRequestStudent>())).ReturnsAsync(student);

            var result = await studentApiController.CreateStudent(createRequest);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(okResult.StatusCode, 200);
            Assert.Equal(okResult.Value, student);

        }

        [Fact]
        public async Task Update_ItemDoesNotExist()
        {
            var update = new UpdateRequestStudent
            {
                Age = 0
            };

            _mockCommandService.Setup(repo => repo.Update(1, update)).ThrowsAsync(new ItemDoesNotExist(Constants.ItemDoesNotExist));

            var result = await studentApiController.UpdateStudent(1, update);

            var ntFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(ntFound.StatusCode, 404);
            Assert.Equal(Constants.ItemDoesNotExist, ntFound.Value);

        }
        [Fact]
        public async Task Update_ValidData()
        {
            var update = new UpdateRequestStudent
            {
                Age = 19
            };


            var student = TestStudentFactory.CreateStudent(1);

            _mockCommandService.Setup(repo => repo.Update(It.IsAny<int>(), It.IsAny<UpdateRequestStudent>())).ReturnsAsync(student);

            var result = await studentApiController.UpdateStudent(1, update);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(okResult.StatusCode, 200);
            Assert.Equal(okResult.Value, student);

        }

        [Fact]
        public async Task Delete_ItemDoesNotExist()
        {
            _mockCommandService.Setup(repo => repo.Delete(1)).ThrowsAsync(new ItemDoesNotExist(Constants.ItemDoesNotExist));

            var result = await studentApiController.DeleteStudent(1);

            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);

            Assert.Equal(notFound.StatusCode, 404);
            Assert.Equal(notFound.Value, Constants.ItemDoesNotExist);

        }

        [Fact]
        public async Task Delete_ValidData()
        {

            var student = TestStudentFactory.CreateStudent(1);

            _mockCommandService.Setup(repo => repo.Delete(It.IsAny<int>())).ReturnsAsync(student);

            var result = await studentApiController.DeleteStudent(1);

            var okresult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(200, okresult.StatusCode);
            Assert.Equal(okresult.Value, student);

        }


    }
}
