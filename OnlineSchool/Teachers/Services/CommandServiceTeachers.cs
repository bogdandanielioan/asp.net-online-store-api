using OnlineSchool.Auth.Models;
using OnlineSchool.System.Constants;
using OnlineSchool.System.Exceptions;
using OnlineSchool.System.Id;
using OnlineSchool.Teachers.Models;
using OnlineSchool.Teachers.Repository.interfaces;
using OnlineSchool.Teachers.Services.interfaces;

namespace OnlineSchool.Teachers.Services
{
    public class CommandServiceTeachers : ICommandServiceTeacher
    {
        private readonly IRepositoryTeacher _repository;
        public CommandServiceTeachers(IRepositoryTeacher repository)
        {
            _repository = repository;
        }

        public async Task<Teacher> CreateAsync(Teacher teacher)
        {
            if (string.IsNullOrWhiteSpace(teacher.Name))
                throw new InvalidName(Constants.InvalidName);
            if (string.IsNullOrWhiteSpace(teacher.Email))
                throw new InvalidName(Constants.InvalidName);

            teacher.Id = IdGenerator.New("teacher");
            teacher.UpdateDate = DateTime.UtcNow;
            // Assign default role to all teachers
            teacher.Role = string.IsNullOrWhiteSpace(teacher.Role) ? SystemRoles.Admin : SystemRoles.Normalize(teacher.Role);

            // Hash and store password if provided
            if (!string.IsNullOrWhiteSpace(teacher.Password))
            {
                var hp = OnlineSchool.Auth.Services.PasswordHasher.HashPassword(teacher.Password);
                teacher.PasswordHash = hp.Hash;
                teacher.PasswordSalt = hp.Salt;
                teacher.Password = null; // do not keep plain password
            }

            return await _repository.CreateAsync(teacher);
        }

        public async Task<Teacher> UpdateAsync(string id, Teacher teacher)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);

            existing.Name = teacher.Name;
            existing.Email = teacher.Email;
            existing.Subject = teacher.Subject;
            existing.UpdateDate = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(id, existing);
            if (updated == null)
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            return updated;
        }

        public async Task<Teacher> DeleteAsync(string id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (deleted == null)
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            return deleted;
        }
    }
}
