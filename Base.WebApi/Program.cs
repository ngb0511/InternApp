using Base.Data.Infrastructure.UnitOfWork;
using Base.Data.Models;
using Base.Data.Repositories;
using Base.Domain.Interfaces;
using Base.Service.Contracts;
using Base.Service.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("https://localhost:7128") // Thay đổi thành nguồn của bạn
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<Task01Context>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ITimingPostService, TimingPostService>();
builder.Services.AddTransient<ITimingPostRepository, TimingRepository>();
builder.Services.AddTransient<IUserAssignService,UserAssignService>();
builder.Services.AddTransient<IUserAssignRepository,UserAssignRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<Task01Context>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("AllowSpecificOrigin");

app.MapControllers();

app.Run();
