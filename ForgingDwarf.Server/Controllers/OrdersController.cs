using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForgingDwarf.Common.Models;
using ForgingDwarf.Server.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ForgingDwarf.Server.Controllers
{
    public class OrdersController : ControllerBase
    {
        private readonly OrderRepository _repo = new OrderRepository();

        [HttpGet]
        public List<Order> GetAllOrders() => _repo.GetAllOrders();
    }
}
