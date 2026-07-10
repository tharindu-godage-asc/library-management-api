using Library.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Api.Infrastructure.Data.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");

        builder.HasKey(b => b.Id);

        builder.HasIndex(b => b.Isbn)
               .IsUnique();

        builder.Property(b => b.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(b => b.Author)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(b => b.Isbn)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(b => b.PublishedYear)
               .IsRequired();

        builder.Property(b => b.TotalCopies)
               .IsRequired();

        builder.Property(b => b.AvailableCopies)
               .IsRequired();
    }
}