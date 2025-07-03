using DemoBackend.Common.Results;
using DemoBackend.Models.Authors.Requests;
using DemoBackend.Models.Authors.Responses;

namespace DemoBackend.Services
{
    public interface IAuthorService
    {
        Task<Result<List<AuthorDetailsResponseModel>>> GetAllAuthorsAsync();
        Task<Result<AuthorDetailsResponseModel>> GetAuthorByIdAsync(Guid id);
        Task<Result<AuthorResponseModel>> CreateAuthorAsync(AuthorRequestModel model);
        Task<Result<Unit>> UpdateAuthorAsync(Guid id, AuthorRequestModel model);
        Task<Result<Unit>> DeleteAuthorAsync(Guid id);
    }
}