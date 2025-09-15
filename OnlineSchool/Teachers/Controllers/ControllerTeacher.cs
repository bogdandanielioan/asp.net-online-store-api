using Microsoft.AspNetCore.Mvc;
using OnlineSchool.System.Constants;
using OnlineSchool.System.Exceptions;
using OnlineSchool.Teachers.Controllers.interfaces;
using OnlineSchool.Teachers.Dto;
using OnlineSchool.Teachers.Models;
using OnlineSchool.Teachers.Services.interfaces;

namespace OnlineSchool.Teachers.Controllers
{
    public class ControllerTeacher : ControllerAPITeacher
    {
        private readonly IQueryServiceTeacher _queryService;
        private readonly ICommandServiceTeacher _commandService;

        public ControllerTeacher(IQueryServiceTeacher queryService, ICommandServiceTeacher commandService)
        {
            _queryService = queryService;
            _commandService = commandService;
        }

        private static DtoTeacherView MapToDto(Teacher t)
        {
            return new DtoTeacherView
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Subject = t.Subject,
                UpdateDate = t.UpdateDate,
                Role = t.Role
            };
        }

        public override async Task<ActionResult<List<DtoTeacherView>>> GetTeachers()
        {
            try
            {
                var items = await _queryService.GetAllAsync();
                var dtos = items.Select(MapToDto).ToList();
                return Ok(dtos);
            }
            catch (ItemsDoNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        public override async Task<ActionResult<DtoTeacherView>> GetById([FromQuery] string id)
        {
            try
            {
                var item = await _queryService.GetByIdAsync(id);
                return Ok(MapToDto(item));
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        public override async Task<ActionResult<DtoTeacherView>> GetByEmail([FromQuery] string email)
        {
            try
            {
                var item = await _queryService.GetByEmailAsync(email);
                return Ok(MapToDto(item));
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        public override async Task<ActionResult<DtoTeacherView>> Create([FromBody] TeacherCreateRequest request)
        {
            try
            {
                var teacher = new Teacher
                {
                    Name = request.Name,
                    Email = request.Email,
                    Subject = request.Subject,
                    Password = request.Password
                };
                var created = await _commandService.CreateAsync(teacher);
                return Ok(MapToDto(created));
            }
            catch (InvalidName ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public override async Task<ActionResult<DtoTeacherView>> Update([FromQuery] string id, [FromBody] TeacherUpdateRequest request)
        {
            try
            {
                var teacher = new Teacher
                {
                    Name = request.Name,
                    Email = request.Email,
                    Subject = request.Subject
                };
                var updated = await _commandService.UpdateAsync(id, teacher);
                return Ok(MapToDto(updated));
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidName ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public override async Task<ActionResult<DtoTeacherView>> Delete([FromQuery] string id)
        {
            try
            {
                var deleted = await _commandService.DeleteAsync(id);
                return Ok(MapToDto(deleted));
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
