using Infrastructure.Filters;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using UrlShortenerApi.Host.Data;
using UrlShortenerApi.Configurations.Configurations;
using Microsoft.Extensions.Hosting;
using System.IO;
using UrlShortenerApi.Host.Repositories.Interfaces;
using UrlShortenerApi.Host.Repositories;
using UrlShortenerApi.Host.Services.Interfaces;
using UrlShortenerApi.Host.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;

var configuration = GetConfiguration();

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(HttpGlobalExceptionFilter));
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        })
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Shortener host",
                Version = "v1",
                Description = "The UrlShortenerApi.Host Service HTTP API"
            });
            #region authorization
            /*
            var authority = configuration["Authorization:Authority"];
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows()
                {
                    Implicit = new OpenApiOAuthFlow()
                    {
                        AuthorizationUrl = new Uri($"{authority}/connect/authorize"),
                        TokenUrl = new Uri($"{authority}/connect/token"),
                        Scopes = new Dictionary<string, string>()
                        {
                            { "mvc", "website" },
                            { "catalog.catalogitem", "catalog.catalogitem" },
                            { "catalog.catalogtype", "catalog.catalogtype" },
                            { "catalog.catalogbrand", "catalog.catalogbrand" },
                            { "order.makeorder", "order.makeorder" },
                        }
                    }
                }
            });

            options.OperationFilter<AuthorizeCheckOperationFilter>();
            */
            #endregion
        });

        services.AddSingleton<IConfiguration>(configuration);
        services.Configure<ShortnerConfig>(configuration);

        // services.AddAuthorization();

        services.AddAutoMapper(typeof(Program));

        services.AddCors(options =>
        {
            options.AddPolicy(
                "CorsPolicy",
                builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration["ConnectionString"]));

        services.AddScoped<IUrlRepository, UrlRepository>();
        services.AddScoped<IUrlService, UrlService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAboutService, AboutService>();
        services.AddScoped<IAboutRepository, AboutRepository>();
        services.AddScoped<IDbContextWrapper<ApplicationDbContext>>(provider =>
        {
            var dbContextFactory = provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
            return new DbContextWrapper<ApplicationDbContext>(dbContextFactory);
        });
        services.AddLogging();
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.Configure(app =>
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shortener host API V1");
            });

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        });
    });

var host = builder.Build();

// CreateDbIfNotExists(host);
host.Run();

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}

void CreateDbIfNotExists(IHost host)
{
    using var scope = host.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        DbInitializer.Initialize(context).Wait();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}
