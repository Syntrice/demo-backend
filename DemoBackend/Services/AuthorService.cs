using System;
using System.Collections.Generic;
using System.Linq;
using DemoBackend.Database;
using DemoBackend.Database.Entities;
using DemoBackend.Models.Authors;
using DemoBackend.Models.Books;
using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Services
{
    public class AuthorService(ApplicationDbContext db) : IAuthorService
    {
        public async Task<List<AuthorDetailsResponseModel>> GetAllAuthorsAsync()
        {
            return await db.Authors.Include((e => e.Books)).Select(e => new AuthorDetailsResponseModel()
            {
                Id = e.Id.ToString(),
                Name = e.Name,
                Books = e.Books.Select(e => new BookResponseModel() { Title = e.Title, Id = e.Id.ToString()})
            }).ToListAsync();
        }

        public async Task<AuthorDetailsResponseModel?> GetAuthorByIdAsync(Guid id)
        {
            var entity = await db.Authors.Include(e => e.Books).FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null) return null;
            return new AuthorDetailsResponseModel()
            {
                Id = entity.Id.ToString(),
                Name = entity.Name,
                Books = entity.Books.Select(e => new BookResponseModel() { Title = e.Title, Id = e.Id.ToString() })
            };
        }

        public async Task<Author> CreateAuthorAsync(Author author)
        {
            db.Authors.Add(author);
            await db.SaveChangesAsync();
            return author;
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            db.Authors.Update(author);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAuthorAsync(Guid id)
        {
            var author = await db.Authors.FirstOrDefaultAsync(a => a.Id == id);
            if (author != null)
            {
                db.Authors.Remove(author);
                await db.SaveChangesAsync();
            }
        }
    }
}