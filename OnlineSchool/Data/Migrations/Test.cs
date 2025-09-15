using FluentMigrator;
using System;

namespace OnlineSchool.Data.Migrations
{
    // Seed the database with test data using the single string Id schema
    [Migration(2025091501)]
    public class SeedTestData : Migration
    {
        private static string NewId(string entity) => $"onl-{Guid.NewGuid()}"; // Keep it simple and consistent with IdGenerator prefix

        public override void Up()
        {
            // Pre-generate IDs to reference across tables
            var stu1 = NewId("student");
            var stu2 = NewId("student");
            var stu3 = NewId("student");
            var stu4 = NewId("student");
            var stu5 = NewId("student");

            var c1 = NewId("course");
            var c2 = NewId("course");
            var c3 = NewId("course");
            var c4 = NewId("course");
            var c5 = NewId("course");

           
            Insert.IntoTable("students").Row(new { Id = stu1, Name = "Alice Johnson", Email = "alice@example.com", Age = 20, UpdateDate = SystemMethods.CurrentDateTime });
            Insert.IntoTable("students").Row(new { Id = stu2, Name = "Bob Smith", Email = "bob@example.com", Age = 22, UpdateDate = SystemMethods.CurrentDateTime });
            Insert.IntoTable("students").Row(new { Id = stu3, Name = "Carol Williams", Email = "carol@example.com", Age = 19, UpdateDate = SystemMethods.CurrentDateTime });
            Insert.IntoTable("students").Row(new { Id = stu4, Name = "Daniel Popescu", Email = "daniel@example.com", Age = 23, UpdateDate = SystemMethods.CurrentDateTime });
            Insert.IntoTable("students").Row(new { Id = stu5, Name = "Elena Ionescu", Email = "elena@example.com", Age = 21, UpdateDate = SystemMethods.CurrentDateTime });

          
            Insert.IntoTable("courses").Row(new { Id = c1, Name = "Algorithms", Department = "Computer Science" });
            Insert.IntoTable("courses").Row(new { Id = c2, Name = "Linear Algebra", Department = "Mathematics" });
            Insert.IntoTable("courses").Row(new { Id = c3, Name = "Microeconomics", Department = "Economics" });
            Insert.IntoTable("courses").Row(new { Id = c4, Name = "Databases", Department = "Computer Science" });
            Insert.IntoTable("courses").Row(new { Id = c5, Name = "Statistics", Department = "Mathematics" });

           
            Insert.IntoTable("studentscard").Row(new { Id = NewId("studentcard"), IdStudent = stu1, Namecard = "STU-0001" });
            Insert.IntoTable("studentscard").Row(new { Id = NewId("studentcard"), IdStudent = stu2, Namecard = "STU-0002" });
            Insert.IntoTable("studentscard").Row(new { Id = NewId("studentcard"), IdStudent = stu3, Namecard = "STU-0003" });
            Insert.IntoTable("studentscard").Row(new { Id = NewId("studentcard"), IdStudent = stu4, Namecard = "STU-0004" });
            Insert.IntoTable("studentscard").Row(new { Id = NewId("studentcard"), IdStudent = stu5, Namecard = "STU-0005" });

            
            Insert.IntoTable("books").Row(new { Id = NewId("book"), IdStudent = stu1, Name = "C# in Depth", Created = SystemMethods.CurrentDateTime });
            Insert.IntoTable("books").Row(new { Id = NewId("book"), IdStudent = stu2, Name = "Introduction to Algorithms", Created = SystemMethods.CurrentDateTime });
            Insert.IntoTable("books").Row(new { Id = NewId("book"), IdStudent = stu3, Name = "Clean Code", Created = SystemMethods.CurrentDateTime });
            Insert.IntoTable("books").Row(new { Id = NewId("book"), IdStudent = stu4, Name = "Design Patterns", Created = SystemMethods.CurrentDateTime });
            Insert.IntoTable("books").Row(new { Id = NewId("book"), IdStudent = stu5, Name = "Refactoring", Created = SystemMethods.CurrentDateTime });

            
            Insert.IntoTable("enrolments").Row(new { Id = NewId("enrolment"), IdCourse = c1, IdStudent = stu1, Created = SystemMethods.CurrentDateTime });
            Insert.IntoTable("enrolments").Row(new { Id = NewId("enrolment"), IdCourse = c2, IdStudent = stu1, Created = SystemMethods.CurrentDateTime });
            Insert.IntoTable("enrolments").Row(new { Id = NewId("enrolment"), IdCourse = c1, IdStudent = stu2, Created = SystemMethods.CurrentDateTime });
            Insert.IntoTable("enrolments").Row(new { Id = NewId("enrolment"), IdCourse = c3, IdStudent = stu3, Created = SystemMethods.CurrentDateTime });
            Insert.IntoTable("enrolments").Row(new { Id = NewId("enrolment"), IdCourse = c4, IdStudent = stu4, Created = SystemMethods.CurrentDateTime });
            Insert.IntoTable("enrolments").Row(new { Id = NewId("enrolment"), IdCourse = c5, IdStudent = stu5, Created = SystemMethods.CurrentDateTime });
        }

        public override void Down()
        {
            // Remove inserted data
            Delete.FromTable("enrolments").AllRows();
            Delete.FromTable("books").AllRows();
            Delete.FromTable("studentscard").AllRows();
            Delete.FromTable("courses").AllRows();
            Delete.FromTable("students").AllRows();
        }
    }
}
