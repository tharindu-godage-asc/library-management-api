namespace Library.Api.Contracts.Borrowings;

public record BorrowingResponse(
    int Id,
    int BookId,
    int MemberId,
    DateTime BorrowedDate,
    DateTime DueDate,
    DateTime? ReturnedDate,
    string Status
);