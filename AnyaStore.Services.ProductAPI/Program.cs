using AnyaStore.Services.ProductAPI;
using AnyaStore.Services.ProductAPI.Data;
using AnyaStore.Services.ProductAPI.Extensions;
using AnyaStore.Services.ProductAPI.Repository;
using AnyaStore.Services.ProductAPI.Repository.IRepository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// add DI
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// explicit control over the creation and registration of the IMapper instance
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// add authentication with JWT
builder.AddAppAuthentication();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// add configuration for swagger
builder.Services.AddSingleton<IConfigureOptions<SwaggerGenOptions>, ConfigurationSwaggerGenOptions>();
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
