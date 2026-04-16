using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using OmnisReview.Repositorys.Interfaces;
using OmnisReview.Services.Interfaces;
using OmnisReview.Data;
using OmnisReview.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
        policy.WithOrigins("http://localhost:8080")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailSender, OmnisReview.Services.EmailSender>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add dependency injection for services and repositories
builder.Services.AddScoped<IAuthRepository, OmnisReview.Repositorys.AuthRepository>();
builder.Services.AddScoped<IAuthService, OmnisReview.Services.AuthService>();
builder.Services.AddScoped<IRoleSeedService, OmnisReview.Services.RoleSeedService>();
builder.Services.AddScoped<IUserSeedService, OmnisReview.Services.UserSeedService>();
builder.Services.AddHttpClient<ITmdbService, OmnisReview.Services.TmdbService>();
builder.Services.AddHttpClient<IGoogleBooksService, OmnisReview.Services.GoogleBooksService>();
builder.Services.AddHttpClient<IRawgService, OmnisReview.Services.RawgService>();

//CONFIGURAÇÃO EF E IDENTITY
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

var app = builder.Build();

// Seed roles and admin user on application startup
using (var scope = app.Services.CreateScope())
{
    var roleSeedService = scope.ServiceProvider.GetRequiredService<IRoleSeedService>();
    await roleSeedService.SeedRolesAsync();

    var userSeedService = scope.ServiceProvider.GetRequiredService<IUserSeedService>();
    await userSeedService.SeedAdminUserAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
        
app.UseCors("DevCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
