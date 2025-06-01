using Microsoft.AspNetCore.Mvc;
using ForgingDwarf.Common.Models;
using ForgingDwarf.Server.Data;
using ForgingDwarf.Common.Utils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
    public async Task<IActionResult> Register([FromBody] ClientRegistrationDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingClient = await _db.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(c => EF.Functions.Collate(c.Name, "SQL_Latin1_General_CP1_CS_AS") == request.Name);

        if (existingClient != null)
            return Conflict("Имя пользователя уже занято");

        var client = new Client
        {
            Name = request.Name,
            Password = PasswordHasher.Hash(request.Password),
            Email = request.Email,
            Phone = request.Phone,
            IsSuperuser = false
        };

        try
        {
            _db.Clients.Add(client);
            await _db.SaveChangesAsync();

            return Ok(new { client.Id, client.Name });
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, "Ошибка сервера при сохранении");
        }
    }

    public class ClientRegistrationDto
    {
        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }
    }
}