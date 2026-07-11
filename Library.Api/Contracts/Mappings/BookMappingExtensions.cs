using Library.Api.Contracts.Books;
using Library.Api.Domain.Entities;

namespace Library.Api.Contracts.Mappings;

public static class BookMappingExtensions
{
    public static BookResponse ToResponse(this Book book)
    {
        return new BookResponse(
            book.Id,
            book.Title,
            book.Author,
            book.Isbn,
            book.PublishedYear,
            book.TotalCopies,
            book.AvailableCopies
        );
    }
}