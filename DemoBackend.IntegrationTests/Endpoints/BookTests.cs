using System.Net;
using System.Net.Http.Json;
using DemoBackend.IntegrationTests.Fixtures;
using DemoBackend.Models.Authors.Requests;
using DemoBackend.Models.Authors.Responses;
using DemoBackend.Models.Books.Requests;
using DemoBackend.Models.Books.Responses;
using Shouldly;
using Xunit.Abstractions;

namespace DemoBackend.IntegrationTests.Endpoints;

public class BookTests : IClassFixture<IntegrationTestFixture>
{
    private readonly IntegrationTestFixture _fixture;
    private readonly ITestOutputHelper _output;

    public BookTests(IntegrationTestFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
    }

    private async Task<Guid> CreateAuthorAsync(string? name = null)
    {
        var model = new AuthorRequestModel { Name = name ?? $"Author {Guid.NewGuid()}" };
        var response = await _fixture.Client.PostAsJsonAsync("/api/author", model);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var author = await response.Content.ReadFromJsonAsync<AuthorDetailsResponseModel>();
        author.ShouldNotBeNull();
        return author!.Id;
    }

    private async Task<Guid> CreateBookAsync(Guid authorId, string? title = null)
    {
        var model = new BookRequestModel
        {
            Title = title ?? $"Book {Guid.NewGuid()}",
            AuthorIds = new[] { authorId }
        };
        var response = await _fixture.Client.PostAsJsonAsync("/api/book", model);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var book = await response.Content.ReadFromJsonAsync<BookDetailsResponseModel>();
        book.ShouldNotBeNull();
        return book!.Id;
    }

    [Fact]
    public async Task GetAllBooks_ReturnsSeededBooks()
    {
        // act
        var response = await _fixture.Client.GetAsync("/api/book");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var books = await response.Content.ReadFromJsonAsync<List<BookDetailsResponseModel>>();
        books.ShouldNotBeNull();
        books.Count.ShouldBeGreaterThan(0);

        _output.WriteLine(books.Select(e => e.Title).Aggregate((a, b) => $"{a}, {b}"));

        // sample assertion on first item
        books![0].Title.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task CreateBook_WithValidModel_ReturnsCreated()
    {
        var authorId = await CreateAuthorAsync();

        var model = new BookRequestModel { Title = "New Book", AuthorIds = new[] { authorId } };
        var response = await _fixture.Client.PostAsJsonAsync("/api/book", model);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var created = await response.Content.ReadFromJsonAsync<BookDetailsResponseModel>();
        created.ShouldNotBeNull();
        created!.Id.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public async Task GetBookById_ReturnsBook()
    {
        var authorId = await CreateAuthorAsync();
        var bookId = await CreateBookAsync(authorId);

        var response = await _fixture.Client.GetAsync($"/api/book/{bookId}");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var book = await response.Content.ReadFromJsonAsync<BookDetailsResponseModel>();
        book.ShouldNotBeNull();
        book!.Id.ShouldBe(bookId);
    }

    [Fact]
    public async Task UpdateBook_ReturnsNoContent()
    {
        var authorId = await CreateAuthorAsync();
        var bookId = await CreateBookAsync(authorId);

        var update = new BookRequestModel { Title = "Updated Title", AuthorIds = new[] { authorId } };
        var response = await _fixture.Client.PutAsJsonAsync($"/api/book/{bookId}", update);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteBook_ReturnsNoContent()
    {
        var authorId = await CreateAuthorAsync();
        var bookId = await CreateBookAsync(authorId);

        var response = await _fixture.Client.DeleteAsync($"/api/book/{bookId}");
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}