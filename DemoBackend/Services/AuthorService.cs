using DemoBackend.Common.Results;
using DemoBackend.Database;
using DemoBackend.Database.Entities;
using DemoBackend.Models.Authors.Requests;
using DemoBackend.Models.Authors.Responses;
using DemoBackend.Models.Books.Responses;
using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Services
{
    public class AuthorService(ApplicationDbContext db) : IAuthorService
    {
        public async Task<Result<List<AuthorDetailsResponseModel>>> GetAllAuthorsAsync()
        {
            return await db.Authors.Include((e => e.Books)).Select(e => new AuthorDetailsResponseModel()
            {
                Id = e.Id.ToString(),
                Name = e.Name,
                Books = e.Books.Select(e => new BookResponseModel() { Title = e.Title, Id = e.Id.ToString() })
            }).ToListAsync();
        }

        public async Task<Result<AuthorDetailsResponseModel>> GetAuthorByIdAsync(Guid id)
        {
            var author = await db.Authors.Include(e => e.Books).FirstOrDefaultAsync(e => e.Id == id);
            if (author == null) return Error.NotFound($"Author with id '{id}' was not found.");
            return new AuthorDetailsResponseModel()
            {
                Id = author.Id.ToString(),
                Name = author.Name,
                Books = author.Books.Select(e => new BookResponseModel() { Title = e.Title, Id = e.Id.ToString() })
            };
        }

        public async Task<Result<AuthorResponseModel>> CreateAuthorAsync(AuthorRequestModel model)
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

        public async Task<Result<Unit>> UpdateAuthorAsync(Guid id, AuthorRequestModel model)
        {
            var author = await db.Authors.FirstOrDefaultAsync(a => a.Id == id);
            if (author is null)
                return Error.NotFound($"Author with id '{id}' was not found.");

            author.Name = model.Name;
            await db.SaveChangesAsync();
            return Unit.Value;
        }

        public async Task<Result<Unit>> DeleteAuthorAsync(Guid id)
        {
            var author = await db.Authors.FirstOrDefaultAsync(a => a.Id == id);
            if (author is null)
                return Error.NotFound($"Author with id '{id}' was not found.");

            db.Authors.Remove(author);
            await db.SaveChangesAsync();
            return Unit.Value;
        }
    }
}