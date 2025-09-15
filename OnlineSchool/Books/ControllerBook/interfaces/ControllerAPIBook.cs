using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Books.Models;

namespace OnlineSchool.Books.Controllers.interfaces
{
    [ApiController]
    [Route("api/v1/books/")]
    [Route("api/v1/ControllerBook/")]
    public abstract class ControllerAPIBook : ControllerBase
    {

        [HttpGet("all")]
        [Authorize(Policy = "Read")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<Book>))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<List<Book>>> GetBooks();


        [HttpGet("findById")]
        [Authorize(Policy = "Read")]
        [ProducesResponseType(statusCode: 200, type: typeof(Book))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<Book>> GetById([FromQuery] string id);

        [HttpGet("findByName")]
        [Authorize(Policy = "Read")]
        [ProducesResponseType(statusCode: 200, type: typeof(Book))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<Book>> GetByName([FromQuery] string name);


    }
}
