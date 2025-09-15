using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineSchool.StudentCards.Controllers;
using OnlineSchool.StudentCards.Controllers.interfaces;
using OnlineSchool.StudentCards.Models;
using OnlineSchool.StudentCards.Services.interfaces;
using OnlineSchool.System.Constants;
using OnlineSchool.System.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.StudentCards.Helpers;

namespace Teste.StudentCards.UnitTests
{
    public class TestControllerStudentCardCard
    {

        private readonly Mock<IQueryServiceStudentCard> _mockQueryService;
        private readonly ControllerAPIStudentCards studentApiController;

        public TestControllerStudentCardCard()
        {
            _mockQueryService = new Mock<IQueryServiceStudentCard>();

            studentApiController = new ControllerStudentCards(_mockQueryService.Object);
        }

        [Fact]
        public async Task GetAll_ItemsDoNotExist()
        {
            _mockQueryService.Setup(repo => repo.GetAll()).ThrowsAsync(new ItemsDoNotExist(Constants.ItemsDoNotExist));

            var restult = await studentApiController.GetStudentCards();

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(restult.Result);

            Assert.Equal(Constants.ItemsDoNotExist, notFoundResult.Value);
            Assert.Equal(404, notFoundResult.StatusCode);

        }

        [Fact]
        public async Task GetAll_ValidData()
        {
            var students = TestStudentCardFactory.CreateStudentCards(5);
            _mockQueryService.Setup(repo => repo.GetAll()).ReturnsAsync(students);

            var result = await studentApiController.GetStudentCards();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var allStudentCards = Assert.IsType<List<StudentCard>>(okResult.Value);

            Assert.Equal(5, allStudentCards.Count);
            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async Task GetById_ItemDoesNotExist()
        {
            _mockQueryService.Setup(repo => repo.GetById(50)).ReturnsAsync((StudentCard)null);

            var restult = await studentApiController.GetById(50);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(restult.Result);

            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal(Constants.ItemDoesNotExist, notFoundResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnStudent()
        {
            var student = TestStudentCardFactory.CreateStudentCard(12);

            _mockQueryService.Setup(repo => repo.GetById(12)).ReturnsAsync(student);

            var result = await studentApiController.GetById(12);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var studentCard = Assert.IsType<StudentCard>(okResult.Value);

            Assert.Equal("121234", studentCard.Namecard);
            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async Task GetByName_ItemDoesNotExist()
        {
            _mockQueryService.Setup(repo => repo.GetByNameAsync("12")).ReturnsAsync((StudentCard)null);

            var restult = await studentApiController.GetByName("12");

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(restult.Result);

            Assert.Equal(Constants.ItemDoesNotExist, notFoundResult.Value);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetByName_ReturnStudent()
        {
            var student = TestStudentCardFactory.CreateStudentCard(12);

            _mockQueryService.Setup(repo => repo.GetByNameAsync("121234")).ReturnsAsync(student);

            var result = await studentApiController.GetByName("121234");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var studentCard = Assert.IsType<StudentCard>(okResult.Value);

            Assert.Equal("121234", studentCard.Namecard);
            Assert.Equal(200, okResult.StatusCode);
        }


    }
}
