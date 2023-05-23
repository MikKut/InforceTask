using Microsoft.EntityFrameworkCore;
using System;
using UrlShortener.Models.Enteties;
using UrlShortenerApi.Host.Data.EntityConfigurations;

namespace UrlShortenerApi.Host.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Url> Urls { get; set; }
    public DbSet<About> About { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<About>().HasNoKey();
        modelBuilder.ApplyConfiguration(new UserTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UrlTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AboutTypeConfiguration());
    }
}
