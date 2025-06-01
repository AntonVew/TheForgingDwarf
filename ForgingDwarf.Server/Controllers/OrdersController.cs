using ForgingDwarf.Common.Models;
using ForgingDwarf.Server.Data;
using ForgingDwarf.Server.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ForgingDwarf.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _db;

        private readonly OrderRepository _repository;

        public OrdersController(AppDbContext db, OrderRepository repository)
        {
            _repository = repository;
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repository.GetAllOrders());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _db.Orders
                .Include(o => o.Client)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            return order;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            try
            {
                // валидация
                if (!ModelState.IsValid)
                {
                    Console.WriteLine($"Ошибки валидации: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors))}");
                    return BadRequest(ModelState);
                }

                var clientExists = await _db.Clients.AnyAsync(c => c.Id == order.ClientId);
                if (!clientExists)
                {
                    Console.WriteLine($"Клиент с ID {order.ClientId} не найден");
                    return BadRequest("Клиент не найден");
                }

                //логирование
                Console.WriteLine($"Создание заказа: {JsonSerializer.Serialize(order)}");

                order.OrderDate = DateTime.UtcNow;

                await using var transaction = await _db.Database.BeginTransactionAsync();
                try
                {
                    _db.Orders.Add(order);
                    await _db.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Console.WriteLine($"Заказ создан, ID: {order.Id}");
                    return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
                }
                catch (DbUpdateException dbEx)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Ошибка БД: {dbEx.InnerException?.Message}");
                    return StatusCode(500, "Ошибка сохранения заказа");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка: {ex}");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var order = await _db.Orders.FindAsync(id);
                if (order == null)
                    return NotFound();

                _db.Orders.Remove(order);
                await _db.SaveChangesAsync();

                Console.WriteLine($"Заказ {id} удален");
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка удаления: {ex.Message}");
                return StatusCode(500, "Ошибка при удалении заказа");
            }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusUpdateDto dto)
        {
            try
            {
                var order = await _db.Orders.FindAsync(id);
                if (order == null)
                    return NotFound();

                order.Status = dto.Status;
                await _db.SaveChangesAsync();

                Console.WriteLine($"Статус заказа {id} изменен на {dto.Status}");
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обновления статуса: {ex.Message}");
                return StatusCode(500, "Ошибка при обновлении статуса");
            }
        }

        public class StatusUpdateDto
        {
            public OrderStatus Status { get; set; }
        }
    }

}
