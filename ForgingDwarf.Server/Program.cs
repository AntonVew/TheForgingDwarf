using ForgingDwarf.Server.Data;
using ForgingDwarf.Server.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Регистрация DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=ForgingDwarfDB;Trusted_Connection=True;"));

builder.Services.AddScoped<OrderRepository>();
builder.Services.AddControllers();
var app = builder.Build();

// Автоматическое создание БД
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.MapControllers();
app.Run();