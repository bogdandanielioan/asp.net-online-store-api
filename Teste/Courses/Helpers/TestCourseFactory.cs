using OnlineSchool.Courses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teste.Courses.Helpers
{
    public class TestCourseFactory
    {
        public static Course CreateCourse(int id)
        {
            return new Course
            {
                Id = id,
                Department = "test"+id,
                Name = "test"+id

            };
        }

        public static List<Course> CreateCourses(int cout)
        {

            List<Course> courses = new List<Course>();

            for (int i = 0; i < cout; i++)
            {
                courses.Add(CreateCourse(i));
            }

            return courses;
        }
    }
}
