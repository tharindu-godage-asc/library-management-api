namespace Library.Api.Contracts.Books;

public record BookResponse(
    int Id,
    string Title,
    string Author,
    string Isbn,
    int PublishedYear,
    int TotalCopies,
    int AvailableCopies
);