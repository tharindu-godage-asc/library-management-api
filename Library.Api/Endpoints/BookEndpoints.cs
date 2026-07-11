using Library.Api.Applications.Services;
using Library.Api.Contracts.Books;
using Library.Api.Contracts.Mappings;
using Library.Api.Domain.Entities;

namespace Library.Api.Endpoints;

public static class BookEndpoints
{
    public static void MapBookEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/books")
            .WithTags("Books");

        // Get All Books
        group.MapGet("/",
            async (BookService service) =>
            {
                var books = await service.GetAllAsync();

                return Results.Ok(
                    books.Select(x => x.ToResponse()));
            });

        // Get Book By Id
        group.MapGet("/{id:int}",
            async (int id, BookService service) =>
            {
                var book = await service.GetByIdAsync(id);

                return Results.Ok(
                    book?.ToResponse());
            });

        // Create Book
        group.MapPost("/",
            async (
                CreateBookRequest request,
                BookService service) =>
            {
                var book = new Book(
                    request.Title,
                    request.Author,
                    request.Isbn,
                    request.PublishedYear,
                    request.TotalCopies);

                await service.CreateAsync(book);

                return Results.Created(
                    $"/api/books/{book.Id}",
                    book.ToResponse());
            });

        // Delete Book
        group.MapDelete("/{id:int}",
            async (
                int id,
                BookService service) =>
            {
                await service.DeleteAsync(id);

                return Results.NoContent();
            });
    }
}