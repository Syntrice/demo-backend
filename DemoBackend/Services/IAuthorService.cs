using System;
using System.Collections.Generic;
using DemoBackend.Database.Entities;
using DemoBackend.Models.Authors;

namespace DemoBackend.Services
{
    public interface IAuthorService
    {
        Task<List<AuthorDetailsResponseModel>> GetAllAuthorsAsync();
        Task<AuthorDetailsResponseModel?> GetAuthorByIdAsync(Guid id);
        Task<Author> CreateAuthorAsync(Author author);
        Task UpdateAuthorAsync(Author author);
        Task DeleteAuthorAsync(Guid id);
    }
}
