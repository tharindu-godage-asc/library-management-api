using Library.Api.Application.Interfaces;
using Library.Api.Applications.Services;
using Library.Api.Common.Exceptions;
using Library.Api.Domain.Entities;
using Moq;
using Xunit;

namespace Library.Api.Tests.Services;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _bookRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    private readonly BookService _service;

    public BookServiceTests()
    {
        _bookRepository = new Mock<IBookRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();

        _service = new BookService(
            _bookRepository.Object,
            _unitOfWork.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenIsbnAlreadyExists()
    {
        var book = new Book(
            "Clean Code",
            "Robert Martin",
            "12345",
            2008,
            5);

        _bookRepository
            .Setup(x => x.GetByIsbnAsync("12345"))
            .ReturnsAsync(book);

        await Assert.ThrowsAsync<ConflictException>(
            () => _service.CreateAsync(book));
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrow_WhenBookNotFound()
    {
        _bookRepository
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((Book?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => _service.GetByIdAsync(1));
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrow_WhenBookNotFound()
    {
        _bookRepository
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((Book?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => _service.DeleteAsync(1));
    }
}