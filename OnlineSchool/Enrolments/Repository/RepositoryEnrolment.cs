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

        private AppDbContext _context;
        private IMapper _mapper;

        public RepositoryEnrolment(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Enrolment>> GetAllAsync()
        {
            return await _context.Enrolments.ToListAsync();
        }

        public async Task<Enrolment> GetByIdAsync(int id)
        {
            List<Enrolment> enrolments = await _context.Enrolments.ToListAsync();

            for (int i = 0; i < enrolments.Count; i++)
            {
                if (enrolments[i].Id == id) return enrolments[i];
            }
            return null;
        }

    }
}
