using Library.Api.Contracts.Borrowings;
using Library.Api.Domain.Entities;

namespace Library.Api.Contracts.Mappings;

public static class BorrowingMappingExtensions
{
    public static BorrowingResponse ToResponse(this Borrowing borrowing)
    {
        return new BorrowingResponse(
            borrowing.Id,
            borrowing.BookId,
            borrowing.MemberId,
            borrowing.BorrowedDate,
            borrowing.DueDate,
            borrowing.ReturnedDate,
            borrowing.Status.ToString()
        );
    }
}