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
                // 1. Проверка валидации модели
                if (!ModelState.IsValid)
                {
                    Console.WriteLine($"Ошибки валидации: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors))}");
                    return BadRequest(ModelState);
                }

                // 2. Проверка существования клиента
                var clientExists = await _db.Clients.AnyAsync(c => c.Id == order.ClientId);
                if (!clientExists)
                {
                    Console.WriteLine($"Клиент с ID {order.ClientId} не найден");
                    return BadRequest("Клиент не найден");
                }

                // 3. Логирование данных перед сохранением
                Console.WriteLine($"Создание заказа: {JsonSerializer.Serialize(order)}");

                // 4. Установка даты
                order.OrderDate = DateTime.UtcNow;

                // 5. Сохранение с транзакцией
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
    }

}
