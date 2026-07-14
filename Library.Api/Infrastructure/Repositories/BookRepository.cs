using Library.Api.Application.Interfaces;
using Library.Api.Domain.Entities;
using Library.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;
using System.Threading.Channels;

namespace Library.Api.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LibraryDbContext _context;

    public BookRepository(LibraryDbContext context)
    {
        _context = context;
    }

    //This is the constructor of the BookRepository class. A constructor is a special
    //method that is automatically called whenever a BookRepository object is created.

    //The parameter 'LibraryDbContext context' is the application's Entity Framework
    //Core database context.It provides access to the database through DbSets such as
    //Books, Members, and Borrowings.

    //The line '_context = context;' stores the provided database context in the
    //private field '_context', allowing all methods in BookRepository to use the
    //same database connection.

//    For example:
//- _context.Books.ToListAsync() retrieves all books.
//- _context.Books.FindAsync(id) retrieves a book by its ID.
//- _context.SaveChangesAsync() saves changes to the database.

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Book?> GetByIsbnAsync(string isbn)
    {
        return await _context.Books
            .FirstOrDefaultAsync(b => b.Isbn == isbn);
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books.ToListAsync();
    }

    public async Task AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
    }

    public void Update(Book book)
    {
        _context.Books.Update(book);
    }

    public void Delete(Book book)
    {
        _context.Books.Remove(book);
    }

    public async Task<IEnumerable<Book>> GetPagedAsync(
        int pageNumber,
        int pageSize)
    {
        return await _context.Books
            .OrderBy(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> SearchAsync(
    string? title,
    string? author)
    {
        var query = _context.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
        {
            query = query.Where(x =>
                x.Title.Contains(title));
        }

        if (!string.IsNullOrWhiteSpace(author))
        {
            query = query.Where(x =>
                x.Author.Contains(author));
        }

        return await query
            .OrderBy(x => x.Id)
            .ToListAsync();
    }
}