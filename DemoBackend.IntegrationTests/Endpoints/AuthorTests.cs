using System.Net;
using System.Net.Http.Json;
using DemoBackend.IntegrationTests.Fixtures;
using DemoBackend.Models.Authors.Requests;
using DemoBackend.Models.Authors.Responses;
using DemoBackend.Models.Books.Requests;
using DemoBackend.Models.Books.Responses;
using Shouldly;

namespace DemoBackend.IntegrationTests.Endpoints;

public class AuthorTests(IntegrationTestFixture fixture)
    : IClassFixture<IntegrationTestFixture>
{
    private readonly IntegrationTestFixture _fixture = fixture;

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
    public async Task GetAllAuthors_ReturnsAuthors()
    {
        var response = await _fixture.Client.GetAsync("/api/author");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateAuthor_ReturnsCreated()
    {
        var model = new AuthorRequestModel { Name = "New Author" };
        var response = await _fixture.Client.PostAsJsonAsync("/api/author", model);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var author = await response.Content.ReadFromJsonAsync<AuthorDetailsResponseModel>();
        author.ShouldNotBeNull();
        author!.Id.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public async Task GetAuthorById_ReturnsAuthor()
    {
        var id = await CreateAuthorAsync();
        var response = await _fixture.Client.GetAsync($"/api/author/{id}");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var author = await response.Content.ReadFromJsonAsync<AuthorDetailsResponseModel>();
        author.ShouldNotBeNull();
        author!.Id.ShouldBe(id);
    }

    [Fact]
    public async Task UpdateAuthor_ReturnsNoContent()
    {
        var id = await CreateAuthorAsync();
        var update = new AuthorRequestModel { Name = "Updated Name" };

        var response = await _fixture.Client.PutAsJsonAsync($"/api/author/{id}", update);
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteAuthor_ReturnsNoContent()
    {
        var id = await CreateAuthorAsync();
        var response = await _fixture.Client.DeleteAsync($"/api/author/{id}");
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetAuthorBooks_ReturnsBooks()
    {
        var authorId = await CreateAuthorAsync();
        _ = await CreateBookAsync(authorId, title: "Book 1");

        var response = await _fixture.Client.GetAsync($"/api/author/{authorId}/books");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var books = await response.Content.ReadFromJsonAsync<List<BookResponseModel>>();
        books.ShouldNotBeNull();
        books!.Count.ShouldBeGreaterThan(0);
    }
}