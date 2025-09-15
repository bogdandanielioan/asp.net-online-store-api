using OnlineSchool.Enrolments.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teste.Enrolments.Helpers
{
    public class TestEnrolmentFactory
    {
        public static Enrolment CreateEnrolment(int id)
        {
            return new Enrolment
            {
                Id = id,
                Created = DateTime.Now,
                IdStudent = id+1,
                IdCourse = id+1

            };
        }

        public static List<Enrolment> CreateEnrolments(int cout)
        {

            List<Enrolment> enrolments = new List<Enrolment>();

            for (int i = 0; i < cout; i++)
            {
                enrolments.Add(CreateEnrolment(i));
            }

            return enrolments;
        }
    }
}
