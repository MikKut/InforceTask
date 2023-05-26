using AutoMapper;
using UrlShortener.Models.Dto;
using UrlShortener.Models.Enteties;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        var configuration = GetConfiguration();

        CreateMap<User, UserDto>();
        CreateMap<Url, UrlDto>()
            .ForMember(dest => dest.ShortenedUrl, opt => opt.MapFrom(src => string.Concat(configuration.GetValue<string>("ClientShortUrl"), src.ShortCode)))
            .ForMember(dest => dest.CreatedById, opt => opt.MapFrom(src => src.CreatedById));
        CreateMap<About, AboutDto>();
        CreateMap<UrlDto, Url>()
            .ForMember(dest => dest.ShortCode, opt => opt.MapFrom(src =>
                src.ShortenedUrl != null
                    ? src.ShortenedUrl.Replace(configuration.GetValue<string>("ClientShortUrl"), string.Empty)
                    : string.Empty));
    }

    private IConfiguration GetConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}