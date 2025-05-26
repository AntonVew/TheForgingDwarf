using Microsoft.AspNetCore.Mvc;
using ForgingDwarf.Common.Models;
using ForgingDwarf.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace ForgingDwarf.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _db; // Используем DbContext вместо репозитория

        public ClientsController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_db.Clients.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] Client client)
        {
            _db.Clients.Add(client);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
