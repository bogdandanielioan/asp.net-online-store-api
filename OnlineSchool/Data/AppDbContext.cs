
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Books.Models;
using OnlineSchool.Courses.Models;
using OnlineSchool.Enrolments.Models;
using OnlineSchool.StudentCards.Models;
using OnlineSchool.Students.Models;


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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

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
        }

    }
}
