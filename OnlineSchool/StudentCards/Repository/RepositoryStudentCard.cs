using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Books.Repository.interfaces;
using OnlineSchool.Data;
using OnlineSchool.StudentCards.Models;
using OnlineSchool.StudentCards.Repository.interfaces;

namespace OnlineSchool.StudentsCard.Repository
{
    public class RepositoryStudentCard : IRepositoryStudentCard
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public RepositoryStudentCard(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<StudentCard>> GetAllAsync()
        {
            return await _context.Studentscard.ToListAsync();
        }

        public async Task<StudentCard> GetByIdAsync(int id)
        {
            var studentCards = await _context.Studentscard.ToListAsync();

            for (int i = 0; i < studentCards.Count; i++)
            {
                if (studentCards[i].Id == id) return studentCards[i];
            }

            return null;
        }

        public async Task<StudentCard> GetByNameAsync(string name)
        {
            var studentCards = await _context.Studentscard.ToListAsync();

            for (int i = 0; i < studentCards.Count; i++)
            {
                if (studentCards[i].Namecard == name) return studentCards[i];
            }

            return null;
        }


    }
}
