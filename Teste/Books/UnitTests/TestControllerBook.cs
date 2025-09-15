using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineSchool.Books.Controllers;
using OnlineSchool.Books.Controllers.interfaces;
using OnlineSchool.Books.Models;
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
    public class TestControllerBook
    {

        private readonly Mock<IQueryServiceBook> _mockQueryService;
        private readonly ControllerAPIBook bookApiController;

        public TestControllerBook()
        {
            _mockQueryService = new Mock<IQueryServiceBook>();

            bookApiController = new ControllerBook(_mockQueryService.Object);
        }

        [Fact]
        public async Task GetAll_ItemsDoNotExist()
        {
            _mockQueryService.Setup(repo => repo.GetAllAsync()).ThrowsAsync(new ItemsDoNotExist(Constants.ItemsDoNotExist));

            var restult = await bookApiController.GetBooks();

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(restult.Result);

            Assert.Equal(Constants.ItemsDoNotExist, notFoundResult.Value);
            Assert.Equal(404, notFoundResult.StatusCode);

        }

        [Fact]
        public async Task GetAll_ValidData()
        {
            var books = TestBookFactory.CreateBooks(5);
            _mockQueryService.Setup(repo => repo.GetAllAsync()).ReturnsAsync(books);

            var result = await bookApiController.GetBooks();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var allBooks = Assert.IsType<List<Book>>(okResult.Value);

            Assert.Equal(5, allBooks.Count);
            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async Task GetById_ItemDoesNotExist()
        {
            _mockQueryService.Setup(repo => repo.GetByIdAsync(50)).ReturnsAsync((Book)null);

            var restult = await bookApiController.GetById(50);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(restult.Result);

            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal(Constants.ItemDoesNotExist, notFoundResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnStudent()
        {
            var book = TestBookFactory.CreateBook(12);

            _mockQueryService.Setup(repo => repo.GetByIdAsync(12)).ReturnsAsync(book);

            var result = await bookApiController.GetById(12);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var bookresult = Assert.IsType<Book>(okResult.Value);

            Assert.Equal("test12", bookresult.Name);
            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async Task GetByName_ItemDoesNotExist()
        {
            _mockQueryService.Setup(repo => repo.GetByNameAsync("12")).ReturnsAsync((Book)null);

            var restult = await bookApiController.GetByName("12");

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(restult.Result);

            Assert.Equal(Constants.ItemDoesNotExist, notFoundResult.Value);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetByName_ReturnStudent()
        {
            var book = TestBookFactory.CreateBook(12);

            _mockQueryService.Setup(repo => repo.GetByNameAsync("test12")).ReturnsAsync(book);

            var result = await bookApiController.GetByName("test12");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var bookres = Assert.IsType<Book>(okResult.Value);

            Assert.Equal("test12", bookres.Name);
            Assert.Equal(200, okResult.StatusCode);
        }



    }
}
