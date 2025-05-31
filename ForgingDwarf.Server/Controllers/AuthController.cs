using Microsoft.AspNetCore.Mvc;
using ForgingDwarf.Common.Models;
using ForgingDwarf.Server.Data;
using ForgingDwarf.Common.Utils;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;

    public AuthController(AppDbContext db)
    {
        _db = db;
    }

    // Регистрация
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Client client)
    {
        // Проверка уникальности имени
        if (_db.Clients.Any(c => c.Name == client.Name))
            return BadRequest("Имя пользователя уже занято");

        // Хеширование пароля
        client.Password = PasswordHasher.Hash(client.Password);

        client.IsSuperuser = false;

        _db.Clients.Add(client);
        await _db.SaveChangesAsync();

        return Ok(new { client.Id, client.Name });
    }

    // Вход
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var client = await _db.Clients.FirstOrDefaultAsync(c => c.Name == request.Name);
        if (client == null || !PasswordHasher.Verify(request.Password, client.Password))
            return Unauthorized("Неверное имя или пароль");

        return Ok(new { client.Id, client.Name });
    }
}

public class LoginRequest
{
    public string Name { get; set; }
    public string Password { get; set; }
}