using Library.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Api.Infrastructure.Data.Configurations;

public class BorrowingConfiguration : IEntityTypeConfiguration<Borrowing>
{
    public void Configure(EntityTypeBuilder<Borrowing> builder)
    {
        builder.ToTable("Borrowings");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.BorrowedDate)
               .IsRequired();

        builder.Property(b => b.DueDate)
               .IsRequired();

        builder.Property(b => b.Status)
               .IsRequired();

        builder.HasOne<Book>()
               .WithMany()
               .HasForeignKey(b => b.BookId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Member>()
               .WithMany()
               .HasForeignKey(b => b.MemberId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}