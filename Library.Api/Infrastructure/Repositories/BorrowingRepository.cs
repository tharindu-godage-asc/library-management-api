using Library.Api.Application.Interfaces;
using Library.Api.Domain.Entities;
using Library.Api.Domain.Enums;
using Library.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Infrastructure.Repositories;

public class BorrowingRepository : IBorrowingRepository
{
    private readonly LibraryDbContext _context;

    public BorrowingRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<Borrowing?> GetByIdAsync(int id)
    {
        return await _context.Borrowings
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Borrowing>> GetAllAsync()
    {
        return await _context.Borrowings.ToListAsync();
    }

    public async Task<IEnumerable<Borrowing>> GetByMemberIdAsync(int memberId)
    {
        return await _context.Borrowings
            .Where(b => b.MemberId == memberId)
            .ToListAsync();
    }

    public async Task<int> GetActiveBorrowingsCountAsync(int memberId)
    {
        return await _context.Borrowings.CountAsync(b =>
            b.MemberId == memberId &&
            b.Status == BorrowingStatus.Borrowed);
    }

    public async Task AddAsync(Borrowing borrowing)
    {
        await _context.Borrowings.AddAsync(borrowing);
    }

    public void Update(Borrowing borrowing)
    {
        _context.Borrowings.Update(borrowing);
    }
}