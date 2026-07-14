using Library.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Infrastructure.Data
{
    public class LibraryDbContext : DbContext
    {
        /*
 * LibraryDbContext
 * ----------------
 * LibraryDbContext is the bridge between the application and the PostgreSQL database.
 * It inherits from Entity Framework Core's DbContext, which provides the functionality
 * to query, insert, update, and delete data.
 *
 * The DbContext tracks changes made to entities and translates LINQ queries into SQL
 * statements that are executed against the database.
 */
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }
        /*
 * Constructor
 * -----------
 * Receives the database configuration (connection string, provider, etc.)
 * from ASP.NET Core's Dependency Injection (DI) container.
 *
 * The ': base(options)' passes these configuration options to the DbContext
 * base class so EF Core knows how to connect to the database.
 */

        /*
 * DbSet Properties
 * ----------------
 * Each DbSet<T> represents a table in the database.
 * EF Core uses these properties to perform CRUD operations.
 *
 * Books       -> Books table
 * Members     -> Members table
 * Borrowings  -> Borrowings table
 *
 * Examples:
 * _context.Books.ToListAsync()      -> SELECT * FROM Books
 * _context.Books.Add(book)          -> INSERT INTO Books
 * _context.Books.Remove(book)       -> DELETE FROM Books
 */
 
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Member> Members => Set<Member>();
        public DbSet<Borrowing> Borrowings => Set<Borrowing>();

        /*
 * OnModelCreating()
 * -----------------
 * This method is called when EF Core builds the database model.
 *
 * ApplyConfigurationsFromAssembly() automatically finds every class in the
 * current assembly that implements IEntityTypeConfiguration<T> and applies
 * those configurations.
 *
 * This keeps entity configuration (relationships, constraints, property rules,
 * indexes, etc.) separate from the entity classes, making the project cleaner
 * and easier to maintain.
 *
 * Example:
 * BookConfiguration.cs
 * MemberConfiguration.cs
 * BorrowingConfiguration.cs
 *
 * All of these configuration classes are discovered and applied automatically.
 */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryDbContext).Assembly);
        }
    }
}
