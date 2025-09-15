using OnlineSchool.Enrolments.Dto;
using OnlineSchool.Enrolments.Models;

namespace OnlineSchool.Enrolments.Repository.interfaces
{
    public interface IRepositoryEnrolment
    {

        Task<List<Enrolment>> GetAllAsync();
        Task<Enrolment> GetByIdAsync(int id);

    }
}
