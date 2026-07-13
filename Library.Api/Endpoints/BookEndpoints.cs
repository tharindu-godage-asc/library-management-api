using Microsoft.AspNetCore.Mvc;
using Library.Api.Applications.Services;
using Library.Api.Common.Filters;
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

        // Get All Books with Pagination
        _ = group.MapGet("/",
            async (
                BookService service,
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 10) =>
            {
                if (pageNumber <= 0)
                {
                    return Results.BadRequest(
                        "Page number must be greater than 0.");
                }

                if (pageSize <= 0)
                {
                    return Results.BadRequest(
                        "Page size must be greater than 0.");
                }

                var books =
                    await service.GetPagedAsync(
                        pageNumber,
                        pageSize);

                return Results.Ok(
                    books.Select(x => x.ToResponse()));
            });

        //Search By Author or Title
        group.MapGet("/search",
            async (
                string? title,
                string? author,
                BookService service) =>
            {
                var books =
                    await service.SearchAsync(
                        title,
                        author);

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
            })
            .AddEndpointFilter<ValidationFilter<CreateBookRequest>>();

        //Update Book
        group.MapPut("/{id:int}",
            async (
                int id,
                UpdateBookRequest request,
                BookService service) =>
            {
                await service.UpdateAsync(id, request);

                return Results.NoContent();
            })
            .AddEndpointFilter<ValidationFilter<UpdateBookRequest>>();

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