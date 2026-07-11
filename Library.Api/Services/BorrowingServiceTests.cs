using Library.Api.Application.Interfaces;
using Library.Api.Applications.Services;
using Library.Api.Domain.Entities;
using Library.Api.Infrastructure.Repositories;
using Moq;
using Xunit;

namespace Library.Api.Tests.Services;

public class BorrowingServiceTests
{
    private readonly Mock<IBookRepository> _bookRepository;
    private readonly Mock<IMemberRepository> _memberRepository;
    private readonly Mock<IBorrowingRepository> _borrowingRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    private readonly BorrowingService _service;

    public BorrowingServiceTests()
    {
        _bookRepository = new Mock<IBookRepository>();
        _memberRepository = new Mock<IMemberRepository>();
        _borrowingRepository = new Mock<IBorrowingRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();

        _service = new BorrowingService(
            _bookRepository.Object,
            _memberRepository.Object,
            _borrowingRepository.Object,
            _unitOfWork.Object);
    }

    //Book Cannot Be Borrowed When No Copies Are Available
    [Fact]
    public async Task BorrowBook_ShouldThrow_WhenNoCopiesAvailable()
    {
        var book = new Book(
            "Test",
            "Author",
            "123",
            2020,
            1);

        book.BorrowCopy();

        var member =
            new Member(
                "John",
                "john@test.com",
                "123");

        _bookRepository
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(book);

        _memberRepository
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(member);

        _borrowingRepository
            .Setup(x =>
                x.GetActiveBorrowingsCountAsync(1))
            .ReturnsAsync(0);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.BorrowBookAsync(1, 1));
    }

    //Inactive Member Cannot Borrow
    [Fact]
    public async Task BorrowBook_ShouldThrow_WhenMemberInactive()
    {
        var member =
            new Member(
                "John",
                "john@test.com",
                "123");

        member.Deactivate();

        _memberRepository
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(member);

        await Assert.ThrowsAsync<Exception>(
            () => _service.BorrowBookAsync(1, 1));
    }

    //Member Cannot Borrow More Than 3 Books
    [Fact]
    public async Task BorrowBook_ShouldThrow_WhenBorrowLimitExceeded()
    {
        var member =
            new Member(
                "John",
                "john@test.com",
                "123");

        var book = new Book(
            "Test",
            "Author",
            "123",
            2020,
            5);

        _memberRepository
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(member);

        _bookRepository
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(book);

        _borrowingRepository
            .Setup(x =>
                x.GetActiveBorrowingsCountAsync(1))
            .ReturnsAsync(3);

        await Assert.ThrowsAsync<Exception>(
            () => _service.BorrowBookAsync(1, 1));
    }

    //Returning Book Increases Available Copies
    [Fact]
    public async Task ReturnBook_ShouldIncreaseAvailableCopies()
    {
        var book = new Book(
            "Test",
            "Author",
            "123",
            2020,
            5);

        book.BorrowCopy();

        var borrowing =
            new Borrowing(1, 1);

        _borrowingRepository
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(borrowing);

        _bookRepository
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(book);

        var before = book.AvailableCopies;

        await _service.ReturnBookAsync(1);

        Assert.Equal(
            before + 1,
            book.AvailableCopies);
    }

    //Book Cannot Be Returned Twice
    [Fact]
    public async Task ReturnBook_ShouldThrow_WhenReturnedTwice()
    {
        var book = new Book(
            "Test",
            "Author",
            "123",
            2020,
            5);

        var borrowing =
            new Borrowing(1, 1);

        borrowing.MarkReturned();

        _bookRepository
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(book);

        _borrowingRepository
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(borrowing);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.ReturnBookAsync(1));
    }
}