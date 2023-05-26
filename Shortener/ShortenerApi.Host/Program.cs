using Infrastructure.Filters;
using Microsoft.OpenApi.Models;
using UrlShortenerApi.Host.Data;
using UrlShortenerApi.Configurations.Configurations;
using UrlShortenerApi.Host.Repositories.Interfaces;
using UrlShortenerApi.Host.Repositories;
using UrlShortenerApi.Host.Services.Interfaces;
using UrlShortenerApi.Host.Services;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;

var configuration = GetConfiguration();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
})
    .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true)
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "shortener HTTP API",
        Version = "v1",
        Description = "The Server Service HTTP API"
    });
});

builder.Services.Configure<ShortnerConfig>(configuration);

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddDbContextFactory<ApplicationDbContext>(opts => opts.UseNpgsql(configuration["ConnectionString"]));

builder.Services.AddScoped<IUrlRepository, UrlRepository>();
builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAboutService, AboutService>();
builder.Services.AddScoped<IAboutRepository, AboutRepository>();
builder.Services.AddScoped<IDbContextWrapper<ApplicationDbContext>>(provider =>
{
var dbContextFactory = provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
return new DbContextWrapper<ApplicationDbContext>(dbContextFactory);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        builder => builder
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

app.UseSwagger()
    .UseSwaggerUI(setup =>
    {
        setup.SwaggerEndpoint($"{configuration["PathBase"]}/swagger/v1/swagger.json", "Shortener.API V1");
    });

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapControllers();
});

CreateDbIfNotExists(app);
app.Run();

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
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            DbInitializer.Initialize(context).Wait();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }
}