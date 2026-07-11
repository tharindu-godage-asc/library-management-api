namespace Library.Api.Contracts.Borrowings;

public record CreateBorrowingRequest(
    int BookId,
    int MemberId
);