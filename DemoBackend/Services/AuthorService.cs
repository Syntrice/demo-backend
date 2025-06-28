using System;
using System.Collections.Generic;
using System.Linq;
using DemoBackend.Database;
using DemoBackend.Database.Entities;

namespace DemoBackend.Services
{
    public class AuthorService(ApplicationDbContext db) : IAuthorService
    {
        public IEnumerable<Author> GetAllAuthors()
        {
            return db.Authors.ToList();
        }

        public Author? GetAuthorById(Guid id)
        {
            return db.Authors.Find(id);
        }

        public void AddAuthor(Author author)
        {
            db.Authors.Add(author);
            db.SaveChanges();
        }

        public void UpdateAuthor(Author author)
        {
            db.Authors.Update(author);
            db.SaveChanges();
        }

        public void DeleteAuthor(Guid id)
        {
            var author = db.Authors.Find(id);
            if (author != null)
            {
                db.Authors.Remove(author);
                db.SaveChanges();
            }
        }
    }
}
