using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Books.Models;
using OnlineSchool.StudentCards.Controllers.interfaces;
using OnlineSchool.StudentCards.Models;
using OnlineSchool.StudentCards.Services.interfaces;
using OnlineSchool.System.Constants;
using OnlineSchool.System.Exceptions;

namespace OnlineSchool.StudentCards.Controllers
{
    public class ControllerStudentCards : ControllerAPIStudentCards
    {
        private IQueryServiceStudentCard _queryService;

        public ControllerStudentCards(IQueryServiceStudentCard queryService)
        {
            _queryService = queryService;
        }

        public override async Task<ActionResult<List<StudentCard>>> GetStudentCards()
        {
            try
            {
                var students = await _queryService.GetAll();

                return Ok(students);

            }
            catch (ItemsDoNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        public override async Task<ActionResult<StudentCard>> GetByName([FromQuery] string name)
        {

            try
            {
                var student = await _queryService.GetByNameAsync(name);
                if (student == null)
                {
                    throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
                }
                return Ok(student);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }

        }

        public override async Task<ActionResult<StudentCard>> GetById([FromQuery] int id)
        {

            try
            {
                var student = await _queryService.GetById(id);
                if (student == null)
                {
                    throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
                }
                return Ok(student);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }

        }


    }
}
