using Library.Api.Domain.Entities;

namespace Library.Api.Application.Interfaces;

public interface IBorrowingService
{
    Task<Borrowing> BorrowBookAsync(
        int memberId,
        int bookId);

    Task ReturnBookAsync(int borrowingId);

    Task<IEnumerable<Borrowing>> GetHistoryAsync();

    Task<IEnumerable<Borrowing>> GetMemberHistoryAsync(
        int memberId);
}
