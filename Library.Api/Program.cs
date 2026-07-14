using Library.Api.Application.Interfaces;
using Library.Api.Applications.Services;
using Library.Api.Infrastructure.Data;
using Library.Api.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Library.Api.Middleware;
using FluentValidation;
using Library.Api.Applications.Validators;
using Library.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);


// Database
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Repositories
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IBorrowingRepository, BorrowingRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IBorrowingService, BorrowingService>();


//Validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookRequestValidator>();


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();


// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// HTTPS
app.UseHttpsRedirection();


// Endpoint Registration
app.MapBookEndpoints();
app.MapMemberEndpoints();
app.MapBorrowingEndpoints();


app.Run();