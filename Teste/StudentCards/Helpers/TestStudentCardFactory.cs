using OnlineSchool.Books.Models;
using OnlineSchool.StudentCards.Models;
using OnlineSchool.Students.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.StudentCards.Helpers
{
    public class TestStudentCardFactory
    {


        public static StudentCard CreateStudentCard(int id)
        {
            return new StudentCard
            {
                Id = id,
                Namecard = id + "1234",
                IdStudent = id

            };
        }
        public static List<StudentCard> CreateStudentCards(int cout) {
        
            List<StudentCard> studentCards = new List<StudentCard>();

            for(int i=0; i < cout; i++)
            {
                studentCards.Add(CreateStudentCard(i));
            }

            return studentCards;
        }

    }
}
