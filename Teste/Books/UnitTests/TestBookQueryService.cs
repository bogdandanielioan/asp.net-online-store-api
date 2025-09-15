using Moq;
using OnlineSchool.Books.Models;
using OnlineSchool.Books.Repository.interfaces;
using OnlineSchool.Books.Services;
using OnlineSchool.Books.Services.interfaces;
using OnlineSchool.System.Constants;
using OnlineSchool.System.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teste.Books.Helpers;

namespace Teste.Books.UnitTests
{
    public class TestBookQueryService
    {
        private readonly Mock<IRepositoryBook> _mock;
        private readonly IQueryServiceBook _bookQueryService;

        public TestBookQueryService()
        {
            _mock = new Mock<IRepositoryBook>();
            _bookQueryService = new QueryServiceBook(_mock.Object);

        }

        [Fact]
        public async Task GetAllBooks_ThrowItemsDoNoeExistException()
        {
            _mock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Book>());

            var exception = await Assert.ThrowsAsync<ItemsDoNotExist>(() => _bookQueryService.GetAllAsync());

            Assert.Equal(Constants.ItemsDoNotExist, exception.Message);
        }

        [Fact]
        public async Task GetAllStudents_ReturnAllStudents()
        {
            var books = TestBookFactory.CreateBooks(5);
            _mock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(books);


            var result = await _bookQueryService.GetAllAsync();

            Assert.NotNull(result);
            Assert.Contains(books[1], result);

        }

        [Fact]
        public async Task GetById_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.GetByIdAsync(50)).ReturnsAsync((Book)null);

            var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _bookQueryService.GetByIdAsync(50));

            Assert.Equal(Constants.ItemDoesNotExist, exception.Message);
        }

        [Fact]
        public async Task GetById_ReturnStudent()
        {
            var book = TestBookFactory.CreateBook(12);

            _mock.Setup(repo => repo.GetByIdAsync(12)).ReturnsAsync(book);

            var result = await _bookQueryService.GetByIdAsync(12);

            Assert.NotNull(result);
            Assert.Equal(book, result);

        }

        [Fact]
        public async Task GetByName_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.GetByNameAsync("test")).ReturnsAsync((Book)null);
            var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _bookQueryService.GetByNameAsync("test"));

            Assert.Equal(Constants.ItemDoesNotExist, exception.Message);
        }

        [Fact]
        public async Task GetByName_ReturnStudent()
        {
            var book = TestBookFactory.CreateBook(5);
            _mock.Setup(repo => repo.GetByNameAsync("test5")).ReturnsAsync(book);
            var result = await _bookQueryService.GetByNameAsync("test5");

            Assert.NotNull(result);
            Assert.Equal(book, result);

        }




    }
}
