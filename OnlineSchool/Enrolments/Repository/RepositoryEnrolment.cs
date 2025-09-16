using AutoMapper;
using OnlineSchool.Enrolments.Dto;
using OnlineSchool.Enrolments.Models;
using OnlineSchool.Data;
using OnlineSchool.Enrolments.Repository.interfaces;
using Microsoft.EntityFrameworkCore;

namespace OnlineSchool.Enrolments.Repository
{
    public class RepositoryEnrolment : IRepositoryEnrolment
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RepositoryEnrolment(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Enrolment>> GetAllAsync()
        {
            return await BuildEnrolmentQuery().ToListAsync();
        }

        public async Task<Enrolment?> GetByIdAsync(string id)
        {
            return await BuildEnrolmentQuery()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        private IQueryable<Enrolment> BuildEnrolmentQuery()
        {
            return _context.Enrolments
                .AsNoTracking()
                .Include(e => e.Course)
                .Include(e => e.Student);
        }

    }
}
