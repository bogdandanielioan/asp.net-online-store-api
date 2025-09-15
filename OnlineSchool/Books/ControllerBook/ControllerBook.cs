using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Books.Controllers.interfaces;
using OnlineSchool.Books.Models;
using OnlineSchool.Books.Services.interfaces;
using OnlineSchool.System.Constants;
using OnlineSchool.System.Exceptions;

namespace OnlineSchool.Books.Controllers
{
    public class ControllerBook : ControllerAPIBook
    {

        private IQueryServiceBook _query;

        public ControllerBook(IQueryServiceBook query)
        {
            _query = query;
        }


        public override async Task<ActionResult<List<Book>>> GetBooks()
        {
            try
            {
                var book = await _query.GetAllAsync();

                return Ok(book);
            }
            catch (ItemsDoNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }
        public override async Task<ActionResult<Book>> GetById([FromQuery] int id)
        {
            try
            {
                var book = await _query.GetByIdAsync(id);

                if (book == null)
                {
                    throw new ItemDoesNotExist(Constants.ItemDoesNotExist); 
                }
                return Ok(book);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        public override async Task<ActionResult<Book>> GetByName([FromQuery] string name)
        {
            try
            {
                var book = await _query.GetByNameAsync(name);
                if (book == null)
                {
                    throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
                }
                return Ok(book);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
