using DemoBackend.Common.Results;
using DemoBackend.Models.Books.Requests;
using DemoBackend.Models.Books.Responses;

namespace DemoBackend.Services;

public interface IBookService
{
    Task<Result<List<BookDetailsResponseModel>>> GetAllBooksAsync();
    Task<Result<BookDetailsResponseModel>> GetBookByIdAsync(Guid id);
    Task<Result<BookDetailsResponseModel>> CreateBookAsync(BookRequestModel model);
    Task<Result<Unit>> UpdateBookAsync(Guid id, BookRequestModel model);
    Task<Result<Unit>> DeleteBookAsync(Guid id);
    Task<Result<List<BookDetailsResponseModel>>> GetAllBooksByAuthorIdAsync(Guid authorId);
}