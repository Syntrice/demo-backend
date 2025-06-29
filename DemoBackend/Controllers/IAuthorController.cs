using DemoBackend.Models.Authors;
using DemoBackend.Models.Authors.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DemoBackend.Controllers;

public interface IAuthorController
{
    Task<IActionResult> GetAllAuthors();
    Task<IActionResult> GetAuthorById(Guid id);
    Task<IActionResult> CreateAuthor(AuthorRequestModel model);
    Task<IActionResult> UpdateAuthor(Guid id, AuthorRequestModel model);
    Task<IActionResult> DeleteAuthor(Guid id);
    Task<IActionResult> GetAuthorBooks(Guid id);
}