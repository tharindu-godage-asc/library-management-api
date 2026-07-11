using Library.Api.Domain.Entities;

namespace Library.Api.Application.Interfaces;

public interface IBorrowingRepository
{
    Task<Borrowing?> GetByIdAsync(int id);

    Task<IEnumerable<Borrowing>> GetAllAsync();

    Task<IEnumerable<Borrowing>> GetByMemberIdAsync(int memberId);

    Task<int> GetActiveBorrowingsCountAsync(int memberId);

    Task AddAsync(Borrowing borrowing);

    void Update(Borrowing borrowing);
}