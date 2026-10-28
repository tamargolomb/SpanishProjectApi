using BLL.Mapper;
using BLL.Services;
using DAL;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var imagesFolder = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, builder.Configuration["ImagesFolder"]));
Directory.CreateDirectory(imagesFolder);

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
//DAL
builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<ILessonsRepository, LessonsRepository>();
builder.Services.AddScoped<ILevelsRepository, LevelsRepository>();
builder.Services.AddScoped<IReviewsAndRatingsRepository, ReviewsAndRatingsRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

// BLL
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IImageService>(new ImageService(imagesFolder));
builder.Services.AddScoped<IReviewAndRatingService, ReviewAndRatingService>();
builder.Services.AddScoped<ILevelService, LevelService>();
builder.Services.AddScoped<ILessonService, LessonService>();

var app = builder.Build();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesFolder),
    RequestPath = "/images"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
