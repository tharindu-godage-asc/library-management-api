using Library.Api.Domain.Entities;

namespace Library.Api.Application.Interfaces;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(int id);

    Task<Book?> GetByIsbnAsync(string isbn);

    Task<IEnumerable<Book>> GetAllAsync();

    Task AddAsync(Book book);

    void Update(Book book);

    void Delete(Book book);
}