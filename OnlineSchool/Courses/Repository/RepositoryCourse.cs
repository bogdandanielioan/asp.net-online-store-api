using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Courses.Dto;
using OnlineSchool.Courses.Models;
using OnlineSchool.Courses.Repository.interfaces;
using OnlineSchool.Data;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Models;
using OnlineSchool.System.Id;
using System.ComponentModel;

namespace OnlineSchool.Courses.Repository
{
    public class RepositoryCourse : IRepositoryCourse
    {


        private AppDbContext _context;
        private IMapper _mapper;

        public RepositoryCourse(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DtoCourseView>> GetAllAsync()
        {
            var courses = await _context.Courses
                .AsNoTracking()
                .Include(c => c.EnrolledStudents)
                .ToListAsync();

            // Preload all referenced students to avoid N+1 queries
            var allStudentIds = courses
                .SelectMany(c => c.EnrolledStudents.Select(es => es.IdStudent))
                .Distinct()
                .ToList();

            var studentsById = await _context.Students
                .AsNoTracking()
                .Where(s => allStudentIds.Contains(s.Id))
                .ToDictionaryAsync(s => s.Id);

            var courseView = new List<DtoCourseView>(courses.Count);

            foreach (var course in courses)
            {
                var dtoCourseView = new DtoCourseView
                {
                    Id = course.Id,
                    Name = course.Name,
                    Department = course.Department,
                    EnrolledStudents = course.EnrolledStudents
                        .Select(es => studentsById.TryGetValue(es.IdStudent, out var s)
                            ? new DtoStudentViewForCourse { Name = s.Name, Email = s.Email, Age = s.Age }
                            : new DtoStudentViewForCourse())
                        .ToList()
                };

                courseView.Add(dtoCourseView);
            }

            return courseView;
        }

        public async Task<Course> GetById(string id)
        {
            return await _context.Courses
                .AsNoTracking()
                .Include(c => c.EnrolledStudents)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<DtoCourseView> GetByIdAsync(string id)
        {
            var course = await _context.Courses
                .AsNoTracking()
                .Include(c => c.EnrolledStudents)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return null;

            var idStudents = course.EnrolledStudents.Select(es => es.IdStudent).Distinct().ToList();
            var studentsById = await _context.Students
                .AsNoTracking()
                .Where(s => idStudents.Contains(s.Id))
                .ToDictionaryAsync(s => s.Id);

            var dtoCourseView = new DtoCourseView
            {
                Id = course.Id,
                Name = course.Name,
                Department = course.Department,
                EnrolledStudents = course.EnrolledStudents
                    .Select(es => studentsById.TryGetValue(es.IdStudent, out var s)
                        ? new DtoStudentViewForCourse { Name = s.Name, Email = s.Email, Age = s.Age }
                        : new DtoStudentViewForCourse())
                    .ToList()
            };

            return dtoCourseView;
        }

        public async Task<DtoCourseView> GetByNameAsync(string name)
        {
            List<Course> allCourses = await _context.Courses.Include(s => s.EnrolledStudents).ToListAsync();

            var course = (Course)null;
            for (int i = 0; i < allCourses.Count; i++)
            {
                if (allCourses[i].Name.Equals(name))
                {
                    course= allCourses[i];
                }
            }

            if(course == null) 
            return null;

            DtoCourseView dtoCourseView = new DtoCourseView();

            dtoCourseView.Name = course.Name;
            dtoCourseView.Department = course.Department;

            List<DtoStudentViewForCourse> studentView = new List<DtoStudentViewForCourse>();

            List<string> idStudents = course.EnrolledStudents.Select(s => s.IdStudent).ToList();

            foreach (var idStudent in idStudents)
            {
                DtoStudentViewForCourse student = new DtoStudentViewForCourse();
                Student studentById = _context.Students.Find(idStudent);
                student.Name = studentById.Name;
                student.Email = studentById.Email;
                student.Age = studentById.Age;

                studentView.Add(student);
            }

            dtoCourseView.EnrolledStudents = studentView;

            return dtoCourseView;
        }
        public async Task<Course> GetByName(string name)
        {
            List<Course> allCourses = await _context.Courses.Include(s => s.EnrolledStudents).ToListAsync();


            for (int i = 0; i < allCourses.Count; i++)
            {
                if (allCourses[i].Name.Equals(name))
                {
                    return allCourses[i];
                }
            }

            return null;
        }


        public async Task<Course> Create(CreateRequestCourse request)
        {

            var course = _mapper.Map<Course>(request);
            course.Id = IdGenerator.New("course");

            _context.Courses.Add(course);

            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course> Update(string id, UpdateRequestCourse request)
        {

            var course = await _context.Courses.FindAsync(id);

            course.Name = request.Name ?? course.Name;
            course.Department = request.Department ?? course.Department;

            _context.Courses.Update(course);

            await _context.SaveChangesAsync();

            return course;
        }

        public async Task<Course> DeleteById(string id)
        {
            var course = await _context.Courses.FindAsync(id);

            _context.Courses.Remove(course);

            await _context.SaveChangesAsync();

            return course;
        }


    }
}
