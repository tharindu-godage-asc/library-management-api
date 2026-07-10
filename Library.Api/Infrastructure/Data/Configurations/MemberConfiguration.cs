using Library.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Api.Infrastructure.Data.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members");

        builder.HasKey(m => m.Id);

        builder.HasIndex(m => m.Email)
               .IsUnique();

        builder.Property(m => m.FullName)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(m => m.Email)
               .IsRequired()
               .HasMaxLength(256);

        builder.Property(m => m.PhoneNumber)
               .HasMaxLength(20);

        builder.Property(m => m.RegisteredDate)
               .IsRequired();

        builder.Property(m => m.IsActive)
               .IsRequired();
    }
}