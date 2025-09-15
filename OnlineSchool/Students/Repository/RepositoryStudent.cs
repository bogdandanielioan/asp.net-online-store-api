using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Books.Models;
using OnlineSchool.Courses.Dto;
using OnlineSchool.Courses.Models;
using OnlineSchool.Data;
using OnlineSchool.Enrolments.Dto;
using OnlineSchool.Enrolments.Models;
using OnlineSchool.StudentCards.Models;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Models;
using OnlineSchool.Students.Repository.interfaces;
using OnlineSchool.System.Id;
using System;

namespace OnlineSchool.Students.Repository
{
    public class RepositoryStudent : IRepositoryStudent
    {

        private AppDbContext _context;
        private IMapper _mapper;

        public RepositoryStudent(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DtoStudentView>> GetAllAsync()
        {
            var students = await _context.Students
                .AsNoTracking()
                .Include(s => s.StudentBooks)
                .Include(s => s.CardNumber)
                .Include(s => s.MyCourses)
                .ToListAsync();

            // Preload courses referenced by students to avoid repeated lookups
            var allCourseIds = students
                .SelectMany(s => s.MyCourses.Select(mc => mc.IdCourse))
                .Distinct()
                .ToList();

            var coursesById = await _context.Courses
                .AsNoTracking()
                .Where(c => allCourseIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id);

            var studentViews = new List<DtoStudentView>(students.Count);

            foreach (var student in students)
            {
                var dtoStudentView = new DtoStudentView
                {
                    Id = student.Id,
                    Name = student.Name,
                    Email = student.Email,
                    MyCardNumber = student.CardNumber,
                    StudentBooks = student.StudentBooks,
                    MyCourses = student.MyCourses
                        .Select(mc => coursesById.TryGetValue(mc.IdCourse, out var c)
                            ? new DtoCourseViewForStudents { Name = c.Name, Department = c.Department }
                            : new DtoCourseViewForStudents())
                        .ToList()
                };

                studentViews.Add(dtoStudentView);
            }

            return studentViews;
        }
        public async Task<Student> GetById(string id)
        {
            return await _context.Students
                .AsNoTracking()
                .Include(s => s.StudentBooks)
                .Include(s => s.CardNumber)
                .Include(s => s.MyCourses)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<DtoStudentView> GetByIdAsync(string id)
        {
            var student = await _context.Students
                .AsNoTracking()
                .Include(s => s.StudentBooks)
                .Include(s => s.CardNumber)
                .Include(s => s.MyCourses)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null) return null;

            var courseIds = student.MyCourses.Select(mc => mc.IdCourse).Distinct().ToList();
            var coursesById = await _context.Courses
                .AsNoTracking()
                .Where(c => courseIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id);

            var dtoStudentView = new DtoStudentView
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                MyCardNumber = student.CardNumber,
                StudentBooks = student.StudentBooks,
                MyCourses = student.MyCourses
                    .Select(mc => coursesById.TryGetValue(mc.IdCourse, out var c)
                        ? new DtoCourseViewForStudents { Name = c.Name, Department = c.Department }
                        : new DtoCourseViewForStudents())
                    .ToList()
            };

            return dtoStudentView;
        }

        public async Task<StudentCard> CardByIdAsync(string id)
        {
            var student = await _context.Students
                .AsNoTracking()
                .Include(s => s.CardNumber)
                .FirstOrDefaultAsync(s => s.Id == id);

            return student?.CardNumber;
        }

        public async Task<DtoStudentView> GetByNameAsync(string name)
        {
            var student = await _context.Students
                .AsNoTracking()
                .Include(s => s.StudentBooks)
                .Include(s => s.CardNumber)
                .Include(s => s.MyCourses)
                .FirstOrDefaultAsync(s => s.Name == name);

            if (student == null) return null;

            var courseIds = student.MyCourses.Select(mc => mc.IdCourse).Distinct().ToList();
            var coursesById = await _context.Courses
                .AsNoTracking()
                .Where(c => courseIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id);

            var dtoStudentView = new DtoStudentView
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                MyCardNumber = student.CardNumber,
                StudentBooks = student.StudentBooks,
                MyCourses = student.MyCourses
                    .Select(mc => coursesById.TryGetValue(mc.IdCourse, out var c)
                        ? new DtoCourseViewForStudents { Name = c.Name, Department = c.Department }
                        : new DtoCourseViewForStudents())
                    .ToList()
            };

