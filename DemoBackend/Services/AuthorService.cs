using DemoBackend.Common.Mapping;
using DemoBackend.Common.Results;
using DemoBackend.Database;
using DemoBackend.Database.Entities;
using DemoBackend.Models.Authors.Requests;
using DemoBackend.Models.Authors.Responses;
using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Services;

public interface IAuthorService
{
    Task<Result<List<AuthorDetailsResponseModel>>> GetAllAuthorsAsync();
    Task<Result<AuthorDetailsResponseModel>> GetAuthorByIdAsync(Guid id);
    Task<Result<AuthorResponseModel>> CreateAuthorAsync(AuthorRequestModel model);
    Task<Result<Unit>> UpdateAuthorAsync(Guid id, AuthorRequestModel model);
    Task<Result<Unit>> DeleteAuthorAsync(Guid id);
}

public class AuthorService(ApplicationDbContext db, IMapper mapper) : IAuthorService
{
    public async Task<Result<List<AuthorDetailsResponseModel>>> GetAllAuthorsAsync()
    {
        return await mapper
            .Map<Author, AuthorDetailsResponseModel>(db.Authors.Include(e => e.Books))
            .ToListAsync();
    }

    public async Task<Result<AuthorDetailsResponseModel>> GetAuthorByIdAsync(Guid id)
    {
        var entity = await mapper
            .Map<Author, AuthorDetailsResponseModel>(db.Authors.Include(e => e.Books))
            .FirstOrDefaultAsync(e => e.Id == id);
        if (entity == null) return Error.NotFound($"Author with id '{id}' was not found.");
        return entity;
    }

    public async Task<Result<AuthorResponseModel>> CreateAuthorAsync(AuthorRequestModel model)
    {
        var author = new Author { Id = Guid.NewGuid(), Name = model.Name };
        db.Authors.Add(author);
        await db.SaveChangesAsync();
        return mapper.Map<Author, AuthorResponseModel>(author);
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