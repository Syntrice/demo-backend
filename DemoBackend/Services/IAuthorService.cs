using System;
using System.Collections.Generic;
using DemoBackend.Database.Entities;

namespace DemoBackend.Services
{
    public interface IAuthorService
    {
        IEnumerable<Author> GetAllAuthors();
        Author? GetAuthorById(Guid id);
        void AddAuthor(Author author);
        void UpdateAuthor(Author author);
        void DeleteAuthor(Guid id);
    }
}
