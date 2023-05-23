using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Models.Enteties;

namespace UrlShortenerApi.Host.Data.EntityConfigurations;

public class UrlTypeConfiguration 
    : IEntityTypeConfiguration<Url>
{
    public void Configure(EntityTypeBuilder<Url> builder)
    {
        builder.ToTable("Urls");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedNever();

        builder.Property(u => u.OriginalUrl)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(u => u.ShortCode)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(u => u.CreatedById)
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.HasOne(u => u.CreatedBy)
            .WithMany()
            .HasForeignKey(u => u.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}