using BLL.Mapper;
using BLL.Services;
using DAL;
using DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using OpenAI;
using projectApi.Filters;
using System.ClientModel;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//--------AI Agent
var apiKey = builder.Configuration["GEMINI:APIKEY"];
//?? throw new InvalidOperationException("Gemini:ApiKey is missing");
// הכתובת של Gemini שמדברת בפורמט של OpenAI
var options = new OpenAIClientOptions
{
    Endpoint = new Uri("https://generativelanguage.googleapis.com/v1beta/openai/")
};
IChatClient innerClient = new OpenAIClient(new ApiKeyCredential(apiKey), options)
    .GetChatClient("gemini-2.5-flash")
    .AsIChatClient();

builder.Services.AddChatClient(innerClient)
    .UseFunctionInvocation();   // מריץ את הכלים שהמודל מבקש

builder.Services.AddScoped<IAgentService, AgentService>();

//Jwt
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
    throw new InvalidOperationException("Jwt:Key is missing. Set the environment variable Jwt__Key.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();


var imagesFolder = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, builder.Configuration["ImagesFolder"]));
Directory.CreateDirectory(imagesFolder);

// Services
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

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
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAgentService, AgentService>();
builder.Services.AddScoped<IPasswordHasher<Users>, PasswordHasher<Users>>();


var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesFolder),
    RequestPath = "/images"
});


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
