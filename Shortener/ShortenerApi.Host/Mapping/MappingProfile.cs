using AutoMapper;
using UrlShortener.Models.Dto;
using UrlShortener.Models.Enteties;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace UrlShortenerApi.Host.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Url, UrlDto>()
                .ForMember(dest => dest.ShortenedUrl, opt => opt.MapFrom(src => ConfigurationManager.AppSettings["ClientShortUrl"] + src.ShortCode))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));
            CreateMap<About, AboutDto>();
            CreateMap<UrlDto, Url>()
    .ForMember(dest => dest.ShortCode, opt => opt.MapFrom(src => src.ShortenedUrl.Replace(ConfigurationManager.AppSettings["ClientShortUrl"], String.Empty)));

        }
    }
}
