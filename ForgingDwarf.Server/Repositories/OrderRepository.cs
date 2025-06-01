using ForgingDwarf.Common.Models;
using ForgingDwarf.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace ForgingDwarf.Server.Repositories
{
    public class OrderRepository
    {
        private readonly AppDbContext _db;

        public OrderRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> ClientExistsAsync(int clientId)
        {
            return await _db.Clients.AnyAsync(c => c.Id == clientId);
        }

        public async Task AddOrderAsync(Order order)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
        }

        public List<Order> GetAllOrders()
        {
            return _db.Orders.Include(o => o.Client).ToList();
        }
    }
}