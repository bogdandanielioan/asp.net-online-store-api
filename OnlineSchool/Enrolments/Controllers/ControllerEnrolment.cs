using Microsoft.AspNetCore.Mvc;
using OnlineSchool.Enrolments.Dto;
using OnlineSchool.Enrolments.Models;
using OnlineSchool.Enrolments.Services.interfaces;
using OnlineSchool.Enrolments.Controllers.interfaces;
using OnlineSchool.System.Exceptions;
using OnlineSchool.Students.Models;
using OnlineSchool.System.Constants;

namespace OnlineSchool.Enrolments.Controllers
{
    public class ControllerEnrolment : ControllerAPIEnrolment
    {
        private IQueryServiceEnrolment _queryService;

        public ControllerEnrolment(IQueryServiceEnrolment queryService)
        {
            _queryService = queryService;
        }

        public override async Task<ActionResult<List<Enrolment>>> GetEnrolments()
        {
            try
            {
                var enromlemts = await _queryService.GetAll();

                return Ok(enromlemts);

            }
            catch (ItemsDoNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        public override async Task<ActionResult<Enrolment>> GetById([FromQuery] int id)
        {

            try
            {
                var enromlemt = await _queryService.GetById(id);
                if (enromlemt == null)
                {
                    throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
                }
                return Ok(enromlemt);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }

        }

    }
}
