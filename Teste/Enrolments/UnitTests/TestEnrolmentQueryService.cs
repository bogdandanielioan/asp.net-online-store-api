using Moq;
using OnlineSchool.Enrolments.Models;
using OnlineSchool.Enrolments.Repository.interfaces;
using OnlineSchool.Enrolments.Services;
using OnlineSchool.Enrolments.Services.interfaces;
using OnlineSchool.System.Constants;
using OnlineSchool.System.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teste.Enrolments.Helpers;

namespace Teste.Enrolments.UnitTests
{
    public class TestEnrolmentQueryService
    {

        private readonly Mock<IRepositoryEnrolment> _mock;
        private readonly IQueryServiceEnrolment _enrolmentQueryService;

        public TestEnrolmentQueryService()
        {
            _mock = new Mock<IRepositoryEnrolment>();
            _enrolmentQueryService = new QueryServiceEnrolment(_mock.Object);

        }

        [Fact]
        public async Task GetAllEnrolments_ThrowItemsDoNoeExistException()
        {
            _mock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Enrolment>());

            var exception = await Assert.ThrowsAsync<ItemsDoNotExist>(() => _enrolmentQueryService.GetAll());

            Assert.Equal(Constants.ItemsDoNotExist, exception.Message);
        }

        [Fact]
        public async Task GetAllStudents_ReturnAllStudents()
        {
            var enrolments = TestEnrolmentFactory.CreateEnrolments(5);
            _mock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(enrolments);


            var result = await _enrolmentQueryService.GetAll();

            Assert.NotNull(result);
            Assert.Contains(enrolments[1], result);

        }

        [Fact]
        public async Task GetById_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.GetByIdAsync(50)).ReturnsAsync((Enrolment)null);

            var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _enrolmentQueryService.GetById(50));

            Assert.Equal(Constants.ItemDoesNotExist, exception.Message);
        }

        [Fact]
        public async Task GetById_ReturnStudent()
        {
            var enrolment = TestEnrolmentFactory.CreateEnrolment(12);

            _mock.Setup(repo => repo.GetByIdAsync(12)).ReturnsAsync(enrolment);

            var result = await _enrolmentQueryService.GetById(12);

            Assert.NotNull(result);
            Assert.Equal(enrolment, result);

        }


    }
}
