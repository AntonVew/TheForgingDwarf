using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForgingDwarf.Common.Models;
using ForgingDwarf.Server.Data;

namespace ForgingDwarf.Server.Repositories
{
    public class OrderRepository
    {
        private readonly AppDbContext _db;

        // Изменён конструктор
        public OrderRepository(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        public void AddOrder(Order order)
        {
            _db.Orders.Add(order);
            _db.SaveChanges();
        }

        public List<Order> GetAllOrders() => _db.Orders.ToList();
    }
}
