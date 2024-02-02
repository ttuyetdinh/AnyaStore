using AnyaStore.Services.AuthAPI.Data;
using AnyaStore.Services.AuthAPI.Models;
using AnyaStore.Services.AuthAPI.Services;
using AnyaStore.Services.AuthAPI.Services.IServices;
using AnyaStore.Services.CouponAPI.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// setting up the ASP.NET Core Identity system with Entity Framework as the storage provider, 
// using the specified DbContext, and with the default token providers.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
// add config password for Identity User
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 1;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
});
// binding the JWT settings from the application's configuration to the JwtOptions class. 
// This allows the settings to be injected and used elsewhere in the application, such as in the code that generates or validates JWTs.
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("APISettings:JwtOptions"));

// Add DI services
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// create a custom service extension to upadate the db if migrations is exsiting
app.DBMigration();

app.Run();
