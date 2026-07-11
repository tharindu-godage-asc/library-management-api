using Library.Api.Application.Interfaces;
using Library.Api.Domain.Entities;

namespace Library.Api.Applications.Services;

public class BorrowingService
{
    private readonly IBookRepository _bookRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IBorrowingRepository _borrowingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BorrowingService(
        IBookRepository bookRepository,
        IMemberRepository memberRepository,
        IBorrowingRepository borrowingRepository,
        IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _memberRepository = memberRepository;
        _borrowingRepository = borrowingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Borrowing> BorrowBookAsync(
        int memberId,
        int bookId)
    {
        var member =
            await _memberRepository.GetByIdAsync(memberId)
            ?? throw new KeyNotFoundException("Member not found.");

        if (!member.IsActive)
        {
            throw new InvalidOperationException(
                "Member is inactive.");
        }

        var activeBorrowings =
            await _borrowingRepository
                .GetActiveBorrowingsCountAsync(memberId);

        if (activeBorrowings >= 3)
        {
            throw new InvalidOperationException(
                "Member borrowing limit exceeded.");
        }

        var book =
            await _bookRepository.GetByIdAsync(bookId)
            ?? throw new KeyNotFoundException("Book not found.");

        // Uses your domain logic
        book.BorrowCopy();

        var borrowing = new Borrowing(
            bookId,
            memberId);

        await _borrowingRepository.AddAsync(borrowing);

        _bookRepository.Update(book);

        await _unitOfWork.SaveChangesAsync();

        return borrowing;
    }

    public async Task ReturnBookAsync(int borrowingId)
    {
        var borrowing =
            await _borrowingRepository.GetByIdAsync(borrowingId)
            ?? throw new KeyNotFoundException(
                "Borrowing record not found.");

        var book =
            await _bookRepository.GetByIdAsync(borrowing.BookId)
            ?? throw new KeyNotFoundException(
                "Book not found.");

        borrowing.MarkReturned();

        book.ReturnCopy();

        _borrowingRepository.Update(borrowing);
        _bookRepository.Update(book);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<Borrowing>> GetHistoryAsync()
    {
        return await _borrowingRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Borrowing>> GetMemberHistoryAsync(
        int memberId)
    {
        return await _borrowingRepository
            .GetByMemberIdAsync(memberId);
    }
}