using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Models.Enteties;

namespace UrlShortenerApi.Host.Data.EntityConfigurations;

public class AboutTypeConfiguration 
    : IEntityTypeConfiguration<About>
{
    public void Configure(EntityTypeBuilder<About> builder)
    {

        builder.ToTable("About");

        builder.Property(a => a.Content)
            .IsRequired()
            .HasMaxLength(2000);

        builder.HasIndex(a => a.Content).IsUnique();
    }
}