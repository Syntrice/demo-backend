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
                Books = e.Books.Select(e => new BookResponseModel() { Title = e.Title, Id = e.Id.ToString() })
            }).ToListAsync();
        }

        public async Task<AuthorDetailsResponseModel?> GetAuthorByIdAsync(Guid id)
        {
            var author = await db.Authors.Include(e => e.Books).FirstOrDefaultAsync(e => e.Id == id);
            if (author == null) return null;
            return new AuthorDetailsResponseModel()
            {
                Id = author.Id.ToString(),
                Name = author.Name,
                Books = author.Books.Select(e => new BookResponseModel() { Title = e.Title, Id = e.Id.ToString() })
            };
        }

        public async Task<AuthorResponseModel> CreateAuthorAsync(AuthorRequestModel model)
        {
            var author = new Author { Id = Guid.NewGuid(), Name = model.Name };
            db.Authors.Add(author);
            await db.SaveChangesAsync();
            return new AuthorResponseModel()
            {
                Id = author.Id.ToString(),
                Name = author.Name,
            };
        }

        public async Task UpdateAuthorAsync(Guid id, AuthorRequestModel model)
        {
            var author = await db.Authors.FirstOrDefaultAsync(a => a.Id == id);
            if (author == null) return;
            author.Name = model.Name;
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