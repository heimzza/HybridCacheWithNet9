using FormulaOne.Api.Filters;
using FormulaOne.Api.Services;
using FormulaOne.DataService.Persistence;
using FormulaOne.Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SchemaFilter<SwaggerIgnoreIdFilter>();
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddMemoryCache();

builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = builder.Configuration.GetConnectionString("Redis");
    option.InstanceName = "FormulaOne_";
});

builder.Services.AddScoped<ICachingService, CachingService>();

#pragma warning disable EXTEXP0018
builder.Services.AddHybridCache(options =>
{
    options.MaximumKeyLength = 30;

    options.MaximumPayloadBytes = 1024 * 1024; // 1 MB

    options.DefaultEntryOptions = new HybridCacheEntryOptions()
    {
        Expiration = TimeSpan.FromSeconds(30)
    };
});
#pragma warning restore EXTEXP0018

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();