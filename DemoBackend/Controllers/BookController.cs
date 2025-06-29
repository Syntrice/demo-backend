using DemoBackend.Services;
using Microsoft.AspNetCore.Mvc;
using DemoBackend.Models.Books;
using DemoBackend.Models.Books.Requests;
using DemoBackend.Models.Errors;
using FluentValidation;

namespace DemoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController(IBookService bookService, IValidator<BookRequestModel> bookRequestValidator)
    : ControllerBase, IBookController
{
    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await bookService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBookById(Guid id)
    {
        var book = await bookService.GetBookByIdAsync(id);
        if (book == null) return NotFound();
        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] BookRequestModel model)
    {
        var validationResult = await bookRequestValidator.ValidateAsync(model);
        
        if (!validationResult.IsValid)
        {
            return BadRequest(new ErrorResponseModel()
                { Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList() });
        }

        var created = await bookService.CreateBookAsync(model);
        return CreatedAtAction(nameof(GetBookById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBook(Guid id, [FromBody] BookRequestModel model)
    {
        var validationResult = await bookRequestValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ErrorResponseModel()
                { Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList() });
        }

        await bookService.UpdateBookAsync(id, model);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        await bookService.DeleteBookAsync(id);
        return NoContent();
    }
}