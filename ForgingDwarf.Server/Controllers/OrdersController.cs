using Microsoft.AspNetCore.Mvc;
using ForgingDwarf.Common.Models;
using ForgingDwarf.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace ForgingDwarf.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _db; // Используем DbContext вместо репозитория

        public OrdersController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        //public IActionResult GetAll()
        //{
        //    var orders = _db.Orders
        //        .Include(o => o.Client) // Если нужно загрузить связанные данные
        //        .ToList();
        //    return Ok(orders);
        //}

        [HttpGet("clients")]
        public IActionResult GetClients()
        {
            var clients = _db.Clients.AsNoTracking().ToList();
            return Ok(clients);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            return Ok(order); // Возвращаем созданный заказ с ID
        }
    }
}