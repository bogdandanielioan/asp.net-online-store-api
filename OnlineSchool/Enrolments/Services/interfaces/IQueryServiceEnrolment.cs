using OnlineSchool.Enrolments.Models;

namespace OnlineSchool.Enrolments.Services.interfaces
{
    public interface IQueryServiceEnrolment
    {
        Task<List<Enrolment>> GetAll();

        Task<Enrolment> GetById(int id);
    }
}
