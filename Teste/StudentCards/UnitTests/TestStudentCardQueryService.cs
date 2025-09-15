using Moq;
using OnlineSchool.StudentCards.Models;
using OnlineSchool.StudentCards.Repository.interfaces;
using OnlineSchool.StudentCards.Services;
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
    public class TestStudentCardQueryService
    {
        private readonly Mock<IRepositoryStudentCard> _mock;
        private readonly IQueryServiceStudentCard _studentQueryService;

        public TestStudentCardQueryService()
        {
            _mock = new Mock<IRepositoryStudentCard>();
            _studentQueryService = new QueryServiceStudentCards(_mock.Object);

        }

        [Fact]
        public async Task GetAllStudentCards_ThrowItemsDoNoeExistException()
        {
            _mock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<StudentCard>());

            var exception = await Assert.ThrowsAsync<ItemsDoNotExist>(() => _studentQueryService.GetAll());

            Assert.Equal(Constants.ItemsDoNotExist, exception.Message);
        }

        [Fact]
        public async Task GetAllStudents_ReturnAllStudents()
        {
            var students = TestStudentCardFactory.CreateStudentCards(5);
            _mock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(students);


            var result = await _studentQueryService.GetAll();

            Assert.NotNull(result);
            Assert.Contains(students[1], result);

        }

        [Fact]
        public async Task GetById_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.GetByIdAsync(50)).ReturnsAsync((StudentCard)null);

            var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _studentQueryService.GetById(50));

            Assert.Equal(Constants.ItemDoesNotExist, exception.Message);
        }

        [Fact]
        public async Task GetById_ReturnStudent()
        {
            var student = TestStudentCardFactory.CreateStudentCard(12);

            _mock.Setup(repo => repo.GetByIdAsync(12)).ReturnsAsync(student);

            var result = await _studentQueryService.GetById(12);

            Assert.NotNull(result);
            Assert.Equal(student, result);

        }

        [Fact]
        public async Task GetByName_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.GetByNameAsync("test")).ReturnsAsync((StudentCard)null);
            var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _studentQueryService.GetByNameAsync("test"));

            Assert.Equal(Constants.ItemDoesNotExist, exception.Message);
        }

        [Fact]
        public async Task GetByName_ReturnStudent()
        {
            var student = TestStudentCardFactory.CreateStudentCard(5);
            _mock.Setup(repo => repo.GetByNameAsync("test5")).ReturnsAsync(student);
            var result = await _studentQueryService.GetByNameAsync("test5");

            Assert.NotNull(result);
            Assert.Equal(student, result);

        }

    }
}
