using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Courses.Dto;
using OnlineSchool.Courses.Models;
using OnlineSchool.Courses.Repository.interfaces;
using OnlineSchool.Data;
using OnlineSchool.Students.Dto;
using OnlineSchool.System.Id;
using System.Linq;

namespace OnlineSchool.Courses.Repository
{
    public class RepositoryCourse : IRepositoryCourse
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RepositoryCourse(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DtoCourseView>> GetAllAsync()
        {
            var courses = await BuildCourseQuery().ToListAsync();

            return courses.Select(MapToDto).ToList();
        }

        public async Task<Course?> GetById(string id)
        {
            return await BuildCourseQuery()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<DtoCourseView?> GetByIdAsync(string id)
        {
            var course = await BuildCourseQuery()
                .FirstOrDefaultAsync(c => c.Id == id);

            return course == null ? null : MapToDto(course);
        }

        public async Task<DtoCourseView?> GetByNameAsync(string name)
        {
            var course = await BuildCourseQuery()
                .FirstOrDefaultAsync(c => c.Name == name);

            return course == null ? null : MapToDto(course);
        }

        public async Task<Course?> GetByName(string name)
        {
            return await BuildCourseQuery()
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<Course> Create(CreateRequestCourse request)
        {
            var course = _mapper.Map<Course>(request);
            course.Id = IdGenerator.New("course");

            _context.Courses.Add(course);

            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course?> Update(string id, UpdateRequestCourse request)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return null;

            course.Name = request.Name ?? course.Name;
            course.Department = request.Department ?? course.Department;

            await _context.SaveChangesAsync();

            return course;
        }

        public async Task<Course?> DeleteById(string id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return null;

            _context.Courses.Remove(course);

            await _context.SaveChangesAsync();

            return course;
        }

        private IQueryable<Course> BuildCourseQuery()
        {
            return _context.Courses
                .AsNoTracking()
                .Include(c => c.EnrolledStudents)
                    .ThenInclude(e => e.Student);
        }

        private static DtoCourseView MapToDto(Course course)
        {
            return new DtoCourseView
            {
                Id = course.Id,
                Name = course.Name,
                Department = course.Department,
                EnrolledStudents = course.EnrolledStudents?
                    .Select(es => new DtoStudentViewForCourse
                    {
                        Name = es.Student?.Name ?? string.Empty,
                        Email = es.Student?.Email ?? string.Empty,
                        Age = es.Student?.Age ?? 0
                    })
                    .ToList() ?? new List<DtoStudentViewForCourse>()
            };
        }
    }
}
