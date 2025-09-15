using OnlineSchool.Books.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teste.Books.Helpers
{
    public class TestBookFactory
    {
        public static Book CreateBook(int id)
        {
            return new Book
            {
                Id = id,
                Created = DateTime.Now,
                IdStudent = id,
                Name = "test" + id

            };
        }

        public static List<Book> CreateBooks(int cout)
        {

            List<Book> book = new List<Book>();

            for (int i = 0; i < cout; i++)
            {
                book.Add(CreateBook(i));
            }

            return book;
        }
    }
}
