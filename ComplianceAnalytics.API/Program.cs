using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ComplianceAnalytics.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ComplianceAnalytics.Domain.Repositories;
using ComplianceAnalytics.Infrastructure.Repositories;
using ComplianceAnalytics.Application.Services;
using ComplianceAnalytics.Infrastructure.Service;
using Microsoft.Extensions.Caching.Distributed;


var builder = WebApplication.CreateBuilder(args);

// ---------- Controllers + Swagger ----------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------- Repositories + Services ----------
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ITaskControllerService, TaskControllerService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AnalyticsService>(sp =>
    new AnalyticsService(
        sp.GetRequiredService<IDistributedCache>(),
        builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ---------- EF Core ----------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ---------- Redis ----------
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "ComplianceAnalytics_";
});

// ---------- Authentication (JWT) ----------
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// ---------- Authorization (roles) ----------
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

var app = builder.Build();

// ---------- Middleware ----------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication(); // 🔥 must come before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
