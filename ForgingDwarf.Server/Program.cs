using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using ForgingDwarf.Server.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=ForgingDwarfDB;Trusted_Connection=True;"));

// Добавляем поддержку API-контроллеров
builder.Services.AddControllers();

var app = builder.Build();

// Настройка маршрутов
app.MapControllers();

// Запускаем сервер
app.Run();