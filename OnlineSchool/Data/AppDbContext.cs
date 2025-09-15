
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Books.Models;
using OnlineSchool.Courses.Models;
using OnlineSchool.Enrolments.Models;
using OnlineSchool.StudentCards.Models;
using OnlineSchool.Students.Models;
using OnlineSchool.Teachers.Models;


namespace OnlineSchool.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Enrolment> Enrolments { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<StudentCard> Studentscard { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("students");
            modelBuilder.Entity<Book>().ToTable("books");
            modelBuilder.Entity<StudentCard>().ToTable("studentscard");
            modelBuilder.Entity<Course>().ToTable("courses");
            modelBuilder.Entity<Enrolment>().ToTable("enrolments");
            modelBuilder.Entity<Teacher>().ToTable("teachers");

            modelBuilder.Entity<Book>()
                .HasOne(a => a.Student)
                .WithMany(s => s.StudentBooks)
                .HasForeignKey(a => a.IdStudent)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<StudentCard>()
                .HasOne(a => a.Student)
                .WithOne(a=>a.CardNumber)
                .HasForeignKey<StudentCard>(a=>a.IdStudent)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Enrolment>()
                .HasOne(a => a.Student)
                .WithMany(s => s.MyCourses)
                .HasForeignKey(a => a.IdStudent)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Enrolment>()
                .HasOne(a => a.Course)
                .WithMany(s => s.EnrolledStudents)
                .HasForeignKey(a => a.IdCourse)
                .OnDelete(DeleteBehavior.Cascade);

            // Teacher 1 - many Courses
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
