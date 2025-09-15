using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineSchool.Enrolments.Controllers.interfaces;
using OnlineSchool.Enrolments.Controllers;
using OnlineSchool.Enrolments.Models;
using OnlineSchool.Enrolments.Services.interfaces;
using OnlineSchool.System.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teste.Enrolments.Helpers;
using OnlineSchool.System.Constants;

namespace Teste.Enrolments.UnitTests
{
    public class TestEnrolmentController
    {

        private readonly Mock<IQueryServiceEnrolment> _mockQueryService;
        private readonly ControllerAPIEnrolment enrolmentApiController;

        public TestEnrolmentController()
        {
            _mockQueryService = new Mock<IQueryServiceEnrolment>();

            enrolmentApiController = new ControllerEnrolment(_mockQueryService.Object);
        }

        [Fact]
        public async Task GetAll_ItemsDoNotExist()
        {
            _mockQueryService.Setup(repo => repo.GetAll()).ThrowsAsync(new ItemsDoNotExist(Constants.ItemsDoNotExist));

            var restult = await enrolmentApiController.GetEnrolments();

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(restult.Result);

            Assert.Equal(Constants.ItemsDoNotExist, notFoundResult.Value);
            Assert.Equal(404, notFoundResult.StatusCode);

        }

        [Fact]
        public async Task GetAll_ValidData()
        {
            var enrolments = TestEnrolmentFactory.CreateEnrolments(5);
            _mockQueryService.Setup(repo => repo.GetAll()).ReturnsAsync(enrolments);

            var result = await enrolmentApiController.GetEnrolments();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var allEnrolments = Assert.IsType<List<Enrolment>>(okResult.Value);

            Assert.Equal(5, allEnrolments.Count);
            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async Task GetById_ItemDoesNotExist()
        {
            _mockQueryService.Setup(repo => repo.GetById(50)).ReturnsAsync((Enrolment)null);

            var restult = await enrolmentApiController.GetById(50);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(restult.Result);

            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal(Constants.ItemDoesNotExist, notFoundResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnStudent()
        {
            var enrolment = TestEnrolmentFactory.CreateEnrolment(12);

            _mockQueryService.Setup(repo => repo.GetById(12)).ReturnsAsync(enrolment);

            var result = await enrolmentApiController.GetById(12);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var enrolmentresult = Assert.IsType<Enrolment>(okResult.Value);

            Assert.Equal(13, enrolmentresult.IdStudent);
            Assert.Equal(200, okResult.StatusCode);

        }


    }
}
