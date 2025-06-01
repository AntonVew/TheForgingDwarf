using ForgingDwarf.Common.Models;
using ForgingDwarf.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForgingDwarf.Server.Controllers
{
    public class ClientsController
    {
        private readonly AppDbContext _db;

        public ClientsController(AppDbContext db) => _db = db;

        [HttpGet("for-dropdown")]
        public async Task<ActionResult<List<Client>>> GetForDropdown()
        {
            return await _db.Clients
                .Select(c => new Client { Id = c.Id, Name = c.Name })
                .ToListAsync();
        }
    }
}
