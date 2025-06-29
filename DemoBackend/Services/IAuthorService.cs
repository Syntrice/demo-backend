using System;
using System.Collections.Generic;
using DemoBackend.Database.Entities;
using DemoBackend.Models.Authors;
using DemoBackend.Models.Authors.Requests;
using DemoBackend.Models.Authors.Responses;

namespace DemoBackend.Services
{
    public interface IAuthorService
    {
        Task<List<AuthorDetailsResponseModel>> GetAllAuthorsAsync();
        Task<AuthorDetailsResponseModel?> GetAuthorByIdAsync(Guid id);
        Task<AuthorResponseModel> CreateAuthorAsync(AuthorRequestModel model);
        Task UpdateAuthorAsync(Guid id, AuthorRequestModel model);
        Task DeleteAuthorAsync(Guid id);
    }
}
