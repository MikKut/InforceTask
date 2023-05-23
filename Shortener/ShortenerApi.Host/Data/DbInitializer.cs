using Microsoft.Extensions.Configuration;
using UrlShortener.Models.Enteties;
using UrlShortener.Models.Enums;

namespace UrlShortenerApi.Host.Data;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        _ = await context.Database.EnsureCreatedAsync();

        if (!context.Urls.Any())
        {
            await context.Urls.AddRangeAsync(GetPreconfiguredUrls());
            _ = await context.SaveChangesAsync();
        }

        if (!context.Users.Any())
        {
            await context.Users.AddRangeAsync(GetPreconfiguredUsers());
            _ = await context.SaveChangesAsync();
        }

        if (!context.About.Any())
        {
            await context.About.AddRangeAsync(GetPreconfiguredAbout());
            _ = await context.SaveChangesAsync();
        }
    }

    private static IEnumerable<Url> GetPreconfiguredUrls()
    {
        return new List<Url>()
        {
            new Url() { Id = new Guid("f40884b7-2b59-4a8c-8fb1-79175f5f04c6"), CreatedAt = new DateTime(2022, 9, 15, 10, 23, 45), OriginalUrl = "https://www.youtube.com/results?search_query=mozart", ShortCode = "abrakadabra228", CreatedById = new Guid("25d9f16d-d4e7-4f8b-9c78-42568f829caa")},
            new Url() { Id = new Guid("e91b6420-3c99-40e5-a34d-42dd62879f2a"),CreatedAt = new DateTime(2022, 5, 15, 10, 23, 45), OriginalUrl = "https://www.youtube.com/watch?v=27uXkPstJNU&ab_channel=CLASSICALCHANNEL", ShortCode = "abrakadabra229", CreatedById = new Guid("67e8c907-9a20-4b89-8b04-eb3f4a4e26a0") },
        };
    }

    private static IEnumerable<User> GetPreconfiguredUsers()
    {
        return new List<User>()
        {
            new User(new Guid("25d9f16d-d4e7-4f8b-9c78-42568f829caa"), "admin", "password", Role.Admin, new DateTime(2022, 9, 15, 10, 23, 0)),
            new User(new Guid("67e8c907-9a20-4b89-8b04-eb3f4a4e26a0"), "user", "password", Role.User,  new DateTime(2022, 5, 15, 10, 23, 0)),
        };
    }

    private static About GetPreconfiguredAbout()
    {
        return new About()
        {
            Content = "Some description"
        };
    }
}