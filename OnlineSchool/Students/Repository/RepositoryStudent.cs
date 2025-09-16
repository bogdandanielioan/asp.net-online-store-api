using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Books.Models;
using OnlineSchool.Courses.Dto;
using OnlineSchool.Courses.Models;
using OnlineSchool.Data;
using OnlineSchool.Auth.Models;
using OnlineSchool.Enrolments.Dto;
using OnlineSchool.Enrolments.Models;
using OnlineSchool.StudentCards.Models;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Models;
using OnlineSchool.Students.Repository.interfaces;
using OnlineSchool.System.Id;
using System;
using System.Linq;

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
            var students = await BuildStudentQuery().ToListAsync();

            return students.Select(MapToDto).ToList();
        }
        public async Task<Student?> GetById(string id)
        {
            return await BuildStudentQuery()
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<DtoStudentView?> GetByIdAsync(string id)
        {
            var student = await BuildStudentQuery()
                .FirstOrDefaultAsync(s => s.Id == id);

            return student == null ? null : MapToDto(student);
        }

        public async Task<StudentCard?> CardByIdAsync(string id)
        {
            return await _context.Students
                .AsNoTracking()
                .Where(s => s.Id == id)
                .Select(s => s.CardNumber)
                .FirstOrDefaultAsync();
        }

        public async Task<DtoStudentView?> GetByNameAsync(string name)
        {
            var student = await BuildStudentQuery()
                .FirstOrDefaultAsync(s => s.Name == name);

            return student == null ? null : MapToDto(student);
        }


        public async Task<Student> Create(CreateRequestStudent request)
        {

            var student = _mapper.Map<Student>(request);
            student.Id = IdGenerator.New("student");
            // Assign default role to all students
            student.Role = string.IsNullOrWhiteSpace(student.Role) ? SystemRoles.User : SystemRoles.Normalize(student.Role);

            // Hash and store password if provided
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                var hp = OnlineSchool.Auth.Services.PasswordHasher.HashPassword(request.Password);
                student.PasswordHash = hp.Hash;
                student.PasswordSalt = hp.Salt;
            }

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

        public async Task<Student?> Update(string id, UpdateRequestStudent request)
        {

            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return null;

            student.Age = request.Age ?? student.Age;
            student.Email = request.Email ?? student.Email;
            student.Name = request.Name ?? student.Name;
            student.UpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<Student?> DeleteById(string id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return null;

            _context.Students.Remove(student);

            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<Student?> CreateBookForStudent(string idStudent, BookCreateDTO createRequestBook)
        {
            var student = await _context.Students.FindAsync(idStudent);

            if (student == null)
                return null;

            var book = _mapper.Map<Book>(createRequestBook);
            book.Id = IdGenerator.New("book");
            book.IdStudent = student.Id;

            var booksEntry = _context.Entry(student).Collection(s => s.StudentBooks);
            if (!booksEntry.IsLoaded)
                await booksEntry.LoadAsync();

            student.StudentBooks ??= new List<Book>();

            student.StudentBooks.Add(book);

            await _context.SaveChangesAsync();
            return student;

        }

        public async Task<Student?> UpdateBookForStudent(string idStudent,string idBook, BookUpdateDTO bookUpdateDTO)
        {
            var student = await _context.Students.FindAsync(idStudent);

            if (student == null)
                return null;

            var booksEntry = _context.Entry(student).Collection(s => s.StudentBooks);
            if (!booksEntry.IsLoaded)
                await booksEntry.LoadAsync();

            var book = student.StudentBooks?.FirstOrDefault(b => b.Id == idBook);

            if (book == null)
                return student;

            book.Name = bookUpdateDTO.Name ?? book.Name;
            book.Created = bookUpdateDTO.Created_at ?? book.Created;

            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<Student?> DeleteBookForStudent(string idStudent, string idBook)
        {
            var student = await _context.Students.FindAsync(idStudent);

            if (student == null)
                return null;

            var booksEntry = _context.Entry(student).Collection(s => s.StudentBooks);
            if (!booksEntry.IsLoaded)
                await booksEntry.LoadAsync();

            var book = student.StudentBooks?.FirstOrDefault(b => b.Id == idBook);

            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }

            return student;
        }

        public async Task<Student?> EnrollmentCourse(string idStudent, Course course)
        {
            var student = await _context.Students.FindAsync(idStudent);

            if (student == null)
                return null;

            var requestEnrolment = new CreateRequestEnrolment
            {
                Created = DateTime.UtcNow,
                IdCourse = course.Id
            };

            var enrolment = _mapper.Map<Enrolment>(requestEnrolment);
            enrolment.Id = IdGenerator.New("enrolment");
            enrolment.IdStudent = idStudent;

            var coursesEntry = _context.Entry(student).Collection(s => s.MyCourses);
            if (!coursesEntry.IsLoaded)
                await coursesEntry.LoadAsync();

            student.MyCourses ??= new List<Enrolment>();

            if (student.MyCourses.Any(n => n.IdCourse == enrolment.IdCourse))
                return null;

            _context.Enrolments.Add(enrolment);

            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student?> UnEnrollmentCourse(string idStudent, Course course)
        {
            var student = await _context.Students.FindAsync(idStudent);

            if (student == null)
                return null;

            var enrolment = await _context.Enrolments
                .FirstOrDefaultAsync(n => n.IdCourse == course.Id && n.IdStudent == idStudent);

            if (enrolment == null)
                return null;

            _context.Enrolments.Remove(enrolment);

            await _context.SaveChangesAsync();
            return student;
        }

        private IQueryable<Student> BuildStudentQuery()
        {
            return _context.Students
                .AsNoTracking()
                .Include(s => s.StudentBooks)
                .Include(s => s.CardNumber)
                .Include(s => s.MyCourses)
                    .ThenInclude(e => e.Course);
        }

        private static DtoStudentView MapToDto(Student student)
        {
            return new DtoStudentView
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                MyCardNumber = student.CardNumber,
                StudentBooks = student.StudentBooks ?? new List<Book>(),
                MyCourses = student.MyCourses == null
                    ? new List<DtoCourseViewForStudents>()
                    : student.MyCourses
                        .Select(mc => new DtoCourseViewForStudents
                        {
                            Name = mc.Course?.Name ?? string.Empty,
                            Department = mc.Course?.Department ?? string.Empty
                        })
                        .ToList(),
                Role = student.Role
            };
        }
    }
}
