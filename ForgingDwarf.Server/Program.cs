using ForgingDwarf.Server.Data;
using Microsoft.EntityFrameworkCore;
using ForgingDwarf.Server.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=ForgingDwarfDB;Trusted_Connection=True;"));

builder.Services.AddScoped<OrderRepository>();

builder.Services.AddControllers();
var app = builder.Build();

//автоматически создаём БД
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.MapControllers();

app.MapGet("/", () => Results.Json(new
{
    Status = "API работает",
    Endpoints = new
    {
        Orders = "/api/orders",
        Clients = "/api/clients",
        Login = "/api/auth/login",
        Register = "/api/auth/register"
    }
}));
app.MapGet("/api", () => "API Endpoints:\n/api/orders\n/api/clients\n/api/auth/login\n/api/auth/register");

app.Run();