            return dtoStudentView;
        }


        public async Task<Student> Create(CreateRequestStudent request)
        {

            var student = _mapper.Map<Student>(request);
            student.Id = IdGenerator.New("student");

            var card = _mapper.Map<StudentCard>(request);
            card.Id = IdGenerator.New("studentcard");
            card.IdStudent = student.Id;
            var ran = new Random();

            card.Namecard = student.Name + ran.Next(0,100000);
            _context.Studentscard.Add(card);
            _context.Students.Add(student);

            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student> Update(string id, UpdateRequestStudent request)
        {

            var student = await _context.Students.FindAsync(id);

            student.Age = request.Age ?? student.Age;
            student.Email = request.Email ?? student.Email;
            student.Name = request.Name ?? student.Name;
            student.UpdateDate = DateTime.Now;
            _context.Students.Update(student);

            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<Student> DeleteById(string id)
        {
            var student = await _context.Students.FindAsync(id);

            _context.Students.Remove(student);

            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<Student> CreateBookForStudent(string idStudent, BookCreateDTO createRequestBook)
        {
            var student = await _context.Students.FindAsync(idStudent);


            Book book = _mapper.Map<Book>(createRequestBook);
            book.Id = IdGenerator.New("book");

            if (student.StudentBooks == null)
                _context.Entry(student).Collection(s => s.StudentBooks).Load();

            if (student.StudentBooks == null)
               student.StudentBooks = new List<Book>();


            student.StudentBooks.Add(book);
          //  _context.Students.Update(student);
            await _context.SaveChangesAsync();
            return student;

        }

        public async Task<Student> UpdateBookForStudent(string idStudent,string idBook, BookUpdateDTO bookUpdateDTO)
        {
            var student = await _context.Students.FindAsync(idStudent);

            var books = student.StudentBooks;
            var book = (Book)null;
            for (int i = 0; i < books.Count; i++)
            {
                if (books[i].Id == idBook)
                {
                    book = books[i];
                    break;
                }
            }

            book.Name = bookUpdateDTO.Name ?? book.Name;
            book.Created = bookUpdateDTO.Created_at ?? book.Created;

            _context.Update(book);

            if (student.StudentBooks == null)
                _context.Entry(student).Collection(s => s.StudentBooks).Load();

            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<Student> DeleteBookForStudent(string idStudent, string idBook)
        {
            var student = await _context.Students.FindAsync(idStudent);

            var books = student.StudentBooks;
            var book = (Book)null;
            for (int i = 0; i < books.Count; i++)
            {
                if (books[i].Id == idBook)
                {
                    book = books[i];
                    break;
                }
            }

            _context.Books.Remove(book);
            

            if (student.StudentBooks == null)
                _context.Entry(student).Collection(s => s.StudentBooks).Load();

          //  student.StudentBooks.Remove(book);

            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<Student> EnrollmentCourse(string idStudent, Course course)
        {
            var student = await _context.Students.FindAsync(idStudent);

            var requestEnrolment = new CreateRequestEnrolment
            {
                Created = DateTime.Now,
                IdCourse = course.Id
            };

            Enrolment enrolment = _mapper.Map<Enrolment>(requestEnrolment);
            enrolment.Id = IdGenerator.New("enrolment");

            enrolment.IdStudent = idStudent;
            if (student.MyCourses == null)
                _context.Entry(student).Collection(s => s.MyCourses).Load();

            if (student.MyCourses == null)
                student.MyCourses = new List<Enrolment>();

            if (student.MyCourses.Find(n => n.IdCourse == enrolment.IdCourse) != null)
                return null;

            _context.Enrolments.Add(enrolment);
           // student.MyCourses.Add(enrolment);

            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student> UnEnrollmentCourse(string idStudent, Course course)
        {
            var student = await _context.Students.FindAsync(idStudent);

            var entrolment = await _context.Enrolments.FirstOrDefaultAsync(n => n.IdCourse == course.Id && n.IdStudent == idStudent);

            _context.Enrolments.Remove(entrolment);
            // student.MyCourses.Add(enrolment);

            await _context.SaveChangesAsync();
            return student;
        }
    }
}
