using Library.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Database
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("LibraryDb")));


// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Repositories
// builder.Services.AddScoped<IBookRepository, BookRepository>();
// builder.Services.AddScoped<IMemberRepository, MemberRepository>();
// builder.Services.AddScoped<IBorrowingRepository, BorrowingRepository>();


// Services
// builder.Services.AddScoped<IBookService, BookService>();
// builder.Services.AddScoped<IMemberService, MemberService>();
// builder.Services.AddScoped<IBorrowingService, BorrowingService>();


var app = builder.Build();


// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// HTTPS
app.UseHttpsRedirection();


// Endpoint Registration
// app.MapBookEndpoints();
// app.MapMemberEndpoints();
// app.MapBorrowingEndpoints();


app.Run();