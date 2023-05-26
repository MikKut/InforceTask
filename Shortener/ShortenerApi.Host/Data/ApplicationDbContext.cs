using Microsoft.EntityFrameworkCore;
using System;
using UrlShortener.Models.Enteties;
using UrlShortenerApi.Host.Data.EntityConfigurations;

namespace UrlShortenerApi.Host.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Url> Urls { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UrlTypeConfiguration());
    }
}
