using Library.Api.Application.Interfaces;
using Library.Api.Common.Exceptions;
using Library.Api.Contracts.Books;
using Library.Api.Domain.Entities;

namespace Library.Api.Applications.Services;

public class BookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BookService(
        IBookRepository bookRepository,
        IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _bookRepository.GetAllAsync();
    }

    public async Task<Book> GetByIdAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book is null)
        {
            throw new NotFoundException(
                "Book not found.");
        }

        return book;
    }

    public async Task<Book> CreateAsync(Book book)
    {
        var existingBook =
            await _bookRepository.GetByIsbnAsync(book.Isbn);

        if (existingBook is not null)
        {
            throw new InvalidOperationException(
                "ISBN already exists.");
        }

        await _bookRepository.AddAsync(book);

        await _unitOfWork.SaveChangesAsync();

        return book;
    }

    public async Task DeleteAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book is null)
        {
            throw new KeyNotFoundException("Book not found.");
        }

        _bookRepository.Delete(book);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(
    int id,
    UpdateBookRequest request)
    {
        var book =
            await _bookRepository.GetByIdAsync(id);

        if (book is null)
        {
            throw new NotFoundException(
                "Book not found.");
        }

        var existingBook =
            await _bookRepository.GetByIsbnAsync(
                request.Isbn);

        if (existingBook is not null &&
            existingBook.Id != id)
        {
            throw new ConflictException(
                "ISBN already exists.");
        }

        book.Update(
            request.Title,
            request.Author,
            request.Isbn,
            request.PublishedYear,
            request.TotalCopies);

        _bookRepository.Update(book);

        await _unitOfWork.SaveChangesAsync();
    }
}