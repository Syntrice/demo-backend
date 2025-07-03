using DemoBackend.Common.Results;
using DemoBackend.Models.Books.Requests;
using DemoBackend.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DemoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController(IBookService bookService, IValidator<BookRequestModel> bookRequestValidator)
    : ControllerBase, IBookController
{
    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var result = await bookService.GetAllBooksAsync();
        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBookById(Guid id)
    {
        var result = await bookService.GetBookByIdAsync(id);
        if (result.IsFailure) return result.ToProblemDetailsResponse((this));
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] BookRequestModel model)
    {
        var validationResult = await bookRequestValidator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetailsResponse(this);
        }

        var result = await bookService.CreateBookAsync(model);
        if (result.IsFailure) return result.ToProblemDetailsResponse(this);
        var created = result.Value;
        return CreatedAtAction(nameof(GetBookById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBook(Guid id, [FromBody] BookRequestModel model)
    {
        var validationResult = await bookRequestValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetailsResponse(this);
        }

        var result = await bookService.UpdateBookAsync(id, model);
        if (result.IsFailure) return result.ToProblemDetailsResponse(this);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        var result = await bookService.DeleteBookAsync(id);
        if (result.IsFailure) return result.ToProblemDetailsResponse(this);
        return NoContent();
    }
}