using DemoBackend.Common.Results;
using DemoBackend.Models.Authors.Requests;
using DemoBackend.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController(
        IAuthorService authorService,
        IBookService bookService,
        IValidator<AuthorRequestModel> authorRequestValidator)
        : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await authorService.GetAllAuthorsAsync();
            return Ok(authors.Value);
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetAuthorById(Guid id)
        {
            var result = await authorService.GetAuthorByIdAsync(id);
            return result.IsFailure ? result.ToProblemDetailsResponse(this) : Ok(result.Value);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorRequestModel model)
        {
            var validationResult = await authorRequestValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return validationResult.ToProblemDetailsResponse(this);
            }

            var result = await authorService.CreateAuthorAsync(model);
            if (result.IsFailure) return result.ToProblemDetailsResponse(this);
            var author = result.Value;
            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author);
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody] AuthorRequestModel model)
        {
            var validationResult = await authorRequestValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return validationResult.ToProblemDetailsResponse(this);
            }

            var result = await authorService.UpdateAuthorAsync(id, model);
            return result.IsFailure ? result.ToProblemDetailsResponse(this) : NoContent();
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteAuthor(Guid id)
        {
            var result = await authorService.DeleteAuthorAsync(id);
            return result.IsFailure ? result.ToProblemDetailsResponse(this) : NoContent();
        }

        [HttpGet("{id:guid}/books")]
        [Authorize]
        public async Task<IActionResult> GetAuthorBooks(Guid id)
        {
            var books = await bookService.GetAllBooksByAuthorIdAsync(id);
            return books.IsFailure ? books.ToProblemDetailsResponse(this) : Ok(books.Value);
        }
    }
}