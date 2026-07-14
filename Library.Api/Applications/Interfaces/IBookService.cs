using Library.Api.Contracts.Books;
using Library.Api.Domain.Entities;

namespace Library.Api.Application.Interfaces;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAllAsync();

    Task<Book> GetByIdAsync(int id);

    Task<Book> CreateAsync(Book book);

    Task UpdateAsync(
        int id,
        UpdateBookRequest request);

    Task DeleteAsync(int id);

    Task<IEnumerable<Book>> GetPagedAsync(
        int pageNumber,
        int pageSize);

    Task<IEnumerable<Book>> SearchAsync(
        string? title,
        string? author);
}