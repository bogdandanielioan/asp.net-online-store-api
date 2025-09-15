using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Courses.Dto;
using OnlineSchool.Courses.Models;
using OnlineSchool.Courses.Repository.interfaces;
using OnlineSchool.Data;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Models;
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
            List<Course> courses = await _context.Courses.Include(s => s.EnrolledStudents).ToListAsync();

            List<DtoCourseView> courseView = new List<DtoCourseView>();

            foreach (var course in courses)
            {
                DtoCourseView dtoCourseView = new DtoCourseView();

                dtoCourseView.Id = course.Id;
                dtoCourseView.Name = course.Name;
                dtoCourseView.Department = course.Department;

                List<DtoStudentViewForCourse> studentView = new List<DtoStudentViewForCourse>();

                List<int> idStudents = course.EnrolledStudents.Select(s => s.IdStudent).ToList();

                foreach(var idStudent in idStudents)
                {
                    DtoStudentViewForCourse student = new DtoStudentViewForCourse();
                    Student studentById = _context.Students.Find(idStudent);
                    student.Name = studentById.Name;
                    student.Email = studentById.Email;
                    student.Age = studentById.Age;

                    studentView.Add(student);
                }

                dtoCourseView.EnrolledStudents = studentView;

                courseView.Add(dtoCourseView);
            }

            return courseView;
        }

        public async Task<Course> GetById(int id)
        {
            List<Course> courses = await _context.Courses.Include(s => s.EnrolledStudents).ToListAsync();

            for (int i = 0; i < courses.Count; i++)
            {
                if (courses[i].Id == id) return courses[i];
            }
            return null;
        }

        public async Task<DtoCourseView> GetByIdAsync(int id)
        {
            List<Course> courses = await _context.Courses.Include(s => s.EnrolledStudents).ToListAsync();

            var course = (Course)null;
            for (int i = 0; i < courses.Count; i++)
            {
                if (courses[i].Id == id) course = courses[i];
            }

            if(course == null)
            return null;

            DtoCourseView dtoCourseView = new DtoCourseView();

            dtoCourseView.Name = course.Name;
            dtoCourseView.Department = course.Department;

            List<DtoStudentViewForCourse> studentView = new List<DtoStudentViewForCourse>();

            List<int> idStudents = course.EnrolledStudents.Select(s => s.IdStudent).ToList();

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

            List<int> idStudents = course.EnrolledStudents.Select(s => s.IdStudent).ToList();

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

            _context.Courses.Add(course);

            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course> Update(int id, UpdateRequestCourse request)
        {

            var course = await _context.Courses.FindAsync(id);

            course.Name = request.Name ?? course.Name;
            course.Department = request.Department ?? course.Department;

            _context.Courses.Update(course);

            await _context.SaveChangesAsync();

            return course;
        }

        public async Task<Course> DeleteById(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            _context.Courses.Remove(course);

            await _context.SaveChangesAsync();

            return course;
        }


    }
}
