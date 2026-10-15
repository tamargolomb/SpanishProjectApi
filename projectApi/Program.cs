using Microsoft.EntityFrameworkCore;
using DAL;
using Microsoft.Extensions.DependencyInjection;
using BLL.Mapper;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<SpanishDB>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Configuration.GetConnectionString("SpanishProject");
builder.Services.AddAutoMapper( cfg =>
{
    cfg.AddProfile<MappingProfile>();
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
