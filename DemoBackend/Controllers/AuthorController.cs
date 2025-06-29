using System;
using Microsoft.AspNetCore.Mvc;
using DemoBackend.Services;
using DemoBackend.Models.Authors;
using DemoBackend.Models.Authors.Requests;
using DemoBackend.Models.Errors;
using FluentValidation;

namespace DemoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController(
        IAuthorService authorService,
        IBookService bookService,
        IValidator<AuthorRequestModel> authorRequestValidator)
        : ControllerBase, IAuthorController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await authorService.GetAllAuthorsAsync();
            return Ok(authors);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAuthorById(Guid id)
        {
            var author = await authorService.GetAuthorByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorRequestModel model)
        {
            var validationResult = await authorRequestValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorResponseModel()
                    { Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList()});
            }

            var author = await authorService.CreateAuthorAsync(model);
            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody] AuthorRequestModel model)
        {
            var validationResult = await authorRequestValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorResponseModel()
                    { Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList()});
            }

            await authorService.UpdateAuthorAsync(id, model);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAuthor(Guid id)
        {
            await authorService.DeleteAuthorAsync(id);
            return NoContent();
        }

        [HttpGet("{id:guid}/books")]
        public async Task<IActionResult> GetAuthorBooks(Guid id)
        {
            var books = await bookService.GetAllBooksByAuthorIdAsync(id);
            return Ok(books);
        }
    }
}